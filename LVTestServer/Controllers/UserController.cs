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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repository, IMapper mapper)
        {
            _userRepository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public IActionResult GetUsers([FromQuery] PagingParameters pagingParameters)
        {
            var users = _userRepository.GetUsers(pagingParameters);

            var metadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages,
                users.HasNext,
                users.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            var usersDto = _mapper.Map<List<UserDto>>(users);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(usersDto);
        }

        [HttpGet("{username}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public IActionResult GetUser(string username)
        {
            if (!_userRepository.UserExists(username))
                return NotFound();

            return Ok(_mapper.Map<UserDto>(_userRepository.GetUser(username)));
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet("{userId}/posts")]
        [ProducesResponseType(200, Type = typeof(List<PostDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPosts(int userId, PagingParameters pagingParameters)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();

            var posts = _userRepository.GetPosts(userId, pagingParameters);

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

            var postsDto = _mapper.Map<List<PostDto>>(posts);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(postsDto);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateUser([FromBody] UserDto createUser)
        {
            if (createUser is null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = _userRepository.GetAllUsers()
                .Where(u => u.Name.Trim().ToUpper() == createUser.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (users is not null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            var userMap = _mapper.Map<User>(createUser);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Created");
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateUser(int userId, [FromBody] UserDto updateUser)
        {
            if (updateUser is null)
                return BadRequest(ModelState);

            if (userId != updateUser.UserId)
                return BadRequest(ModelState);
            if (!_userRepository.UserExists(userId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(updateUser);
            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "something went wrong updating User");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();
            var deleteUser = _userRepository.GetUser(userId);

            if (!_userRepository.DeleteUser(deleteUser))
            {
                ModelState.AddModelError("", "something went wrong deleting Post");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
