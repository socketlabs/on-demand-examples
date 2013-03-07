using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotificationsEndpoint.Models;

namespace NotificationsEndpoint.Controllers
{
    public class HomeController : Controller
    {
        // we're using an in-memory collection to store data in for the purposes of this demo.
        // in a real application, you'd probably be storing these events in a file system or database
        private static List<SocketLabsEvent> _storedEvents;
        
        private static Dictionary<long, KeyValuePair<string, string>> _config;

        public HomeController()
        {
            if (_storedEvents == null)
                _storedEvents = new List<SocketLabsEvent>();
            
            if (_config == null)
            {
                _config = new Dictionary<long, KeyValuePair<string, string>>
                        {
                            // Format: Your ServerId, new KeyValuePair<string, string>("YOUR SECRETKEY","YOUR VALIDATIONKEY")
                            // If you have (or might add) multiple servers to your account, using a collection for the config
                            // will allow you to store results for multiple servers with the same endpoint
                            {9998, new KeyValuePair<string, string>("Nth6D3WbYKCyDd7ePekk", "bKeGTASnMjajGcEt9Xfn")},  
                            {9999, new KeyValuePair<string, string>("bKeGTASnMjajGcEt9Xfn", "Nth6D3WbYKCyDd7ePekk")},                      
                        };
            }
        }

        // GET /home/index
        [HttpGet, ActionName("index")]
        public ActionResult GetEvents()
        {
            // This is for DEMO purposes only.  SocketLabs does NOT recommend exposing these events to the outside world
            return Json(_storedEvents, JsonRequestBehavior.AllowGet);
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
                    }

                    return Content(config.Value); // the Validation Key...  not returning this will prevent proper function of your endpoint
                }
            }

            return new HttpUnauthorizedResult(); // ServerId vs SecretKey validation failed
        }

    }
}
