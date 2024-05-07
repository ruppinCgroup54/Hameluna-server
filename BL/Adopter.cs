using System.Net;
using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class Adopter :Persone
    {



        public Adopter(string phoneNumber, string firstName, string lastName, string email, DateTime dateOfBirth, string houseMembers, string dogsPlace, string additionalPets, string experience, string note, Address address) : base(phoneNumber, firstName, lastName, email)
        {
            DateOfBirth = dateOfBirth;
            HouseMembers = houseMembers;
            DogsPlace = dogsPlace;
            AdditionalPets = additionalPets;
            Experience = experience;
            Note = note;
            Address = address;
        }

        public Adopter():base()
        {
            DateOfBirth = DateTime.Now;
            HouseMembers = "";
            DogsPlace = "";
            AdditionalPets = "";
            Experience = "";
            Note = "";
            Address = new();
        }


        public DateTime DateOfBirth { get; set; }
        public string HouseMembers { get; set; }
        public string DogsPlace { get; set; }
        public string AdditionalPets { get; set; }
        public string Experience { get; set; }
        public string Note { get; set; }
        public Address Address { get; set; }
        public float Age { get => (DateOnly.FromDateTime(DateTime.Now).Year - DateOfBirth.Year); }




        public string Insert()
        {
            AdopterDBService db = new();

            return db.InsertAdopter(this);

        }

        public int Update()
        {
            AdopterDBService db = new();

            return db.UpdateAdopter(this);

        }

        public static List<Adopter> ReadAll()
        {
            AdopterDBService db = new();
            return db.ReadAdopters();
        }

        public static Adopter ReadOne(string id)
        {
            AdopterDBService db = new();
            return db.ReadAdopter(id);
        }

        public static int Delete(string id)
        {
            AdopterDBService db = new();
            return db.DeleteAdopter(id);
        }

    }
}
