namespace tryitter.Models;
public class StudentLogin
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public object ValidateProperty(string v)
    {
        throw new NotImplementedException();
    }
}