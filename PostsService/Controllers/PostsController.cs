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
    private readonly IUrlService _urlService;


    public PostsController(IPostsRepository postsRepository, IPostsPublishService postsPublishService, IUrlService urlService)
    {
        this._postsRepository = postsRepository;
        this._postsPublishService = postsPublishService;
        this._urlService = urlService;
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

        Console.WriteLine("Service invocation to UrlService");
        var response = await this._urlService.GetUrl(dto.Title);
        model.Url = response.Url;

        Console.WriteLine("Storing post in database");
        this._postsRepository.CreatePost(model);
        this._postsRepository.SaveChanges();

        Console.WriteLine("Publishing event");
        await this._postsPublishService.PublishPost(model);

        return model;
    }
}
