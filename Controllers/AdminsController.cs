using System.Text.Json;
using hameluna_server.BL;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : Controller
    {
        // GET: api/<VolunteerController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Admin[]> Get()
        {
            try
            {
                List<Admin> vl = Admin.ReadAll();
                return Ok(vl);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }

        // GET api/<VolunteerController>/5
        [HttpGet("{id}")]
        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Admin))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(string id)
        {
            try
            {
                Admin ad = Admin.ReadOne(id);
                if (ad.PhoneNumber == "")
                {
                    return NotFound($"There is no admin with phone number {id}");
                }
                return Ok(ad);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        //api/admin/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult LogIn(JsonElement ad)
        {
            try
            {
                int shelterNumber = Admin.Login(ad);
                return Ok(shelterNumber);
            }
            catch (Exception e)
            {

                return Unauthorized(e.Message);
            }
        }


        // POST api/<VolunteerController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Admin))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Post([FromBody] Admin ad)
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

        public IActionResult Put(string id, [FromBody] Admin ad)
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
                    return NotFound($"There is no admin with phone number {id}");
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
                int numEffected = Admin.Delete(id);
                if (numEffected == 0)
                {
                    return NotFound($"There is no admin with phone number {id}");
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
