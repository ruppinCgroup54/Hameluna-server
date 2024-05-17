using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using hameluna_server.BL;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using OpenAI_API.Chat;

namespace hameluna_server.DAL
{
    public class ChatDBService
    {

        public string Prompt { get; set; }

        public ChatDBService()
        {
            DogDBService dogDB = new();
            List<Dog> dogs = dogDB.ReadAll();

            Prompt = $"Your role:\r\nYou make matches between dogs and users.\r\n\r\nwhat do you get:\r\n1. An array of dogs that you can offer to the user.\r\nEach dog is an object that appears in the array as Json format. \r\n\r\n2. List of questions to which you need to get answers from the user.\r\n\r\nyour first message to the user:\r\nהיי אני דוגבוט ואני הולך למצוא לך את הכלב המושלם!\r\nספר לי על עצמך ועל הכלב שאתה מחפש.\r\n\r\nTone:\r\nchildish \r\n\r\nGuidelines:\r\n- don't print the questions in the first message.\r\n- The all conversation will be in Hebrew.\r\n- You need to get from each user information about him and his dog preferences based on the set of questions we will  give you.\r\n- for every answer you received give a nicely tone feedback, no more then 5-10 words. \r\n - You must direct the user to give you answers to the questions you received \r\n- the question will be asked in a different message\r\n- the questions will be asked randomly \r\n- When you have answers to 6 of the questions, you will access the list of dogs you received and calculate for each dog its match percentage to the user.\r\n- The matching percentage must be calculated based on the information you received from the user , the dog's characteristics and your knowledge about the dog's breeds.\r\n- You will return a list of dogs in a JSON array that contains the fields: {{\r\nid - (numberId field from the list of dogs),\r\nmatchPrecent -the match percentage you calculated \r\n}}\r\n- When you send the list of dogs you will first type the word \"finish\" and then the list of dogs in a diffrent message. \r\n\r\n" +
                $"dogs = {dogs}" +
                $"\nQuestions:\r\n- היכן יגור הכלב? בתוך או מחוץ לבית?\r\n- מה גודל הבית?\r\n- האם יש חצר בבית?\r\n- כמה שעות הכלב יהיה לבד?\r\n- האם יש לך זמן לעשות טיולים ארוכים?\r\n- האם יש חיות מחמד נוספות בבית?\r\n- האם יש ניסיון בגידול כלבים?\r\n- מי גר בבית?\r\n- מה גודל הכלב שאתם מחפשים?\r\n- מה גיל הכלב שאתם מחפשים? (גור,צעיר, בוגר, מבוגר)\r\n- האם יש לכם גזע מועדף?\r\n- האם יש לכם מין מועדף?";


        }

        //connects to mongo and return the "users" collection
        IMongoCollection<BsonDocument> GetCollection()
        {
            // get mongoDB connection string and get database object
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("MongoDBHameluna");
            var settings = MongoClientSettings.FromConnectionString(cStr);
            // Set the ServerApi field of the settings object to set the version of the Stable API on the client
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            // Create a new client and connect to the server
            var client = new MongoClient(settings);
            var database = client.GetDatabase("DogBot");
            return database.GetCollection<BsonDocument>("Users");
        }


        public string CreateDocument()
        {
            IMongoCollection<BsonDocument> collection ;

            try
            {
                collection = GetCollection();

            }
            catch (Exception ex)
            {
                throw(ex);
            }

            BsonArray messages = new BsonArray
            {
                new BsonDocument{
                    { "role","System" },
                    { "content",this.Prompt }
                },
                new BsonDocument{
                    { "role","Assistant" },
                    { "content","how can i help you today" }
                }
            };

            var document = new BsonDocument
            {
                {"messages", messages}
            };

            collection.InsertOne(document);

            return document["_id"].ToString();

        }

        public BsonArray GetUserMessages(string id)
        {

            IMongoCollection<BsonDocument> collection;

            try
            {
                collection = GetCollection();

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            BsonDocument userChat = collection.Find(filter).FirstOrDefault().AsBsonDocument;

            BsonArray messages = userChat["messages"].AsBsonArray;

          
            return messages;
        }

        public List<ChatMessage> ConvertConverastion(BsonArray arr)
        {
            List<ChatMessage> chatList = new();

            for (int i = 0; i < arr.Count; i++)
            {
                chatList.Add(ConvertFromBsonToChat(arr[i].AsBsonDocument));
            }
            return chatList;

        }

        public ChatMessage ConvertFromBsonToChat(BsonDocument mess)
        {
            ChatMessageRole current ;

            switch (mess["role"].ToString())
            {
                case "system":
                    current = ChatMessageRole.System;
                    break; 
                case "user":
                    current = ChatMessageRole.User;
                    break; 
                case "assistant":
                    current = ChatMessageRole.Assistant;
                    break;
                default:
                    current = ChatMessageRole.System;
                    break;
            }

            ChatMessage newMess = new ChatMessage(current, mess["content"].ToString());

            return newMess;

        }

    }
}
