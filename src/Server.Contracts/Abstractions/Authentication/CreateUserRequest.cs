namespace Server.Contracts.Abstractions.Authentication;

public class CreateUserRequest
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public CreateUserRequest(string fullName, string email, string password)
    {
        FullName = fullName;
        Password = password;
        Email = email;
    }
}
