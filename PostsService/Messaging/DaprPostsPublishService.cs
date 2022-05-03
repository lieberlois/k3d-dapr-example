using PostsService.Models;
using Dapr.Client;

namespace PostsService.Messaging
{
    public class DaprPostsPublishService : IPostsPublishService
    {
        private readonly DaprClient _daprClient;

        public DaprPostsPublishService()
        {
            this._daprClient = new DaprClientBuilder().Build();
        }

        public async Task PublishPost(Post post)
        {
            await this._daprClient.PublishEventAsync<Post>("pubsub", "posts", post);
        }
    }
}