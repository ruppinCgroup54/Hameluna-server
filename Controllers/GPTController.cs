using hameluna_server.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Completions;

namespace hameluna_server.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class GPTController : ControllerBase
    {
        [HttpGet]
        [Route("UseChatGpt")]
        public async Task<IActionResult> UseChatGpt(string query)
        {
            //get the api key
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string apiKey =  configuration.GetSection("OpenAISetting").GetValue("ApiKey", "string");


            string outputResuolt = "";

            // create a connection to ChatGPT
            var OpenAi = new OpenAIAPI(apiKey);

            // create chat request - ope a chart with Gpt
            ChatRequest chatRequest = new();

            //crate messages list
            List<ChatMessage> mess = new();

            ChatMessage newMess = new ChatMessage(ChatMessageRole.User, query);
            mess.Add(newMess);

            chatRequest.Messages = mess;


            chatRequest.Model = OpenAI_API.Models.Model.GPT4_Turbo;

            var chats = OpenAi.Chat.CreateChatCompletionAsync(chatRequest);

            foreach (var chat in chats.Result.Choices)
            {
                outputResuolt += chat.Message.TextContent;

            }
            return Ok(outputResuolt);

        }

    }
}
