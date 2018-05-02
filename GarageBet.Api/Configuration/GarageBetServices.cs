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

            services.AddAuthorization(configuration =>
            {
                configuration.AddPolicy("CanCloseMatches", policy =>
                {
                    policy.RequireClaim("CanCloseMatches", "true");
                });
            });

            services.Configure<JwtConfiguration>(Configuration.GetSection("Jwt"));
        }

        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<DomainConfiguration>(Configuration.GetSection("Database"));
            services.AddDbContext<DataContext>(options =>
            {
                options.UseMySql(Configuration.GetSection("Domain")["ConnectionString"], config =>
                {
                    config.MigrationsAssembly("GarageBet.Api");
                });
            });

            ConfigureRepositoryDependencies(services);
        }

        private static void ConfigureRepositoryDependencies(IServiceCollection services)
        {
            services.AddScoped<IBetRepository, BetRepository>();
            services.AddScoped<IChampionshipRepository, ChampionshipRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
