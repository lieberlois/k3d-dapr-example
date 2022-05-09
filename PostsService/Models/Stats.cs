using System.Text.Json.Serialization;

namespace PostsService.Models;

public class Stats
{
    public int Id { get; set; }
    public int TitleCount { get; set; }
    public int BodyCount { get; set; }

    // Foreign Key
    public int PostId { get; set; }

    [JsonIgnore]
    public virtual Post Post { get; set; }
}
