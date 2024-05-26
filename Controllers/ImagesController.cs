using hameluna_server.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {


        // POST api/<CellController>
        [HttpPost("shelterId/{shelterId}/dogId/{dogId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Post(string shelterId, int dogId, [FromForm] List<IFormFile> images)
        {
            DBservices db = new();
            try
            {
                string path = await db.insertProfileImage(shelterId, dogId, images[0]);
                return Ok(db.insertProfileImage(shelterId, dogId, images[0]));
            }
            catch (Exception)
            {
                return BadRequest("image is failed to insert");
            }
        }
    }
}
