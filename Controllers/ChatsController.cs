using System.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        // GET: api/<ChatsController>
        [HttpGet]
        public async Task<string> Get()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("MongoDBHameluna");

            var settings = MongoClientSettings.FromConnectionString(cStr);
            // Set the ServerApi field of the settings object to set the version of the Stable API on the client
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            // Create a new client and connect to the server
            var client = new MongoClient(settings);

            try
            {

                var dbList = client.ListDatabases().ToList();
                var database = client.GetDatabase("ChatApp");
                var collection = database.GetCollection<BsonDocument>("users");

                //var currentUser = new BsonDocument {
                //    { "student_id", 10000 }, 
                //    {"scores", new BsonArray {
                //            new BsonDocument { { "type", "exam" }, { "score", 88.12334193287023 } },
                //            new BsonDocument { { "type", "quiz" }, { "score", 74.92381029342834 } },
                //            new BsonDocument { { "type", "homework" }, { "score", 89.97929384290324 } },
                //            new BsonDocument { { "type", "homework" }, { "score", 82.12931030513218 } }
                //            }
                //        }, 
                //    { "class_id", 480 }
                //};


                var currentUser = new BsonDocument
                {
                    {"userID","109.303.20290.2" },
                    {"message",new BsonArray
                    {
                        new BsonDocument{
                            {"role","user"},
                            {"content","Hiii" }
                        }
                    }
                    }
                };

                await collection.InsertOneAsync(currentUser);

                return "done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        // GET api/<ChatsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ChatsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ChatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
