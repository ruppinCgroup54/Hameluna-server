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
            List<string> breeds= db.GetAllBreeds();
            return breeds;
        }    
        
        [HttpGet("Colors")]
        public IEnumerable<string> GetColors()
        {
            DBservices db = new();
            List<string> colors= db.GetAllColors();
            return colors;
        } 
        
        [HttpGet("Cities")]
        public IEnumerable<string> GetCities()
        {
            
            return Address.GetCities();
        }

        //// GET api/<DataController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<DataController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<DataController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<DataController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
