﻿using System.Data;
using hameluna_server.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionRequestsController : ControllerBase
    {

        // GET: api/<AdoptionRequestController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AdoptionRequest[]> Get()
        {
            try
            {
                List<AdoptionRequest> sh = AdoptionRequest.ReadAll();
                return Ok(sh);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }
         // GET: api/<AdoptionRequestController>
        [HttpGet("adopter/{phoneNumber}/dog/{dogId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<AdoptionRequest> GetByadopter(string phoneNumber, int dogId)
        {
            try
            {
                AdoptionRequest sh = AdoptionRequest.ReadByAdopter( phoneNumber, dogId);
                if (sh.RequestId == 0 )
                {
                    return NotFound("There is no request for this addopter");
                    
                }
                return Ok(sh);
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }

        }


        // POST api/<AdoptionRequestController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AdoptionRequest))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public IActionResult Post([FromBody] AdoptionRequest ar)
        {
            try
            {
                ar.Insert();

                return CreatedAtAction(nameof(Get), new { id = ar.RequestId }, ar);

            }
            catch (ConstraintException c)
            {
                return Conflict("כבר שלחת בקשה על הכלב הזה, נווו....");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }

        // PUT api/<AdoptionRequestController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Put(int id, [FromBody] AdoptionRequest ar)
        {
            try
            {
                if (ar == null || ar.RequestId != id)
                {
                    return BadRequest();
                }
                int numEffected = await ar.Update();
                if (numEffected == 0)
                {
                    return NotFound($"There is no shelter with id {id}");
                }

                return NoContent();

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);

            }
        }

        // DELETE api/<AdoptionRequestController>/5
        [HttpDelete("shelter/{shelterNumber}/dog/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int shelterNumber, int id)
        {
            AdoptionRequest ad = new() { RequestId=id,Dog=new() { ShelterNumber=shelterNumber} };
            try
            {
                if (id == -1)
                {
                    return BadRequest();
                }
                int numEffected = ad.Delete(id);
                if (numEffected == 0)
                {
                    return NotFound($"There is no request with id {id}");
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
