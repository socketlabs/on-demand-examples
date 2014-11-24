using Newtonsoft.Json;
using NotificationApiEndpoint.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace NotificationApiEndpoint.Api
{
    [RoutePrefix("api/notifications")]
    public class NotificationsController : ApiController
    {
        // We're using an in-memory collection to store data in for the purposes of this demo.
        // in a real application, you'd probably be storing these events in a file system or database
        private static readonly List<SocketLabsEvent> DeliveryEvents = new List<SocketLabsEvent>();
        private static readonly List<SocketLabsEvent> TrackingEvents = new List<SocketLabsEvent>();
        private static readonly List<SocketLabsEvent> UnknownEvents = new List<SocketLabsEvent>();
        private static Dictionary<long, KeyValuePair<string, string>> _config;

        public NotificationsController()
        {
            if (_config == null)
            {
                // Load a test key from Web.config, if it exists.
                var server = ConfigurationManager.AppSettings["TestServer"];
                var secretKey = ConfigurationManager.AppSettings["TestSecretKey"];
                var validationKey = ConfigurationManager.AppSettings["TestValidationKey"];

                _config = new Dictionary<long, KeyValuePair<string, string>>
                {
                    // Add in the test credentials from the Web.Config
                    {Convert.ToInt64(server), new KeyValuePair<string, string>(secretKey, validationKey)}
                    
                    // Format: Your ServerId, new KeyValuePair<string, string>("YOUR SECRETKEY","YOUR VALIDATIONKEY")
                    // If you have (or might add) multiple servers to your account, using a collection for the config
                    // will allow you to store results for multiple servers with the same endpoint
                    
                    // {serverIsGoesHere, new KeyValuePair<string, string>("YOUR SECRETKEY", "YOUR VALIDATIONKEY")},
                };
            }
        }

        private HttpResponseMessage ReturnData(object data)
        {
            return Request.CreateResponse(HttpStatusCode.OK, 
                new
                {
                    Json = data, 
                    DeliveryCount = DeliveryEvents.Count, 
                    TrackingCount = TrackingEvents.Count, 
                    UnknownCount = UnknownEvents.Count
                });
        }

        [HttpGet, Route("")]
        public HttpResponseMessage Get()
        {
            var data = JsonConvert.SerializeObject(new {DeliveryEvents, TrackingEvents, UnknownEvents}, Formatting.Indented);
            return ReturnData(data);
        }

        [HttpGet, Route("delivery")]
        public HttpResponseMessage GetDeliveryEventsOnly()
        {
            var data = JsonConvert.SerializeObject(new { DeliveryEvents }, Formatting.Indented);
            return ReturnData(data);
        }

        [HttpGet, Route("tracking")]
        public HttpResponseMessage GetTrackingEventsOnly()
        {
            var data = JsonConvert.SerializeObject(new { TrackingEvents }, Formatting.Indented);
            return ReturnData(data);
        }

        [HttpGet, Route("unknown")]
        public HttpResponseMessage GetUnknownEventsOnly()
        {
            var data = JsonConvert.SerializeObject(new { UnknownEvents }, Formatting.Indented);
            return ReturnData(data);
        }

        [HttpGet, Route("reset")]
        public HttpResponseMessage Reset()
        {
            DeliveryEvents.Clear();
            TrackingEvents.Clear();
            UnknownEvents.Clear();
            var data = JsonConvert.SerializeObject(new { DeliveryEvents, TrackingEvents, UnknownEvents }, Formatting.Indented);
            return ReturnData(data);
        }

        [HttpPost, Route("~/NotificationEndpoint")]
        public HttpResponseMessage PostEvent(FormDataCollection formCollection)
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
                            DeliveryEvents.Add(model);
                            // you might save this in a separate database table or repository from the others
                            break;
                        case EventType.Complaint:
                            DeliveryEvents.Add(model);
                            // you might save this in a separate database table or repository from the others
                            break;
                        case EventType.Failed:
                            DeliveryEvents.Add(model);
                            // you might save this in a separate database table or repository from the others
                            break;
                        case EventType.Tracking:
                            TrackingEvents.Add(model);
                            // you might save this in a separate database table or repository from the others
                            break;
                        default:
                            UnknownEvents.Add(model);
                            // you might save this in a separate database table or repository from the others
                            break;
                    }

                    // the Validation Key...  not returning this will prevent proper function of your endpoint
                    return Request.CreateResponse(HttpStatusCode.OK, config.Value);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, "Secret Key validation failed!");
        }
    }
}
