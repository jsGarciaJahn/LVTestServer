namespace LVTestServer.Models
{
    public class User
    {
        public int UserId { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public string Name { get; set; }

    }
}
