using System;
using System.IO;
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
            const string apiUrl = @"https://api.socketlabs.com/v1/";
            const string apiMethodCall = "accountData"; // accountData, messagesFailed, messagesQueued, messagesProcessed, messagesFblReported, & messagesOpenClick

            // build the URI
            var apiUri = new Uri(apiUrl + apiMethodCall + "?type=xml&serverId=" + serverId);

            // create the request
            var request = WebRequest.Create(apiUri).WithBasicAuthentication(userName, password);
        
            try
            {
                // Make the api call
                using (var response = request.GetResponse())
                {

                    // process the response
                    var responseData = response.GetResponseStream();
                    if (responseData == null) return;

                    using (var reader = new StreamReader(responseData))
                    {
                        // load and display the data
                        var apiResponse = XDocument.Load(reader);
                        Console.Write(apiResponse); 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  // display any error messages
            }
            finally
            {
                Console.Read(); // keep the console window open 
            }

        }

	}

   

	internal static class WebRequestExtensions
	{
		public static WebRequest WithBasicAuthentication(this WebRequest req, String userName, String userPassword)
		{
			var authInfo = userName + ":" + userPassword;
			authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			req.Headers["Authorization"] = "Basic " + authInfo;
			return req;
		}
	}
}