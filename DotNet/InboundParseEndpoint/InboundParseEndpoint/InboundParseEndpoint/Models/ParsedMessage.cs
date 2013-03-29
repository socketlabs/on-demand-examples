using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InboundParseEndpoint.Models
{
    public class ParsedMessage
    {
        public string SecretKey { get; set; }
        public Message Message { get; set; }
        public string InboundMailFrom { get; set; }
        public string InboundRcptTo { get; set; }
        public string InboundIpAddress { get; set; }
        public string ErrorLog { get; set; }
        public float SpamScore { get; set; }
        public string SpamDetails { get; set; }
    }

    public class Message
    {
        public string TextCharSet { get; set; }
        public string HtmlCharSet { get; set; }
        public Attachment[] EmbeddedMedia { get; set; }
        public string MailingId { get; set; }
        public string MessageId { get; set; }
        public string Subject { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public CustomHeader[] CustomHeaders { get; set; }
        public Address[] To { get; set; }
        public Address[] Cc { get; set; }
        public Address[] Bcc { get; set; }
        public Address From { get; set; }
        public Address ReplyTo { get; set; }
        public string Charset { get; set; }
        public Attachment[] Attachments { get; set; }
    }
    public class Address
    {
        public string EmailAddress { get; set; }
        public string FriendlyName { get; set; }
    }

    public class Attachment
    {
        public string Name;
        public string ContentType;
        public string Content;
        public CustomHeader[] CustomHeaders;
        public string ContentId;
    }
    public class CustomHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}