using System;
using System.Threading.Tasks;
using GuestGroupChat.Services.Clients.CommunicationsGateway;
using GuestGroupChat.Services.Clients.CommunicationsGateway.Model;

namespace GuestGroupChat.Services
{
    public class CommunicationsGatewayService: ICommunicationsGatewayService
    {
        private readonly ICommunicationsGatewayClient _communicationsGatewayClient;

        public CommunicationsGatewayService(
            ICommunicationsGatewayClient communicationsGatewayClient)
        {
            _communicationsGatewayClient = communicationsGatewayClient;
        }

        public async Task SendEmail(SendEmailRequestDto sendEmailRequestDto)
        {

                await _communicationsGatewayClient.SendEmail(sendEmailRequestDto);
        }
    }
}