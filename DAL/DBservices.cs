﻿using System;
using System.Data;
using System.Data.SqlClient;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using System.Security.Cryptography;
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
            cmd.Parameters.AddWithValue("@CityName", address.City);
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

    public int UpdateAddress(Address address)
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

        cmd = AddressSPCmd("UserAddressTableIUD", con, address, "Update");             // create the command

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

    //-------------------------------------------------------- GET  LISTS ---------------------------------------------------------------

    public SqlCommand GetListSPCmd(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;
    }

    public SqlCommand CitiesSPCmd(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text


        return cmd;
    }

    public List<string> GetAllCities()
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

        cmd = GetListSPCmd("CityTableIUD", con);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<string> cities = new();

            while (dataReader.Read())
            {
                string c = dataReader["CityName"].ToString();
                cities.Add(c);
            }
            return cities;
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

    public List<string> GetAllBreeds()
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

        cmd = GetListSPCmd("BreedTableIUD_", con);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<string> breeds = new();

            while (dataReader.Read())
            {
                string a = dataReader["Breed"].ToString();
                breeds.Add(a);
            }
            return breeds;
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

    public List<string> GetAllColors()
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

        cmd = GetListSPCmd("ColorTableIUD", con);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<string> colors = new();

            while (dataReader.Read())
            {
                string a = dataReader["ColorName"].ToString();
                colors.Add(a);
            }
            return colors;
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

    public List<string> GetAllCharacteristics()
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

        cmd = GetListSPCmd("CharacteristicsTableIUD", con);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<string> characteristics = new();

            while (dataReader.Read())
            {
                string c = dataReader["attribute"].ToString();
                characteristics.Add(c);
            }
            return characteristics;
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

    private SqlCommand DailySPCmd(String spName, SqlConnection con, string action, string item = "")
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@StatementType", action);

        cmd.Parameters.AddWithValue("@Item", item);

        return cmd;
    }

    public List<string> GetAllDailyRoutins()
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

        cmd = DailySPCmd("DailyItemIUD", con, "Select");             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<string> dailyRoutins = new();

            while (dataReader.Read())
            {
                string c = dataReader["Item"].ToString();
                dailyRoutins.Add(c);
            }
            return dailyRoutins;
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

    private SqlCommand RoutinShelterSPCmd(String spName, SqlConnection con, string action, int shelter, string item = "")
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@StatementType", action);

        cmd.Parameters.AddWithValue("@shelterNumber", shelter);

        cmd.Parameters.AddWithValue("@item", item);

        return cmd;
    }

    public int InsertDailyToShelter(int shelter, string item)
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

        cmd = RoutinShelterSPCmd("RoutineShelterIUD", con, "Insert", shelter, item);             // create the command

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

    public List<RoutinItem> GetDailyFromShelter(int shelter)
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

        cmd = RoutinShelterSPCmd("RoutineShelterIUD", con, "Select", shelter);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<RoutinItem> items = new();
            while (dataReader.Read())
            {
                string a = dataReader["Item"].ToString();
                int b = Convert.ToInt32(dataReader["ItemID"]);

                items.Add(new(b,a));
            }
            return items;
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

    //------------------------------------------------------ Files & Images ----------------------------------------------------------------


    public List<string> GetDogImages(int id)
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

        cmd = FilesSPCmd("DogsFiles", con, "GetImages", "", id);          // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<string> characteristics = new();

            while (dataReader.Read())
            {
                string c = dataReader["Path"].ToString();
                characteristics.Add(c);
            }
            return characteristics;
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

    public List<string> GetDogFiles(int id)
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

        cmd = FilesSPCmd("DogsFiles", con, "GetFiles", "", id);          // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<string> files = new();

            while (dataReader.Read())
            {
                string c = dataReader["Url"].ToString();
                files.Add(c);
            }
            return files;
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

    public List<string> GetSheltersImages()
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

        cmd = FilesSPCmd("DogsFiles", con, "GetShelters", "", -1);          // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<string> images = new();

            while (dataReader.Read())
            {
                string c = dataReader["PhotoUrl"].ToString();
                images.Add(c);
            }
            return images;
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

    public async Task<List<string>> InsertDogImages(string shelterId, List<IFormFile> images)
    {
        List<string> imageLinks = new();

        string path = System.IO.Directory.GetCurrentDirectory();


        //check for shelters diractory if noe exists create new one with shleter id
        string shelterDir = Path.Combine(path, "uploadedImages/" + shelterId);
        if (!Directory.Exists(shelterDir))
        {
            Directory.CreateDirectory(shelterDir);
        }

        long size = images.Sum(i => i.Length);

        foreach (var formImage in images)
        {
            if (formImage.Length > 0)
            {
                var imagePath = Path.Combine(shelterDir, formImage.FileName + DateTime.Now.ToString());

                using (var stream = System.IO.File.Create(imagePath))
                {
                    await formImage.CopyToAsync(stream);
                }
                imageLinks.Add(formImage.FileName);
            }
        }

        return imageLinks;
    }

    public async Task<string> InsertProfileImage(string shelterId, int dogId, IFormFile image)
    {
        string imageLink = "";

        string path = System.IO.Directory.GetCurrentDirectory();


        //check for shelters diractory if noe exists create new one with shleter id
        string shelterDir = Path.Combine(path, "uploadedImages/" + shelterId);
        if (!Directory.Exists(shelterDir))
        {
            Directory.CreateDirectory(shelterDir);
        }

        long size = image.Length;

        if (image.Length > 0)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + image.FileName;
            var imagePath = Path.Combine(shelterDir, fileName);

            using (var stream = System.IO.File.Create(imagePath))
            {
                await image.CopyToAsync(stream);
            }
            imageLink = $"Images/{shelterId}/{fileName}";
        }

        return imageLink;
    }

    public async Task<string> InsertFile(string shelterId, int dogId, IFormFile file)
    {
        string imageLink = "";

        string path = System.IO.Directory.GetCurrentDirectory();


        //check for shelters diractory if noe exists create new one with shleter id
        string shelterDir = Path.Combine(path, "uploadedFiles/" + shelterId);
        if (!Directory.Exists(shelterDir))
        {
            Directory.CreateDirectory(shelterDir);
        }

        long size = file.Length;

        if (file.Length > 0)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + file.FileName;
            var filePath = Path.Combine(shelterDir, fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            imageLink = $"Files/{shelterId}/{fileName}";
        }

        return imageLink;
    }

    public async Task<string> InsertShelterImage(IFormFile image)
    {
        string imageLink = "";

        string path = System.IO.Directory.GetCurrentDirectory();


        //check for shelters diractory if noe exists create new one with shleter id
        string shelterDir = Path.Combine(path, "uploadedImages/shelters");
        if (!Directory.Exists(shelterDir))
        {
            Directory.CreateDirectory(shelterDir);
        }

        long size = image.Length;

        if (image.Length > 0)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + image.FileName;
            var imagePath = Path.Combine(shelterDir, fileName);

            using (var stream = System.IO.File.Create(imagePath))
            {
                await image.CopyToAsync(stream);
            }
            imageLink = $"Images/shelters/{fileName}";
        }

        return imageLink;
    }

    public int DeleteFile( string Url, bool isFile)
    {
        string imageLink = "";
        string selectAction, selectedProc;


        string path = Directory.GetCurrentDirectory();

        if (isFile)
        {
            path += Url.Replace("Files", "/uploadedFiles");
            selectAction = "DeleteFile";

        }
        else
        {
            path += Url.Replace("Images", "/uploadedImages");
            selectAction = "DeleteImage";

        }


        File.Delete(path);

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
        cmd = FilesSPCmd("DogsFiles", con, selectAction, Url, -1);

        try
        {
            return cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {

            throw new Exception("profile image is fail to uploaded.");
        }

    }

    public SqlCommand FilesSPCmd(String spName, SqlConnection con, string action, string url, int dogId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@StatementType", action);

        if (url != null)
        {
            cmd.Parameters.AddWithValue("@Url", url);
            cmd.Parameters.AddWithValue("@DogId", dogId);
        }

        return cmd;
    }

    public SqlCommand FilesArraySPCmd(String spName, SqlConnection con, string action, List<string> urls, int dogId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@StatementType", action);

        if (urls != null)
        {
            cmd.Parameters.AddWithValue("@Url", urls);
            cmd.Parameters.AddWithValue("@ListUrl", ConvertToTable(urls));
            cmd.Parameters.AddWithValue("@DogId", dogId);
        }

        return cmd;
    }

    public int InsertProfile(string url, int dogId)
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
        cmd = FilesSPCmd("DogsFiles", con, "InsertProfile", url, dogId);

        try
        {
            return cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {

            throw new Exception("profile image is fail to uploaded.");
        }


    }

    public int InsertImagesToData(string url, int dogId)
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
        cmd = FilesSPCmd("DogsFiles", con, "InsertImages", url, dogId);

        try
        {
            return cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            DBservices.WriteToErrorLog(ex);
            throw new Exception("profile image is fail to uploaded.");
        }


    }

    public int InsertFileToData(string url, int dogId)
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
        cmd = FilesSPCmd("DogsFiles", con, "InsertFiles", url, dogId);

        try
        {
            return cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {

            throw new Exception("profile image is fail to uploaded.");
        }

    }

    //------------------------------------------------------ HELPERS -----------------------------------------------------------

    public static void WriteToErrorLog(Exception temp)
    {
        using (StreamWriter writer = new StreamWriter("LogErrors.txt", append: true)) // append: true to append to the file
        {
            do
            {
                writer.WriteLine(temp.Message);
                temp = temp.InnerException;
            } while (temp != null);
        }
    }

    public static void WriteToErrorLog(string temp)
    {
        using (StreamWriter writer = new StreamWriter("LogErrors.txt", append: true)) // append: true to append to the file
        {
            writer.WriteLine(temp);
        }
    }

    public DataTable ConvertToTable(List<string> list)
    {
        DataTable table = new();
        DataColumn dc = new("item", typeof(string));
        table.Columns.Add(dc);
        foreach (string item in list)
        {
            table.Rows.Add(item);
        }
        return table;
    }
}

