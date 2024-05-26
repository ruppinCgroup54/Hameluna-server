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

        [HttpGet]
        public ActionResult<string> GetNewID()
        {
            // create new chat and return the new chat id
            try
            {
                return Ok(new { id = Chat.CreateChat() });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        // GET api/<ChatsController>/5
        [HttpGet("{id}")]
        public IActionResult GetChat(string id)
        {
            Chat c = new();
            try
            {
                return Ok(new { chat = c.GetConversation(id) });
            }
            catch (Exception)
            {
                return BadRequest("Could not retrive your conversation.");
            }
        }



        [HttpPost("{id}")]
        public IActionResult UseChatGpt(string id, [FromBody] JsonMessage js)
        {
            Chat c = new(id);
            try
            {
                JsonMessage ans = c.GetAnswer(js);
                return Ok(ans);

            }
            catch (Exception)
            {

                return BadRequest("could not send");
            }


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
