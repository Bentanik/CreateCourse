using Microsoft.Extensions.DependencyInjection;
using Server.Application.Interfaces;
using Server.Application.Services;
using Server.Application.Utils;
namespace Server.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentTime, CurrentTime>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<TokenGenerator>();
        services.AddSingleton<RegisterTokenGenerator>();
        services.AddSingleton<AccessTokenGenerator>();
        services.AddSingleton<RefreshTokenGenerator>();
        services.AddTransient<IFileSerice, FileService>();    

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICourseService, CourseService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
