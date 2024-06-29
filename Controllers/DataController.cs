using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using hameluna_server.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        // GET: api/<DataController>
        [HttpGet("Breeds")]
        public IEnumerable<string> GetBreeds()
        {
            DBservices db = new();
            List<string> breeds = db.GetAllBreeds();
            return breeds;
        }

        [HttpGet("Colors")]
        public IEnumerable<string> GetColors()
        {
            DBservices db = new();
            List<string> colors = db.GetAllColors();
            return colors;
        }

        [HttpGet("Cities")]
        public IEnumerable<string> GetCities()
        {

            return Address.GetCities();
        }

        [HttpGet("Characteristics")]
        public IEnumerable<string> GetCharacteristics()
        {
            DBservices db = new();
            List<string> characteristics = db.GetAllCharacteristics();
            return characteristics;
        }

        [HttpGet("DailyRoutines")]
        public ActionResult<List<string>> GetDailyRoutines()
        {
            DBservices db = new();
            try
            {
                List<string> dailyRoutines = db.GetAllCharacteristics();
                return Ok(dailyRoutines);
            }
            catch(Exception e)
            {
                return BadRequest( e.Message );
            }
        }


    }
}
