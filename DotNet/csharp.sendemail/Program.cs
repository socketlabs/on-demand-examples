using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Xml.Linq;

namespace SocketLabs.OnDemand.Api
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			string smtpHost = "YOUR-SMTP-HOST";
			int smtpPort = 2525; // standard is port 25 but that is blocked by many ISP's
			string smtpUserName = "YOUR-SMTP-USER-NAME";
			string smtpPassword = "YOUR-SMTP-API-PASSWORD";

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
				// ** can be set in config **

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
