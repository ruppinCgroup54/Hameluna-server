using hameluna_server.DAL;
using Microsoft.AspNetCore.Identity;

namespace hameluna_server.BL
{
    public class AdoptionRequest
    {


        public int RequestId { get; set; }
        public Adopter Adopter { get; set; }
        public DateTime SendDate { get; set; }
        public Dog Dog { get; set; }
        public string Status { get; set; }

        public AdoptionRequest(int requestId, Adopter adopter, DateTime sendDate, Dog dog, string status)
        {
            RequestId = requestId;
            Adopter = adopter;
            SendDate = sendDate;
            Dog = dog;
            Status = status;
        }

        public AdoptionRequest()
        {
            Adopter = new();
            Dog = new();
            SendDate = DateTime.Now;
        }

        public void Insert()
        {
            AdoptionRequestDBService db = new();

            try
            {
                this.Adopter.Insert();
            }
            catch
            {
                this.Adopter.Update();
                this.Adopter = Adopter.ReadOne(Adopter.PhoneNumber);

                Console.WriteLine("User exits");
            }
            finally
            {
                this.RequestId = db.InsertAdoptionRequest(this);
                if (this.RequestId > 0)
                {
                    if (this.Status != "trial period")
                    {

                        FireBaseDBService fireDb = new();

                        fireDb.SetAdoptionRequest(Dog.ShelterNumber, this);
                    }

                    InsertToDoOfEndTrail();

                }

            }


        }

        public async Task<int> Update()
        {
            AdoptionRequestDBService db = new();

            int ans = db.UpdateAdoptionRequest(this);

            if (ans > 0)
            {
                FireBaseDBService fireDb = new();

                fireDb.DeleteAdoptionRequest(Dog.ShelterNumber, this);

                await InsertToDoOfEndTrail();

            }

            return ans;

        }

        public static List<AdoptionRequest> ReadAll()
        {
            AdoptionRequestDBService db = new();
            return db.ReadAdoptionRequest();
        }
        public static AdoptionRequest ReadByAdopter(string phoneNumber, int dogId)
        {
            AdoptionRequestDBService db = new();
            return db.ReadAdoptionRequest(phoneNumber, dogId);
        }

        public async Task InsertToDoOfEndTrail()
        {
            if (this.Status == "trial period")
            {
                ToDoItem toDoItem = new(0, false, DateTime.Now.AddMonths(1), "סוף תקופת הניסיון של הכלב " + this.Dog.Name, 0, this.Dog.ShelterNumber, "");

                toDoItem.Insert();

                string adoptionText = string.Format($@"
                                    <div style=""font-family: 'Arial', sans-serif;
                                        background-color: #F5F5F5;
                                        color: #4B3F35;
                                        direction: rtl;
                                        text-align: right;
                                        margin: 0;
                                        padding: 0;overflow-y: scroll;"">

                                                <div style=""  width: 700px;
                                            height: 350px;
                                            margin: 20px auto;
                                            padding: 20px;
                                            background-color: white;
                                            border: 1px solid #DCDCDC;
                                            position: relative; text-align: center;
                                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);"">

                                                    <img style=""position: absolute;height:inherit; width: inherit ; right: 20px;"" src=""https://proj.ruppin.ac.il/cgroup54/test2/tar1/Images/adoptionImage.png"" alt="""">

                                                    <div style="" margin-top: 150px;
                                                    font-size: 20px; "">
                                                        <p><strong>שם הכלב:</strong> {Dog.Name}</p>
                                                        <p><strong>שם המאמץ:</strong> {Adopter.FirstName + " " + Adopter.LastName}</p>
                                                        <p><strong>תאריך יומולדת:</strong> {Dog.DateOfBirth.ToString("dd'/'MM'/'yyyy")}</p>
                                                        <p><strong>תאריך אימוצלת:</strong> {DateTime.Now.ToString("dd'/'MM'/'yyyy")}</p>
                                                    </div>

                                                </div>
                                            </div>
");

                string adoptionFile = await PdfService.GeneratePdfAsync(adoptionText, Dog);

                DogDBService dbd = new();
                dbd.InsertFileToData(adoptionFile, Dog.NumberId);

            }
        }


        public int Delete(int id)
        {
            AdoptionRequestDBService db = new();
            int ans = db.DeleteAdoptionRequest(id);
            this.RequestId = id;
            if (ans > 0)
            {
                FireBaseDBService fireDb = new();
                fireDb.DeleteAdoptionRequest(Dog.ShelterNumber, this);
            }
            return ans;
        }
    }
}
