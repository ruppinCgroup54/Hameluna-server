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
                string path = await db.InsertProfileImage(shelterId, dogId, images[0]);
                return Ok(db.InsertProfile(path, dogId));
            }
            catch (Exception)
            {
                return BadRequest("image is failed to insert");
            }
        }  
        // POST api/<CellController>
        [HttpPost("addImages/shelterId/{shelterId}/dogId/{dogId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> AddImages(string shelterId, int dogId, [FromForm] List<IFormFile> images)
        {
            try
            {
                Dog dog = new(){ NumberId = dogId };
                List<string> paths = await dog.AddImages(shelterId, images);
                return Ok(paths);
            }
            catch (Exception)
            {
                return BadRequest("image is failed to insert");
            }
        }
        // POST api/<CellController>
        [HttpPost("shelterImage")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> PostShelterImage([FromForm] List<IFormFile> images)
        {
            DBservices db = new();
            try
            {
                if (images.Count==0)
                {
                    return Ok("");
                }
                string path = await db.InsertShelterImage( images[0]);
                return Ok(path);
            }
            catch (Exception)
            {
                return BadRequest("image is failed to insert");
            }
        }

        // GET: api/<CellController>
        [HttpGet("dogId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string[]> Get(int id)
        {
            try
            {
                Dog d = new() { NumberId = id };
               
                return Ok(d.GetAllImages());
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }

    }
}
