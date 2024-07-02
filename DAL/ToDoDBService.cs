using hameluna_server.BL;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace hameluna_server.DAL
{
    public class ToDoDBService : DBservices
    {
        public ToDoDBService() : base()
        {
            spIUD = "ToDoTableIUD";
        }

        //create command for general SP CRUD
        private SqlCommand ToDoItemSPCmd(String spName, SqlConnection con, ToDoItem todo, string action)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@StatementType", action);

            cmd.Parameters.AddWithValue("@ToDoId", todo.ToDoId);
            cmd.Parameters.AddWithValue("@Title", todo.Title);
            cmd.Parameters.AddWithValue("@Done", todo.Done);
            cmd.Parameters.AddWithValue("@DoDate", todo.DoDate);
            cmd.Parameters.AddWithValue("@Description", todo.Description);
            cmd.Parameters.AddWithValue("@Repetition", todo.Repetition);
            cmd.Parameters.AddWithValue("@ShelterNumber", todo.ShelterNumber);



            return cmd;
        }

        public int InsertToDoItem(ToDoItem todo)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = ToDoItemSPCmd(spIUD, con, todo, "Insert");             // create the command

            try
            {
                int newToDoItemId = Convert.ToInt32( cmd.ExecuteScalar()); // execute the command
                return newToDoItemId;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                // close the db connection
                con?.Close();
            }

        }

        public int UpdateToDoItem(ToDoItem todo)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = ToDoItemSPCmd(spIUD, con, todo, "Update");             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                // close the db connection
                con?.Close();
            }

        }

        //public int DeleteToDoItem(string id)
        //{

        //    SqlConnection con;
        //    SqlCommand cmd;

        //    try
        //    {
        //        con = connect(conString); // create the connection
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }

        //    cmd = ToDoItemSPCmd(spIUD, con, new() { PhoneNumber = id }, "delete");             // create the command

        //    try
        //    {
        //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
        //        return numEffected;
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }

        //    finally
        //    {
        //        // close the db connection
        //        con?.Close();
        //    }

        //}

        public List<ToDoItem> ReadToDoItemByDate(int shelterNum, DateTime d)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = ToDoItemSPCmd(spIUD, con, new() {ShelterNumber=shelterNum, DoDate=d}, "SelectByDate");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return ReadToDosFromReader(dataReader);   
               
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                con?.Close();
            }

        }     

        public ToDoItem ReadToDo(int todoId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = ToDoItemSPCmd(spIUD, con, new() {ToDoId=todoId}, "SelectOne");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return ReadToDosFromReader(dataReader)[0];   
               
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                con?.Close();
            }

        }

        public List<JsonObject> GetCountItemByDays(int shelter ,DateTime d)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = ToDoItemSPCmd(spIUD, con, new() { DoDate = d,ShelterNumber=shelter }, "CountByDaysForMonth");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                List<JsonObject> TdList = new();

                while (dataReader.Read())
                {
                    JsonObject c = new()
                    {
                        new("dayInMonth", Convert.ToDateTime( dataReader["day"])),
                        new("countItems", Convert.ToInt32( dataReader["numOfItems"]))
                    };

                    TdList.Add(c);
                }
                return TdList;

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                con?.Close();
            }

        }

        public List<ToDoItem> ReadToDosFromReader(SqlDataReader dataReader)
        {
            List<ToDoItem> TdList = new();

            while (dataReader.Read())
            {
                ToDoItem a = new()
                {
                    ToDoId = Convert.ToInt32(dataReader["TodoId"]),
                    Done = Convert.ToBoolean(dataReader["Done"]),
                    DoDate = Convert.ToDateTime(dataReader["DoDate"]),
                    Title = dataReader["title"].ToString(),
                    Description = dataReader["Description"].ToString(),
                    Repetition= Convert.ToInt32( dataReader["Repetition"]),
                    ShelterNumber= Convert.ToInt32( dataReader["ShelterNumber"]),



                };
                TdList.Add(a);
            }
            return TdList;
        }
    }
}
