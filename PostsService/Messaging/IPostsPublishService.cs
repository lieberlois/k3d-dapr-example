using PostsService.Models;

namespace PostsService.Messaging
{
    public interface IPostsPublishService
    {
        Task PublishPost(Post post);
    }
}