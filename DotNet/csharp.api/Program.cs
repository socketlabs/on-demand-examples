using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace SocketLabs.OnDemand.Api
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			int accountId = 1000; // YOUR-ACCOUNT-ID
			string userName = "YOUR-USER-NAME";
			string password = "YOUR-API-PASSWORD";

			var apiUri = new Uri("https://api.socketlabs.com/messagesProcessed?type=xml&accountId=" + accountId);
			var creds = new NetworkCredential(userName, password);
			var auth = creds.GetCredential(apiUri, "Basic");

			WebRequest request = WebRequest.Create(apiUri);
			request.Credentials = auth;

			using (WebResponse response = request.GetResponse())
			using (StreamReader reader = new StreamReader(response.GetResponseStream()))
			{
				XDocument apiResponse = XDocument.Load(reader);

				var addressResponses =
					from item in apiResponse.Descendants("collection").Descendants("item")
					select new {
						ToAddress = item.Element("ToAddress").Value,
						Response = item.Element("Response").Value
					};

				foreach (var item in addressResponses)
				{
					Console.WriteLine(String.Format("**** (( {0} )) ****", item.ToAddress));
					Console.WriteLine(item.Response);
				}
			}

			Console.Read();
		}
	}
}
