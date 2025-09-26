namespace EventSourcing.Functional.GetAvailableMovies;

public record ReadModel
{

    public Guid Id { get; init; }
    public string Title { get; init; }
    public int NumberOfAvailableSeats { get; init; }
    public DateTimeOffset DisplayTime { get; init; }
    public double TicketPrice { get; init; }

}