using System;
using System.Threading;
using GuestGroupChat.EventHandlers;
using GuestGroupChat.Services;
using GuestGroupChat.Services.Clients.CommunicationsGateway;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OT.EDA.Bus;
using OT.EDA.Enums;
using OT.EDA.Logging;

namespace GuestGroupChat
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var idInfo = new IdentifyingInformation(
                "communications",
                "Comms.Reservation.Reminder.Policy",
                "communications-team@opentable.com");
            
            var busOpts = new BusOptions()
                .SetIdentifyingInformation(idInfo)
                .SetLogger(EDALogger.Global)
                .SetRegion(Region.RS);

            busOpts.SetDeployEnvironment(DeployEnvironment.PP);
            bool connected = false;
            int attempt = 1;
            while (!connected && attempt < 5)
            {
                try
                {
                    services.AddSingleton<IBus>(new Bus(busOpts));
                    connected = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error contacting EDA ({e.Message})");
                    Console.WriteLine(e);
                    attempt++;
                    Thread.Sleep(TimeSpan.FromSeconds(attempt * 10));
                }
            }
            
            if (!connected)
                throw new ApplicationException("Could not connect to EDA (I did try a few times but it would not answer)");

            services.AddSingleton<IEdaSource, EdaSource>();

            services.AddSingleton<IConsumeFromEda, ReservationConsumer>();

            services.AddSingleton<IHostedService, GuestGroupChat>();
            services.AddTransient<ICommunicationsGatewayClient, CommunicationsGatewayClient>();
            services.AddTransient<ICommunicationsGatewayService, CommunicationsGatewayService>();
        }

        public void Configure()
        {
            
        }
    }
}