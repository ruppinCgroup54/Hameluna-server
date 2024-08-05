using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using hameluna_server.BL;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DogsController : Controller
    {

        // GET: api/<DogController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Dog[]> Get()
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


        // GET: api/<DogController>
        [HttpGet("DogsForUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Dog[]> GetByUser(string id)
        {
            try
            {
                List<Dog> dogs = Dog.GetDogsForUser(id);
                return Ok(dogs);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }

         [HttpGet("favorites/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Dog[]> GetUserFavorites(string id)
        {
            try
            {
                List<Dog> dogs = Dog.GetFavorites(id);
                return Ok(dogs);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }


        // GET api/<DogController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dog))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(int id)
        {
            try
            {
                Dog d = Dog.ReadOne(id);
                if (d.NumberId == -1)
                {
                    return NotFound($"There is no dog with id number {id}");
                }
                return Ok(d);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        // GET api/<DogController>/5
        [HttpGet("DogStatus/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetStatus(int id)
        {
            try
            {
                Dog d = new() { NumberId=id};
                if (d.NumberId == -1)
                {
                    return NotFound($"There is no dog with id number {id}");
                }
                return Ok(d.GetStatus());
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        // POST api/<DogController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Dog))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Post([FromBody] Dog dog)
        {
            try
            {
                return Ok(dog.Insert());

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        
        // POST api/<DogController>
        [HttpPost("favorites/{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult AddToFavorites(string id,[FromBody] int[] favs)
        {
            try
            {
                return Ok(Dog.UpdateFavorites(favs,id));

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // PUT api/<DogController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult Put(int id, [FromBody] Dog d)
        {
            try
            {
                if (d == null || d.NumberId != id)
                {
                    return BadRequest();
                }
                int numEffected = d.Update();
                if (numEffected == 0)
                {
                    return NotFound($"There is no dog with id number {id}");
                }

                return NoContent();

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);

            }
        }

        // DELETE api/<DogController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            try
            {
                //if (id == "")
                //{
                //    return BadRequest();
                //}
                int numEffected = Dog.Delete(id);
                if (numEffected == 0)
                {
                    return NotFound($"There is no dog with id number {id}");
                }

                return NoContent();

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);

            }
        }
    }
}
