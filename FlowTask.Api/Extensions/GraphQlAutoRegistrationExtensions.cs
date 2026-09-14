namespace FlowTask.Api.GraphQL;

using System.Reflection;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;

public static class GraphQlAutoRegistrationExtensions
{
    public static IRequestExecutorBuilder AddTypeExtensionsFromAssembly(
        this IRequestExecutorBuilder builder,
        Assembly assembly)
    {
        var extensionTypes = assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetCustomAttribute<ExtendObjectTypeAttribute>() is not null)
            .ToList();

        // TEMP DEBUG — remove once confirmed.
        Console.WriteLine($"[GraphQL] Discovered {extensionTypes.Count} type extension(s) in {assembly.GetName().Name}:");
        foreach (var type in extensionTypes)
        {
            var target = type.GetCustomAttribute<ExtendObjectTypeAttribute>()!.Name;
            Console.WriteLine($"[GraphQL]   - {type.FullName}  ->  extends \"{target}\"");
        }

        foreach (var type in extensionTypes)
        {
            builder.AddTypeExtension(type);
        }

        return builder;
    }
}