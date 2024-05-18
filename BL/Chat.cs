using System.Text.Json.Nodes;
using hameluna_server.DAL;
using MongoDB.Bson;
using OpenAI_API;
using OpenAI_API.Chat;

namespace hameluna_server.BL
{
    public class Chat
    {
        public Chat()
        {
        }

        public string Id { get; set; }
        public List<ChatMessage> ChatMessages { get; set; }
        public List<Dog> LovesDogs { get; set; }

        static public string CreateChat()
        {
            ChatDBService db = new();

            return db.CreateDocument();
        }

        public JsonObject GetAnswer(JsonObject message, string id)
        {
            ChatDBService chatDB = new();

            BsonArray messages = chatDB.GetUserMessages(id);
            messages.Add(BsonDocument.Parse(message.ToString()));

            this.ChatMessages = chatDB.ConvertConverastion(messages);

            return new JsonObject
            {
                ["role"] = "assistant",
                ["content"] = SendToChat()
            };

        }

        public string SendToChat()
        {
            //get the api key
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string apiKey = configuration.GetSection("OpenAISetting").GetValue("ApiKey", "string");

            // create a connection to ChatGPT
            var OpenAi = new OpenAIAPI(apiKey);


            string outputResuolt = "";


            // create chat request - open a chart with Gpt
            ChatRequest chatRequest = new()
            {
                Messages = this.ChatMessages,
                Model = OpenAI_API.Models.Model.GPT4_Turbo,
                Temperature = 0.2,
                MaxTokens=1000
            };


            var chats = OpenAi.Chat.CreateChatCompletionAsync(chatRequest);

            foreach (var chat in chats.Result.Choices)
            {
                outputResuolt += chat.Message.TextContent;

            }
            return outputResuolt;
        }




    }
}
