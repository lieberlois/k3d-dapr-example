using PostsService.Models;

namespace PostsService.Messaging
{
    public interface IUrlService
    {
        Task<UrlResponse> GetUrl(string title);
    }
}