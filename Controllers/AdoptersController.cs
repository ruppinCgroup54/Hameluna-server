using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using hameluna_server.BL;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptersController : Controller
    {

        // GET: api/<VolunteerController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Adopter[]> Get()
        {
            try
            {
                List<Adopter> ad = Adopter.ReadAll();
                return Ok(ad);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }

        // GET api/<VolunteerController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Adopter))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(string id)
        {
            try
            {
                Adopter ad = Adopter.ReadOne(id);
                if (ad.PhoneNumber == "")
                {
                    return NotFound($"There is no adopter with phone number {id}");
                }
                return Ok(ad);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }
        [HttpGet("contect/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetContectData(string id)
        {
            try
            {
                Adopter ad = Adopter.ReadOne(id);
                if (ad.PhoneNumber == "")
                {
                    return NotFound($"There is no adopter with phone number {id}");
                }

                return Ok(ad);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        // POST api/<VolunteerController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Adopter))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Post([FromBody] Adopter ad)
        {
            try
            {

                string newId = ad.Insert();

                return CreatedAtAction(nameof(Get), new { id = newId }, ad);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // PUT api/<VolunteerController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult Put(string id, [FromBody] Adopter ad)
        {
            try
            {
                if (ad == null || ad.PhoneNumber != id)
                {
                    return BadRequest();
                }
                int numEffected = ad.Update();
                if (numEffected == 0)
                {
                    return NotFound($"There is no adopter with phone number {id}");
                }

                return NoContent();

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);

            }
        }

        // DELETE api/<VolunteerController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(string id)
        {
            try
            {
                if (id == "")
                {
                    return BadRequest();
                }
                int numEffected = Adopter.Delete(id);
                if (numEffected == 0)
                {
                    return NotFound($"There is no adopter with phone number {id}");
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
