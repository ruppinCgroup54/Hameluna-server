using hameluna_server.BL;
using System.Data.SqlClient;
using System.Data;

namespace hameluna_server.DAL
{
    public class AdminDBService : DBservices
    {
        public AdminDBService() : base()
        {
             spIUD = "AdminTableIUD";
        }

        //create command for general SP CRUD
        private SqlCommand AdminSPCmd(String spName, SqlConnection con, Admin admin, string action)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@StatementType", action);

            if (admin != null)
            {
                cmd.Parameters.AddWithValue("@PhoneNumber", admin.PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", admin.Email);
                cmd.Parameters.AddWithValue("@FirstName", admin.FirstName);
                cmd.Parameters.AddWithValue("@LastName", admin.LastName);
                cmd.Parameters.AddWithValue("@UserName", admin.UserName);
                cmd.Parameters.AddWithValue("@Password", admin.Password);
            }


            return cmd;
        }

        public string InsertAdmin(Admin admin)
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

            cmd = AdminSPCmd(spIUD, con, admin, "Insert");             // create the command

            try
            {
                string newAdminId = (string)cmd.ExecuteScalar(); // execute the command
                return newAdminId;
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

        public int UpdateAdmin(Admin admin)
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

            cmd = AdminSPCmd(spIUD, con, admin, "Update");             // create the command

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

        public int DeleteAdmin(string id)
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

            cmd = AdminSPCmd(spIUD, con, new(){PhoneNumber=id}, "delete");             // create the command

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

        public List<Admin> ReadAdmin()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<Admin> AdList = new();

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = AdminSPCmd(spIUD, con, new(), "Select");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Admin a = new()
                    {
                        Email = dataReader["Email"].ToString(),
                        PhoneNumber = dataReader["PhoneNumber"].ToString(),
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString(),
                        UserName = dataReader["UserName"].ToString(),
                        Password = dataReader["Password"].ToString()
                        
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

        public Admin ReadAdmin(string id)
        {

            SqlConnection con;
            SqlCommand cmd;
            Admin ad = new() { PhoneNumber = id };

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = AdminSPCmd(spIUD, con, ad, "SelectOne");             // create the command

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
                        UserName = dataReader["UserName"].ToString(),
                        Password = dataReader["Password"].ToString()
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
