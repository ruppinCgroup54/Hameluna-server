using hameluna_server.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerController : ControllerBase
    {
        // GET: api/<VolunteerController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Volunteer[]> Get()
        {
            try
            {
                List<Volunteer> vl = Volunteer.ReadAll();
                return Ok(vl);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
            
        }

        // GET api/<VolunteerController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(Volunteer))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(string id)
        {
            try
            {
                Volunteer vl = Volunteer.ReadOne(id);
                if (vl.PhoneNumber=="")
                {
                    return NotFound($"There is no volunteer with phone number {id}");
                }
                return Ok(vl);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        // POST api/<VolunteerController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created,Type = typeof(Volunteer))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Post([FromBody] Volunteer vol)
        {
            try
            {
                string newId = vol.Insert();
                return CreatedAtAction( nameof(Get),new { id = newId } ,vol);

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

        public IActionResult Put(string id, [FromBody] Volunteer vol)
        {
            try
            {
                if (vol==null || vol.PhoneNumber!=id)
                {
                    return BadRequest();
                }
                int numEffected = vol.Update();
                if (numEffected==0)
                {
                    return NotFound($"There is no volunteer with phone number {id}");
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
                if (id=="")
                {
                    return BadRequest();
                }
                int numEffected = Volunteer.Delete(id);
                if (numEffected == 0)
                {
                    return NotFound($"There is no volunteer with phone number {id}");
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
