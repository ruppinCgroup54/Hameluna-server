using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class AdoptionRequest
    {

        public int RequestId { get; set; }
        public Persone Adopter { get; set; }
        public DateTime SendDate { get; set; }
        public int DogId { get; set; }
        public string Status { get; set; }

        public AdoptionRequest(int requestId, Persone adopter, DateTime sendDate, int dogId, string status)
        {
            RequestId = requestId;
            Adopter = adopter;
            SendDate = sendDate;
            DogId = dogId;
            Status = status;
        }

        public AdoptionRequest()
        {
            Adopter = new();
            SendDate = DateTime.Now;
        }

        public void Insert()
        {
            AdoptionRequestDBService db = new();

            try
            {
                Adopter ad = new() { 
                    PhoneNumber=this.Adopter.PhoneNumber ,
                    FirstName=this.Adopter.FirstName ,
                    LastName=this.Adopter.LastName ,
                    Email=this.Adopter.Email 
                    };
                ad.Insert();
            }
            catch
            {
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
