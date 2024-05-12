using hameluna_server.DAL;
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
        public List<Dog> LovesDogs{ get; set; }

        static public string CreateChat()
        {
            ChatDBService db = new();

            return db.CreateDocument();
        }
        
        


    }
}
