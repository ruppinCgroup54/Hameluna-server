using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class Volunteer : Persone
    {

        public Volunteer(string phoneNumber, string firstName, string lastName, string email, string password,string shelterId)
            : base(phoneNumber, firstName, lastName, email)
        {
            
            Password = password;
            ShelterId = shelterId;
        }
        public Volunteer() : base()
        {
            Password = "";
            ShelterId = "";

        }

        public string Password { get; set; }
        public string ShelterId { get; set; }

        public string Insert()
        {
            VolunteerDBService db = new();

            return db.InsertVoluteer(this);

        }
        
        public int Update()
        {
            VolunteerDBService db = new();

            return db.UpdateVoluteer(this);

        }

        public static List<Volunteer> ReadAll()
        {
            VolunteerDBService db = new();
            return db.ReadVolunteer();
        }   
        
        public static Volunteer ReadOne(string id)
        {
            VolunteerDBService db = new();
            return db.ReadVolunteer(id);
        } 
        
        public static int Delete(string id)
        {
            VolunteerDBService db = new();
            return db.DeleteVolunteer(id);
        }
    }
}
