using System;
using System.Net;
using System.Net.Mail;

namespace DACN.Core.Email
{
    public class MailService : IMailService
    {
        public bool SendMailAsync(string SendMailTo, string SendMailSubject, string SendMailBody)
        {
            String SendMailFrom = "binhcid34@gmail.com";
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
                //Console.WriteLine("Email Successfully Sent");
                //Console.ReadKey();
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                //Console.ReadKey();
                return false;
            }
        }
    }
}
