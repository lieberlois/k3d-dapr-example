using PostsService.Models;

namespace PostsService.Messaging
{
    public class RedisPostsPublishService : IPostsPublishService
    {

        public async Task PublishPost(Post post)
        {
            throw new NotImplementedException();
            // await this._RedisClient.PublishEventAsync<Post>("pubsub", "posts", post);
        }
    }
}