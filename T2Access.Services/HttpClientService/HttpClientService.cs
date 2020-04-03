using System;
using System.Net.Http;
using System.Net.Http.Headers;
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


        public async Task<HttpResponseMessage> GetAsync(string uri, string accept = "application/json", string token = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = BaseUri;

                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var httpResponse = await httpClient.GetAsync(uri);

                return httpResponse;
            }
        }





        public async Task<HttpResponseMessage> PostAsync(string uri, object Model, string accept = "application/json", string token = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = BaseUri;
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var jsonPayload = new StringContent(JsonConvert.SerializeObject(Model), Encoding.UTF8, "application/json");
                var httpResponse = await httpClient.PostAsync(uri, jsonPayload);



                return httpResponse;
            }
        }


        public async Task<HttpResponseMessage> PutAsync(string uri, object Model, string accept = "application/json", string token = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = BaseUri;
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                string jsonTransport = JsonConvert.SerializeObject(Model);
                var jsonPayload = new StringContent(jsonTransport, Encoding.UTF8, "application/json");
                var httpResponse = await httpClient.PutAsync(uri, jsonPayload);
                return httpResponse;
            }
        }



        public async Task<HttpResponseMessage> DeleteAsync(string uri, string token = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = BaseUri;
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var httpResponse = await httpClient.DeleteAsync(uri);
                return httpResponse;
            }
        }


    }
}
