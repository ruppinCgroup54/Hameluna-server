using System;
using System.Data;
using System.Data.SqlClient;
using hameluna_server.BL;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public string conString { get; set; }
    public string spIUD { get; set; }


    public DBservices()
    {
        conString = "hameluna_test1";
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString(conString);
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    public SqlCommand AddressSPCmd(String spName, SqlConnection con, Address address, string action)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@StatementType", action);

        if (address != null)
        {
            cmd.Parameters.AddWithValue("@Id", address.Id);
            cmd.Parameters.AddWithValue("@City", address.City);
            cmd.Parameters.AddWithValue("@StreetName", address.StreetName);
            cmd.Parameters.AddWithValue("@HouseNumber", address.HouseNumber);
            cmd.Parameters.AddWithValue("@Region", address.Region);

        }


        return cmd;
    }

    public int InsertAddress(Address address)
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

        //insert new address
        cmd = AddressSPCmd("UserAddressTableIUD", con, address, "Insert");

        try
        {
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex)
        {

            throw new InvalidExpressionException("Invalide Address");
        }

      
    }



}
