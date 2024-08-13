using hameluna_server.BL;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace hameluna_server.DAL
{
    public class DailyRoutineDBService : DBservices
    {

        public DailyRoutineDBService() : base()
        {
            spIUD = "DailyRoutineIUD";
        }

        //create command for general SP CRUD
        private SqlCommand DailyRoutineSPCmd(String spName, SqlConnection con, DailyRoutine dailyRoutine, string action)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@StatementType", action);
            //cmd.Parameters.AddWithValue("@routineId", dailyRoutine.RoutineId);
            cmd.Parameters.AddWithValue("@fillDate", dailyRoutine.FillDate);
            cmd.Parameters.AddWithValue("@shelterNumber", dailyRoutine.ShelterNumber);
            cmd.Parameters.AddWithValue("@note", dailyRoutine.Note);
            cmd.Parameters.AddWithValue("@dogNumberId", dailyRoutine.DogNumberId);
            cmd.Parameters.AddWithValue("@VolunteerPhoneNumber", dailyRoutine.VolunteerPhoneNumber);
            cmd.Parameters.AddWithValue("@routineId", dailyRoutine.RoutineId);

            DataTable table = ConvertToTable(dailyRoutine.DogExceptions);
            cmd.Parameters.AddWithValue("@routineException", table);


            return cmd;
        }

        public int InsertDailyRoutine(DailyRoutine dailyRoutine)
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


            cmd = DailyRoutineSPCmd(spIUD, con, dailyRoutine, "Insert");             // create the command

            try
            {
                int newRoutine = Convert.ToInt32(cmd.ExecuteScalar()); // execute the command

                return newRoutine;


            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                // close the db connection
                con?.Close();
            }

        }

        public int GetPassDailyStatus(int dogNumberId)
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


            cmd = DailyRoutineSPCmd(spIUD, con, new() { DogNumberId = dogNumberId }, "TodayDogRoutine");             // create the command

            try
            {
                int num = Convert.ToInt32(cmd.ExecuteScalar()); // execute the command

                return num;


            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                // close the db connection
                con?.Close();
            }

        }

        public int UpdateRoutineException(RoutineException routineException)
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

            cmd = DailyRoutineSPCmd(spIUD, con, new() { DogExceptions = new() { routineException } }, "Update");             // create the command

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

        public int DeleteDailyRoutine(int id)
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

            cmd = DailyRoutineSPCmd(spIUD, con, new() { RoutineId = id }, "Delete");             // create the command

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

        public DailyRoutine ReadRoutineByDog(int dogNumberId)
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

            DailyRoutine dr = new() { DogExceptions = new(), DogNumberId = dogNumberId };

            cmd = DailyRoutineSPCmd(spIUD, con, dr, "SelectTodayRoutine");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    if (dr.RoutineId == 0)
                    {


                        dr.RoutineId = Convert.ToInt32(dataReader["routineId"]);
                        dr.DogNumberId = Convert.ToInt32(dataReader["dogNumberId"]);
                        dr.FillDate = Convert.ToDateTime(dataReader["fillDate"]);
                        dr.VolunteerPhoneNumber = dataReader["VolunteerPhoneNumber"].ToString();
                        dr.Note = dataReader["Note"].ToString();
                    }


                    RoutineException re = new()
                    {
                        IsHandled = Convert.ToBoolean(dataReader["completed"]),
                        IsOk = Convert.ToBoolean(dataReader["status"]),
                        ItemId = Convert.ToInt32(dataReader["itemId"]),
                        RoutineId = dr.RoutineId
                    };
                    dr.DogExceptions.Add(re);

                    
                }

                return dr;
;

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
        public List<FullRoutineException> ReadExceptionsByDog(int dogNumberId)
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

            DailyRoutine dr = new() { DogExceptions = new(), DogNumberId = dogNumberId };

            cmd = DailyRoutineSPCmd(spIUD, con, dr, "SelectByDog");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return ReadFullExceptionsFromReader(dataReader);

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

        public List<FullRoutineException> ReadExceptionsByRoutine(DailyRoutine routine)
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


            cmd = DailyRoutineSPCmd(spIUD, con, routine, "SelectByRoutineId");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return ReadFullExceptionsFromReader(dataReader);

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


        public DataTable ConvertToTable(List<RoutineException> list)
        {
            DataTable table = new();

            table.Columns.Add(new DataColumn("routineItem", typeof(string)));
            table.Columns.Add(new DataColumn("routineId", typeof(int)));
            table.Columns.Add(new DataColumn("isOk", typeof(bool)));
            table.Columns.Add(new DataColumn("isHandled", typeof(bool)));

            foreach (RoutineException item in list)
            {

                DataRow nr = table.NewRow();
                nr["routineItem"] = item.ItemId;
                nr["routineId"] = item.RoutineId;
                nr["isOk"] = item.IsOk;
                nr["isHandled"] = item.IsHandled;

                table.Rows.Add(nr);
            }
            return table;
        }

        public List<FullRoutineException> ReadFullExceptionsFromReader(SqlDataReader dataReader)
        {
            List<FullRoutineException> fullRoutineExceptions = new();
            while (dataReader.Read())
            {

                FullRoutineException fr = new()
                {

                    RoutineId = Convert.ToInt32(dataReader["routineId"]),
                    ItemId = Convert.ToInt32(dataReader["itemId"]),
                    RoutineItem = dataReader["item"].ToString(),
                    IsOk = Convert.ToBoolean(dataReader["status"]),
                    IsHandled = Convert.ToBoolean(dataReader["completed"]),
                    DogId = Convert.ToInt32(dataReader["dogNumberId"]),
                    FillDate = Convert.ToDateTime(dataReader["fillDate"]),
                    VolunteerName = dataReader["vFullName"].ToString(),
                    DogName = dataReader["Name"].ToString(),


                };

                fullRoutineExceptions.Add(fr);
            }

            return fullRoutineExceptions;
        }

    }
}
