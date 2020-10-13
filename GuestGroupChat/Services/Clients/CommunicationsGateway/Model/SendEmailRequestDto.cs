namespace GuestGroupChat.Services.Clients.CommunicationsGateway.Model
{
    public class SendEmailRequestDto
    {
        public SendEmailRequest SendEmailRequest { get; set; } = null!;
        public string OtDomain { get; set; } = null!;
        public string AcceptLanguage { get; set; } = null!;
    }
}