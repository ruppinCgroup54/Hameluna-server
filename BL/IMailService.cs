using Org.BouncyCastle.Asn1.Pkcs;

namespace hameluna_server.BL
{
    public interface IMailService
    {
        bool SendMail(MailData Mail_Data);
        public Task CreatePdfFromHtml(string html);
    }

}
