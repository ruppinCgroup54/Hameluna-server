using hameluna_server.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SheltersController : ControllerBase
    {
        // GET: api/<ShelterController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Shelter[]> Get()
        {
            try
            {
                List<Shelter> sh = Shelter.ReadAll();
                return Ok(sh);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }

        // GET api/<ShelterController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Shelter))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(int id)
        {
            try
            {
                Shelter sh = Shelter.ReadOne(id);
                if (sh.ShelterId == -1)
                {
                    return NotFound($"There is no shelter with id {id}");
                }
                return Ok(sh);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        // POST api/<ShelterController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Shelter))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Post([FromBody] Shelter sh)
        {
            try
            {
                 sh.Insert();

                return CreatedAtAction(nameof(Get), new { id = sh.ShelterId }, sh);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // PUT api/<ShelterController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult Put(int id, [FromBody] Shelter sh)
        {
            try
            {
                if (sh == null || sh.ShelterId != id)
                {
                    return BadRequest();
                }
                int numEffected = sh.Update();
                if (numEffected == 0)
                {
                    return NotFound($"There is no shelter with id {id}");
                }

                return NoContent();

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);

            }
        }

        // DELETE api/<ShelterController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id == -1)
                {
                    return BadRequest();
                }
                int numEffected = Shelter.Delete(id);
                if (numEffected == 0)
                {
                    return NotFound($"There is no shelter with id {id}");
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
