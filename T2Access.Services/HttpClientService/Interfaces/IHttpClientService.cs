using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace T2Access.Services.HttpClientService
{
    public interface IHttpClientService 
    {
        Uri BaseUri { get; set; }

        Task<HttpResponseMessage> GetAsync(string uri, string accept = "application/json", string token = null);
        Task<HttpResponseMessage> PostAsync(string uri, object Model, string accept = "application/json", string token = null);
        Task<HttpResponseMessage> PutAsync(string Route, object Model, string accept = "application/json", string token = null);
        Task<HttpResponseMessage> DeleteAsync(string uri, string token = null);

    }
}
