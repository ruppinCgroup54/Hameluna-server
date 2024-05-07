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
        }
        public Dog(string chipNumber, int numberId, string name, DateTime dateOfBirth, string gender, DateTime entranceDate, bool isAdoptable, string size, bool adopted, bool isReturned, int cellId, List<string> color, List<string> breed)
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
        public double Age { get => Convert.ToDouble(DateTime.Now - DateOfBirth); }

        public int Insert()
        {
            DogDBService db = new();

            this.NumberId = db.InsertDog(this);
            db.InsertBreedOfDog(this);
            db.InsertColorOfDog(this);
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
