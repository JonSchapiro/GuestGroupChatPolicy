using System.Net.Http;
using OT.API.Service.Core.Http.Client.DSL.Steps;

namespace GuestGroupChat.Services.Clients
{
    public interface IHttpClientBuilderFactory
    {
        IHttpClientBuilder Builder(HttpClient httpClient);
    }
}