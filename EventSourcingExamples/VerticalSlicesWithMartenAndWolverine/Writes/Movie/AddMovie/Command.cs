namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.AddMovie;

public record Command(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, TicketPrice TicketPrice);