using System.Reflection;
using JasperFx.Events;
using Marten;
using Wolverine.Marten;

namespace WebApplication1;

public static class MartenRegistryScanner
{
    public static void ApplyAllConfigurations(this StoreOptions options, IServiceProvider serviceProvider)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var registryType = typeof(MartenRegistrar);

        var registries = assembly.GetTypes()
            .Where(t => registryType.IsAssignableFrom(t) && t is { IsAbstract: false, IsClass: true })
            .Select(Activator.CreateInstance)
            .Cast<MartenRegistrar>();

        foreach (var registry in registries)
        {
            registry.Register(options, serviceProvider);
            // options.Schema.Include(registry);
        }
    }
}
public class AppendToStream(Guid id, params object[] events) : IMartenOp
{
    public void Execute(IDocumentSession session)
    {
        if (id == Guid.Empty) return;
        session.Events.Append(id, events);
    }

    public static AppendToStream NoOp => new(Guid.Empty);
}


public class ArchiveStream(Guid id, string reason,  params object[] events) : IMartenOp
{
    public void Execute(IDocumentSession session)
    {
        if(id == Guid.Empty) return;

        session.Events.Append(id, events);
        session.Events.Append(id, new Archived(reason));
    }

    public static ArchiveStream NoOp => new(Guid.Empty, string.Empty);
}

public abstract class MartenRegistrar
{
    public abstract void Register(StoreOptions options, IServiceProvider serviceProvider);
}