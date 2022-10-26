using LVTestClient.Data;

namespace LVTestClient.Pages
{
    public partial class User
    {
        private List<Data.User>? users;
        private MetaData metaData { get; set; } = new MetaData();
        private PagingParameters pagingParameters = new PagingParameters();
        private string? newUser;
        private bool isRunning = false;

        protected override async Task OnInitializedAsync()
        {
            await FetchData();
        }
        private async Task SelectedPage(int page)
        {
            pagingParameters.PageNumber = page;
            await FetchData();
        }

        private async Task FetchData()
        {
            var pagingResponse = await client.GetUsersAsync(pagingParameters);
            users = pagingResponse.Items;
            metaData = pagingResponse.MetaData;
        }

        private async Task AddUser()
        {
            if (string.IsNullOrEmpty(newUser)) return;
            Data.User user = new() { Name = newUser };
            await client.PostUserAsync(user);
            await FetchData();
            newUser = "";
            StateHasChanged();
        }

        private async Task DeleteUser(Data.User user)
        {
            await client.DeleteUserAsync((int)user.UserId);
            await FetchData();
            StateHasChanged();
        }

        private async Task EditUser(Data.User user)
        {
            await client.UpdateUserAsync(user);
            await FetchData();
            StateHasChanged();
        }
    }
}
