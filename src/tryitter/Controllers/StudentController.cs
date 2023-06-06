using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tryitter.Models;
using tryitter.Repository;
using tryitter.Entities;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace tryitter.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private const string ErrorStudentNotFound = "Student not found";
    private readonly StudentRepository _repository;

    public StudentController(StudentRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public IActionResult CreateStudent(Student student)
    {
        var response = _repository.AddStudent(student);
        if (response == "Email already exists")
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("/Login")]
    public IActionResult Login(StudentLogin studentlogin)
    {
        var response = _repository.Login(studentlogin);
        if (response == "Student not found")
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateStudent(int id, Student student)
    {
        student.StudentId = id;
        string response = _repository.UpdateStudent(student);
        if(response == "student updated")
        {
            return NoContent();
        }
        return BadRequest(response);
    }

    [HttpDelete("{id}")]

    public IActionResult DeleteStudent(int id)
    {
        string response = _repository.DeleteStudent(id);
        if (response == "student deleted")
        {
            return NoContent();
        }
        return BadRequest(response);
    }

    [HttpGet("{id}")]
    public IActionResult GetStudent(int id)
    {
        var student = _repository.GetStudentById(id);
        if (student == null)
        {
            return BadRequest(ErrorStudentNotFound);
        }

        var studentResult = new StudentResponse
        {
            StudentId = student.StudentId,
            Name = student.Name,
            Email = student.Email,
            Status = student.Status,
        };
        return Ok(studentResult);
    }

    [HttpGet("Name")]
    public IActionResult GetStudent(string name)
    {
        var student = _repository.GetStudent(name);
        if (student != null)
        {
            var studentResult = new StudentResponse
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Email = student.Email,
                Status = student.Status,
            };
            return Ok(studentResult);
        }

        return BadRequest(ErrorStudentNotFound);
    }

    [HttpGet]
    public IActionResult GetAllStudents()
    {
        var students = _repository.GetAllStudents();
        return Ok(students);
    }
}