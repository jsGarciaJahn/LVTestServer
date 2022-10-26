using System.Text.Json.Serialization;

namespace LVTestClient.Data
{
    public class Post
    {
        [JsonPropertyName("postId")]
        public int? PostId { get; set; }
        [JsonPropertyName("userId")]
        public int? UserId { get; set; }
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
