using System.Text;

namespace IotApis.HttpClients
{
    public interface IApi
    {
        Task<HttpResponseMessage> Get(System.Net.Http.HttpClient client, string path);
        Task<HttpResponseMessage> Post(System.Net.Http.HttpClient client, string path, string body);
    }
    public class Api : IApi
    {
        const int maxAttempt = 5;

        public async Task<HttpResponseMessage> Get(System.Net.Http.HttpClient client, string path)
        {
            var response = await client.GetAsync(path);

            return response;
        }

        public async Task<HttpResponseMessage> Post(System.Net.Http.HttpClient client, string path, string body)
        {
            var response
                = await client.PostAsync(path, new StringContent(body, Encoding.UTF8, "application/json"));

            return response;
        }
    }
}
