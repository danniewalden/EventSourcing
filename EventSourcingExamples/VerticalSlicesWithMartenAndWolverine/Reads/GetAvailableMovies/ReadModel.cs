namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Reads.GetAvailableMovies;

public record ReadModel(Guid Id, string Title, int NumberOfAvailableSeats, DateTimeOffset DisplayTime, double TicketPrice);