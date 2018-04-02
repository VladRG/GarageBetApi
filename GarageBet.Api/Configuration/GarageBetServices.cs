using GarageBet.Domain.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using GarageBet.Data.Repositories;
using GarageBet.Data.Interfaces;
using GarageBet.Data;

namespace GarageBet.Api.Configuration
{
    public static class GarageBetServices
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = Configuration.GetSection("Jwt")["Issuer"],
                       ValidAudience = Configuration.GetSection("Jwt")["Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(
                               Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt")["Key"])
                           )
                   };
               });

            services.Configure<JwtConfiguration>(Configuration.GetSection("Jwt"));
        }

        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<DomainConfiguration>(Configuration.GetSection("Database"));
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetSection("Domain")["ConnectionString"]
                    );
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMatchBetRepository, MatchBetRepository>();
        }
    }
}
