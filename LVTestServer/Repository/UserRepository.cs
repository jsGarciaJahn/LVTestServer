using LVTestServer.Controllers;
using LVTestServer.Helper;
using LVTestServer.Interfaces;
using LVTestServer.Models;

namespace LVTestServer.Repository
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(ContentContext context)
        {
            _context = context;
        }

        private readonly ContentContext _context;

        public User GetUser(int userId) => _context.Users.Single(user => user.UserId == userId);

        public PagedList<User> GetUsers(PagingParameters pagingParameters) => 
            PagedList<User>.ToPagedList(GetAllUsers(), pagingParameters.PageNumber, pagingParameters.PageSize);

        public IQueryable<User> GetAllUsers() => _context.Users.OrderBy(user => user.UserId);

        public bool UserExists(int userId) => _context.Users.Any(user => user.UserId == userId);

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool Save() => _context.SaveChanges() > 0;

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            var posts = _context.Posts.Where(p => p.User == user);
            foreach (var post in posts)
                _context.Remove(post);
            _context.Remove(user);
            return Save();
        }

        public PagedList<Post> GetPosts(int userId, PagingParameters pagingParameters) =>
            PagedList<Post>.ToPagedList(GetAllPosts(userId), pagingParameters.PageNumber, pagingParameters.PageSize);
        public IQueryable<Post> GetAllPosts(int userId) => _context.Posts.Where(post => post.User.UserId == userId);

        public User GetUser(string username)
        {
            return _context.Users.Single(user => user.Name.Trim().ToUpper() == username.Trim().ToUpper());
        }

        public bool UserExists(string username) => _context.Users.Any(user => user.Name.Trim().ToUpper() == username.Trim().ToUpper());
    }
}
