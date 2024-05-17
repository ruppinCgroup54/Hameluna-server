using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class Address
    {
        public Address()
        {
        }

        public Address(int id, int houseNumber, string streetName, string city, string region)
        {
            Id = id;
            HouseNumber = houseNumber;
            StreetName = streetName;
            City = city;
            Region = region;
        }

        public int Id { get; set; }
        public int HouseNumber { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string Region { get; set; }

        public int Insert()
        {
            try
            {
                DBservices db = new();

                this.Id= db.InsertAddress(this);

                return this.Id;
            }
            catch (Exception)
            {

                throw new Exception("Invlid address");
            }
    

        }

        static public List<string> GetCities()
        {
            DBservices db = new();
            return db.GetAllCities();
        }

    }
}
