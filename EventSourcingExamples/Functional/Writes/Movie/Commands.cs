namespace WebApplication1.Functional.Writes.Movie;

public record AddMovie(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, TicketPrice TicketPrice);


