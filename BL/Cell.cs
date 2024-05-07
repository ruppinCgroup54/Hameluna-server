using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class Cell
    {
        public Cell()
        {
            DogsInCell = new();
        }

        public int Number { get; set; }
        public int Capacity { get; set; }
        public int Id { get; set; }
        public int ShelterNumber { get; set; }
        public List<Dog> DogsInCell { get; set; }
        public Cell(int number=-1, int capacity=-1, int id=-1, int shelterNumber=-1)
        {
            Number = number;
            Capacity = capacity;
            Id = id;
            ShelterNumber = shelterNumber;
        }
 
        public void Insert()
        {
            CellDBService db = new();

            this.Id = db.InsertCell(this);
        }

        public int Update()
        {
            CellDBService db = new();

            return db.UpdateCell(this);

        }

        public static List<Cell> ReadAll()
        {
            CellDBService db = new();
            return db.ReadCell();
        }
        
        public static List<Cell> ReadAllFromShelter(int shelterId)
        {
            CellDBService db = new();
            List<Cell> cells = db.ReadShelterCells(shelterId);

            DogDBService dogDb = new();
            List<Dog> dogsInShelter = dogDb.ReadDogByShelter(shelterId);

            for (int i = 0; i < cells.Count; i++)
            {
                //List<Dog> dogs = dogsInShelter.FindAll((d) => d.CellId == cells[i].Id);
                //for (int j = 0; j < dogs.Count; j++)
                //{
                //   dogs[j].Breed = dogDb.GetDogBreed(dogs[j].NumberId);
                //}
                cells[i].DogsInCell = dogsInShelter.FindAll((d) => d.CellId == cells[i].Id);
            }

            return cells;
        }

        public static Cell ReadOne(int id)
        {
            CellDBService db = new();
            return db.ReadCell(id);
        }

        public static int Delete(int id)
        {
            CellDBService db = new();
            return db.DeleteCell(id);
        }
    }
}
