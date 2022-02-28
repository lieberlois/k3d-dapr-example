using PostsService.Models;

namespace PostsService.DaprServices
{
    public interface IPostsPublishService
    {
        Task PublishPost(Post post);
    }
}