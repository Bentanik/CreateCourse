﻿using Server.Application.Utils;
using Server.Contracts.Settings;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Server.Contracts.DTOs.Authentication;

namespace Server.Application.Services;

public class AccessTokenGenerator
{
    private readonly TokenGenerator _tokenGenerators;
    private readonly JwtSettings _jwtSettings;
    public AccessTokenGenerator(TokenGenerator tokenGenerators, IOptions<JwtSettings> jwtSettings)
    {
        _tokenGenerators = tokenGenerators;
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(LoginUserDTO userDto)
    {
        List<Claim> claims = new() {
            new Claim(ClaimTypes.Email, userDto.Email),
            new Claim(ClaimTypes.Role, userDto.Role)
        };
        return _tokenGenerators.GenerateToken(_jwtSettings.AccessSecretToken, _jwtSettings.Issuer, _jwtSettings.Audience, _jwtSettings.AccessTokenExpMinute, claims);
    }
}
