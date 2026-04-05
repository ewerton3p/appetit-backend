namespace Appetit.API.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("Appetit.Application");
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"));
            foreach (var type in types)
            {
                var interfaceType = type.GetInterface($"I{type.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
            }
            return services;
        }
    }
}
