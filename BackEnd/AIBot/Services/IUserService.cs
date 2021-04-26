using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

using Buaa.AIBot.Controllers.Models;
using Buaa.AIBot.Repository;
using Buaa.AIBot.Repository.Models;

using BNBCrypt = BCrypt.Net.BCrypt;

namespace Buaa.AIBot.Services
{
    public interface IUserService
    {
        Task<string> AuthorizeAccountAsync(UserBody userBody);
    }

    public class UserService : IUserService
    {
        public UserService(IOptions<TokenManagement> tokenManagement, IUserRepository userRepository)
        {
            this.tokenManagement = tokenManagement.Value;
            this.userRepository = userRepository;
        }

        private readonly Dictionary<AuthLevel, string> roleMap = new Dictionary<AuthLevel, string>
        {
            {AuthLevel.None, "Traveler"},
            {AuthLevel.User, "User"},
            {AuthLevel.Admin, "Administrator"}
        };

        private readonly TokenManagement tokenManagement;

        private readonly IUserRepository userRepository;

        private async Task<UserInfo> AuthenticateAccountAsync(UserBody userbody)
        {
            UserInfo userInfo = await userRepository.SelectUserByEmailAsync(userbody.Email);
            if (!(userInfo == null))
            {
                if (BNBCrypt.Verify(userbody.Password, userInfo.Bcrypt))
                {
                    return userInfo;
                }
            }
            return null;
        }
        
        public async Task<string> AuthorizeAccountAsync(UserBody userBody)
        {
            string token = string.Empty;
            UserInfo userInfo = await AuthenticateAccountAsync(userBody);
            if(userInfo == null)
            {
                return token;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, userInfo.Email),
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