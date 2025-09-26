namespace EventSourcing.Functional.Movie;

public record AddMovie(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, TicketPrice TicketPrice);


