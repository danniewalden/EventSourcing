namespace WebApplication1.Functional.Writes.Movie;

public record IncreaseTicketPriceBy(TicketPrice Amount);

public record DecreaseTicketPriceBy(TicketPrice Amount);

public record AddMovie(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, TicketPrice TicketPrice);


