using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Amazon.Runtime.Internal.Auth;
using hameluna_server.BL;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OpenAI_API.Chat;
using static System.Net.Mime.MediaTypeNames;
using static MongoDB.Driver.WriteConcern;

namespace hameluna_server.DAL
{
    public class ChatDBService
    {

        public string Prompt { get; set; }

        public ChatDBService()
        {

            Prompt = $"Your role:\r\nYou make matches between dogs and users.\r\n\r\nwhat do you get:\r\n1. An array of dogs that you can offer to the user.\r\nEach dog is an object that appears in the array as Json format. \r\n\r\n2. List of questions to which you need to get answers from the user.\r\n\r\nyour first message to the user:\r\nהיי אני דוגבוט ואני הולך למצוא לך את הכלב המושלם!\r\nספר לי על עצמך ועל הכלב שאתה מחפש.\r\n\r\nTone:\r\nchildish \r\n\r\nGuidelines:\r\n- don't print the questions in the first message.\r\n- The all conversation will be in Hebrew.\r\n- You need to get from each user information about him and his dog preferences based on the set of questions we will  give you.\r\n- for every answer you received give a nicely tone feedback, no more then 5-10 words. \r\n - You must direct the user to give you answers to the questions you received \r\n- the question will be asked in a different message\r\n- the questions will be asked randomly \r\n- When you have answers to 6 of the questions, you will access the list of dogs you received and calculate for each dog its match percentage to the user.\r\n- The matching percentage must be calculated based on the information you received from the user , the dog's characteristics and your knowledge about the dog's breeds.\r\n- You will return a list of all the dogs, even if they have 0 precent match, in a JSON array that contains the fields: {{\r\nid - (numberId field from the list of dogs),\r\nmatchPrecent -the match percentage you calculated \r\n}}\r\n- When you send the list of dogs you will first type the word \"finish\" and then the list of dogs in a diffrent message. \r\n\r\n" +
                $"\nQuestions:\r\n- היכן יגור הכלב? בתוך או מחוץ לבית?\r\n- מה גודל הבית?\r\n- האם יש חצר בבית?\r\n- כמה שעות הכלב יהיה לבד?\r\n- האם יש לך זמן לעשות טיולים ארוכים?\r\n- האם יש חיות מחמד נוספות בבית?\r\n- האם יש ניסיון בגידול כלבים?\r\n- מי גר בבית?\r\n- מה גודל הכלב שאתם מחפשים?\r\n- מה גיל הכלב שאתם מחפשים? (גור,צעיר, בוגר, מבוגר)\r\n- האם יש לכם גזע מועדף?\r\n- האם יש לכם מין מועדף?" +
                $"dogs = "; 


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
            IMongoCollection<BsonDocument> collection;

            DogDBService dogDB = new();

            List<Dog> dogs = dogDB.ReadAll().FindAll((item) => item.IsAdoptable);


            try
            {
                collection = GetCollection();

            }
            catch (Exception ex)
            {
                DBservices.WriteToErrorLog(ex);
                throw ex;
            }

            BsonArray messages = new BsonArray
            {
                new BsonDocument{
                    { "role","system" },
                    { "content",this.Prompt + dogs.ToJson() },
                    {"indFinish",false }

                },
                new BsonDocument{
                    { "role","assistant" },
                    { "content"," היי אני דוגבוט ואני הולך למצוא לך את הכלב המושלם!\r\n    ספר לי על עצמך ועל הכלב שאתה מחפש." },
                    {"indFinish",false }
                }
            };

            var user = new BsonDocument
            {
                {"messages", messages}
            };

            collection.InsertOne(user);

            return user["_id"].ToString();

        }

        public void UpdateMessages(List<JsonMessage> messages, string id)
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

            // Creates a filter for all documents 
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            // Creates instructions to update the "name" field of the first document
            // that matches the filter
            var update = Builders<BsonDocument>.Update
                .Set("messages", messages);

            // Updates the first document that has a "name" value of "Bagels N Buns"
            try
            {
                collection.UpdateOne(filter, update).ModifiedCount.ToString();

            }
            catch (Exception ex)
            {
                DBservices.WriteToErrorLog(ex);

                throw (ex);
            }
        }

        public void UpdateDogRank(List<JsonRank> dogsRanks, string id)
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

            // Creates a filter for all documents 
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            // Creates instructions to update the "name" field of the first document
            // that matches the filter

            var update = Builders<BsonDocument>.Update
                .Set("dogsRanks", dogsRanks);


            try
            {
                collection.UpdateOne(filter, update).ModifiedCount.ToString();

            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        public int UpdateFavoritesDogs(int[] favDogs, string id)
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

            // Creates a filter for all documents 
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            // Creates instructions to update the "name" field of the first document
            // that matches the filter

            var update = Builders<BsonDocument>.Update
                .Set("favorites", favDogs);


            try
            {
                return Convert.ToInt32(collection.UpdateOne(filter, update).ModifiedCount);

            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        public JsonMessage[] GetUserMessages(string id)
        {

            IMongoCollection<BsonDocument> users;

            try
            {
                users = GetCollection();

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(id));
            BsonDocument userChat = users.Find(filter).FirstOrDefault();
            try
            {
                //convert the bson array in messages to array of json objects
                JsonMessage[] messages = BsonSerializer.Deserialize<JsonMessage[]>(userChat["messages"].AsBsonArray.ToJson());
                return messages;

            }
            catch (Exception ex)
            {

                throw (ex);
            }



        }

        public List<JsonRank> GetDogRank(string id)
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
            BsonDocument userChat = collection.Find(filter).FirstOrDefault();

            try
            {
                //convert the bson array in messages to array of json objects
                List<JsonRank> dogsList = BsonSerializer.Deserialize<List<JsonRank>>(userChat["dogsRanks"].AsBsonArray.ToJson());
                return dogsList.OrderByDescending(d => d.matchPrecent).ToList();
            }
            catch (Exception)
            {

                throw new NullReferenceException("Could't find dogs for you");
            }


        }

        public List<int> GetUserFavorites(string id)
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
            BsonDocument userChat = collection.Find(filter).FirstOrDefault();

            try
            {
                //convert the bson array in messages to array of json objects
                List<int> dogsList = BsonSerializer.Deserialize<List<int>>(userChat["favorites"].AsBsonArray.ToJson());
                return dogsList;
            }
            catch (Exception)
            {

                return new();
            }


        }

        public List<ChatMessage> ConvertConverastion(List<JsonMessage> arr)
        {
            List<ChatMessage> chatList = new();

            for (int i = 0; i < arr.Count; i++)
            {
                chatList.Add(ConvertFromBsonToChat(arr[i]));
            }
            return chatList;

        }

        public ChatMessage ConvertFromBsonToChat(JsonMessage mess)
        {
            ChatMessageRole current;

            switch (mess.role)
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

            ChatMessage newMess = new ChatMessage(current, mess.content);

            return newMess;

        }

    }
}
