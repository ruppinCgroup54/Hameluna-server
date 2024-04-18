using hameluna_server.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CellsController : ControllerBase
    {
        // GET: api/<CellController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Cell[]> Get()
        {
            try
            {
                List<Cell> cl = Cell.ReadAll();
                return Ok(cl);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }

        [HttpGet("shelter/{sheltereNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Cell[]> GetByShelter(int sheltereNumber)
        {
            try
            {
                List<Cell> cl = Cell.ReadAll();
                return Ok(cl);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }

        // GET api/<CellController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Cell))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(int id)
        {
            try
            {
                Cell cl = Cell.ReadOne(id);
                if (cl.Id == -1)
                {
                    return NotFound($"There is no cell with id {id}");
                }
                return Ok(cl);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }

        // POST api/<CellController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Cell))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Post([FromBody] Cell sh)
        {
            try
            {
                sh.Insert();

                return CreatedAtAction(nameof(Get), new { id = sh.Id }, sh);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // PUT api/<CellController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult Put(int id, [FromBody] Cell cl)
        {
            try
            {
                if (cl == null || cl.Id != id)
                {
                    return BadRequest();
                }
                int numEffected = cl.Update();
                if (numEffected == 0)
                {
                    return NotFound($"There is no cell with id {id}");
                }

                return NoContent();

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);

            }
        }

        // DELETE api/<CellController>/5
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
                int numEffected = Cell.Delete(id);
                if (numEffected == 0)
                {
                    return NotFound($"There is no cell with id {id}");
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
