using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace PL
{
    public static class EmailService
    {
        public static void SendEmail(string messageText, string senderEmailAddress)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                var emailAddress = "spacealerthelp@gmail.com";
                smtpClient.Credentials = new NetworkCredential(emailAddress, "spacealerthelp");
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(emailAddress);
                    message.To.Add(emailAddress);
                    var subject = "Space Alert Resolver Message";
                    if (!string.IsNullOrWhiteSpace(senderEmailAddress))
                        subject += string.Format(CultureInfo.CurrentCulture, " From {0}", senderEmailAddress);
                    message.Body = messageText;
                    message.Subject = subject;
                    smtpClient.Send(message);
                }
            }
        }
    }
}
