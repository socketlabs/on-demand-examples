using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace SocketLabs.ReportingApi.CSharp
{
	internal class Program
	{
		private static void Main()
		{
			const int serverId = 1000; // YOUR-SERVER-ID
			const string userName = "YOUR-USER-NAME";
			const string password = "YOUR-API-PASSWORD";

			var apiUri = new Uri("https://api.socketlabs.com/v1/messagesProcessed?type=xml&serverId=" + serverId);

			WebRequest request = WebRequest.Create(apiUri).WithBasicAuthentication(userName, password);

			using (WebResponse response = request.GetResponse())
			{
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					XDocument apiResponse = XDocument.Load(reader);

					var addressResponses =
						from item in apiResponse.Descendants("collection").Descendants("item")
						let xToAddress = item.Element("ToAddress")
						where xToAddress != null
						let xResponse = item.Element("Response")
						where xResponse != null
						select new
						       	{
									ToAddress = xToAddress.Value,
						       		Response = xResponse.Value
						       	};

					foreach (var item in addressResponses)
					{
						Console.WriteLine(String.Format("**** (( {0} )) ****", item.ToAddress));
						Console.WriteLine(item.Response);
					}
				}
			}

			Console.Read();
		}
	}

	internal static class WebRequestExtensions
	{
		public static WebRequest WithBasicAuthentication(this WebRequest req, String userName, String userPassword)
		{
			string authInfo = userName + ":" + userPassword;
			authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			req.Headers["Authorization"] = "Basic " + authInfo;
			return req;
		}
	}
}