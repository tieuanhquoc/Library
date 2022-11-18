using Microsoft.Extensions.DependencyInjection;

namespace TieuAnhQuoc.RegisterService;

public static class RegisterService
{
    public static IServiceCollection RegisAllService(this IServiceCollection services, string[] projects,
        string[]? ignoreProjects = null)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName != null && projects.Any(z => x.FullName.Contains(z)))
            .ToList();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes().Where(x => x.Name.EndsWith("Service") || x.Name.EndsWith("Repository"))
                .ToList();

            if (ignoreProjects != null && ignoreProjects.Any())
                types = types.Where(x => !ignoreProjects.Contains(x.Name)).ToList();

            foreach (var type in types)
            {
                if (type.BaseType != null)
                {
                    var interfaceType = type.GetInterfaces().Except(type.BaseType.GetInterfaces()).FirstOrDefault();

                    if (interfaceType != null)
                    {
                        services.AddScoped(interfaceType, type);
                    }
                }
            }
        }

        return services;
    }
}