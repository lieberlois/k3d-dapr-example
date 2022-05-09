using PostsService.Models;

namespace PostsService.Data
{
    public interface IPostsRepository
    {
        IEnumerable<Post> GetPosts();
        void CreatePost(Post post);
        bool SaveChanges();
    }
}