using System;

namespace GuestGroupChat.Services.Clients.CommunicationsGateway.Model
{
    public class SendEmailRequest
    {
        public string To { get; set; } = null!;
        public string TemplateName { get; set; } = null!;
        public string[]? Cc { get; set; }
        public string[]? Bcc { get; set; }
        public dynamic TagSet { get; set; } = null!;
        public DateTime DeliverBeforeUtc { get; set; }
        public SendEmailMetadata? Metadata { get; set; }
    }
}