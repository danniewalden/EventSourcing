namespace WebApplication1.Functional.Writes.Movie;

public record MovieEvent;

public record MovieAdded(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, TicketPrice TicketPrice) : MovieEvent;

public record TicketPriceIncreased(Guid MovieId, TicketPrice Amount) : MovieEvent;
public record TicketPriceDecreased(Guid MovieId, TicketPrice Amount) : MovieEvent;
public record MarkAsScreened(Guid MovieId) : MovieEvent;