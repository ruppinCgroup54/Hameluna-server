using hameluna_server.BL;
using System.Data;
using System.Data.SqlClient;

namespace hameluna_server.DAL
{
    public class VolunteerDBService : DBservices
    {
        public VolunteerDBService() : base()
        {
            string spIUD = "VolunteerTableIUD";
        }


        //create command for general SP CRUD
        private SqlCommand VolunteerSPCmd(String spName, SqlConnection con, Volunteer volunteer, string action)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@StatementType", action);

            if (volunteer != null)
            {
                cmd.Parameters.AddWithValue("@PhoneNumber", volunteer.PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", volunteer.Email);
                cmd.Parameters.AddWithValue("@FirstName", volunteer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", volunteer.LastName);
            }


            return cmd;
        }

        public string InsertVoluteer(Volunteer volunteer)
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

            cmd = VolunteerSPCmd(spIUD, con, volunteer, "Insert");             // create the command

            try
            {
                string newVoluteerId = (string)cmd.ExecuteScalar(); // execute the command
                return newVoluteerId;
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

        public int UpdateVoluteer(Volunteer volunteer)
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

            cmd = VolunteerSPCmd(spIUD, con, volunteer, "Update");             // create the command

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
        
        public int DeleteVolunteer(string id)
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

            cmd = VolunteerSPCmd(spIUD, con, new() { PhoneNumber=id}, "delete");             // create the command

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

        public List<Volunteer> ReadVolunteer()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<Volunteer> VolList = new();

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = VolunteerSPCmd(spIUD, con, new(), "Select");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Volunteer v = new()
                    {
                        Email = dataReader["Email"].ToString(),
                        PhoneNumber = dataReader["PhoneNumber"].ToString(),
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString()
                    };
                    VolList.Add(v);
                }
                return VolList;
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

        public Volunteer ReadVolunteer(string id)
        {

            SqlConnection con;
            SqlCommand cmd;
            Volunteer Vol = new() ;

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = VolunteerSPCmd(spIUD, con, new() { PhoneNumber=id}, "SelectOne");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Vol = new()
                    {
                        Email = dataReader["Email"].ToString(),
                        PhoneNumber = dataReader["PhoneNumber"].ToString(),
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString()
                    };
                }
                return Vol;
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
