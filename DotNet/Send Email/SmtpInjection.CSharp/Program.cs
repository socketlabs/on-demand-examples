using System;
using System.Net;
using System.Net.Mail;

namespace SocketLabs.SmtpInjection.CSharp
{
    internal class Program
    {
        private static void Main()
        {
            const string smtpHost = "smtp.socketlabs.com";
            const int smtpPort = 2525; // standard is port 25 but that is blocked by many ISP's
            const string smtpUserName = "YOUR-SMTP-USER-NAME";
            const string smtpPassword = "YOUR-SMTP-API-PASSWORD";

            var creds = new NetworkCredential(smtpUserName, smtpPassword);
            var auth = creds.GetCredential(smtpHost, smtpPort, "Basic");

            using (var msg = new MailMessage())
            using (var smtp = new SmtpClient())
            {
                // ** can be set in config **
                // you can skip this set by setting your credentials in the web.config or app.config
                // http://msdn.microsoft.com/en-us/library/w355a94k.aspx
                smtp.Host = smtpHost;
                smtp.Port = smtpPort;
                smtp.Credentials = auth;

                //Add SocketLabs MessageID and MailingID [ https://support.socketlabs.com/kb/48 ]
                msg.Headers.Add("X-xsMessageId", "MyCampaign");
                msg.Headers.Add("X-xsMailingId", "12345");
                
                msg.From = new MailAddress("ben@contoso.com");
                msg.To.Add("jane@contoso.com");
                msg.Subject = "Using the SMTP client with SocketLabs.";
                msg.Body = "<h1>Hello</h1><p>How are you doing today?</p>";
                msg.IsBodyHtml = true;

                smtp.Send(msg);
            }

            Console.WriteLine("Email Sent");
            Console.Read();
        }
    }
}
