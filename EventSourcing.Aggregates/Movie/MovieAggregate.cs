namespace EventSourcing.Aggregates.Movie;

public class MovieAggregate : AggregateRoot<MovieEvent>
{
    public Guid MovieId { get; private set; } = Guid.Empty;
    public TicketPrice TicketPrice { get; private set; } = TicketPrice.From(0);
    public bool IsScreened { get; private set; }

    public void Apply(MovieAdded @event)
    {
        MovieId = @event.MovieId;
        TicketPrice = @event.TicketPrice;
    }

    public void Apply(TicketPriceIncreased @event) => TicketPrice += @event.Amount;

    public void Apply(TicketPriceDecreased @event) => TicketPrice -= @event.Amount;
    public void Apply(MovieScreened _) => IsScreened = true;

    public static MovieAggregate Add(Guid id, string title, int numberOfSeats, DateTimeOffset displayTime, TicketPrice ticketPrice)
    {
        var aggregate = new MovieAggregate();
        var movieAdded = new MovieAdded(id, title, numberOfSeats, displayTime, ticketPrice);
        aggregate.Apply(movieAdded);
        aggregate.RaiseEvent(movieAdded);
        return aggregate;
    }

    public void IncreaseTicketPrice(TicketPrice amount)
    {
        EnsureNotScreened();

        // calculate the new ticket price
        // the value object will return an exception if its policies are violated
        _ = TicketPrice + amount;
        var ticketPriceIncreased = new TicketPriceIncreased(MovieId, amount);
        Apply(ticketPriceIncreased);
        RaiseEvent(ticketPriceIncreased);
    }

    public void DecreaseTicketPrice(TicketPrice amount)
    {
        EnsureNotScreened();

        // calculate the new ticket price
        // the value object will return an exception if its policies are violated
        _ = TicketPrice + amount;
        var ticketPriceDecreased = new TicketPriceDecreased(MovieId, amount);
        Apply(ticketPriceDecreased);
        RaiseEvent(ticketPriceDecreased);
    }

    public void MarkAsScreened()
    {
        if (IsScreened) return; // for idempotency
        var screened = new MovieScreened(MovieId);
        Apply(screened);
        RaiseEvent(screened);
    }

    private void EnsureNotScreened()
    {
        if (IsScreened) throw new InvalidOperationException("Cannot increase ticket price after screening");
    }
}