using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InboundParseEndpoint.Models;

namespace InboundParseEndpoint.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<ParsedMessage> ParsedMessages = new List<ParsedMessage>();
        private readonly Dictionary<int, KeysContainer> _keys = new Dictionary<int, KeysContainer>();

        public HomeController()
        {
            // Sample ServerId = 9999, Validation Key = "myValidationKey", Secret Key = "mySecretKey"
            _keys.Add(9999, new KeysContainer
                {
                    SecretKey = "mySecretKey",
                    ValidationKey = "myValidationKey"
                });
        }

        public void Index()
        {
        }

        // This Action will receive POSTed parsed messages.
        // ASP.NET MVC3 will automaticlly bind received JSON
        // to the included C# Model "ParsedMessage". These C# objects
        // can then be manipulated and/or persisted as needed.
        // Note the logic to also return the configured Validation Key when required.
        [HttpPost]
        public string Inbound(ParsedMessage parsedMessage, int serverId = 0, string type = null, string secretKey = null)
        {
            if (type != null && type.ToLower().Contains("validation") && secretKey == _keys[serverId].SecretKey)
            {
                // Return the validation key
                return _keys[serverId].ValidationKey;
            }

            // For this demo, just put into an in-memory collection
            ParsedMessages.Add(parsedMessage);
            return "Message Received";
        }

        // To view the contents of received parsed messages (in a raw form),
        // navigate to /ShowReceivedMessages.
        // In this example, messages are only stored in-memory and will be lost
        // when the app restarts.
        public JsonResult ShowReceivedMessages()
        {
            return Json(ParsedMessages, JsonRequestBehavior.AllowGet);
        }
    }

    public class KeysContainer
    {
        public string SecretKey { get; set; }
        public string ValidationKey { get; set; }
    }
}
