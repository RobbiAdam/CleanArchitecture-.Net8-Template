using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Mappings;

namespace Template.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            MappingConfig.Configure();

            return services;
        }
    }
}
