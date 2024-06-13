using System.Text.Json.Nodes;
using hameluna_server.DAL;
using MongoDB.Bson;

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
            Attributes = new();
            ShelterNumber = -1;
        }
        public Dog(int shelterNumber, string chipNumber, int numberId, string name, DateTime dateOfBirth, string gender, DateTime entranceDate, string size, int cellId, List<string> color, List<string> breed, List<string> attributes, bool isAdoptable = false, bool isReturned = false, bool adopted = false)
        {
            ShelterNumber = shelterNumber;
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
            Attributes = attributes;
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
        public int ShelterNumber { get; set; }
        public List<string> Color { get; set; }
        public List<string> Breed { get; set; }
        public List<string> Attributes { get; set; }
        public string ProfileImage { get; set; }
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
            return years + months / 12;
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
            db.InsertBreedOfDog(this);
            db.InsertColorOfDog(this);
            db.InsertCharecterOfDog(this);

            return db.UpdateDog(this);

        }

        public List<string> GetAllImages()
        {
            DBservices db = new();

            return db.GetDogImages(this.NumberId);
        }        
        public List<string> GetAllFiles()
        {
            DBservices db = new();

            return db.GetDogFiles(this.NumberId);
        }

        public async Task<List<string>> AddImages(string shelterId, List<IFormFile> images)
        {
            DBservices db = new();
            List<string> paths = new();
            foreach (var img in images)
            {
                string path = await db.InsertProfileImage(shelterId, this.NumberId, img);
                await Console.Out.WriteLineAsync(path);
                db.InsertImagesToData(path, this.NumberId);
                paths.Add(path);
            }
            Console.WriteLine(paths.ToJson());
            return paths;

        }

        public static int DeleteImage(string url)
        {
            DBservices db = new();
            return db.DeleteFile(url, false);
        }  
        public static int DeleteFile(string url)
        {
            DBservices db = new();
            return db.DeleteFile(url, true);
        }

        public static List<Dog> ReadAll()
        {
            DogDBService db = new();
            return db.ReadAll();
        }

        public static List<Dog> GetDogsForUser(string userId)
        {
            ChatDBService chatDB = new();
            //get all the dog sorted from the best match down
            List<JsonRank> dogsRank = chatDB.GetDogRank(userId);
            List<int> favorites = chatDB.GetUserFavorites(userId);

            dogsRank = dogsRank.FindAll(d => !favorites.Contains(d.id));

            DogDBService dogDB = new();
            List<Dog> dogs = dogDB.ReadAll();

            List<Dog> filteredDogs = new();

            foreach (JsonRank dr in dogsRank)
            {

                filteredDogs.Add(dogs.FirstOrDefault(d => d.NumberId == dr.id));

            }
            return filteredDogs;

        }

        public static List<Dog> GetFavorites(string userId)
        {
            ChatDBService chatDB = new();
            //get all the dog sorted from the best match down
            List<int> favorites = chatDB.GetUserFavorites(userId);

            DogDBService dogDB = new();
            List<Dog> dogs = dogDB.ReadAll();

            List<Dog> filteredDogs = new();

            foreach (int fav in favorites)
            {

                filteredDogs.Add(dogs.FirstOrDefault(d => d.NumberId == fav));

            }
            return filteredDogs;

        }

        public static int UpdateFavorites(int[] newFav, string userId)
        {
            ChatDBService chatDB = new();
            //get all the dog sorted from the best match down
            return chatDB.UpdateFavoritesDogs(newFav, userId);
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
