using System.Text.Json;
using hameluna_server.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        // GET: api/<CellController>
        [HttpGet("dogId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string[]> Get(int id)
        {
            try
            {
                Dog d = new() { NumberId = id };

                return Ok(d.GetAllFiles());
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }


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
                List<string> paths = new();
                for (int i = 0; i < files.Count; i++)
                {
                    string path = await db.InsertFile(shelterId, dogId, files[i]);
                    int num = db.InsertFileToData(path, dogId);
                    paths.Add(path);
                }
                return Ok(paths);
            }
            catch (Exception)
            {
                return BadRequest("file is failed to insert");
            }
        }

        // DELETE api/<DogController>/5
        [HttpDelete("{dogId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteDogFile(string dogId, [FromBody] JsonElement obj)
        {
            string url = obj.GetProperty("url").ToString();

            try
            {

                int numEffected = Dog.DeleteFile(url);
                if (numEffected == 0)
                {
                    return NotFound($"There is no dog File with url: {url}");
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
