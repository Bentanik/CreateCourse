using System;
namespace Server.Contracts.Abstractions.Authentication;

public class LoginData
{
    public LoginData(Guid id, string fullName)
    {
        Id = id;
        FullName = fullName;
    }
    public Guid Id { get; set;}
    public string FullName { get; set; }

}
