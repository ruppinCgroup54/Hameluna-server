using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class Cell
    {
        public Cell()
        {

        }

        public int Number { get; set; }
        public int Capacity { get; set; }
        public int Id { get; set; }
        public int ShelterNumber { get; set; }

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
            return db.ReadCell();
        }

        public static Cell ReadOne(int id)
        {
            CellDBService db = new();
            return new();
            //return db.ReadCell(id);
        }

        public static int Delete(int id)
        {
            CellDBService db = new();
            return db.DeleteCell(id);
        }
    }
}
