namespace LVTestServer.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }
    }
}
