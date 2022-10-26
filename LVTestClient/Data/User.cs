using System.Text.Json.Serialization;

namespace LVTestClient.Data
{
    public class User
    {
        [JsonPropertyName("userId")]
        public int? UserId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
