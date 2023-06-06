using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using tryitter.Entities;
using tryitter.Models;
using tryitter.Services;

namespace tryitter.Repository
{
    public class StudentRepository
    {
        private readonly TryitterContext _context;

        public StudentRepository(TryitterContext context)
        {
            _context = context;
        }

        // * Cria um novo estudante depois de verificar se não tem aluno com o mesmo e-mail
        public string AddStudent(Student studentInput)
        {
            Student studentDB = _context.Students.AsNoTracking().Where(c => c.Email == studentInput.Email).FirstOrDefault();
            if (studentDB != null)
            {
                return "Email already exists";
            }

            var newStudent = new Student
            {
                Name = studentInput.Name,
                Email = studentInput.Email,
                Status = studentInput.Status,
                Password = new Hash(SHA512.Create()).CriptografarSenha(studentInput.Password),
            };
            _context.Students.Add(newStudent);
            _context.SaveChanges();
            return "Student created";
        }

        // * Loga com um estudante e retorna o token
        public string Login(StudentLogin studentLogin)
        {
            var studentdb = _context.Students.AsNoTracking().Where(c => c.Email == studentLogin.Email).FirstOrDefault();
            if (studentdb == null)
            {
                return "Student not found";
            }

            if (studentdb.Email == studentLogin.Email && new Hash(SHA512.Create()).VerificarSenha(studentLogin.Password, studentdb.Password))
            {
                return new TokenGenerator().Generate(studentdb);
            }

            return "Student not found";
        }

        // * Depois de verificar se o e-mail informado não existe no DB. E se o estudante se o estudante que esta sendo atualizado for o mesmo, atualiza o cadastro do estudante
        public string UpdateStudent(int id, Student studentInput)
        {
            var currentStateofStudent = _context.Students.AsNoTracking().Where(c => c.StudentId == id).FirstOrDefault();
            if (currentStateofStudent == null)
            {
                return "Student not found";
            }

            var hasStudantWithThisEmail = _context.Students.AsNoTracking().Where(c => c.Email == studentInput.Email).FirstOrDefault();

            if (currentStateofStudent.Email != studentInput.Email && hasStudantWithThisEmail != null)
            {
                return "Email already exists";
            }

            var student = new Student
            {
                StudentId = id,
                Name = studentInput.Name,
                Email = studentInput.Email,
                Status = studentInput.Status,
                Password = new Hash(SHA512.Create()).CriptografarSenha(studentInput.Password),
            };
            _context.Students.Update(student);
            _context.SaveChanges();
            return "Student updated";
        }

        // *  exclui o estudante e post relacionados (Cascate)
        public string DeleteStudent(Student student)
        {
            var posts = _context.Posts.Where(p => p.StudentId == student.StudentId);
            _context.Students.Remove(student);
            _context.Posts.RemoveRange(posts);
            _context.SaveChanges();
            return "Student remove";
        }

        // * retorna o estudante pelo nome
        public Student GetStudent(string name)
        {
            Student student = _context.Students.FirstOrDefault(x => x.Name == name);
            return student;
        }

        // * retorna o estudante pelo id
        public Student GetStudentById(int id)
        {
            Student student = _context.Students.Find(id);
            return student;
        }

        // * retorna todos os estudantes registrados
        public List<StudentResponse> GetAllStudents()
        {
            var listStudents = new List<StudentResponse>();
            var students = _context.Students.ToList();

            foreach (Student student in students)
            {
                listStudents.Add(new StudentResponse
                {
                    StudentId = student.StudentId,
                    Name = student.Name,
                    Email = student.Email,
                    Status = student.Status
                });
            }

            return listStudents;
        }
    }
}