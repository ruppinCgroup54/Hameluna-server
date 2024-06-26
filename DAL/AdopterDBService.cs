using hameluna_server.BL;
using System.Data.SqlClient;
using System.Data;

namespace hameluna_server.DAL
{
    public class AdopterDBService : DBservices
    {
        public AdopterDBService() : base()
        {
            spIUD = "AdoptersTableIUD";
        }

        //create command for general SP CRUD
        private SqlCommand AdopterSPCmd(String spName, SqlConnection con, Adopter adopter, string action)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@StatementType", action);

            if (adopter != null)
            {
                cmd.Parameters.AddWithValue("@PhoneNumber", adopter.PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", adopter.Email);
                cmd.Parameters.AddWithValue("@FirstName", adopter.FirstName);
                cmd.Parameters.AddWithValue("@LastName", adopter.LastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", adopter.DateOfBirth);
                cmd.Parameters.AddWithValue("@HouseMembers", adopter.HouseMembers);
                cmd.Parameters.AddWithValue("@DogsPlace", adopter.DogsPlace);
                cmd.Parameters.AddWithValue("@AdditionalPets", adopter.AdditionalPets);
                cmd.Parameters.AddWithValue("@Experience", adopter.Experience);
                cmd.Parameters.AddWithValue("@Note", adopter.Note);
                cmd.Parameters.AddWithValue("@Addressid", adopter.Address.Id);
            }


            return cmd;
        }

        public string InsertAdopter(Adopter adopter)
        {

            SqlConnection con;
            SqlCommand cmd, addressCmd;

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            //insert new address
            addressCmd = AddressSPCmd("UserAddressTableIUD", con, adopter.Address, "Insert");

            try
            {

                int insertedAddress = Convert.ToInt32(addressCmd.ExecuteScalar());
                adopter.Address.Id = insertedAddress;
            }
            catch (Exception ex)
            {

                adopter.Address= new();
            }

            cmd = AdopterSPCmd(spIUD, con, adopter, "Insert");             // create the command

            try
            {
                string newAdopterId = (string)cmd.ExecuteScalar(); // execute the command
                return newAdopterId;
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

        public int UpdateAdopter(Adopter adopter)
        {

            SqlConnection con;
            SqlCommand cmd, addressCmd;

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            //insert new address
            addressCmd = AddressSPCmd("UserAddressTableIUD", con, adopter.Address, "Update");

            try
            {
                addressCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw new InvalidExpressionException("Invalide Address");
            }
            cmd = AdopterSPCmd(spIUD, con, adopter, "Update");             // create the command

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

        public int DeleteAdopter(string id)
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

            cmd = AdopterSPCmd(spIUD, con, new() { PhoneNumber = id }, "delete");             // create the command

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

        public List<Adopter> ReadAdopters()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<Adopter> AdList = new();

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = AdopterSPCmd(spIUD, con, new(), "Select");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Adopter a = new()
                    {
                        Email = dataReader["Email"].ToString(),
                        PhoneNumber = dataReader["PhoneNumber"].ToString(),
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString(),
                        DateOfBirth = DateTime.Parse(dataReader["DateOfBirth"].ToString()),
                        HouseMembers = dataReader["HouseMembers"].ToString(),
                        DogsPlace = dataReader["DogsPlace"].ToString(),
                        AdditionalPets = dataReader["AdditionalPets"].ToString(),
                        Experience = dataReader["Experience"].ToString(),
                        Note = dataReader["Note"].ToString(),
                        Address = new()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            HouseNumber = Convert.ToInt32(dataReader["HouseNumber"]),
                            City = dataReader["CityName"].ToString(),
                            StreetName = dataReader["StreetName"].ToString(),
                            Region = dataReader["Region"].ToString()
                        }


                    };
                    AdList.Add(a);
                }
                return AdList;
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

        public Adopter ReadAdopter(string id)
        {

            SqlConnection con;
            SqlCommand cmd;
            Adopter ad = new();

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = AdopterSPCmd(spIUD, con, new() { PhoneNumber = id }, "SelectOne");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    ad = new()
                    {
                        Email = dataReader["Email"].ToString(),
                        PhoneNumber = dataReader["PhoneNumber"].ToString(),
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString(),
                        DateOfBirth = DateTime.Parse(dataReader["DateOfBirth"].ToString()),
                        HouseMembers = dataReader["HouseMembers"].ToString(),
                        DogsPlace = dataReader["DogsPlace"].ToString(),
                        AdditionalPets = dataReader["AdditionalPets"].ToString(),
                        Experience = dataReader["Experience"].ToString(),
                        Note = dataReader["Note"].ToString(),
                        Address = new()
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            HouseNumber = Convert.ToInt32(dataReader["HouseNumber"]),
                            City = dataReader["CityName"].ToString(),
                            StreetName = dataReader["StreetName"].ToString(),
                            Region = dataReader["Region"].ToString()
                        }
                    };
                }
                return ad;
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
