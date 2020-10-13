using System.Threading.Tasks;
using GuestGroupChat.Services.Clients.CommunicationsGateway.Model;

namespace GuestGroupChat.Services.Clients.CommunicationsGateway
{
    public interface ICommunicationsGatewayClient
    {
        Task SendEmail(SendEmailRequestDto sendPushRequestDto);
    }
}