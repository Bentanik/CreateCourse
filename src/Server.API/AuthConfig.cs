﻿using ChatServer.API.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Server.API;

public static class AuthConfig
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings.GetValue<string>("AccessSecretToken");
        var issuer = jwtSettings.GetValue<string>("Issuer");
        var audience = jwtSettings.GetValue<string>("Audience");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddJwtBearer(options =>
       {
           options.SaveToken = true;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = issuer,
               ValidAudience = audience,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
               ClockSkew = TimeSpan.Zero
           };

           options.Events = new JwtBearerEvents
           {
               OnAuthenticationFailed = context =>
               {
                   if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                   {
                       context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                   }
                   return Task.CompletedTask;
               }
           };
       });


        services.AddAuthorization(options =>
        {
            options.AddPolicy(IdentityData.AdminUserPolicyName,
                p => p.RequireClaim(ClaimTypes.Role, "admin"));

            options.AddPolicy(IdentityData.UserPolicyName,
                p => p.RequireClaim(ClaimTypes.Role, "user"));
        });

        return services;
    }
}