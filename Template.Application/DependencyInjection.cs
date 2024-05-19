using Microsoft.Extensions.DependencyInjection;

namespace Template.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            
            return services;
        }
    }
}
