using LVTestClient.Data;
using Microsoft.AspNetCore.Components;

namespace LVTestClient.Pages
{
    public partial class Index
    {

        private List<Post>? posts;
        private List<Data.User>? users;
        private MetaData metaData { get; set; } = new MetaData();
        private PagingParameters pagingParameters = new PagingParameters();


        private string? newMessage;
        private int? selectedUser;
        private bool isRunning = false;

        protected override async Task OnInitializedAsync()
        {
            await FetchUsers();
            await FetchPosts();
        }

        private async Task FetchUsers()
        {
            users = new();
            var userPagingParams = new PagingParameters { PageSize = 100};
            PagingResponse<Data.User> response;
            do
            {
                response = await client.GetUsersAsync(userPagingParams);
                users.AddRange(response.Items);
                userPagingParams.PageNumber++;
            } while (response.MetaData.HasNext);
            
            users = users.OrderBy(u => u.Name).ToList();
            if (users.Count() > 0)
                selectedUser = users.First().UserId;
        }
        private async Task SelectedPage(int page)
        {
            pagingParameters.PageNumber = page;
            await FetchPosts();
        }

        private async Task FetchPosts(string? searchTerm = "")
        {
            PagingResponse<Post> pagingResponse;
            if (string.IsNullOrEmpty(searchTerm))
                pagingResponse = await client.GetPostsAsync(pagingParameters);
            else
                pagingResponse = await client.SearchPostsAsync(searchTerm, pagingParameters);
            posts = pagingResponse.Items;
            metaData = pagingResponse.MetaData;
        }

        private async Task EditMessage(Post message)
        {
            await client.UpdatePostAsync(message);
            await FetchPosts();
            StateHasChanged();
        }

        private async Task DeleteMessage(Post message)
        {
            await client.DeletePostAsync((int)message.PostId);
            await FetchPosts();
            StateHasChanged();
        }

        private async Task SearchPosts(ChangeEventArgs e)
        {
            string? searchterm = e.Value?.ToString() ?? null;
            pagingParameters.PageNumber = 1;
            await FetchPosts(searchterm);
            StateHasChanged();
        }

        private async Task AddMessage()
        {
            if (string.IsNullOrEmpty(newMessage) || selectedUser is null) return;
            isRunning = true;
            var message = new Post { Content = newMessage, Created = DateTime.Now, UserId = selectedUser };
            await client.CreatePostAsync(message);
            await FetchPosts();
            isRunning = false;
            newMessage = "";
            StateHasChanged();
        }
    }
}
