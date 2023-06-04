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
        public IActionResult GetPostsByStudentId(int id, bool last)
        {
            var response = _repository.GetAllPostsByStudentId(id);
            if(response.Count() == 0) return NotFound();
            if (last) return Ok(response.OrderByDescending(x => x.PostId).FirstOrDefault());
            return Ok(response);
        }
    }
}
