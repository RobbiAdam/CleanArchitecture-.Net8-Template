﻿using Microsoft.Extensions.DependencyInjection;
using Template.Application.Services.Users;

namespace Template.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}