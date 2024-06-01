using System.Net;
using System.Xml.Linq;
using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class Shelter
    {
        public Shelter()
        {
            ShelterId = -1;
            AdminDetails = new();
            FacebookUserName = "";
            FacebookPassword = "";
            InstagramUserName = "";
            InstagramPassword = "";
            TimeToReport = DateTime.Now;
            Name = "";
            PhotoUrl = "";
            Address = new();
      
        }

        public int ShelterId { get; set; }
        public Admin AdminDetails { get; set; }
        public string FacebookUserName { get; set; }
        public string FacebookPassword { get; set; }
        public string InstagramUserName { get; set; }
        public string InstagramPassword{ get; set; }
        public DateTime TimeToReport { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public Address Address { get; set; }
        public List<string> DailyRoutine { get; set; }
        public List<Cell> Cells { get; set; }


        public Shelter(int shelterId, Admin adminDetails, string facebookUserName, string facebookPassword, string instagramUserName, string instagramPassword, DateTime timeToReport, string name, string photoUrl, Address address, List<string> dailyRoutine, List<Cell> cells)
        {
            ShelterId = shelterId;
            AdminDetails = adminDetails;
            FacebookUserName = facebookUserName;
            FacebookPassword = facebookPassword;
            InstagramUserName = instagramUserName;
            InstagramPassword = instagramPassword;
            TimeToReport = timeToReport;
            Name = name;
            PhotoUrl = photoUrl;
            Address = address;
            DailyRoutine = dailyRoutine;
            Cells = cells;
        }

        public void Insert()
        {
            ShelterDBService db = new();

            this.ShelterId= db.InsertShelter(this);

            foreach (Cell cell in Cells)
            {
                cell.ShelterNumber = this.ShelterId;
                cell.Insert();
            }
        }

        public int Update()
        {
            ShelterDBService db = new();

            return db.UpdateShelter(this);

        }

        public static List<Shelter> ReadAll()
        {
            ShelterDBService db = new();
            return db.ReadShelter();
        }

        public static Shelter ReadOne(int id)
        {
            ShelterDBService db = new();
            return db.ReadShelter(id);
        }

        public static int Delete(int id)
        {
            ShelterDBService db = new();
            return db.DeleteShelter(id);
        }
    }
}
