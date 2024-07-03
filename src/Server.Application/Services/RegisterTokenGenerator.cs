using Server.Application.Utils;
using Server.Contracts.Settings;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Server.Application.Services;

public class RegisterTokenGenerator
{
    private readonly TokenGenerator _tokenGenerators;
    private readonly EmailRegisterSetting _emailRegisterSetting;
    public RegisterTokenGenerator(TokenGenerator tokenGenerators, IOptions<EmailRegisterSetting> emailRegister)
    {
        _tokenGenerators = tokenGenerators;
        _emailRegisterSetting = emailRegister.Value;
    }

    public string GenerateToken(string email)
    {
        List<Claim> claims = new() {
            new Claim(ClaimTypes.Email, email),
        };
        return _tokenGenerators.GenerateToken(_emailRegisterSetting.SecretToken, _emailRegisterSetting.Issuer, _emailRegisterSetting.Audience, _emailRegisterSetting.EmailExpMinute, claims);
    }
}
