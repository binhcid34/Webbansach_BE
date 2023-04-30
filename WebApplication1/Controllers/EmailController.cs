using DACN.Core.Email;
using DACN.Core.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMailService _mail;

        public EmailController(IMailService mail)
        {
            _mail = mail;
        }

        [HttpPost("sendmail")]
        public ResponseModel SendMailAsync(String SendMailTo, String SendMailBody)
        {
            var res = new ResponseModel();
            String SendMailFrom = "binhcid34@gmail.com";
            String SendMailSubject = "Email Subject";

            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage email = new MailMessage();
                // START
                email.From = new MailAddress(SendMailFrom);
                email.To.Add(SendMailTo);
                email.CC.Add(SendMailFrom);
                email.Subject = SendMailSubject;
                email.Body = SendMailBody;
                //END
                SmtpServer.Timeout = 5000;
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(SendMailFrom, "dnlcxlvzkzmkmmft");
                SmtpServer.Send(email);

                res.Message = "Email Successfully Sent";
                res.Status = 200;
                res.Success = true;
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
                res.Message= ex.Message;
                return res;
            }

        }
    }
}
