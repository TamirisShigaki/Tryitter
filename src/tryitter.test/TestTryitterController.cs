using Xunit;
using FluentAssertions;
using System.Net.Http.Headers;
using System.Text;
using tryitter.Services;
using tryitter.Models;

namespace tryitter.Test;

public class TestTryitterController : IClassFixture<TestTryitterContext<Program>>
{
    private readonly HttpClient _client;
    public TestTryitterController(TestTryitterContext<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // ! Teste para Students
    [Fact]
    public async Task CreateStudent()
    {
        var student = new Student { Name = "Claudio", Email = "claudio@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * Cria o estudante
        var jsonToAdd = "{\"name\":\"Claudio\",\"email\":\"claudio@gmail.com\",\"password\":\"Senha123*\",\"status\":\"Estudante\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
        var result = await _client.PostAsync("/Student", stringContent);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Student created");

        // * Delete estudante do DB
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        await _client.DeleteAsync("/Student/5");
    }

    [Fact]
    public async Task NotCreateStudentWithExistEmail()
    {
        // * Não cria um estudante com um email que existe no DB
        var jsonToAdd = "{\"name\":\"Tom\",\"email\":\"tom@gmail.com\",\"password\":\"Senha123*\",\"status\":\"Estudando\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
        var result = await _client.PostAsync("/Student", stringContent);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var resultString = result.Content.ReadAsStringAsync().Result;
        resultString.Should().Be("Email already exists");
    }

    [Fact]
    public async Task Login()
    {
        var student = new Student { Name = "Paulo", Email = "paulo@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * Cria estudante
        var jsonToAdd = "{\"name\":\"Paulo\",\"email\":\"paulo@gmail.com\",\"password\":\"Senha123*\",\"status\":\"Estudando\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
        await _client.PostAsync("/Student", stringContent);

        // * Login
        var jsonToAddLogin = "{\"email\":\"paulo@gmail.com\",\"password\":\"Senha123*\"}";
        var stringContentLogin = new StringContent(jsonToAddLogin, Encoding.UTF8, "application/json");
        var resultLogin = await _client.PostAsync("/Login", stringContentLogin);
        resultLogin.StatusCode.Should().Be((System.Net.HttpStatusCode)200);

        // * Reinicia o DB
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        await _client.DeleteAsync("/Student/6");
    }

    [Fact]
    public async Task LoginWithANotExistingStudent()
    {
        var jsonToAddLoginWithError = "{\"email\":\"jhon@gmail\",\"password\":\"Senha\"}";
        var stringContentLogin = new StringContent(jsonToAddLoginWithError, Encoding.UTF8, "application/json");
        var resultLogin = await _client.PostAsync("/Login", stringContentLogin);
        resultLogin.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var resultLoginString = resultLogin.Content.ReadAsStringAsync().Result;
        resultLoginString.Should().Be("Student not found");
    }

    [Fact]
    public async Task GetAllStudents()
    {
        var resultGetAllStudents = await _client.GetAsync("/Student");
        resultGetAllStudents.StatusCode.Should().Be((System.Net.HttpStatusCode)200);

        var getAllStudentsResultString = resultGetAllStudents.Content.ReadAsStringAsync().Result;
        getAllStudentsResultString.Should().Be("[{\"studentId\":1,\"name\":\"Tamiris\",\"email\":\"tamiris@gmail.com\",\"status\":\"Estudando\"},{\"studentId\":2,\"name\":\"Joao\",\"email\":\"joao@gmail.com\",\"status\":\"Estudando\"},{\"studentId\":3,\"name\":\"Binho\",\"email\":\"binho@gmail.com\",\"status\":\"Estudando\"},{\"studentId\":4,\"name\":\"Frida\",\"email\":\"frida@gmail.com\",\"status\":\"Estudando\"}]");
    }

    [Fact]
    public async Task GetStudentById()
    {
        var resultGetStudentById = await _client.GetAsync("/Student/1");
        resultGetStudentById.StatusCode.Should().Be((System.Net.HttpStatusCode)200);

        var getStudentByIdResultString = resultGetStudentById.Content.ReadAsStringAsync().Result;
        getStudentByIdResultString.Should().Be("{\"studentId\":1,\"name\":\"Tamiris\",\"email\":\"tamiris@gmail.com\",\"status\":\"Estudando\"}");
    }

    [Fact]
    public async Task NotGetStudentNotExistId()
    {
        var resultGetStudentById = await _client.GetAsync("/Student/58");
        resultGetStudentById.StatusCode.Should().Be((System.Net.HttpStatusCode)400);

        var getStudentByIdResultString = resultGetStudentById.Content.ReadAsStringAsync().Result;
        getStudentByIdResultString.Should().Be("Student not found");
    }

    [Fact]
    public async Task GetStudentByName()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/Student/Name"),
            Content = new StringContent("{\"name\":\"Joao\"}", Encoding.UTF8, "application/json"),
        };
        var result = await _client.SendAsync(request).ConfigureAwait(false);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var getStudentsResultString = result.Content.ReadAsStringAsync().Result;
        getStudentsResultString.Should().Be("{\"studentId\":2,\"name\":\"Joao\",\"email\":\"joao@gmail.com\",\"status\":\"Estudando\"}");
    }

    [Fact]
    public async Task NotGetStudentNotExistName()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/Student/Name"),
            Content = new StringContent("{\"name\":\"Afonso\"}", Encoding.UTF8, "application/json"),
        };
        var result = await _client.SendAsync(request).ConfigureAwait(false);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var getStudentsResultString = result.Content.ReadAsStringAsync().Result;
        getStudentsResultString.Should().Be("Student not found");
    }

    [Fact]
    public async Task DeleteStudentById()
    {
        var student = new Student { Name = "Adriana", Email = "adriana@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * cria o estudante
        var jsonToAdd = "{\"name\":\"Adriana\",\"email\":\"adriana@gmail.com\",\"password\":\"Senha123*\",\"status\":\"Estudando\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
        var result = await _client.PostAsync("/Student", stringContent);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);

        // * Deleta o estudante
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var resultDeleteStudent = await _client.DeleteAsync("Student/6");
        resultDeleteStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var resultDeleteStudentString = resultDeleteStudent.Content.ReadAsStringAsync().Result;
        resultDeleteStudentString.Should().Be("Student remove");
    }

    [Fact]
    public async Task DeleteStudentErrorNotExitingId()
    {
        var student = new Student { Name = "Adriana", Email = "adriana@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * Deleta o estudante
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var resultDeleteStudent = await _client.DeleteAsync("Student/68");
        resultDeleteStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var resultDeleteStudentString = resultDeleteStudent.Content.ReadAsStringAsync().Result;
        resultDeleteStudentString.Should().Be("Student not found");
    }

    [Fact]
    public async Task DeleteStudentWithoutToken()
    {
        // * Deleta o estudante
        var resultDeleteStudent = await _client.DeleteAsync("Student/1");
        resultDeleteStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
    }

    [Fact]
    public async Task UpdateStudent()
    {
        var student = new Student { Name = "Frida", Email = "frida@gmail.com", Password = "Senha123*", Status = "Estudando" };
        //UpdateStudent
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var jsonToAdd = "{\"name\":\"Tamiriss\",\"email\":\"tamiris@gmail.com\",\"password\":\"Senha123*\",\"status\":\"Estudando\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
        var resultUpdateStudent = await _client.PutAsync("Student/1", stringContent);
        resultUpdateStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var resultUpdateStudentString = resultUpdateStudent.Content.ReadAsStringAsync().Result;
        resultUpdateStudentString.Should().Be("Student updated");
    }

    [Fact]
    public async Task UpdateStudentWithoutToken()
    {
        var jsonToAdd = "{\"name\":\"Tamiriss\",\"email\":\"tamiris@gmail.com\",\"password\":\"Senha123*\",\"status\":\"Estudando\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
        var resultUpdateStudent = await _client.PutAsync("Student/1", stringContent);
        resultUpdateStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
    }

    [Fact]
    public async Task UpdateStudentNotExitEmail()
    {
        var student = new Student { Name = "Tamiriss", Email = "tamiris@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * Deleta estudante
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var jsonToAdd = "{\"name\":\"Tamiriss\",\"email\":\"tami@gmail.com\",\"password\":\"Senha123*\",\"status\":\"Estudando\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
        var resultUpdateStudent = await _client.PutAsync("Student/1", stringContent);
        resultUpdateStudent.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var resultUpdateStudentString = resultUpdateStudent.Content.ReadAsStringAsync().Result;
        resultUpdateStudentString.Should().Be("Email already exists");
    }

    // ! Teste para Post
    [Fact]
    public async Task CreatePost()
    {
        var student = new Student { Name = "Tamiris", Email = "tamiris@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * Cria o Post
        var jsonToAdd = "{\"content\":\"Post aqui\",\"image\":\"string\",\"studentEmail\":\"tamiris@gmail.com\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");

        // * Token
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.PostAsync("/Post", stringContent);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Post Created");

        // * Deleta Post do DB
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("/Post/6"),
            Content = new StringContent("{\"studentEmail\":\"tamiris@gmail.com\"}", Encoding.UTF8, "application/json"),
        };
        await _client.SendAsync(request).ConfigureAwait(false);
    }

    [Fact]
    public async Task CreatePostWithoutToken()
    {
        var jsonToAdd = "{\"content\":\"postagem\",\"image\":\"string\",\"studentEmail\":\"tamiris@gmail.com\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
        var result = await _client.PostAsync("/Post", stringContent);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
    }

    [Fact]
    public async Task UpdatePost()
    {
        var student = new Student { Name = "Tamiris", Email = "tamiris@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * Atualiza Post
        var jsonToAdd = "{\"content\":\"Texto 1\",\"image\":\"string\",\"studentEmail\":\"tamiris@gmail.com\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");

        // * Token
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.PutAsync("/Post/1", stringContent);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Post updated");
    }

    [Fact]
    public async Task UpdatePostWithoutToken()
    {
        var jsonToAdd = "{\"content\":\"Texto 1\",\"image\":\"string\",\"studentEmail\":\"tamiris@gmail.com\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");
        var result = await _client.PutAsync("/Post/1", stringContent);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
    }

    [Fact]
    public async Task UpdateNotStudentPost()
    {
        var student = new Student { Name = "João", Email = "joao@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * Atualiza Post
        var jsonToAdd = "{\"content\":\"Texto\",\"image\":\"string\",\"studentEmail\":\"joao@gmail.com\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");

        // * Token
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.PutAsync("/Post/1", stringContent);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Not Alowed");
    }

    [Fact]
    public async Task UpdateNotExintingPost()
    {
        var student = new Student { Name = "Joao", Email = "joao@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * Atualiza Post
        var jsonToAdd = "{\"content\":\"Texto\",\"image\":\"string\",\"studentEmail\":\"joao@gmail.com\"}";
        var stringContent = new StringContent(jsonToAdd, Encoding.UTF8, "application/json");

        // * Token
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var result = await _client.PutAsync("/Post/53", stringContent);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Post not found");
    }

    [Fact]
    public async Task DeletePost()
    {
        var student = new Student { Name = "Joao", Email = "joao@gmail.com", Password = "Senha123*", Status = "Estudando" };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("/Post/3"),
            Content = new StringContent("{\"studentEmail\":\"joao@gmail.com\"}", Encoding.UTF8, "application/json"),
        };

        // * Token
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // * Deleta Post
        var result = await _client.SendAsync(request).ConfigureAwait(false);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Post deleted");
    }

    [Fact]
    public async Task DeletePostWithoutToken()
    {
        // * Deleta Post
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("/Post/3"),
            Content = new StringContent("{\"studentEmail\":\"joao@gmail.com\"}", Encoding.UTF8, "application/json"),
        };
        var result = await _client.SendAsync(request).ConfigureAwait(false);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
    }

    [Fact]
    public async Task DeleteNotExintPost()
    {
        var student = new Student { Name = "Joao", Email = "joao@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * Token
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // * Deleta Post
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("/Post/99"),
            Content = new StringContent("{\"studentEmail\":\"paulo@gmail.com\"}", Encoding.UTF8, "application/json"),
        };
        var result = await _client.SendAsync(request).ConfigureAwait(false);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Post not found");
    }

    [Fact]
    public async Task DeleteNotStudentPost()
    {
        var student = new Student { Name = "Joao", Email = "joao@gmail.com", Password = "Senha123*", Status = "Estudando" };

        // * Token
        var token = new TokenGenerator().Generate(student);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // * Deleta Post
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("/Post/1"),
            Content = new StringContent("{\"studentEmail\":\"joao@gmail.com\"}", Encoding.UTF8, "application/json"),
        };
        var result = await _client.SendAsync(request).ConfigureAwait(false);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)401);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Not Allowed");
    }

    [Fact]
    public async Task GetPostById()
    {
        var result = await _client.GetAsync("Post/2");
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("{\"postId\":2,\"content\":\"Texto 2\",\"creatAt\":\"2023-01-022T08:50:00\",\"updatetAt\":\"2023-06-04T06:35:00\",\"image\":null,\"studentId\":2}");
    }

    [Fact]
    public async Task GetPostByNotExintId()
    {
        var result = await _client.GetAsync("Post/87");
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
    }

    [Fact]
    public async Task GetAllPosts()
    {
        var result = await _client.GetAsync("Post");
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
    }

    [Fact]
    public async Task GetPostsByStudentId()
    {
        var result = await _client.GetAsync("Post/Student/3");
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("[{\"postId\":4,\"content\":\"Texto 4\",\"creatAt\":\"2023-01-022T08:50:00\",\"updatetAt\":\"2023-06-04T06:35:00\",\"image\":null,\"studentId\":3},{\"postId\":5,\"content\":\"Texto 5\",\"creatAt\":\"2023-01-022T08:50:00\",\"updatetAt\":\"2023-06-04T06:35:00\",\"image\":null,\"studentId\":3}]");
    }

    [Fact]
    public async Task GetPostsByNotExistStudentId()
    {
        var result = await _client.GetAsync("Post/Student/89");
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Student not found");
    }

    [Fact]
    public async Task GetLastPostsByStudentId()
    {
        var result = await _client.GetAsync("Post/Last/Student/3");
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("{\"postId\":5,\"content\":\"Texto 5\",\"creatAt\":\"2023-01-022T08:50:00\",\"updatetAt\":\"2023-06-04T06:35:00\",\"image\":null,\"studentId\":3}");
    }

    [Fact]
    public async Task GetLastPostsByNotExistStudentId()
    {
        var result = await _client.GetAsync("Post/Last/Student/99");
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Student not found");
    }

    [Fact]
    public async Task GetPostsByStudentName()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/Post/StudentName"),
            Content = new StringContent("{\"name\":\"Binho\"}", Encoding.UTF8, "application/json"),
        };
        var result = await _client.SendAsync(request).ConfigureAwait(false);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("[{\"postId\":4,\"content\":\"Texto 40\",\"creatAt\":\"2023-01-022T08:50:00\",\"updatetAt\":\"2023-06-04T06:35:00\",\"image\":null,\"studentId\":3},{\"postId\":5,\"content\":\"Texto 50\",\"creatAt\":\"2023-01-022T08:50:00\",\"updatetAt\":\"2023-06-04T06:35:00\",\"image\":null,\"studentId\":3}]");
    }

    [Fact]
    public async Task GetPostByNotExistStudentName()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/Post/StudentName"),
            Content = new StringContent("{\"name\":\"Binhoo\"}", Encoding.UTF8, "application/json"),
        };
        var result = await _client.SendAsync(request).ConfigureAwait(false);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Student not found");
    }

    [Fact]
    public async Task GetLastPostByStudentName()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/Post/Last/StudentName"),
            Content = new StringContent("{\"name\":\"Binho\"}", Encoding.UTF8, "application/json"),
        };
        var result = await _client.SendAsync(request).ConfigureAwait(false);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)200);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("{\"postId\":5,\"content\":\"Texto 56\",\"creatAt\":\"2023-01-022T08:50:00\",\"updatetAt\":\"2023-06-04T06:35:00\",\"image\":null,\"studentId\":3}");
    }

    [Fact]
    public async Task GetLastPostByNotExistStudentName()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/Post/Last/StudentName"),
            Content = new StringContent("{\"name\":\"Binhoo\"}", Encoding.UTF8, "application/json"),
        };
        var result = await _client.SendAsync(request).ConfigureAwait(false);
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)400);
        var stringResult = result.Content.ReadAsStringAsync().Result;
        stringResult.Should().Be("Student not found");
    }
}