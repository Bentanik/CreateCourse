using AutoMapper;
using ChatServer.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.Interfaces;
using Server.Application.Validations;
using Server.Contracts.Abstractions.Authentication;
using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTOs.Authentication;

namespace Server.API.Controllers;

[ApiController]
[Route("/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;

    public AuthenticationController(IAuthenticationService authenticationService, IMapper mapper)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    [ProducesResponseType(200, Type = typeof(Result<object>))]
    [ProducesResponseType(400, Type = typeof(Result<object>))]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateUserRequest req)
    {
        var validator = new RegisterRequestValidator();
        var validatorResult = validator.Validate(req);
        if (validatorResult.IsValid == false)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Message = "Missing value!",
                Data = validatorResult.Errors.Select(x => x.ErrorMessage),
            });
        }

        var result = await _authenticationService.Register(_mapper.Map<CreateUserDTO>(req));

        return Ok(result);
    }

    [HttpGet("register/active_account/{token}")]
    [ProducesResponseType(200, Type = typeof(Result<object>))]
    [ProducesResponseType(400, Type = typeof(Result<object>))]
    public async Task<IActionResult> ActiveAccount(string token)
    {
        if (token == null)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Message = "Missing value!",
            });
        }

        var result = await _authenticationService.ActiveAccount(token);

        return Ok(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(Result<object>))]
    [ProducesResponseType(400, Type = typeof(Result<object>))]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var validator = new LoginRequestValidator();
        var validatorResult = validator.Validate(req);
        if (validatorResult.IsValid == false)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Message = "Missing value!",
                Data = validatorResult.Errors.Select(x => x.ErrorMessage),
            });
        }

        var result = await _authenticationService.LoginAccount(_mapper.Map<LoginUserDTO>(req));

        if (result?.Error == 1)
        {
            return Ok(result);
        }

        LoginResponse<LoginData> response = result?.Data as LoginResponse<LoginData>;

        if (response == null)
        {
            return Ok(new Result<object>
            {
                Error = 1,
                Message = "Please log in again!",
                Data = null
            });
        }

        Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Path = "/",
            SameSite = SameSiteMode.Strict,
        });

        return Ok(new Result<object>
        {
            Error = 0,
            Message = "Login successfully",
            Data = new
            {
                AccessToken = new {
                    Token_type = "Bearer",
                    Token = response.AccessToken,
                },
                User = response.Data
            }
        });
    }

    [HttpGet("refreshtoken")]
    [ProducesResponseType(200, Type = typeof(Result<object>))]
    [ProducesResponseType(400, Type = typeof(Result<object>))]
    public async Task<IActionResult> RefreshToken()
    {
        string refreshToken = Request.Cookies["refreshToken"];
        if(refreshToken == null)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Message = "Missing value!",
            });
        }
        var result = await _authenticationService.RefreshToken(refreshToken);

        if (result?.Error == 1)
        {
            return Ok(result);
        }

        LoginResponse<LoginData> response = result?.Data as LoginResponse<LoginData>;

        if (response == null)
        {
            return Ok(new Result<object>
            {
                Error = 1,
                Message = "Please log in again!",
                Data = null
            });
        }

        Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Path = "/",
            SameSite = SameSiteMode.Strict,
        });

        return Ok(new Result<object>
        {
            Error = 0,
            Message = "Refresh token successfully",
            Data = new
            {
                AccessToken = new
                {
                    Token_type = "Bearer",
                    Token = response.AccessToken,
                },
                User = response.Data
            }
        });
    }

    [Authorize]
    [RequireRole(IdentityData.UserPolicyName)]
    [HttpDelete("logout")]
    [ProducesResponseType(200, Type = typeof(Result<object>))]
    [ProducesResponseType(400, Type = typeof(Result<object>))]
    public async Task<IActionResult> Logout([FromQuery] Guid userId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Result<object>
            {
                Error = 1,
                Message = "Missing value!",
                Data = null,
            });
        }

        Response.Cookies.Delete("refreshToken");
        var result = await _authenticationService.DeleteRefreshToken(userId);
        return Ok(result);
    }
}
