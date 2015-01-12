namespace SocketLabs.InjectionApi.CSharp
{
    /// <summary>
    /// This example program shows how to send SMTP messages through HTTP via the SocketLabs
    /// Email On-Demand Injection API. These examples show both simple and more complicated
    /// sample uses. For complete information on the use of all supported parameters, please
    /// see the official API documentation on the SocketLabs website.
    /// </summary>
    internal class Program
    {
        private static void Main()
        {
            // Uncomment this to route all messages through a local Fiddler
            // default installation in order to see outgoing and incoming
            // JSON and XML.
            //WebRequest.DefaultWebProxy = new WebProxy("127.0.0.1", 8888);

            // Used to define the api key used by the samples.
            const string apiKey = "YOUR API KEY HERE";
            const int serverId = 0; // "YOUR SERVER ID HERE";

            // Used to define the API call location.
            const string apiUrl = "https://inject.socketlabs.com/api/v1/email";

            // Simple JSON examples.

            // Example 1 - POST Request via SocketLabs object.
            Samples.SimpleInjectionViaSdkObjectAsJson(serverId, apiKey, apiUrl);

            // Example 2 - POST Request via Key/Value pair composite objects.
            Samples.SimpleInjectionViaRestSharpAsJson(serverId, apiKey, apiUrl);

            // Example 3 - POST Request via raw JSON.
            Samples.SimpleInjectionViaStringAsJson(serverId, apiKey, apiUrl);

            // Simple XML example.

            Samples.SimpleInjectionViaSdkObjectAsXml(serverId, apiKey, apiUrl);

            // Complete JSON example. Multiple recipients and inline mail merge.

            Samples.CompleteInjectionViaSdkObjectAsJson(serverId, apiKey, apiUrl);
        }
    }
}
