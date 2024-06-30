using hameluna_server.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ToDoItem> Get(int id)
        {
            try
            {
                ToDoItem vl = ToDoItem.ReadOne(id);
                return Ok(vl);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }

        // GET api/<VolunteerController>/5
        [HttpGet("shelter/{shelterNumber}/date/{date}")]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ToDoItem>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get(int shelterNumber,DateTime date)
        {
            try
            {
                List<ToDoItem> tdList = ToDoItem.ReadByDate(shelterNumber, date);
                if (tdList.Count == 0)
                {
                    return NotFound($"אין משימות בתאריך "+date.ToShortDateString());
                }
                return Ok(tdList);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }



        // POST api/<VolunteerController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ToDoItem))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Post([FromBody] ToDoItem td)
        {
            try
            {
                int newId = td.Insert();
                return CreatedAtAction(nameof(Get), new { id = newId }, td);

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

        public IActionResult Put(int id, [FromBody] ToDoItem td)
        {
            try
            {
                if (td == null || td.ToDoId != id)
                {
                    return BadRequest();
                }
                int numEffected = td.Update();
                if (numEffected == 0)
                {
                    return NotFound($"לא קיימת משימה כזאת");
                }

                return NoContent();

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);

            }
        }

        //// DELETE api/<VolunteerController>/5
        //[HttpDelete("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult Delete(string id)
        //{
        //    try
        //    {
        //        if (id == "")
        //        {
        //            return BadRequest();
        //        }
        //        int numEffected = ToDoItem.Delete(id);
        //        if (numEffected == 0)
        //        {
        //            return NotFound($"There is no admin with phone number {id}");
        //        }

        //        return NoContent();

        //    }
        //    catch (Exception e)
        //    {

        //        return BadRequest(e.Message);

        //    }
        //}

    }
}
