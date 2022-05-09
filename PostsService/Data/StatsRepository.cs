using PostsService.Models;

namespace PostsService.Data
{
    public class StatsRepository : IStatsRepository
    {

        private readonly AppDbContext _context;

        public StatsRepository(AppDbContext context)
        {
            this._context = context;
        }

        public void SaveStats(Stats stats)
        {
            var existing = this._context.Stats.FirstOrDefault(p => p.PostId == stats.PostId);

            if (existing != null)
            {
                this._context.Stats.Remove(existing);
            }

            this._context.Stats.Add(stats);
        }

        public bool SaveChanges()
        {
            return this._context.SaveChanges() >= 0;
        }

    }
}