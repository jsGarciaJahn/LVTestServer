using AutoMapper;
using LVTestServer.Dto;
using LVTestServer.Interfaces;
using LVTestServer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LVTestServer.Controllers
{
    [Route("api/v0/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository, IUserRepository userRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("{postId}")]
        [ProducesResponseType(200, Type = typeof(PostDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPost(int postId)
        {
            if (!_postRepository.PostExists(postId))
                return NotFound();

            var post = _mapper.Map<PostDto>(_postRepository.GetPost(postId));

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return Ok(post);
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PostDto>))]
        public IActionResult GetPosts([FromQuery] PostSearchOptions searchOptions, [FromQuery] PostFilterOptions filterOptions, [FromQuery] PagingParameters pagingParameters)
        {
            if (!filterOptions.IsValid)
                return BadRequest("Invalid Datevalues");

            var posts = _postRepository.GetPosts(searchOptions, filterOptions, pagingParameters);

            var metadata = new
            {
                posts.TotalCount,
                posts.PageSize,
                posts.CurrentPage,
                posts.TotalPages,
                posts.HasNext,
                posts.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            var postsMap = _mapper.Map<List<PostDto>>(posts);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(postsMap);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreatePost([FromQuery] int userId, [FromBody] PostDto createPost)
        {
            if (createPost is null)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(userId))
                return NotFound();

            var user = _userRepository.GetUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var postMap = _mapper.Map<Post>(createPost);
            postMap.User = user;

            if (!_postRepository.CreatePost(postMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Created");
        }

        [HttpPut("{postId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdatePost(int postId, [FromBody] PostDto updatePost)
        {
            if (updatePost is null)
                return BadRequest(ModelState);

            if (postId != updatePost.PostId)
                return BadRequest(ModelState);
            if (!_postRepository.PostExists(postId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            var postMap = _mapper.Map<Post>(updatePost);
            if (!_postRepository.UpdatePost(postMap))
            {
                ModelState.AddModelError("", "something went wrong updating Post");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{postId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePost(int postId)
        {
            if (!_postRepository.PostExists(postId))
                return NotFound();
            var deletePost = _postRepository.GetPost(postId);

            if (!_postRepository.DeletePost(deletePost))
            {
                ModelState.AddModelError("", "something went wrong deleting Post");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
