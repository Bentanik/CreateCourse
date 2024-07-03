namespace Server.Contracts.Abstractions.Authentication;

public class RefreshTokenUserRequest
{
    public RefreshTokenUserRequest(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
    public string RefreshToken { get; set; }
}
