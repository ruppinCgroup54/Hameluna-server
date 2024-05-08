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
        [HttpPost("{shelterId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Post(string shelterId,[FromForm] List<IFormFile> images)
        {
            try
            {
                List<string> imageLinks = new();

                //string path = System.IO.Directory.GetCurrentDirectory();

                ////add shalter diractory if not exists

                //string shelterDir = Path.Combine(path, "uploadedImages/" + shelterId + "/");
                //if (!Directory.Exists(shelterDir))
                //{
                //    Directory.CreateDirectory(shelterDir);
                //}

                //long size = images.Sum(i => i.Length);

                //foreach (var formImage in images)
                //{
                //    if (formImage.Length > 0)
                //    {
                //        var imagePath = Path.Combine(shelterDir, formImage.FileName);

                //        using (var stream = System.IO.File.Create(imagePath))
                //        {
                //            await formImage.CopyToAsync(stream);
                //        }
                //        imageLinks.Add(formImage.FileName);
                //    }
                //}

                return Ok(imageLinks);
            }
            catch
            {
                return BadRequest("oops");
            }
            //try
            //{
            //    sh.Insert();

            //    return CreatedAtAction(nameof(Get), new { id = sh.Id }, sh);

            //}
            //catch (Exception e)
            //{

            //    return BadRequest(e.Message);
            //}
        }
    }
}
