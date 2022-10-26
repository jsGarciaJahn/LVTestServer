using LVTestServer.Controllers;
using LVTestServer.Dto;
using LVTestServer.Helper;
using LVTestServer.Models;

namespace LVTestServer.Interfaces
{
    public interface IPostRepository
    {
        Post GetPost(int postId);
        PagedList<Post> GetPosts(PostSearchOptions searchOptions, PostFilterOptions filterOptions, PagingParameters pagingParameters);
        bool PostExists(int postId);
        bool CreatePost(Post post);
        bool UpdatePost(Post post);
        bool DeletePost(Post post);
        bool Save();
    }
}
