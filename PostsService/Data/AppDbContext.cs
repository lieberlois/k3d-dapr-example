using Microsoft.EntityFrameworkCore;
using PostsService.Models;

namespace PostsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts)
        {

        }

        public DbSet<Post> Posts { get; set; }
    }
}