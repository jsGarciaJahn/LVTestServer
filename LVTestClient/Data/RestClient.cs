using LVTestServer.Controllers;
using System.Text.Json;

namespace LVTestClient.Data
{
    public class Restclient
    {
        private readonly HttpClient _client;
        public Restclient(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Server");
        }

        public async Task CreatePostAsync(Post post) =>
     await _client.PostAsJsonAsync($"post?userId={post.UserId}", post);

        public async Task DeletePostAsync(int postId) =>
            await _client.DeleteAsync($"post/{postId}");

        public async Task<Post> GetPostAsync(int postId) =>
            await _client.GetFromJsonAsync<Post>($"post/{postId}");

        public async Task<PagingResponse<Post>> GetPostsAsync(PagingParameters pagingParameters)
        {
            var response = await _client.GetAsync($"post?pagenumber={pagingParameters.PageNumber}&SortDescending=true");
            var content = await response.Content.ReadAsStringAsync();
            return new PagingResponse<Post>
            {
                Items = JsonSerializer.Deserialize<List<Post>>(content),
                MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First())
            };
        }

        public async Task<PagingResponse<Post>> SearchPostsAsync(string match, PagingParameters pagingParameters)
        {
            var response = await _client.GetAsync($"post?containsKeyword={match}&pagenumber={pagingParameters.PageNumber}&SortDescending=true");
            var content = await response.Content.ReadAsStringAsync();
            return new PagingResponse<Post>
            {
                Items = JsonSerializer.Deserialize<List<Post>>(content),
                MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First())
            };
        }

        public async Task UpdatePostAsync(Post post) =>
            await _client.PutAsJsonAsync($"post/{post.PostId}", post);

        public async Task DeleteUserAsync(int userId) =>
           await _client.DeleteAsync($"user/{userId}");

        public async Task<User> GetUserAsync(string username) =>
            await _client.GetFromJsonAsync<User>($"user/{username}");

        public async Task<User> GetUserAsync(int userId) =>
            await _client.GetFromJsonAsync<User>($"user/{userId}");

        public async Task<PagingResponse<User>> GetUsersAsync(PagingParameters pagingParameters)
        {
            var response = await _client.GetAsync($"user?pagenumber={pagingParameters.PageNumber}");
            var content = await response.Content.ReadAsStringAsync();
            return new PagingResponse<User>
            {
                Items = JsonSerializer.Deserialize<List<User>>(content),
                MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First())
            };
        }

        public async Task PostUserAsync(User user) =>
            await _client.PostAsJsonAsync($"user", user);

        public async Task UpdateUserAsync(User user) =>
            await _client.PutAsJsonAsync($"user/{user.UserId}", user);
    }
}
