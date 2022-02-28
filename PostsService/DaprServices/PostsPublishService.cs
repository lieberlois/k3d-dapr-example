using PostsService.Models;
using Dapr.Client;

namespace PostsService.DaprServices
{
    public class PostsPublishService : IPostsPublishService
    {
        private readonly DaprClient _daprClient;

        public PostsPublishService()
        {
            this._daprClient = new DaprClientBuilder().Build();
        }

        public async Task PublishPost(Post post)
        {
            Console.WriteLine($"Publishing Post {post.Title}");
            await this._daprClient.PublishEventAsync<Post>("pubsub", "posts", post);
        }
    }
}