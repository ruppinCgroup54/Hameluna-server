using hameluna_server.BL;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Xml.Linq;

namespace hameluna_server.DAL
{
    public class ShelterDBService : DBservices
    {

        public ShelterDBService() : base()
        {
            spIUD = "ShelterTableIUD";
        }

        //create command for general SP CRUD
        private SqlCommand ShelterSPCmd(String spName, SqlConnection con, Shelter shelter, string action)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@StatementType", action);

            if (shelter != null)
            {
                cmd.Parameters.AddWithValue("@ShelterNumber", shelter.ShelterId);
                cmd.Parameters.AddWithValue("@AdminPhoneNumber", shelter.AdminDetails.PhoneNumber);
                cmd.Parameters.AddWithValue("@FacebookUserName", shelter.FacebookUserName);
                cmd.Parameters.AddWithValue("@FacebookPassword", shelter.FacebookPassword);
                cmd.Parameters.AddWithValue("@InstagramUserName", shelter.InstagramUserName);
                cmd.Parameters.AddWithValue("@InstagramPassword", shelter.InstagramPassword);
                cmd.Parameters.AddWithValue("@TimeToReport", shelter.TimeToReport);
                cmd.Parameters.AddWithValue("@Name", shelter.Name);
                cmd.Parameters.AddWithValue("@PhotoUrl", shelter.PhotoUrl);
                cmd.Parameters.AddWithValue("@AddressId", shelter.Address.Id);
            }


            return cmd;
        }

        public int InsertShelter(Shelter shelter)
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
                shelter.AdminDetails.Insert();
            }
            catch (Exception)
            {
                Console.WriteLine("Admin exists");
            }
            try
            {
                shelter.Address.Insert();
            }
            catch (Exception)
            {
                Console.WriteLine("Address exists");
            }

            cmd = ShelterSPCmd(spIUD, con, shelter, "Insert");             // create the command

            try
            {
                int newShelterId = Convert.ToInt32(cmd.ExecuteScalar()); // execute the command
                return newShelterId;
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

        public int UpdateShelter(Shelter shelter)
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

            cmd = ShelterSPCmd(spIUD, con, shelter, "Update");             // create the command

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

        public int DeleteShelter(int id)
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

            cmd = ShelterSPCmd(spIUD, con, new() { ShelterId = id }, "delete");             // create the command

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

        public List<Shelter> ReadShelter()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<Shelter> ShelList = new();

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = ShelterSPCmd(spIUD, con, new(), "Select");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Admin adminOfShelter = Admin.ReadOne(dataReader["AdminPhoneNumber"].ToString());

                    Shelter s = new()
                    {
                        ShelterId = Convert.ToInt32(dataReader["ShelterNumber"]),
                        AdminDetails = adminOfShelter,
                        FacebookUserName = dataReader["FacebookUserName"].ToString(),
                        FacebookPassword = dataReader["FacebookPassword"].ToString(),
                        InstagramUserName = dataReader["InstegramUserName"].ToString(),
                        InstagramPassword = dataReader["InstegramPassword"].ToString(),
                        TimeToReport = Convert.ToDateTime(dataReader["TimeToReport"].ToString()),
                        Name = dataReader["Name"].ToString(),
                        PhotoUrl = dataReader["PhotoUrl"].ToString(),
                        Address = new()
                        {
                            Id = Convert.ToInt32(dataReader["AddressId"]),
                            HouseNumber = Convert.ToInt32(dataReader["HouseNumber"]),
                            City = dataReader["City"].ToString(),
                            StreetName = dataReader["StreetName"].ToString(),
                            Region = dataReader["Region"].ToString()

                        },
                    };
                    ShelList.Add(s);
                }
                return ShelList;
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

        public Shelter ReadShelter(int id)
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

            cmd = ShelterSPCmd(spIUD, con, new() { ShelterId = id }, "SelectOne");             // create the command
            Shelter s = new();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Admin adminOfShelter = Admin.ReadOne(dataReader["AdminPhoneNumber"].ToString());

                    s = new()
                    {
                        ShelterId = Convert.ToInt32(dataReader["ShelterNumber"]),
                        AdminDetails = adminOfShelter,
                        FacebookUserName = dataReader["FacebookUserName"].ToString(),
                        FacebookPassword = dataReader["FacebookPassword"].ToString(),
                        InstagramUserName = dataReader["InstegramUserName"].ToString(),
                        InstagramPassword = dataReader["InstegramPassword"].ToString(),
                        TimeToReport = Convert.ToDateTime(dataReader["TimeToReport"].ToString()),
                        Name = dataReader["Name"].ToString(),
                        PhotoUrl = dataReader["PhotoUrl"].ToString(),
                        Address = new()
                        {
                            Id = Convert.ToInt32(dataReader["AddressId"]),
                            HouseNumber = Convert.ToInt32(dataReader["HouseNumber"]),
                            City = dataReader["City"].ToString(),
                            StreetName = dataReader["StreetName"].ToString(),
                            Region = dataReader["Region"].ToString()

                        }
                    };
                    }

                return s;
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
    }
}
