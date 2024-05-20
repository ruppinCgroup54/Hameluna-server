using System.Data;
using System.Text.Json.Nodes;
using hameluna_server.BL;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OpenAI_API.Chat;
using OpenAI_API;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        // GET: api/<ChatsController>
        //[HttpGet]
        //public async Task<string> Get()
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json").Build();
        //    string cStr = configuration.GetConnectionString("MongoDBHameluna");

        //    var settings = MongoClientSettings.FromConnectionString(cStr);
        //    // Set the ServerApi field of the settings object to set the version of the Stable API on the client
        //    settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        //    // Create a new client and connect to the server
        //    var client = new MongoClient(settings);

        //    try
        //    {

        //        var dbList = client.ListDatabases().ToList();
        //        var database = client.GetDatabase("ChatApp");
        //        var collection = database.GetCollection<BsonDocument>("users");

        //        //var currentUser = new BsonDocument {
        //        //    { "student_id", 10000 }, 
        //        //    {"scores", new BsonArray {
        //        //            new BsonDocument { { "type", "exam" }, { "score", 88.12334193287023 } },
        //        //            new BsonDocument { { "type", "quiz" }, { "score", 74.92381029342834 } },
        //        //            new BsonDocument { { "type", "homework" }, { "score", 89.97929384290324 } },
        //        //            new BsonDocument { { "type", "homework" }, { "score", 82.12931030513218 } }
        //        //            }
        //        //        }, 
        //        //    { "class_id", 480 }
        //        //};


        //        var currentUser = new BsonDocument
        //        {
        //            {"userID","109.303.20290.2" },
        //            {"message",new BsonArray
        //            {
        //                new BsonDocument{
        //                    {"role","user"},
        //                    {"content","Hiii" }
        //                }
        //            }
        //            }
        //        };

        //        await collection.InsertOneAsync(currentUser);

        //        return "done";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }


        //}


        [HttpGet]
        public ActionResult<string> GetNewID()
        {
            try
            {
                return Ok(new { id = Chat.CreateChat() });
            }
            catch (Exception)
            {

                return BadRequest("Sorry there was a problem");
            }

        }


        // GET api/<ChatsController>/5
        [HttpGet("{id}")]
        public IActionResult GetChat(string id)
        {
            Chat c = new();
            try
            {
                return Ok(new {chat= c.GetConversation(id) });
            }
            catch (NullReferenceException ne)
            {
                return NotFound(new { id = ne.Message });
            }
            catch (Exception)
            {
                return BadRequest("Could not retrive your conversation.");
            }
        }



        [HttpPost("{id}")]
        public IActionResult UseChatGpt(string id,[FromBody] JsonMessage js)
        {
            Chat c = new();

            JsonMessage ans =c.GetAnswer(js,id);
            return Ok(ans);
           

        }


        // GET: api/<ChatController>
        [HttpGet("DogForUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Dog[]> GetByUser(string id)
        {
            try
            {
                List<Dog> dogs = Dog.ReadAll();
                return Ok(dogs);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }
    }
}
