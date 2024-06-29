using System.Data;
using hameluna_server.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyRoutinesController : ControllerBase
    {

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public ActionResult<DailyRoutine[]> Get()
        //{
        //    try
        //    {
        //        List<FullRoutineException> fr = FullRoutineException.ReadByDog(dogId);
        //        return Ok(fr);
        //    }
        //    catch (Exception e)
        //    {

        //        return StatusCode(StatusCodes.Status400BadRequest, e.Message);
        //    }

        //} 
        [HttpGet("fullExceptions/{dogId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<FullRoutineException[]> GetFullExceptions(int dogId)
        {
            try
            {
                List<FullRoutineException> fr = FullRoutineException.ReadByDog(dogId);
                return Ok(fr);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }


        // POST api/<DailyRoutinesController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(List<FullRoutineException>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult Post([FromBody] DailyRoutine dr)
        {
            try
            {
                List<FullRoutineException> dogEx =  dr.Insert();

                return CreatedAtAction(nameof(GetFullExceptions), new{dogId=dr.DogNumberId} ,dogEx);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }


        // PUT api/<AdoptionRequestController>/5
        [HttpPut("routineId/{routine}/itemId/{item}/")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult Put(int item ,int routine, [FromBody] RoutineException re)
        {
            try
            {
                if (re == null || (re.RoutineId != routine && re.ItemId!=item))
                {
                    return BadRequest();
                }
                int numEffected = re.Update();
                if (numEffected == 0)
                {
                    return NotFound($"לא קיימת חריגה");
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
