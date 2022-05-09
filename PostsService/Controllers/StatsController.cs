using Dapr;
using Microsoft.AspNetCore.Mvc;
using PostsService.Data;
using PostsService.Dtos;
using PostsService.Models;

namespace StatsService.Controllers;

[ApiController]
[Route("[controller]")]
public class StatsController : ControllerBase
{
    private readonly IStatsRepository _statsRepository;

    public StatsController(IStatsRepository statsRepository)
    {
        this._statsRepository = statsRepository;
    }


    [Topic("pubsub", "stats")]
    [HttpPost]
    public void CreateStats([FromBody] StatsCreateDto stats)
    {
        Console.WriteLine($"Storing stats for post {stats.PostId} in database");

        var stat = new Stats
        {
            PostId = stats.PostId,
            TitleCount = stats.TitleCount,
            BodyCount = stats.BodyCount,
        };

        this._statsRepository.SaveStats(stat);
        this._statsRepository.SaveChanges();
    }
}
