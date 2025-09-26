using EventSourcing.DCB.Events;
using Shared;

namespace EventSourcing.DCB;

public record AddMovie(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, double TicketPrice)
{
    public static DeciderResult<MovieEvent> Decide(AddMovie command) => new MovieAdded(command.MovieId, command.Title, command.NumberOfSeats, command.DisplayTime, command.TicketPrice);
}