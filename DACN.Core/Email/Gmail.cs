    using System;
    using System.Net;
    using System.Net.Mail;

namespace DACN.Core.Email
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            String SendMailFrom = "Sender Email";
            String SendMailTo = "Reciever Email";
            String SendMailSubject = "Email Subject";
            String SendMailBody = "Email Body";

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
                SmtpServer.Credentials = new NetworkCredential(SendMailFrom, "Google App Password");
                SmtpServer.Send(email);

                Console.WriteLine("Email Successfully Sent");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }

        }
    }
}