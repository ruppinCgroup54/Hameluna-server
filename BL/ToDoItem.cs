using hameluna_server.DAL;

namespace hameluna_server.BL
{
    public class ToDoItem
    {
        public ToDoItem()
        {
        }

        public ToDoItem(int toDoId, bool done, DateTime doDate, string title, int repetition, int shelterNumber, string description)
        {
            ToDoId = toDoId;
            Done = done;
            DoDate = doDate;
            Title = title;
            Repetition = repetition;
            ShelterNumber = shelterNumber;
            Description = description;
        }


        public int ToDoId { get; set; }
        public bool Done { get; set; }
        public DateTime DoDate { get; set; }
        public string Title { get; set; }
        public int Repetition { get; set; }
        public int ShelterNumber { get; set; }
        public string Description { get; set; }

        public int Insert()
        {
            ToDoDBService db = new();

            ToDoId= db.InsertToDoItem(this);

            return ToDoId;

        }

        public int Update()
        {
            ToDoDBService db = new();

            return db.UpdateToDoItem(this);

        }

        public static List<ToDoItem> ReadByDate(int shelterNum, DateTime date)
        {
            ToDoDBService db = new();
            return db.ReadToDoItemByDate(shelterNum,date);
        }
        public static ToDoItem ReadOne(int todoId)
        {
            ToDoDBService db = new();
            return db.ReadToDo(todoId);
        }

    }
}
