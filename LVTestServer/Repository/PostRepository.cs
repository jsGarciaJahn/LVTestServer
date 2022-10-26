using LVTestServer.Controllers;
using LVTestServer.Helper;
using LVTestServer.Interfaces;
using LVTestServer.Models;

namespace LVTestServer.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly ContentContext _context;
        public PostRepository(ContentContext context)
        {
            _context = context;
        }

        public bool CreatePost(Post post)
        {
            _context.Add(post);
            return Save();
        }

        public bool DeletePost(Post post)
        {
            _context.Remove(post);
            return Save();
        }

        public Post GetPost(int postId) => _context.Posts.Single(post => post.PostId == postId);

        public bool PostExists(int postId) => _context.Posts.Any(post => post.PostId == postId);

        public bool Save() => _context.SaveChanges() > 0;

        public PagedList<Post> GetPosts(PostSearchOptions searchOptions, PostFilterOptions filterOptions, PagingParameters pagingParameters)
        {

            IQueryable<Post> query = _context.Posts.Where(p => p.Created < filterOptions.DateMax);
            if (filterOptions.DateMin is not null)
                query = query.Where(p => p.Created > filterOptions.DateMin);
            
            if (!string.IsNullOrEmpty(searchOptions.FromUser))
                query = query.Where(p => p.User.Name.Contains(searchOptions.FromUser));
            
            if (!string.IsNullOrEmpty(searchOptions.ContainsKeyword))
                query = query.Where(p => p.Content.Contains(searchOptions.ContainsKeyword));
            
            if (filterOptions.SortDescending)
                query = query.OrderByDescending(p => p.Created);

            return PagedList<Post>.ToPagedList(query, pagingParameters.PageNumber, pagingParameters.PageSize);
        }

        public IQueryable<Post> GetAllPosts() => _context.Posts;

        public bool UpdatePost(Post post)
        {
            _context.Update(post);
            return Save();
        }
    }
}
