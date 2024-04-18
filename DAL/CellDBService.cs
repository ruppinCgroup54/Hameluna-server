using hameluna_server.BL;
using System.Data.SqlClient;
using System.Data;

namespace hameluna_server.DAL
{
    public class CellDBService:DBservices
    {
        public CellDBService() : base()
        {
            spIUD = "CellTableIUD";
        }

        //create command for general SP CRUD
        private SqlCommand CellSPCmd(String spName, SqlConnection con, Cell cell, string action)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@StatementType", action);

            if (cell != null)
            {
                cmd.Parameters.AddWithValue("@Number", cell.Number);
                cmd.Parameters.AddWithValue("@capacity", cell.Capacity);
                cmd.Parameters.AddWithValue("@ShelterNumber", cell.ShelterNumber);
                cmd.Parameters.AddWithValue("@id", cell.Id);
            }


            return cmd;
        }

        public int InsertCell(Cell cell)
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

            cmd = CellSPCmd(spIUD, con, cell, "Insert");             // create the command

            try
            {
                int newCellId = Convert.ToInt32(cmd.ExecuteScalar()); // execute the command
                return newCellId;
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

        public int UpdateCell(Cell cell)
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

            cmd = CellSPCmd(spIUD, con, cell, "Update");             // create the command

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

        public int DeleteCell(int id)
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

            cmd = CellSPCmd(spIUD, con, new() { Id = id }, "delete");             // create the command

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

        public List<Cell> ReadCell()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<Cell> cList = new();

            try
            {
                con = connect(conString); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CellSPCmd(spIUD, con, new(), "Select");             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {

                    Cell c = new()
                    {
                        Number = Convert.ToInt32(dataReader["Number"]),
                        Capacity = Convert.ToInt32(dataReader["Capacity"]),
                        Id = Convert.ToInt32(dataReader["Id"]),
                        ShelterNumber = Convert.ToInt32(dataReader["ShelterNumber"])
                    };
                    cList.Add(c);

                };
                return cList;
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

        //public List<Cell> ReadCell(int i)
        //{

        //    SqlConnection con;
        //    SqlCommand cmd;
        //    List<Cell> cList = new();

        //    try
        //    {
        //        con = connect(conString); // create the connection
        //    }
        //    catch (Exception ex)
        //    {
        //        write to log
        //        throw (ex);
        //    }

        //    cmd = CellSPCmd(spIUD, con, new(), "Select");             // create the command

        //    try
        //    {
        //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        //        while (dataReader.Read())
        //        {

        //            Cell c = new()
        //            {
        //                Number = Convert.ToInt32(dataReader["Number"]),
        //                Capacity = Convert.ToInt32(dataReader["Capacity"]),
        //                Id = Convert.ToInt32(dataReader["Id"]),
        //                ShelterNumber = Convert.ToInt32(dataReader["ShelterNumber"])
        //            };
        //            cList.Add(c);

        //        };
        //        return cList;
        //    }
        //    catch (Exception ex)
        //    {
        //        write to log
        //        throw (ex);
        //    }

        //    finally
        //    {
        //        con?.Close();
        //    }

        //}

        //public Cell ReadCell(int id)
        //{

        //    SqlConnection con;
        //    SqlCommand cmd;

        //    try
        //    {
        //        con = connect(conString); // create the connection
        //    }
        //    catch (Exception ex)
        //    {
        //        write to log
        //        throw (ex);
        //    }

        //    cmd = CellSPCmd(spIUD, con, new() { Id = id }, "SelectOne");             // create the command
        //    Cell c = new();

        //    try
        //    {
        //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        //        while (dataReader.Read())
        //        {
        //            c = new()
        //            {
        //                Number = Convert.ToInt32(dataReader["Number"]),
        //                Capacity = Convert.ToInt32(dataReader["Capacity"]),
        //                Id = Convert.ToInt32(dataReader["Id"]),
        //                ShelterNumber = Convert.ToInt32(dataReader["ShelterNumber"])
        //            };
        //        }

        //        return c;
        //    }
        //    catch (Exception ex)
        //    {
        //        write to log
        //        throw (ex);
        //    }

        //    finally
        //    {
        //        con?.Close();
        //    }

        //}

    }
}
