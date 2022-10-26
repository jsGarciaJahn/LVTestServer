namespace LVTestServer.Dto
{
    public class PostDto
    {
        public int? PostId { get; set; }
        public int? UserId { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }
    }
}
