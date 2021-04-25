using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

using Buaa.AIBot.Controllers.Models;

namespace Buaa.AIBot.Services
{
    public interface IUserService
    {
        public string AuthorizeAccount(UserInfo userInfo);
    }

    public class UserService : IUserService
    {
        public UserService(IOptions<TokenManagement> tokenManagement)
        {
            this.tokenManagement = tokenManagement.Value;
        }

        private readonly Dictionary<int, string> roleMap = new Dictionary<int, string>
        {
            {0, "Traveler"},
            {1, "User"},
            {2, "Administrator"}
        };

        private readonly TokenManagement tokenManagement;

        private bool AuthenticateAccount(UserInfo userInfo)
        {
            // TODO
            return true;
        }
        
        public string AuthorizeAccount(UserInfo userInfo)
        {
            string token = string.Empty;
            if(!AuthenticateAccount(userInfo))
            {
                return token;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userInfo.Name),
                new Claim(ClaimTypes.Role, roleMap[userInfo.Auth])
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(tokenManagement.Issuer, tokenManagement.Audience, claims, 
                expires: DateTime.Now.AddMinutes(tokenManagement.AccessExpiration), 
                signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }
    }

    public static class ConfigureUserServices
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.Configure<TokenManagement>(configuration.GetSection("tokenConfig"));
            var token = configuration.GetSection("tokenConfig").Get<TokenManagement>();
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.
                    GetBytes(token.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = token.Issuer,
                    ValidateAudience = true,
                    ValidAudience = token.Audience,
                };
            });

            services.AddAuthorization(option =>
            {
                option.AddPolicy("UserAdmin", policy => policy.RequireRole("User", "Administrator").Build());
                option.AddPolicy("Admin", policy => policy.RequireRole("Administrator").Build());
            });

            services.AddTransient<IUserService, UserService>();
            return services;
        }
    }
}