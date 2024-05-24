using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Authentication;
using Template.Application.Common.Interfaces.Repositories;
using Template.Infrastructure.Common.Persistence;
using Template.Infrastructure.Repositories;
using Template.Infrastructure.Security.PasswordHasher;
using Template.Infrastructure.Security.TokenGenerator;


namespace Template.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services
               .AddPersistence(configuration)
               .AddAuth(configuration)
               .AddSecurity();

            return services;
        }
        private static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(opt =>
            {
                var connectionString = configuration.GetConnectionString("AuthDb");
                opt.UseSqlite(connectionString);
            });            

            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection AddAuth(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.Section, jwtSettings);
            
            //services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));
            services.AddSingleton(Options.Create(jwtSettings));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                });

            return services;
        }

        private static IServiceCollection AddSecurity(
            this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHash, PasswordHash>();
            return services;
        }      


    }
}
