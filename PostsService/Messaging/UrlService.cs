using PostsService.Models;
using Dapr.Client;

namespace PostsService.Messaging
{
    public class UrlService : IUrlService
    {
        private readonly DaprClient _daprClient;

        public UrlService()
        {
            this._daprClient = new DaprClientBuilder().Build();
        }

        public async Task<UrlResponse> GetUrl(string title)
        {
            var resp = await this._daprClient.InvokeMethodAsync<object, UrlResponse>(
                "url-depl", "url", new { title = title }
            );

            return resp;
        }
    }
}