using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace T2Access.Services.HttpClientService
{
    public class HttpClientService : IHttpClientService
    {

        //  public HttpClient HttpClient { get; protected set; }

        public static Uri BaseUri { get; protected set; }


        public HttpClientService(Uri baseUri)
        {
            BaseUri = baseUri;
        }


        public  async Task<HttpResponseMessage> GetAsync(string Route)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = BaseUri;

                var httpResponse = await httpClient.GetAsync(new Uri(Route));

                return httpResponse;
            }
        }
        public  async Task<HttpResponseMessage> PostAsync(string Route, object Model)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = BaseUri;

                var uri = new Uri(Route);
                
                string jsonTransport = JsonConvert.SerializeObject(Model);
                var jsonPayload = new StringContent(jsonTransport, Encoding.UTF8, "application/json");
                var httpResponse = await httpClient.PostAsync(uri, jsonPayload);
                

                return httpResponse;
            }
        }


        public  async Task<HttpResponseMessage> PutAsync(string Route, object Model)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = BaseUri;

                var uri = new Uri(Route);
                string jsonTransport = JsonConvert.SerializeObject(Model);
                var jsonPayload = new StringContent(jsonTransport, Encoding.UTF8, "application/json");
                var httpResponse = await httpClient.PutAsync(uri, jsonPayload);
                return httpResponse;
            }
        }
        public  async Task<HttpResponseMessage> DeleteAsync(string Route)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = BaseUri;

                var httpResponse = await httpClient.DeleteAsync(new Uri(Route));
                return httpResponse;
            }
        }


    }
}
