using System.Runtime.CompilerServices;
using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class RoutineException
    {
        public RoutineException()
        {
        }

        public RoutineException(int routineId, bool isOk, bool isHandled, int itemId)
        {
            RoutineId = routineId;
            IsOk = isOk;
            IsHandled = isHandled;
            ItemId = itemId;
        }

        public int RoutineId { get; set; }
        public int ItemId { get; set; }
        public bool IsOk { get; set; }
        public bool IsHandled { get; set; }



        public int Update()
        {
            DailyRoutineDBService db = new();

            return db.UpdateRoutineException(this);

        }


        //public string Insert()
        //{
        //    DailyRoutineDBService db = new();

        //    return db.InsertDailyRoutine(this);

        //}


        //public static List<DailyRoutine> ReadAll()
        //{
        //    DailyRoutineDBService db = new();
        //    return db.ReadDailyRoutine();
        //}

        //public static DailyRoutine ReadOne(string id)
        //{
        //    DailyRoutineDBService db = new();
        //    return db.ReadDailyRoutine(id);
        //}

        //public static int Delete(string id)
        //{
        //    DailyRoutineDBService db = new();
        //    return db.DeleteDailyRoutine(id);
        //}

    }

    public class FullRoutineException:RoutineException
    {
        public FullRoutineException()
        {
        }

        public FullRoutineException(string routineItem, DateTime fillDate, string volunteerName, int dogId, string dogName):base()
        {
            RoutineItem = routineItem;
            FillDate = fillDate;
            VolunteerName = volunteerName;
            DogId = dogId;
            DogName = dogName;
        }

        public string RoutineItem { get; set; }
        public DateTime FillDate { get; set; }
        public string VolunteerName { get; set; }
        public int DogId { get; set; }
        public string DogName { get; set; }

        public static List<FullRoutineException> ReadByDog(int dogId)
        {
            DailyRoutineDBService db = new();
            return db.ReadExceptionsByDog(dogId);
        }

    }
}
