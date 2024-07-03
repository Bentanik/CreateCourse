namespace Server.Contracts.Abstractions.Test;

public class TestRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    
    public TestRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
