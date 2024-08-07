using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class RoutinItem
    {
        public RoutinItem(int itemID, string item)
        {
            ItemID = itemID;
            Item = item;
        }

        public int ItemID { get; set; }
        public string Item { get; set; }
    }
    public class DailyRoutine
    {
        public DailyRoutine(int routineId, DateTime fillDate, string note, int dogNumberId, string volunteerPhoneNumber, int shelterNumber, List<RoutineException> dogExceptions)
        {
            RoutineId = routineId;
            FillDate = fillDate;
            Note = note;
            DogNumberId = dogNumberId;
            VolunteerPhoneNumber = volunteerPhoneNumber;
            ShelterNumber = shelterNumber;
            DogExceptions = dogExceptions;
        }

        public DailyRoutine()
        {
            FillDate = DateTime.Now;
            DogExceptions =  new();
        }

        public int RoutineId { get; set; }
        public DateTime FillDate { get; set; }
        public string Note { get; set; }
        public int DogNumberId { get; set; }
        public string VolunteerPhoneNumber { get; set; }
        public int ShelterNumber { get; set; }
        public List<RoutineException> DogExceptions { get; set; }


        public List<FullRoutineException> Insert()
        {
            DailyRoutineDBService db = new();

            RoutineId= db.InsertDailyRoutine(this);

            return db.ReadExceptionsByRoutine(this);


        }

        //public int Update()
        //{
        //    DailyRoutineDBService db = new();

        //    return db.UpdateDailyRoutine(this);

        //}

        //public static List<DailyRoutine> ReadAll()
        //{
        //    DailyRoutineDBService db = new();
        //    return db.ReadDailyRoutines();
        //}

        //public static DailyRoutine ReadOne(int id)
        //{
        //    DailyRoutineDBService db = new();
        //    return db.ReadDailyRoutine(id);
        //}

        //public static int Delete(int id)
        //{
        //    DailyRoutineDBService db = new();
        //    return db.DeleteDailyRoutine(id);
        //}

    }
}
