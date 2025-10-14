using System.Collections.Immutable;

namespace EventSourcing.Aggregates;

public abstract class AggregateRoot<TEventType>
{
    private readonly List<TEventType> _events = [];
    public ImmutableList<TEventType> EventsToBeStored => _events.ToImmutableList();
    public void RaiseEvent(TEventType @event) => _events.Add(@event);
    
}