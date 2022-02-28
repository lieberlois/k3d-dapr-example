using PostsService.Models;

namespace PostsService.DaprServices
{
    public interface IUrlService
    {
        Task<UrlResponse> GetUrl(string title);
    }
}