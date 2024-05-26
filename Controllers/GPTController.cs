using System.Text;
using System.Text.Json.Nodes;
using Amazon.Runtime.Internal;
using hameluna_server.BL;
using hameluna_server.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
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

    }



}

