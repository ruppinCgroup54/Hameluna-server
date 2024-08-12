using hameluna_server.DAL;

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
                if (this.RequestId>0)
                {
                    FireBaseDBService fireDb = new();
                    fireDb.SetAdoptionRequest(Dog.ShelterNumber, this);
                }

            }


        }

        public int Update()
        {
            AdoptionRequestDBService db = new();

            int ans = db.UpdateAdoptionRequest(this);

            if (ans > 0)
            {
                FireBaseDBService fireDb = new();

                fireDb.DeleteAdoptionRequest(Dog.ShelterNumber, this);
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
            return db.ReadAdoptionRequest(phoneNumber,dogId);
        }


        public int Delete(int id)
        {
            AdoptionRequestDBService db = new();
                int ans = db.DeleteAdoptionRequest(id);
            this.RequestId = id;
            if (ans>0)
            {
                FireBaseDBService fireDb = new();
                fireDb.DeleteAdoptionRequest(Dog.ShelterNumber, this);
            }
            return ans;
        }
    }
}
