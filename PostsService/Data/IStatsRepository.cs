using PostsService.Models;

namespace PostsService.Data
{
    public interface IStatsRepository
    {
        void SaveStats(Stats stats);
        bool SaveChanges();
    }
}