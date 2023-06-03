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

        [HttpPost]
        [Route("/posts")]
        public IActionResult CreatePost(Post post)
        {
            var response = _repository.AddPost(post);
            return Ok(response);
        }
    }
}
