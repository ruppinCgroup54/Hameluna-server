using System.Text.Json;
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

        public string role { get; set; }
        public string content { get; set; }

    }
    public class Chat
    {
        public Chat()
        {
        }

        public string Id { get; set; }

        public Chat(string id)
        {
            Id = id;
        }

        public List<ChatMessage> ChatMessages { get; set; }
        public List<Dog> LovesDogs { get; set; }

        static public string CreateChat()
        {
            ChatDBService db = new();

            return db.CreateDocument();
        }

        public JsonMessage GetAnswer(JsonMessage message)
        {
            ChatDBService chatDB = new();

            //gets the user previouse messages
            List<JsonMessage> jsonMessages = chatDB.GetUserMessages(this.Id).ToList();

            // add the new user message
            jsonMessages.Add(message);

            // convert the messages to ChatGpt type for the chat request
            this.ChatMessages = chatDB.ConvertConverastion(jsonMessages);

            JsonMessage response = new()
            {
                role = "assistant", 
                content = GetResponseFromGPT()
            };
            
            // add for the database to continoues saving of the chat
            jsonMessages.Add(response);

            // add incase that we need to get another response from chat
            this.ChatMessages.Add(chatDB.ConvertFromBsonToChat(response));

            CheckIfFinish(response);

            //update the new messages in the chat
            chatDB.UpdateMessages(jsonMessages, this.Id);

            return response;

        }

        public void CheckIfFinish(JsonMessage res)
        {
            ChatDBService chatDb = new();

            if (res.content.Contains("finish"))
            {
                ChatMessage messGetDogs = new ChatMessage
                {
                    Role = ChatMessageRole.System,
                    TextContent = "send the dogs list"
                };

                this.ChatMessages.Add(messGetDogs);
                try
                {
                    string response = GetResponseFromGPT();

                    // extract the json response to json objectss
                    response = response.Replace("`","");
                    List<JsonObject> dogs = JsonSerializer.Deserialize<List<JsonObject>>(response);

                    //update the dog rank array in the data base
                    chatDb.UpdateDogRank(dogs, Id);
                }
                catch (Exception ex)
                {

                    chatDb.UpdateDogRank(new(), Id);

                }



                res.content= res.content.Replace("finish", "");
            }



        }

        public string GetResponseFromGPT()
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
                MaxTokens = 1000
            };


            var chats = OpenAi.Chat.CreateChatCompletionAsync(chatRequest);

            //get the ChatGpt response
            foreach (var chat in chats.Result.Choices)
            {
                outputResuolt += chat.Message.TextContent;

            }
            return outputResuolt;
        }

        public JsonMessage[] GetConversation(string id)
        {
            ChatDBService db = new();
            JsonMessage[] jsonMessages = db.GetUserMessages(id);
            return jsonMessages;
        }


    }
}
