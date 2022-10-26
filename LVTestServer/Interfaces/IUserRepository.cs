using LVTestServer.Controllers;
using LVTestServer.Helper;
using LVTestServer.Models;

namespace LVTestServer.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(int userId);
        User GetUser(string username);
        IQueryable<User> GetAllUsers();
        PagedList<User> GetUsers(PagingParameters pagingParameters);
        PagedList<Post> GetPosts(int userId, PagingParameters pagingParameters);
        bool UserExists(int userId);
        bool UserExists(string username);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}
