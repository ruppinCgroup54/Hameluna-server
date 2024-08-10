using System.Text;
using System.Text.Json.Nodes;
using Amazon.Runtime.Internal;
using hameluna_server.BL;
using hameluna_server.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;

namespace hameluna_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GPTController : ControllerBase
    {

        [HttpGet]
        [HttpGet("asyncsale")]
        public async Task<IActionResult> GetOnSaleProductsAsync()
        {
            Conversation con = Chat.GetStreamFRomCaht();

            try
            {
                // Create a response stream
                Response.ContentType = "text/plain";
                Response.Headers.Add("Cache-Control", "no-cache");
                Response.Headers.Add("Connection", "keep-alive");


                // Stream response from the chatbot
                await con.StreamResponseFromChatbotAsync(async c =>
                {
                    var buffer = Encoding.UTF8.GetBytes(c);
                    await Response.Body.WriteAsync(buffer, 0, buffer.Length);
                    await Response.Body.FlushAsync();
                });

                // Signal end of response
                await Response.Body.FlushAsync();
                return new EmptyResult();

            }
            catch (Exception)
            {

                return BadRequest("done");
            }
        }
        [HttpGet("mongo")]
        public async Task<IActionResult> GetIp()
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Access-Control-Request-Headers", "*");
            client.DefaultRequestHeaders.Add("api-key", "9iCKysgYgHsH4oPsPavJa64Ksqy1E1a2mPA4xohaMUdTkfOjVPNuktlnCjsU0Gd1");




            //var body = @"\{""collection"":""Users"",""database"":""DogBot"", ""dataSource"":""ChatApp"",""projection"":{""_id"": 1}\}";

            //using StringContent jsonContent = new(body,
            //                                           Encoding.UTF8,
            //                                           "application/json");
            try
            {
                // Send the GET request
                HttpResponseMessage response = await client.GetAsync("https://eu-central-1.aws.data.mongodb-api.com/app/data-wqefo/endpoint/GetUsers");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseData = await response.Content.ReadAsStringAsync();

                // Print the response data
                Console.WriteLine(responseData);
                return Ok(responseData);
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP request errors
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception e)
            {
                // Handle general errors
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                // Dispose the HttpClient instance if you don't plan to reuse it
                client.Dispose();
            }

                return NotFound();






        }


        [HttpPost("DailyRoutines/{shelterNumber}")]
        public IActionResult SetDailyRoutine(int shelterNumber, [FromBody] FullRoutineException dr)
        {
            FireBaseDBService db = new();
            try
            {
                int dailyRoutines = db.SetExceptions(shelterNumber,dr);
                return Ok(dailyRoutines);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("DailyRoutines/{shelterNumber}")]
        public ActionResult<Dictionary<string, FullRoutineException>> SetDailyRoutine(int shelterNumber)
        {
            FireBaseDBService db = new();
            try
            {
                Dictionary<string, FullRoutineException> dailyRoutines = db.ShelterExeptions(shelterNumber);
                return Ok(dailyRoutines);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }



}

