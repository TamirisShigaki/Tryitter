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

        [HttpPost("/CreatePost")]
        public IActionResult CreatePost(Post post)
        {
            var response = _repository.AddPost(post);
            return Ok(response);
        }

        [HttpGet("/SearchPostById/{id}")]
        public IActionResult GetPostsByStudentId(int id, bool last)
        {
            var response = _repository.GetAllPostsByStudentId(id);
            if(response.Count() == 0) return NotFound();
            if (last) return Ok(response.OrderByDescending(x => x.PostId).FirstOrDefault());
            return Ok(response);
        }

        [HttpGet("/SearchPostByName/{id}")]
        public IActionResult GetPostsByStudentName(string name, bool last)
        {
            var response = _repository.GetAllPostsByStudentName(name);
            if (response.Count() == 0) return NotFound();
            if (last) return Ok(response.OrderByDescending(x => x.PostId).FirstOrDefault());
            return Ok(response);
        }

        [HttpPut("/UpdatePost/{id}")]
        public IActionResult UpdatePostById(int id, Post post)
        {
            post.PostId = id;
            string response = _repository.UpdatePost(post);
            if(response == "post updated")
            {
                return NoContent();
            }
            return BadRequest(response);
        }

        [HttpDelete("/DeletePost/{id}")]
        public IActionResult DeletePostById(int id)
        {
            string response = _repository.DeletePost(id);
            if(response == "post deleted")
            {
                return NoContent();
            }
            return BadRequest(response);
        }
    }
}
