using Shared;

namespace EventSourcing.Functional.Movie;

public static class MovieDecider
{
    public static DeciderResult<MovieEvent> Decide(MovieState state, object command)
    {
        return (state, command) switch
        {
            (MovieState.Screened, _) => "Cannot apply commands to an already screened movie",
            (MovieState.Initial, AddMovie c) => new MovieAdded(c.MovieId, c.Title, c.NumberOfSeats, c.DisplayTime, c.TicketPrice),

            _ => "Invalid command for the current state"
        };
    }
}