using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Server.Application.Interfaces;
using Server.Contracts.Abstractions.Authentication;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTOs.Authentication;
using Server.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly RegisterTokenGenerator _registerTokenGenerator;
    private readonly AccessTokenGenerator _accessTokenGenerator;
    private readonly RefreshTokenGenerator _refreshTokenGenerator;

    public AuthenticationService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper, IEmailService emailService, RegisterTokenGenerator registerTokenGenerator, IConfiguration configuration, AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _emailService = emailService;
        _registerTokenGenerator = registerTokenGenerator;
        _configuration = configuration;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<Result<object>> Register(CreateUserDTO userDto)
    {
        var isCheckDuplicateEmail = await _unitOfWork.userRepository.GetUserByEmail(userDto.Email);
        if (isCheckDuplicateEmail != null)
        {
            return new Result<object>
            {
                Error = 1,
                Message = "This email already exists, please check again!",
                Data = null,
            };
        }
        userDto.Password = _passwordHasher.HashPassword(userDto.Password);
        var userMapper = _mapper.Map<User>(userDto);
        await _unitOfWork.userRepository.AddAsync(userMapper);
        var result = await _unitOfWork.SaveChangeAsync() > 0 ? true : false;
        if (result == false)
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Registration error, please register again!",
                Data = null,
            };
        };
        var tokenGenerate = _registerTokenGenerator.GenerateToken(userDto.Email);
        EmailRegisterDTO email = new()
        {
            To = userDto.Email,
            Subject = "Register account AppChat",
            Body = $"Please click this link to active account: {_configuration["Client_URL:Url"]}/{_configuration["Client_URL:Active_Account"]}/{tokenGenerate}"
        };
        result = await _emailService.SendMailRegister(email);
        return new Result<object>
        {
            Error = result ? 0 : 1,
            Message = result ? "Registration successful, please check your email to activate your account" : "Registration error, please register again!",
            Data = null,
        };
    }

    public async Task<Result<object>> ActiveAccount(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["EmailRegisterSetting:SecretToken"]);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            var claims = principal.Claims;
            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await _unitOfWork.userRepository.GetUserByEmail(emailClaim);
            if (user == null || user?.Active == true)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Active failed, please try again!",
                    Data = null
                };
            }

            user.Active = true;
            _unitOfWork.userRepository.Update(user);
            var result = await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Register successfully" : "Active failed, please try again!",
                Data = null
            };
        }
        catch
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Active failed, please try again!",
                Data = null
            };
        }
    }

    public async Task<Result<object>> LoginAccount(LoginUserDTO userDto)
    {
        var user = await _unitOfWork.userRepository.GetUserByEmail(userDto.Email);
        if (user == null)
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Email does not exist!",
                Data = null,
            };
        }

        if (user.Active == false)
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Please active account in email",
                Data = null,
            };
        }

        var isCheckPassword = _passwordHasher.VerifyPassword(userDto.Password, user.Password);
        if (isCheckPassword == false)
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Password is wrong, please enter again!",
                Data = null,
            };
        }

        var accessToken = _accessTokenGenerator.GenerateToken(userDto);
        var refreshToken = _refreshTokenGenerator.GenerateToken(userDto);

        user.RefreshToken = refreshToken;
        _unitOfWork.userRepository.Update(user);
        var result = await _unitOfWork.SaveChangeAsync();

        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? "Login successfully" : "Login fail, please again!",
            Data = result > 0 ? new LoginResponse<LoginData>(accessToken, refreshToken, new LoginData(user.Id, user.FullName)) : null,
        };
    }

    public async Task<Result<object>> RefreshToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshSecretToken"]);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            var claims = principal.Claims;
            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await _unitOfWork.userRepository.GetUserByEmail(emailClaim);
            if (user == null || token != user.RefreshToken)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Get token fail, please try again!",
                    Data = null
                };
            }
            var loginUserDTO = _mapper.Map<LoginUserDTO>(user);
            if (user.RoleCodeId == 0) loginUserDTO.Role = "admin";
            else loginUserDTO.Role = "user";

            string newRefreshToken = _refreshTokenGenerator.GenerateToken(loginUserDTO);
            string newAccessToken = _accessTokenGenerator.GenerateToken(loginUserDTO);

            user.RefreshToken = newRefreshToken;
            _unitOfWork.userRepository.Update(user);
            var result = await _unitOfWork.SaveChangeAsync();
            return new Result<object>
            {
                Error = result > 0 ? 0 : 1,
                Message = result > 0 ? "Get token successfully" : "Get token fail, please try again!",
                Data = result > 0 ? new LoginResponse<LoginData>(newAccessToken, newRefreshToken, new LoginData(user.Id, user.FullName)) : null,
            };
        }
        catch
        {
            return new Result<object>
            {
                Error = 1,
                Message = "Active failed, please try again!",
                Data = null
            };
        }
    }

    public async Task<Result<object>> DeleteRefreshToken(Guid userId)
    {
        var user = await _unitOfWork.userRepository.GetByIdAsync(userId);
        user.RefreshToken = null;
        _unitOfWork.userRepository.Update(user);
        var result = await _unitOfWork.SaveChangeAsync();
        return new Result<object>
        {
            Error = result > 0 ? 0 : 1,
            Message = result > 0 ? "Log out success" : "Log out fail",
            Data = null
        };
    }
}
