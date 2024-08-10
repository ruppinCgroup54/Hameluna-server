using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using hameluna_server.BL;
using Newtonsoft.Json;

namespace hameluna_server.DAL
{
    class Connection
    {
        //firebase connection Settings
        public IFirebaseConfig fc = new FirebaseConfig()
        {
            AuthSecret = "FLSYPyWf2fPorpsx7xYx7iaXY8t0XkNFlSGHnEhO",
            BasePath = "https://hameluna-e8140-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        public IFirebaseClient client;
        //Code to warn console if class cannot connect when called.
        public Connection()
        {
            try
            {
                client = new FireSharp.FirebaseClient(fc);
            }
            catch (Exception)
            {
                Console.WriteLine("sunucuya bağlanılamadı");
            }
        }
    }

    public class FireBaseDBService
    {
        Connection conn = new();

        //set Exceptions to database
        public int SetExceptions(int shelterNumber, FullRoutineException dr)
        {
            try { 

                var SetData = conn.client.Set(@$"exceptions/{shelterNumber}/{dr.Id}" , dr);
                return 1;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to Set");
                return 0;
            }

        }

        //Update Exception
        public int UpdateExceptions(int shelterNumber, FullRoutineException dr)
        {
            try
            {

                var SetData = conn.client.Update(@$"exceptions/{shelterNumber}/{dr.Id}", dr);

                return 1;
            }
            catch (Exception)
            {
                Console.WriteLine("Could not update exeption");
                return 0;
            }
        }

        //Delete Exception
        public void DeleteExceptions(int shelterNumber, FullRoutineException dr)
        {
            try
            {
                var SetData = conn.client.Delete(@$"exceptions/{shelterNumber}/{dr.Id}");
            }
            catch (Exception)
            {
                Console.WriteLine("Could not delete exception");
            }
        }

        //List of the Exception
        public Dictionary<string, FullRoutineException> ShelterExeptions(int shelterNumber)
        {
            try
            {
                FirebaseResponse al = conn.client.Get(@$"exceptions/{shelterNumber}");
                Dictionary<string, FullRoutineException> ListData = JsonConvert.DeserializeObject<Dictionary<string, FullRoutineException>>(al.Body.ToString());
                return ListData;
            }
            catch (Exception)
            {
                Console.WriteLine("bir hata ile karşılaşıldı");
                return null;
            }
        }



        //set AdoptionRequest to database
        public int SetAdoptionRequest(int shelterNumber, AdoptionRequest ad)
        {
            try
            {

                var SetData = conn.client.Set(@$"requests/{shelterNumber}/{ad.RequestId}", ad);
                return 1;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to Set");
                return 0;
            }

        }

        //Update AdoptionRequest
        public int UpdateAdoptionRequest(int shelterNumber, AdoptionRequest ad)
        {
            try
            {

                var SetData = conn.client.Set(@$"requests/{shelterNumber}/{ad.RequestId}", ad);


                return 1;
            }
            catch (Exception)
            {
                Console.WriteLine("Could not update AdoptionRequest");
                return 0;
            }
        }

        //Delete AdoptionRequest
        public void DeleteAdoptionRequest(int shelterNumber, AdoptionRequest ad)
        {
            try
            {
                var SetData = conn.client.Delete(@$"requests/{shelterNumber}/{ad.RequestId}");
            }
            catch (Exception)
            {
                Console.WriteLine("Could not delete AdoptionRequest");
            }
        }

        //List of the AdoptionRequest
        public Dictionary<string, AdoptionRequest> ShelterAdoptionRequests(int shelterNumber)
        {
            try
            {
                FirebaseResponse al = conn.client.Get(@$"requests/{shelterNumber}");
                Dictionary<string, AdoptionRequest> ListData = JsonConvert.DeserializeObject<Dictionary<string, AdoptionRequest>>(al.Body.ToString());
                return ListData;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
