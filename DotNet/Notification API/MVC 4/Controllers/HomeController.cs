using Newtonsoft.Json;
using NotificationsEndpoint.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace NotificationsEndpoint.Controllers
{
    // TEST - clear memory controller/method
    // TODO - separate out events into arrays per type (perhaps per page)
    // TODO - Create an overview page with counters/links to each event type
    // TEST - pretty print all of the JSON, possibly annotating it (use JSON.Net)
    // TEST - upgrade to MVC 5

    public class HomeController : Controller
    {
        // We're using an in-memory collection to store data in for the purposes of this demo.
        // in a real application, you'd probably be storing these events in a file system or database
        private static List<SocketLabsEvent> _storedEvents;
        private static List<SocketLabsEvent> _unknownEvents;

        private static Dictionary<long, KeyValuePair<string, string>> _config;

        public HomeController()
        {
            if (_storedEvents == null)
                _storedEvents = new List<SocketLabsEvent>();
            if (_unknownEvents == null)
                _unknownEvents = new List<SocketLabsEvent>();
            
            if (_config == null)
            {
                // Load a test key from Web.config, if it exists.
                var server = ConfigurationManager.AppSettings["TestServer"];
                var secretKey = ConfigurationManager.AppSettings["TestSecretKey"];
                var validationKey = ConfigurationManager.AppSettings["TestValidationKey"];

                _config = new Dictionary<long, KeyValuePair<string, string>>
                {
                    {Convert.ToInt64(server), new KeyValuePair<string, string>(secretKey, validationKey)}
                    
                    // Format: Your ServerId, new KeyValuePair<string, string>("YOUR SECRETKEY","YOUR VALIDATIONKEY")
                    // If you have (or might add) multiple servers to your account, using a collection for the config
                    // will allow you to store results for multiple servers with the same endpoint
                    
                    // {serverIsGoesHere, new KeyValuePair<string, string>("YOUR SECRETKEY", "YOUR VALIDATIONKEY")},
                };
            }
        }

        // GET /home/index
        [HttpGet, ActionName("index")]
        public ActionResult GetEvents()
        {
            // This is for DEMO purposes only.  SocketLabs does NOT recommend exposing these events to the outside world
            var data = JsonConvert.SerializeObject(new {_storedEvents, _unknownEvents}, Formatting.Indented);
            ViewBag.Result = new JsonResult {Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            return View();
        }

        // GET /home/index
        [HttpGet, ActionName("reset")]
        public ActionResult Reset()
        {
            _storedEvents.Clear();
            _unknownEvents.Clear();
            var data = JsonConvert.SerializeObject(new { _storedEvents, _unknownEvents }, Formatting.Indented);
            return new JsonResult {Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        // POST /home/index
        [HttpPost, ActionName("Index"), ValidateInput(false)]
        public ActionResult PostEvent(FormCollection formCollection)
        {
            var model = new SocketLabsEvent(formCollection);

            // check that serverId and secret key match
            if (_config.ContainsKey(model.ServerId))
            {
                var config = _config[model.ServerId];
                if (config.Key == model.SecretKey)
                {
                    // basic example of using the different types to do different actions
                    switch (model.Type)
                    {
                        case EventType.Validation:
                            break; // we shouldn't save this, just return the key as per the API requirements
                        case EventType.Delivered:
                            _storedEvents.Add(model);
                            // you might save this in a separate database table or repository from the others
                            break;
                        case EventType.Complaint:
                            _storedEvents.Add(model);
                            // you might save this in a separate database table or repository from the others
                            break;
                        case EventType.Failed:
                            _storedEvents.Add(model);
                            // you might save this in a separate database table or repository from the others
                            break;
                        case EventType.Tracking:
                            _storedEvents.Add(model);
                            // you might save this in a separate database table or repository from the others
                            break;
                        default:
                            _unknownEvents.Add(model);
                            // you might save this in a separate database table or repository from the others
                            break;
                    }

                    return Content(config.Value); // the Validation Key...  not returning this will prevent proper function of your endpoint
                }
            }

            return new HttpUnauthorizedResult(); // ServerId vs SecretKey validation failed
        }

    }
}
