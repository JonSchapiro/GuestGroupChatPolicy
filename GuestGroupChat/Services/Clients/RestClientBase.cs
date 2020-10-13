using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using GuestGroupChat.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using OT.API.Service.Core.Http.Client.DSL.Commons;
using OT.API.Service.Core.Http.Client.DSL.Steps;
using ServiceInfo = OT.Server.Info.ServiceInfo;

namespace GuestGroupChat.Services.Clients
{
    public abstract class RestClientBase
    {
        private readonly IOptions<AppConfig> _appConfig;
        private readonly IOptions<MetricsConfig> _metricsConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientBuilderFactory _httpClientBuilderFactory;
        private readonly ServiceInfo _serviceInfo;
        private readonly HttpClient _httpClient;

        protected RestClientBase(
            IOptions<AppConfig> appConfig,
            IOptions<MetricsConfig> metricsConfig,
            IHttpContextAccessor httpContextAccessor,
            IHttpClientBuilderFactory httpClientBuilderFactory,
            ServiceInfo serviceInfo,
            HttpClient httpClient
        )
        {
            _appConfig = appConfig;
            _metricsConfig = metricsConfig;
            _httpClientBuilderFactory = httpClientBuilderFactory;
            _serviceInfo = serviceInfo;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.ConnectionClose = false;
        }

        protected IAsyncSendStep<string, string> BuildRequest(
            Func<IHttpClientBuilder, IHttpClientBuilder> configureHttpClient,
            [CallerMemberName] string callerName = ""
        )
        {
            return BuildRequest<string>(configureHttpClient, callerName);
        }

        protected IAsyncSendStep<string, TResponse> BuildRequest<TResponse>(
            Func<IHttpClientBuilder, IHttpClientBuilder> configureHttpClient,
            [CallerMemberName] string callerName = ""
        )
        {
            var clientBuilder = _httpClientBuilderFactory.Builder(_httpClient);
            var configuredClient = configureHttpClient(clientBuilder);

            var circuitBreakerKey =
                $"{configuredClient.MetricActionName}-{configuredClient.MetricApiVersion}-{callerName.ToLower()}";

            clientBuilder.HttpRequestMessageProperties = new Dictionary<string, object>
                { { "CircuitBreakerKey", circuitBreakerKey } };

            var optionsStep = configuredClient
                .Init<TResponse>()
                .Async("guest-group-chat", GetConservedHeaders())
                .With()
                .Headers(CreateDefaultHeaders());

            if (!_metricsConfig.Value.IsEnabled)
            {
                optionsStep.NoMetrics();
            }

            return optionsStep.Build();
        }

        private IMoreHeadersStep CreateDefaultHeaders()
        {
            var headers = OT.API.Service.Core.Http.Client.DSL.Steps.Default.Headers.Add()
                .Header(new Header(HeaderNames.UserAgent, $"{"guest-group-chat"}/{1}"));

            return headers;
        }

        private ISet<Header> GetConservedHeaders()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return (new Dictionary<string, string>() as ISet<Header>)!;
            }

            return _httpContextAccessor.HttpContext.Request.Headers
                .Where(h => ConservedHeaders.Contains(h.Key))
                .Select(h => new Header(h.Key, h.Value.ToString(), true))
                .ToHashSet();
        }

        private static readonly ISet<string> ConservedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            OtHeaderNames.OtRequestId
        };
    }
}