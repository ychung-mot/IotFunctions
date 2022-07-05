using System.Text;

namespace IotApis.HttpClients
{
    public interface IApi
    {
        Task<HttpResponseMessage> Get(System.Net.Http.HttpClient client, string path);
        Task<HttpResponseMessage> Post(System.Net.Http.HttpClient client, string path, string body);
        Task<HttpResponseMessage> GetWithRetry(System.Net.Http.HttpClient client, string path);
        Task<HttpResponseMessage> PostWithRetry(System.Net.Http.HttpClient client, string path, string body);

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

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Status Code: {response.StatusCode}");
            }

            return response;
        }

        public async Task<HttpResponseMessage> GetWithRetry(System.Net.Http.HttpClient client, string path)
        {
            var response = await client.GetAsync(path);

            if (!response.IsSuccessStatusCode)
            {
                for (var attempt = 2; attempt <= maxAttempt; attempt++)
                {
                    await Task.Delay(100 * attempt);

                    response = await client.GetAsync(path);

                    if (response.IsSuccessStatusCode)
                    {
                        break;
                    }
                    else if (attempt == maxAttempt)
                    {
                        string message = "";

                        if (response.Content != null)
                        {
                            var bytes = await response.Content.ReadAsByteArrayAsync();
                            message = Encoding.UTF8.GetString(bytes);
                        }

                        throw new Exception($"Status Code: {response.StatusCode}" + Environment.NewLine + message);
                    }
                }
            }

            return response;
        }

        public async Task<HttpResponseMessage> PostWithRetry(System.Net.Http.HttpClient client, string path, string body)
        {
            var response
                = await client.PostAsync(path, new StringContent(body, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
            {
                for (var attempt = 2; attempt <= maxAttempt; attempt++)
                {
                    await Task.Delay(100 * attempt);

                    response = await client.PostAsync(path, new StringContent(body, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        break;
                    }
                    else if (attempt == maxAttempt)
                    {
                        string message = "";

                        if (response.Content != null)
                        {
                            var bytes = await response.Content.ReadAsByteArrayAsync();
                            message = Encoding.UTF8.GetString(bytes);
                        }

                        throw new Exception($"Status Code: {response.StatusCode}" + Environment.NewLine + message);
                    }
                }
            }

            return response;
        }

    }
}
