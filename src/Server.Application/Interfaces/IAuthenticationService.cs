using Server.Contracts.Abstractions.Shared;
using Server.Contracts.DTOs.Authentication;

namespace Server.Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<object>> Register(CreateUserDTO userDto);
    Task<Result<object>> ActiveAccount(string token);
    Task<Result<object>> LoginAccount(LoginUserDTO userDto);
    Task<Result<object>> RefreshToken(string token);
    Task<Result<object>> DeleteRefreshToken(Guid userId);
}
