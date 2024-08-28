using hameluna_server.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hameluna_server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MailController : ControllerBase
    {
        IMailService Mail_Service = null;
        //injecting the IMailService into the constructor
        public MailController(IMailService _MailService)
        {
            Mail_Service = _MailService;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult< bool>> SendMail(AdoptionRequest ad)
        {
            try
            {
                bool ans = await MailData.SendMail(ad);
                return Ok(ans);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("pdf")]
        public Task CreatePdf(string html)
        {
            return Mail_Service.CreatePdfFromHtml(html);

        } 
        [HttpPost("print")]
        public Task PrintPdf(string html)
        {
            return Mail_Service.CreatePdfFromHtml(html);

        }
    }
}
