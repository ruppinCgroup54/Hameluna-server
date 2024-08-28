using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace hameluna_server.BL
{
    public class MailService : IMailService
    {
        MailSettings Mail_Settings = null;

        public MailService()
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            Mail_Settings = new()
            {
                EmailId = configuration.GetSection("MailSettings").GetValue("EmailId", "string"),
                Name = configuration.GetSection("MailSettings").GetValue("Name", "string"),
                Host = configuration.GetSection("MailSettings").GetValue("Host", "string"),
                UserName = configuration.GetSection("MailSettings").GetValue("UserName", "string"),
                Password = configuration.GetSection("MailSettings").GetValue("Password", "string")
            };
            Mail_Settings.Port = 587;

        }

        public MailService(IOptions<MailSettings> options)
        {
            Mail_Settings = options.Value;

        }

        public async Task CreatePdfFromHtml(string html)
        {
            await PdfService.GeneratePdfAsync(html, new() { NumberId=15,Name="מוקה",ShelterNumber=1});
        }

        public bool SendMail(MailData Mail_Data)
        {
            try
            {
                //MimeMessage - a class from Mimekit
                MimeMessage email_Message = new MimeMessage();
                MailboxAddress email_From = new MailboxAddress(Mail_Settings.Name, Mail_Settings.EmailId);
                email_Message.From.Add(email_From);
                MailboxAddress email_To = new MailboxAddress(Mail_Data.EmailToName, Mail_Data.EmailToId);
                email_Message.To.Add(email_To);
                email_Message.Subject = Mail_Data.EmailSubject;
                BodyBuilder emailBodyBuilder = new BodyBuilder();
                //emailBodyBuilder.HtmlBody = Mail_Data.EmailBody;


                foreach (var att in Mail_Data.Attachments)
                {
                    string fileName = Path.GetFileName(att);
                    emailBodyBuilder.Attachments.Add(fileName,File.ReadAllBytes(att));
                }
                //emailBodyBuilder.TextBody = Mail_Data.EmailBody;
                email_Message.Body = emailBodyBuilder.ToMessageBody();
                //this is the SmtpClient class from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one


                SmtpClient MailClient = new SmtpClient();
                MailClient.Connect(Mail_Settings.Host, Mail_Settings.Port, SecureSocketOptions.StartTls);
                MailClient.Authenticate(Mail_Settings.EmailId, Mail_Settings.Password);
                MailClient.Send(email_Message);
                MailClient.Disconnect(true);
                MailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }



    }
}
