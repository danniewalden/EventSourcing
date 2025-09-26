namespace EventSourcing.DCB.Events;

public record MovieEvent;
public record MovieAdded(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, double TicketPrice) : MovieEvent;
public record TicketPriceIncreased(Guid MovieId, double Amount) : MovieEvent;
public record TicketPriceDecreased(Guid MovieId, double Amount) : MovieEvent;