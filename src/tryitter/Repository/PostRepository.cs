using System.Data.SqlTypes;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using tryitter.Entities;
using tryitter.Models;
using tryitter.Services;

namespace tryitter.Repository
{
    public class PostRepository
    {
        private readonly TryitterContext _context;

        public PostRepository(TryitterContext context)
        {
            _context = context;
        }

        // * Cria um novo post
        public string AddPost(Post postInput)
        {
<<<<<<< HEAD
            Student student = _context.Students.Where(x => x.StudentId == postInput.StudentId).FirstOrDefault();

            if(student == null)
            {
                return "Student not found";
            }
=======
>>>>>>> 8f107df4e5071ab406d6284ba25ca29194968d26
            var newPost = new Post
            {
                Content = postInput.Content,
                CreatAt = DateTime.Now,
                Image = postInput.Image,
                StudentId = postInput.StudentId,
                UpdatetAt = DateTime.Now
            };
            _context.Posts.Add(newPost);
            _context.SaveChanges();
            return "post created";
        }

        // * Lista todos os posts de um aluno procurando pelo Id
        public IEnumerable<PostVO> GetAllPostsByStudentId(int studentid)
        {
            List<PostVO> posts = new List<PostVO>();
            posts = _context.Posts.Where(x => x.StudentId == studentid).Select(a => new PostVO()
            {
                Content = a.Content,
                CreatAt = a.CreatAt,
                PostId = a.PostId,
                Image = a.Image,
                UpdatetAt = a.UpdatetAt,
                StudentId = a.StudentId,
                StudentName = a.Student.Name
            }).ToList();

            return posts;
        }

        public IEnumerable<PostVO> GetAllPostsByStudentName(string name)
        {
            List<PostVO> posts = new List<PostVO>();
            posts = _context.Posts.Where(x => x.Student.Name == name).Select(a => new PostVO()
            {
                Content = a.Content,
                CreatAt = a.CreatAt,
                PostId = a.PostId,
                Image = a.Image,
                UpdatetAt = a.UpdatetAt,
                StudentId = a.StudentId,
                StudentName = a.Student.Name
            }).ToList();
            return posts;
        }

        public string UpdatePost(Post inputpost)
        {
            Post dbPost = _context.Posts.Where(x => x.PostId == inputpost.PostId).FirstOrDefault();
            if (dbPost != null)
            {
                dbPost.Content = inputpost.Content;
                dbPost.UpdatetAt = DateTime.Now;
                dbPost.Image = inputpost.Image;
                _context.SaveChanges();
                return "post updated";
            }
            return "post not found";
        }

        public string DeletePost(int id)
        {
            Post dbPost = _context.Posts.Where(x => x.PostId == id).FirstOrDefault();
            if (dbPost != null)
            {
                _context.Posts.Remove(dbPost);
                _context.SaveChanges();
                return "post deleted";
            }
            return "post not found";
        }

        public IEnumerable<PostVO> GetAllPosts()
        {
            return _context.Posts.Select(x => new PostVO()
            {
                Content = x.Content,
                CreatAt = x.CreatAt,
                PostId = x.PostId,
                Image = x.Image,
                UpdatetAt = x.UpdatetAt,
                StudentId = x.StudentId,
                StudentName = x.Student.Name
            }).ToList();
        }
    }
}
