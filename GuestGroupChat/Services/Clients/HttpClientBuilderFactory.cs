using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OT.API.Service.Core.Http.Client.DSL.Steps;
using OT.API.Service.Core.Http.Client.DSL.Steps.Default;
using OT.Logging;
using OT.NetCore.Api.Discovery.Client;

namespace GuestGroupChat.Services.Clients
{
    public class HttpClientBuilderFactory : IHttpClientBuilderFactory
    {
        private readonly IDiscoveryClient _discoveryClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfigurationRoot _configurationRoot;
        private readonly ILogger _logger;

        public HttpClientBuilderFactory(
            IDiscoveryClient discoveryClient,
            IHttpContextAccessor httpContextAccessor,
            IConfigurationRoot configurationRoot,
            ILogger logger
        )
        {
            _discoveryClient = discoveryClient;
            _httpContextAccessor = httpContextAccessor;
            _configurationRoot = configurationRoot;
            _logger = logger;
        }

        public IHttpClientBuilder Builder(HttpClient httpClient)
        {
            var builder = HttpClientStep<string, string>.Builder(_logger);
            builder.DiscoveryClient = _discoveryClient;
            builder.HttpContextAccessor = _httpContextAccessor;
            builder.ConfigurationRoot = _configurationRoot;
            builder.HttpClient = httpClient;
            return builder;
        }
    }
}