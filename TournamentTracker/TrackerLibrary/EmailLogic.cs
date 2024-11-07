using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace TrackerLibrary
{
    public static class EmailLogic
    {
        public static void SendEmail(string to, string subject, string body)
        {
            SendEmail(new List<string> { to }, new List<string>(), subject, body);
        }
        public static void SendEmail(List<string> to,List<string> bcc, string subject, string body)
        {
            MailAddress fromMailAddress = new MailAddress(GlobalConfig.AppKeyLookup("SenderEmail:Email"), GlobalConfig.AppKeyLookup("SenderEmail:DisplayName"));
            MailMessage mail = new MailMessage
            {
                From = fromMailAddress,
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            foreach (string email in to)
            {
                mail.To.Add(email);
            }
            foreach (string email in bcc)
            {
                mail.Bcc.Add(email);
            }
            SmtpClient client = new SmtpClient
            {
                Host = GlobalConfig.AppKeyLookup("EmailSettings:SmtpServer"),
                Port = int.Parse(GlobalConfig.AppKeyLookup("EmailSettings:SmtpPort")),
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(
                    GlobalConfig.AppKeyLookup("EmailSettings:SmtpUsername"),
                    GlobalConfig.AppKeyLookup("EmailSettings:SmtpPassword")
                ),
                EnableSsl = true
            };

            client.Send(mail);
        }
    }
}
