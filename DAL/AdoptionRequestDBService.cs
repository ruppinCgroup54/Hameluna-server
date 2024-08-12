using hameluna_server.BL;
using System.Data.SqlClient;
using System.Data;
using System.Xml;

namespace hameluna_server.DAL
{
    public class AdoptionRequestDBService : DBservices
    {


        public AdoptionRequestDBService() : base()
        {
            spIUD = "AdoptionRequestTableIUD";
        }

        //create command for general SP CRUD
        private SqlCommand AdoptionRequestSPCmd(String spName, SqlConnection con, AdoptionRequest adoptionRequest, string action)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@StatementType", action);
            cmd.Parameters.AddWithValue("@RequestID", adoptionRequest.RequestId);
            cmd.Parameters.AddWithValue("@SentDate", adoptionRequest.SendDate);
            cmd.Parameters.AddWithValue("@DogNumberId", adoptionRequest.Dog.NumberId);
            cmd.Parameters.AddWithValue("@Optional_adopterPhoneNumber", adoptionRequest.Adopter.PhoneNumber);
            cmd.Parameters.AddWithValue("@Status", adoptionRequest.Status);


            return cmd;
        }

        public int InsertAdoptionRequest(AdoptionRequest adoptionRequest)
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


            cmd = AdoptionRequestSPCmd(spIUD, con, adoptionRequest, "Insert");             // create the command

            try
            {
                int newAdoptionRequestId = Convert.ToInt32(cmd.ExecuteScalar()); // execute the command
                return newAdoptionRequestId;
            }
            catch (Exception ex)
            {


                if (ex.Message.StartsWith("Violation of UNIQUE KEY constraint 'UC_adoption'"))
                {
                    UpdateAdoptionRequest(adoptionRequest);
                    throw new ConstraintException();
                }
                // write to log
                throw (ex);

            }

            finally
            {
                // close the db connection
                con?.Close();
            }

        }

        public int UpdateAdoptionRequest(AdoptionRequest adoptionRequest)
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

            cmd = AdoptionRequestSPCmd(spIUD, con, adoptionRequest, "Update");             // create the command

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

        public int DeleteAdoptionRequest(int id)
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

            cmd = AdoptionRequestSPCmd(spIUD, con, new() { RequestId = id }, "delete");             // create the command

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

        public AdoptionRequest ReadAdoptionRequest(string phoneNumber, int dogId)
        {

            SqlConnection con;
            SqlCommand cmd;
            List<AdoptionRequest> adoptionReqList = new();

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = AdoptionRequestSPCmd(spIUD, con, new() { Adopter = new() { PhoneNumber = phoneNumber }, Dog = new() { NumberId = dogId } }, "SelectByDogAndAdopter");             // create the command
            AdoptionRequest ar = new();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {

                    ar.RequestId = Convert.ToInt32(dataReader["RequestID"]);
                    ar.Status = dataReader["Status"].ToString();
                    ar.SendDate = DateTime.Parse(dataReader["SentDate"].ToString());
                    ar.Adopter = Adopter.ReadOne(dataReader["Optional_adopterPhoneNumber"].ToString());
                    ar.Dog = Dog.ReadOne(Convert.ToInt32(dataReader["DogNumberId"]));
                    adoptionReqList.Add(ar);
                }
                return ar;
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
        public List<AdoptionRequest> ReadAdoptionRequest()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<AdoptionRequest> adoptionReqList = new();

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = AdoptionRequestSPCmd(spIUD, con, new(), "Select");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {

                    AdoptionRequest ar = new()
                    {
                        RequestId = Convert.ToInt32(dataReader["RequestID"]),
                        Status = dataReader["Status"].ToString(),
                        SendDate = DateTime.Parse(dataReader["SentDate"].ToString()),

                    };
                    ar.Adopter = Adopter.ReadOne(dataReader["Optional_adopterPhoneNumber"].ToString());
                    ar.Dog = Dog.ReadOne(Convert.ToInt32(dataReader["DogNumberId"]));
                    adoptionReqList.Add(ar);
                }
                return adoptionReqList;
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
