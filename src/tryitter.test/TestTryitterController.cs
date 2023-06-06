using System.Net.Http.Headers;
using System.Text;
using tryitter.Services;
using tryitter.Models;
using FluentAssertions;
using System.Net;
// ok
namespace tryitter.Test;

public class TestController : IClassFixture<TestTryitterContext<Program>>
{
    private readonly HttpClient _client;
    public TestController(TestTryitterContext<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateStudent()
    {

        var student = new Student
        {
            Name = "joao claudio",
            Email = "joao@email.com",
            Password = "12345",
            Status = "Aceleração C#"
        };


        var result = await _client.PostAsync(
          "/Student",
          new StringContent(
            "{\"name\":\"joao claudio\", \"email\":\"email\",\"password\":\"12345\",\"status\":\"Aceleração C#\"}",
            Encoding.UTF8,
            "application/json")
          );

        result.StatusCode.Should().Be((HttpStatusCode)200);

        result.Content.ReadAsStringAsync().Result.Should().Be("Student created");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
          "Bearer",
          new TokenGenerator().Generate(student)
        );
        await _client.DeleteAsync("/Student/5");
    }

    [Fact]
    public async Task CreateStudentFail()
    {
        var teste = await _client.PostAsync(
          "/Student",
          new StringContent(
            "{\"name\":\"joao claudio\", \"email\":\"doidaomatheus@gmail.com\",\"password\":\"12345\",\"status\":\"Aceleração C#\"}",
            Encoding.UTF8,
            "application/json")
          );

        var stringContent = new StringContent(
          "{\"name\":\"joao\",\"email\":\"doidaomatheus@gmail.com\",\"password\":\"1234\",\"status\":\"Aceleração C#\"}",
          Encoding.UTF8,
          "application/json"
        );

        var result = await _client.PostAsync("/Student", stringContent);

        result.StatusCode.Should().Be((HttpStatusCode)400);
        result.Content.ReadAsStringAsync().Result.Should().Be("Email already exists");
    }

    [Fact]
    public async Task LoginStudent()
    {
        var student = new Student
        {
            Name = "pedro",
            Email = "pedro@gmail.com",
            Password = "12345678",
            Status = "Aceleração C#"
        };

        //Create Student  id:6

        await _client.PostAsync(
            "/Student",
            new StringContent(
              "{\"name\":\"pedro\",\"email\":\"pedro@gmail.com\",\"password\":\"12345678\",\"status\":\"Aceleração C#\"}",
              Encoding.UTF8,
              "application/json")
          );

        //login

        var stringContent = new StringContent(
          "{\"email\":\"pedro@gmail.com\",\"password\":\"12345678\"}",
          Encoding.UTF8,
          "application/json"
          );

        var login = await _client.PostAsync("/Login", stringContent);

        login.StatusCode.Should().Be((HttpStatusCode)200);

        login.Content.ReadAsStringAsync().Result.Should().NotBeEmpty();

        //Delete Student in inMemory DB 

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
          "Bearer",
          new TokenGenerator().Generate(student)
          );

        await _client.DeleteAsync("/Student/6");

    }

    [Fact]
    public async Task LoginStudentFail()
    {
        //login

        var stringContent = new StringContent(
          "{\"email\":\"miguel@gmail\",\"password\":\"123456\"}",
          Encoding.UTF8,
          "application/json"
        );

        var login = await _client.PostAsync("/Login", stringContent);

        login.StatusCode.Should().Be((HttpStatusCode)400);
        login.Content.ReadAsStringAsync().Result.Should().Be("Student not found");
    }

    [Fact]
    public async Task GetAllStudents()
    {
        var teste = await _client.PostAsync(
          "/Student",
          new StringContent(
            "{\"name\":\"joao claudio\", \"email\":\"doidaomatheus@gmail.com\",\"password\":\"12345\",\"status\":\"Aceleração C#\"}",
            Encoding.UTF8,
            "application/json")
          );

        var result = await _client.GetAsync("/Student");

        result.StatusCode.Should().Be((HttpStatusCode)200);
    }

    [Fact]
    public async Task DeleteStudentFail()
    {
        var result = await _client.DeleteAsync("/Student/1");
        result.StatusCode.Should().Be((HttpStatusCode)204);
    }

}