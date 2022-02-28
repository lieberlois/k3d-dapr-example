using Microsoft.AspNetCore.Mvc;
using PostsService.Dtos;
using PostsService.Models;
using PostsService.Data;
using PostsService.DaprServices;

namespace PostsService.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{

    private readonly IPostsRepository _postsRepository;
    private readonly IPostsPublishService _postsPublishService;

    public PostsController(IPostsRepository postsRepository, IPostsPublishService postsPublishService)
    {
        this._postsRepository = postsRepository;
        this._postsPublishService = postsPublishService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Post>> GetPosts()
    {
        return Ok(this._postsRepository.GetPosts());
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost(PostCreateDto dto)
    {
        var model = new Post
        {
            Title = dto.Title,
            Body = dto.Body,
        };

        this._postsRepository.CreatePost(model);
        this._postsRepository.SaveChanges();

        await this._postsPublishService.PublishPost(model);

        return model;
    }
}
