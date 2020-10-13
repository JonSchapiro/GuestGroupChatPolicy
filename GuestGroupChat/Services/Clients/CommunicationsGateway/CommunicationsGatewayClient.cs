using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using GuestGroupChat.Config;
using GuestGroupChat.Services.Clients.CommunicationsGateway.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using OT.API.Service.Core.Http.Client.DSL.Steps;
using ServiceInfo = OT.Server.Info.ServiceInfo;

namespace GuestGroupChat.Services.Clients.CommunicationsGateway
{
    public class CommunicationsGatewayClient : RestClientBase, ICommunicationsGatewayClient
    {
        public CommunicationsGatewayClient(
            IOptions<AppConfig> appConfig, 
            IOptions<MetricsConfig> metricsConfig, 
            IHttpContextAccessor httpContextAccessor, 
            IHttpClientBuilderFactory httpClientBuilderFactory, 
            ServiceInfo serviceInfo, 
            HttpClient httpClient)
            : base(appConfig, metricsConfig, httpContextAccessor, httpClientBuilderFactory, serviceInfo, httpClient)
        {
        }
        public async Task SendEmail(SendEmailRequestDto sendEmailRequestDto)
        {
            var queryString = string.Empty;

            var response = await BuildRequest(
                    ConfigureHttpClientForSendCommunication(
                        sendEmailRequestDto.OtDomain,
                        sendEmailRequestDto.AcceptLanguage,
                        queryString,
                        Channel.Email
                    )
                )
                .PutAsync(sendEmailRequestDto.SendEmailRequest);

            if (!response.IsSuccess)
            {
                return;
            }
        }
        
        private Func<IHttpClientBuilder, IHttpClientBuilder> ConfigureHttpClientForSendCommunication(
            string otDomain,
            string acceptLanguage,
            string queryString,
            Channel channel
        )
        {
            return clientBuilder =>
            {
                var query = string.IsNullOrEmpty(queryString) ? string.Empty : $"/{queryString}";
                var communication = channel.ToString().ToLower();
                ConfigureHttpClientDefault(clientBuilder);
                clientBuilder.Path = $"v1/send/{communication}{query}";
                clientBuilder.MetricApiVersion = "v1";
                clientBuilder.MetricActionName = $"send{communication}";
                clientBuilder.ContentType = MediaTypeNames.Application.Json;
                clientBuilder.Headers = new Dictionary<string, string>
                {
                    { OtHeaderNames.OtDomain, otDomain },
                    { HeaderNames.AcceptLanguage, acceptLanguage }
                };

                return clientBuilder;
            };
        }
        
        private void ConfigureHttpClientDefault(IHttpClientBuilder clientBuilder)
        {
            clientBuilder.Host = "srvc://comms-gateway";
            clientBuilder.ReadTimeout = 5;
        }
    }
}