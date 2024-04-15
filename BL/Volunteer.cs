using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class Volunteer : Persone
    {
        
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
