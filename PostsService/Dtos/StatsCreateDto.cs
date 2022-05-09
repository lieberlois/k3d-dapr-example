namespace PostsService.Dtos
{
    public class StatsCreateDto
    {
        public int PostId { get; set; }

        public int TitleCount { get; set; }

        public int BodyCount { get; set; }
    }
}