namespace Server.Contracts.Abstractions.Authentication;

public class LoginResponse<T>
{
    public LoginResponse(string accessToken, string refreshToken, T data)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        Data = data;
    }

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public T Data { get; set; }
}
