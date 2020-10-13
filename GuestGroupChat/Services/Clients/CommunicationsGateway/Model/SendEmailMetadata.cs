using System;

namespace GuestGroupChat.Services.Clients.CommunicationsGateway.Model
{
    public class SendEmailMetadata
    {
        public long RestaurantId { get; set; }
        public long ConfirmationNumber { get; set; }
        public long? GpId { get; set; }
        public DateTime ReservationDateTime { get; set; }
    }
}