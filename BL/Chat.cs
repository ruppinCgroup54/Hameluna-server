using System.Text.Json.Nodes;
using hameluna_server.DAL;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using OpenAI_API;
using OpenAI_API.Chat;

namespace hameluna_server.BL
{
    public class JsonMessage
    {
        public JsonMessage()
        {
        }

        public JsonMessage(string role, string content)
        {
            this.role = role;
            this.content = content;
        }

        public string  role { get; set; }
        public string content { get; set; } 

    }
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

        public JsonMessage GetAnswer(JsonMessage message, string id)
        {
            ChatDBService chatDB = new();

            List<JsonMessage> messages = chatDB.GetUserMessages(id).ToList();
            messages.Add(message);

            this.ChatMessages = chatDB.ConvertConverastion(messages);

            JsonMessage response = new()
            {
                role = "assistant",
                content = SendToChat()
            };

            messages.Add(response);

            chatDB.UpdateDocument(messages, id);

            return response;

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

        public JsonMessage[] GetConversation(string id)
        {
            ChatDBService db = new();
            JsonMessage[] jsonArray = db.GetUserMessages(id);
            return jsonArray;
        }


    }
}
