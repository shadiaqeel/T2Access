using System.Net.Http;
using System.Threading.Tasks;

namespace T2Access.Services.HttpClientService
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(string Route);
        Task<HttpResponseMessage> PostAsync(string Route, object Model);
        Task<HttpResponseMessage> PutAsync(string Route, object Model);
        Task<HttpResponseMessage> DeleteAsync(string Route);




    }
}
