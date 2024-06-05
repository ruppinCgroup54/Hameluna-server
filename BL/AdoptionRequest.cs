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
                Console.WriteLine("User exits");
            }
            finally
            {
                this.RequestId = db.InsertAdoptionRequest(this);

            }


        }

        public int Update()
        {
            AdoptionRequestDBService db = new();

            return db.UpdateAdoptionRequest(this);

        }

        public static List<AdoptionRequest> ReadAll()
        {
            AdoptionRequestDBService db = new();
            return db.ReadAdoptionRequest();
        }


        public static int Delete(int id)
        {
            AdoptionRequestDBService db = new();
            return db.DeleteAdoptionRequest(id);
        }
    }
}
