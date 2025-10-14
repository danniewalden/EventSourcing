namespace EventSourcing.Functional.Movie;

public record MovieEvent;

public record MovieAdded(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, TicketPrice TicketPrice) : MovieEvent;
public record TicketPriceIncreased(Guid MovieId, TicketPrice IncreasedBy) : MovieEvent;

