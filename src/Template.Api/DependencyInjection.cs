using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Template.Api.Handlers;

namespace Template.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly));

            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                opt.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddExceptionHandler<ExceptionHandler>();

            return services;
        }
    }
}
