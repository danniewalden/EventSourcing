using EventSourcing.DCB.Events;
using Shared;

namespace EventSourcing.DCB.Movie;

public record AddMovie(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, TicketPrice TicketPrice)
{
    public static DeciderResult Decide(AddMovie command) => new MovieAdded(command.MovieId, command.Title, command.NumberOfSeats, command.DisplayTime, command.TicketPrice);
}