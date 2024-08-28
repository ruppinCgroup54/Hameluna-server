namespace hameluna_server.BL
{
    public class MailData
    {
        public string EmailToId { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public List<string> Attachments { get; set; }

        public static async Task<bool> SendMail(AdoptionRequest ad)
        {

            List<string> att = ad.Dog.GetAllFiles();

            for (int i = 0; i < att.Count; i++)
            {

                string path = System.IO.Directory.GetCurrentDirectory();
                //check for shelters diractory if noe exists create new one with shleter id
                string shelterDir = Path.Combine(path, "uploadedFiles");
                string temp = att[i].Remove(0, 5);
                att[i]= shelterDir + temp;
            }


            MailData md = new()
            {
                EmailToId = ad.Adopter.Email,
                EmailSubject = "אימוץ הכלב/ה " + ad.Dog.Name,
                EmailToName = ad.Adopter.FirstName + " " + ad.Adopter.LastName,
                Attachments = att
            };

            MailService ms = new();

            return ms.SendMail(md);
        }

    }
}
