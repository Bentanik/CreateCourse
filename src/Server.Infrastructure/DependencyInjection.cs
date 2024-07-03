using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Application.Interfaces;
using Server.Application.Repositories;
using Server.Application;
using Server.Contracts.Settings;
using Server.Infrastructure.Repositories;
using Server.Infrastructure.Services;

namespace Server.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICourseContentRepository, CourseContentRepository>();

        #region Configuration
        services.Configure<EmailSetting>(configuration.GetSection(EmailSetting.SectionName));
        services.Configure<EmailRegisterSetting>(configuration.GetSection(EmailRegisterSetting.SectionName));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<CloudinarySetting>(configuration.GetSection(CloudinarySetting.SectionName));
        #endregion
        // Database Sql
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}
