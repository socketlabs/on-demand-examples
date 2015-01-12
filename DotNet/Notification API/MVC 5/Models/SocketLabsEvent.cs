using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;

namespace SocketLabs.NotificationApi.TestEndpoint.Mvc5.Models
{
    /// <summary>
    /// An extended Dictionary to store values
    /// </summary>
    public class SocketLabsEvent
    {
        public EventType Type { get; set; }
        public long ServerId { get; set; }
        public string Address { get; set; }
        public DateTime DateTime { get; set; }
        public string MailingId { get; set; }
        public string MessageId { get; set; }
        public string SecretKey { get; set; }
        public Dictionary<string, string> ExtraData { get; set; }

        public SocketLabsEvent()
        {
            DateTime = DateTime.UtcNow;
        }

        public SocketLabsEvent(FormDataCollection formCollection)
            : this()
        {
            // Extract known members from the form collection
            ServerId = Convert.ToInt64(formCollection["ServerId"]);
            Address = formCollection["Address"];
            DateTime = Convert.ToDateTime(formCollection["DateTime"]);
            MailingId = formCollection["MailingId"];
            MessageId = formCollection["MessageId"];
            SecretKey = formCollection["SecretKey"];
            EventType type;
            if (!Enum.TryParse(formCollection["Type"], true, out type))
                type = EventType.Unknown;
            Type = type;

            // Find all additional members from the form collection
            var dictionary = new Dictionary<string, string>();
            foreach (var entry in formCollection)
            {
                if (entry.Key != "Type" &&
                    entry.Key != "ServerId" &&
                    entry.Key != "Address" &&
                    entry.Key != "DateTime" &&
                    entry.Key != "MailingId" &&
                    entry.Key != "MessageId" &&
                    entry.Key != "SecretKey")
                {
                    dictionary[entry.Key] = entry.Value;
                }
            }

            ExtraData = dictionary;
        }
    }
}