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

        public void Index()
        {
        }

        //This Action will receive POSTed parsed messages.
        //ASP.NET MVC3 will automaticlly bind received JSON
        //to the included C# Model "ParsedMessage". These C# objects
        //can then be manipulated and/or persisted as needed.
        [HttpPost]
        public string Index(ParsedMessage parsedMessage)
        {
            //For this demo, just put into an in-memory collection
            ParsedMessages.Add(parsedMessage);
            return "Message Received";
        }

        //To view the contents of received, parsed messages,
        //navigate to /Home/ShowReceivedMessages
        public JsonResult ShowReceivedMessages()
        {
            return Json(ParsedMessages, JsonRequestBehavior.AllowGet);
        }
    }
}
