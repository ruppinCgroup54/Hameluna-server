
using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class Admin :Persone
    {
        public Admin(string phoneNumber, string firstName, string lastName, string email, string userName, string password) 
            : base(phoneNumber, firstName, lastName, email)
        {
            UserName = userName;
            Password = password;
        }

        public Admin():base()
        {
            UserName = "";
            Password = "";

        }

        public string UserName { get; set; }
        public string Password { get; set; }


        public string Insert()
        {
            AdminDBService db = new();

            return db.InsertAdmin(this);

        }

        public int Update()
        {
            AdminDBService db = new();

            return db.UpdateAdmin(this);

        }

        public static List<Admin> ReadAll()
        {
            AdminDBService db = new();
            return db.ReadAdmin();
        }   
        
        public static Admin ReadOne(string id)
        {
            AdminDBService db = new();
            return db.ReadAdmin(id);
        }

        public static int Delete(string id)
        {
            AdminDBService db = new();
            return db.DeleteAdmin(id);
        }

    }
}
