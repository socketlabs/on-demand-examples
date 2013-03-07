using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NotificationsEndpoint.Models
{
    /// <summary>
    /// An extended Dictionary to store values
    /// </summary>
    public class SocketLabsEvent
    {
        public SocketLabsEvent()
        {
            DateTime = DateTime.UtcNow;
        }

        public SocketLabsEvent(FormCollection formCollection)
            : this()
        {
            ServerId = Convert.ToInt64(formCollection["ServerId"]);
            Address = formCollection["Address"];

            EventType type;

            if (Enum.TryParse(formCollection["Type"], true, out type))
            {
                formCollection.Remove("Type");
            }
            else
            {
                type = EventType.Unknown;
            }

            Type = type;

            DateTime = Convert.ToDateTime(formCollection["DateTime"]);
            MailingId = formCollection["MailingId"];
            MessageId = formCollection["MessageId"];
            SecretKey = formCollection["SecretKey"];

            formCollection.Remove("ServerId");
            formCollection.Remove("Address");
            formCollection.Remove("DateTime");
            formCollection.Remove("MailingId");
            formCollection.Remove("MessageId");
            formCollection.Remove("SecretKey");

            ExtraData = formCollection.AllKeys.ToDictionary(k => k, v => formCollection[v]);
        }


        public EventType Type { get; set; }

        public DateTime DateTime { get; set; }

        public string MailingId { get; set; }

        public string MessageId { get; set; }

        public string Address { get; set; }

        public long ServerId { get; set; }

        public string SecretKey { get; set; }

        public Dictionary<string, string> ExtraData { get; set; }
    }
}