using Appetit.Infrastructure.Data.Repositories;
using Appetit.Infrastructure.Data.Repositories.Interfaces;

namespace Appetit.API.Extensions
{
    public static class RepositoriesInjectionExtension
    {
        public static IServiceCollection AddRepositoriesInjections(this IServiceCollection services)
        {

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

            var assembly = AppDomain.CurrentDomain.Load("Appetit.Infrastructure");
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"));

            foreach (var type in types)
            {
                var interfaceType = type.GetInterface($"I{type.Name}");
                if (interfaceType != null)
                    services.AddScoped(interfaceType, type);
            }

            return services;
        }
    }
}
