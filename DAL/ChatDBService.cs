using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Driver;
using OpenAI_API.Chat;

namespace hameluna_server.DAL
{
    public class ChatDBService
    {

         public string Prompt { get; set; }

        public ChatDBService()
        {
           

          
        }

        IMongoCollection<BsonDocument> GetCollection()
        {
            // get mongoDB connection string and get database object
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("MongoDBHameluna");
            var settings = MongoClientSettings.FromConnectionString(cStr);
            // Set the ServerApi field of the settings object to set the version of the Stable API on the client
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            // Create a new client and connect to the server
            var client = new MongoClient(settings);
            var database = client.GetDatabase("ChatApp");
            return database.GetCollection<BsonDocument>("users");
        }
        

         public string CreateDocument()
        {
            var collection = GetCollection();
            
            Prompt = "You are A helpfull assitant";
            BsonArray messages = new BsonArray
            {
                new BsonDocument{
                    { "role","System" },
                    { "content",this.Prompt }
                }, 
                new BsonDocument{
                    { "role","Assistant" },
                    { "content","how can i help you today" }
                }
            };

            

            var document = new BsonDocument
            {
                {"messages", messages}
            };

            collection.InsertOne(document);

            return document["_id"].ToString();
         }





    }
}
