using Microsoft.AspNetCore.Mvc;
using PostsService.Dtos;
using PostsService.Models;
using PostsService.Data;

namespace PostsService.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{

    private readonly IPostsRepository _postsRepository;

    public PostsController(IPostsRepository postsRepository)
    {
        this._postsRepository = postsRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Post>> GetPosts()
    {
        return Ok(this._postsRepository.GetPosts());
    }

    [HttpPost]
    public ActionResult<Post> CreatePost(PostCreateDto dto)
    {
        var model = new Post
        {
            Title = dto.Title,
            Body = dto.Body,
        };

        this._postsRepository.CreatePost(model);
        this._postsRepository.SaveChanges();

        return model;
    }
}
