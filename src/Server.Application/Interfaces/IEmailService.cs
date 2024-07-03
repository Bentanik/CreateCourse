using Server.Contracts.DTOs.Authentication;

namespace Server.Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendMailRegister(EmailRegisterDTO request);
}
