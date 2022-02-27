using PostsService.Models;

namespace PostsService.Data
{
    public class PostsRepository : IPostsRepository
    {

        private readonly AppDbContext _context;

        public PostsRepository(AppDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<Post> GetPosts()
        {
            return this._context.Posts.ToList();
        }

        public void CreatePost(Post post)
        {
            this._context.Posts.Add(post);
        }

        public bool SaveChanges()
        {
            return this._context.SaveChanges() >= 0;
        }
    }
}