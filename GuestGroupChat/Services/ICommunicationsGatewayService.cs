using System.Threading.Tasks;
using GuestGroupChat.Services.Clients.CommunicationsGateway.Model;

namespace GuestGroupChat.Services
{
    public interface ICommunicationsGatewayService
    {
        Task SendEmail(SendEmailRequestDto sendEmailRequestDto);
    }
}