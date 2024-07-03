using Microsoft.AspNetCore.Http.Features;
using Server.API.Services;
using Server.Application.Interfaces;

namespace Server.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddScoped<IClaimsService, ClaimsService>();
        services.AddHttpContextAccessor();
      
        return services;
    }
}
