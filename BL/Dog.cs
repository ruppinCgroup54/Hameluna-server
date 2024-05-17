using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class Dog
    {
        public Dog()
        {
            ChipNumber = "";
            NumberId = -1;
            Name = "";
            DateOfBirth = DateTime.Now;
            Gender = "";
            EntranceDate = DateTime.Now;
            IsAdoptable = false;
            Size = "";
            Adopted = false;
            IsReturned = false;
            CellId = -1;
            Color = new();
            Breed = new();
            Charecteristics = new();
        }
        public Dog(string chipNumber, int numberId, string name, DateTime dateOfBirth, string gender, DateTime entranceDate, string size, bool adopted, int cellId, List<string> color, List<string> breed, List<string> charecteristic, bool isAdoptable=false, bool isReturned=false)
        {
            ChipNumber = chipNumber;
            NumberId = numberId;
            Name = name;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            EntranceDate = entranceDate;
            IsAdoptable = isAdoptable;
            Size = size;
            Adopted = adopted;
            IsReturned = isReturned;
            CellId = cellId;
            Color = color;
            Breed = breed;
            Charecteristics = charecteristic;
        }


        public string ChipNumber { get; set; }
        public int NumberId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime EntranceDate { get; set; }
        public bool IsAdoptable { get; set; }
        public string Size { get; set; }
        public bool Adopted { get; set; }
        public bool IsReturned { get; set; }
        public int CellId { get; set; }
        public List<string> Color { get; set; }
        public List<string> Breed { get; set; }
        public List<string> Charecteristics { get; set; }
        public float Age { get => GetAge(DateOfBirth); }


        public static float GetAge(DateTime dob)
        {
            DateTime dt = DateTime.Now;
            if (dt.Day < dob.Day)
            {
                dt = dt.AddMonths(-1);
            }

            int months = dt.Month - dob.Month;
            if (months < 0)
            {
                dt = dt.AddYears(-1);
                months += 12;
            }

            int years = dt.Year - dob.Year;
            return years+months/12;
        }

        public int Insert()
        {
            DogDBService db = new();

            this.NumberId = db.InsertDog(this);
            db.InsertBreedOfDog(this);
            db.InsertColorOfDog(this);
            db.InsertCharecterOfDog(this);
            return this.NumberId;

        }

        public int Update()
        {
            DogDBService db = new();

            return db.UpdateDog(this);

        }
        public static List<Dog> ReadAll()
        {
            DogDBService db = new(); 
            return db.ReadAll();
        }

        public static Dog ReadOne(int id)
        {
            DogDBService db = new();
            return db.ReadDog(id);
        }

        public static int Delete(int id)
        {
            DogDBService db = new();
            return db.DeleteDog(id);
        }
    }
}
