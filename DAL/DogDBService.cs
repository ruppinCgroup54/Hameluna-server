using hameluna_server.BL;
using System.Data.SqlClient;
using System.Data;

namespace hameluna_server.DAL
{
    public class DogDBService : DBservices
    {
        public DogDBService() : base()
        {
            spIUD = "DogTableIUD";
            spDogBreedIUD = "BreedOfDogTableIUD";
        }
        public string spDogBreedIUD { get; set; }

        //create command for general SP CRUD
        private SqlCommand DogSPCmd(String spName, SqlConnection con, Dog dog, string action)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@StatementType", action);

            if (dog != null)
            {
                cmd.Parameters.AddWithValue("@ChipNumber", dog.ChipNumber);
                cmd.Parameters.AddWithValue("@NumberId", dog.NumberId);
                cmd.Parameters.AddWithValue("@Name", dog.Name);
                cmd.Parameters.AddWithValue("@DateOfBirth", dog.DateOfBirth);
                cmd.Parameters.AddWithValue("@Gender", dog.Gender);
                cmd.Parameters.AddWithValue("@EntranceDate", dog.EntranceDate);
                cmd.Parameters.AddWithValue("@IsAdoptable", dog.IsAdoptable);
                cmd.Parameters.AddWithValue("@Size", dog.Size);
                cmd.Parameters.AddWithValue("@Adopted", dog.Adopted);
                cmd.Parameters.AddWithValue("@IsReturned", dog.IsReturned);
                cmd.Parameters.AddWithValue("@CellId", dog.CellId);
            }


            return cmd;
        }

        public int InsertDog(Dog dog)
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

            cmd = DogSPCmd(spIUD, con, dog, "Insert");             // create the command

            try
            {
                int newDogId = Convert.ToInt32(cmd.ExecuteScalar()); // execute the command
                return newDogId;
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

        public int UpdateDog(Dog dog)
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

            cmd = DogSPCmd(spIUD, con, dog, "Update");             // create the command

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

        public int DeleteDog(int id)
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

            cmd = DogSPCmd(spIUD, con, new() { NumberId = id }, "delete");             // create the command

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

        public List<Dog> ReadAll()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<Dog> dogsList = new();

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = DogSPCmd(spIUD, con, new(), "Select");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Dog a = new()
                    {
                        ChipNumber = dataReader["ChipNumber"].ToString(),
                        NumberId = Convert.ToInt32(dataReader["ChipNumber"]),
                        Name = dataReader["Name"].ToString(),
                        DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"]),
                        Gender = Convert.ToChar(dataReader["Gender"]),
                        EntranceDate = Convert.ToDateTime(dataReader["EntranceDate"]),
                        IsAdoptable = Convert.ToBoolean(dataReader["IsAdoptable"]),
                        Size = dataReader["Size"].ToString(),
                        Adopted = Convert.ToBoolean(dataReader["Adopted"]),
                        IsReturned = Convert.ToBoolean(dataReader["IsReturned"]),
                        CellId = Convert.ToInt32(dataReader["CellId"])
                    };

                    dogsList.Add(a);
                }
                return dogsList;
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

        public Dog ReadDog(int id)
        {

            SqlConnection con;
            SqlCommand cmd;
            Dog d = new() { NumberId = id };

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = DogSPCmd(spIUD, con, d, "SelectOne");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    d = new()
                    {
                        ChipNumber = dataReader["ChipNumber"].ToString(),
                        NumberId = Convert.ToInt32(dataReader["ChipNumber"]),
                        Name = dataReader["Name"].ToString(),
                        DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"]),
                        Gender = Convert.ToChar(dataReader["Gender"]),
                        EntranceDate = Convert.ToDateTime(dataReader["EntranceDate"]),
                        IsAdoptable = Convert.ToBoolean(dataReader["IsAdoptable"]),
                        Size = dataReader["Size"].ToString(),
                        Adopted = Convert.ToBoolean(dataReader["Adopted"]),
                        IsReturned = Convert.ToBoolean(dataReader["IsReturned"]),
                        CellId = Convert.ToInt32(dataReader["CellId"])
                    };
                }
                return d;
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

        private SqlCommand BreedDogSPCmd(String spName, SqlConnection con, int dogId, string dogBreed, string action)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@StatementType", action);

            cmd.Parameters.AddWithValue("@NumberId", dogId);
            cmd.Parameters.AddWithValue("@Breed", dogBreed);

            return cmd;
        }

        public void InsertBreedOfDog(Dog dog)
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


            try
            {
                for (int i = 0; i < dog.Breed.Count; i++)
                {
                    cmd = BreedDogSPCmd(spDogBreedIUD, con, dog.NumberId, dog.Breed[i], "Insert"); // create the command
                }
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
    }
}
