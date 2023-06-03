using Microsoft.AspNetCore.Mvc;
using tryitter.Models;
using tryitter.Repository;

namespace tryitter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostRepository _repository;

        public PostController(PostRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("/Posts")]
        public IActionResult CreatePost(Post post)
        {
            var response = _repository.AddPost(post);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostsByStudentId(int id)
        {
            var response = _repository.GetAllPostsByStudentId(id);
            if(response.Count() == 0) return NotFound();
            return Ok(response);
        }

        [HttpGet]
        [Route("/GetLastPost/{id}")]
        public IActionResult GetLastPostByStudentId(int id)
        {
            var response = _repository.GetLastPost(id);
            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
