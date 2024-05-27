using hameluna_server.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {


        // POST api/<CellController>
        [HttpPost("shelterId/{shelterId}/dogId/{dogId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Post(string shelterId, int dogId, [FromForm] List<IFormFile> files)
        {
            DBservices db = new();
            try
            {
                int num = 0;
                for (int i = 0; i < files.Count; i++)
                {
                    string path = await db.insertFile(shelterId, dogId, files[i]);
                    num = db.InsertFileToData(path, dogId);
                }
                return Ok(num);
            }
            catch (Exception)
            {
                return BadRequest("file is failed to insert");
            }
        }
    }
}
