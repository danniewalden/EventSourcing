using System.Collections.Immutable;

namespace WebApplication1.Oop.Writes.Movie;

public class Aggregate
{
    private readonly List<MovieEvent> _events = [];

    private Aggregate(Guid id, TicketPrice ticketPrice)
    {
        MovieId = id;
        TicketPrice = ticketPrice;
        IsScreened = false;
        MovieId = id;
    }

    public Guid MovieId { get; private set; }
    public double TicketPrice { get; private set; }
    public bool IsScreened { get; private set; }

    public ImmutableList<MovieEvent> EventsToBeStored => _events.ToImmutableList();

    public void Apply(MovieAdded @event)
    {
        MovieId = @event.MovieId;
        TicketPrice = @event.TicketPrice;
    }

    public void Apply(TicketPriceIncreased @event) => TicketPrice += @event.Amount;

    public void Apply(TicketPriceDecreased @event) => TicketPrice -= @event.Amount;

    public static Aggregate Create(Guid id, string title, int numberOfSeats, DateTimeOffset displayTime, TicketPrice ticketPrice)
    {
        var aggregate = new Aggregate(id, ticketPrice);
        aggregate._events.Add(new MovieAdded(id, title, numberOfSeats, displayTime, ticketPrice));
        return aggregate;
    }

    public void IncreaseTicketPrice(TicketPrice amount)
    {
        EnsureNotScreened();
        if (TicketPrice + amount > 500) throw new InvalidOperationException($"Cannot increase price by {amount}, it would violate our policy of max ticket price of 500");
        TicketPrice += amount;
        _events.Add(new TicketPriceIncreased(MovieId, amount));
    }

    public void DecreaseTicketPrice(TicketPrice amount)
    {
        EnsureNotScreened();
        if (TicketPrice - amount < 0) throw new InvalidOperationException("Cannot decrease price by more than the current price");
        TicketPrice -= amount;
        _events.Add(new TicketPriceDecreased(MovieId, amount));
    }

    public void MarkAsScreened()
    {
        if (IsScreened) return; // for idempotency
        IsScreened = true;
    }

    private void EnsureNotScreened()
    {
        if (IsScreened) throw new InvalidOperationException("Cannot increase ticket price after screening");
    }
}

// value object here