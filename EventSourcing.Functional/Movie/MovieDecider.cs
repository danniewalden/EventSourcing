using Shared;

namespace EventSourcing.Functional.Movie;

public static class MovieDecider
{
    public static DeciderResult Decide(MovieState state, object command)
    {
        return (state, command) switch
        {
            (MovieState.Screened, _) => "Cannot apply commands to an already screened movie",
            (MovieState.Initial, AddMovie c) => new MovieAdded(c.MovieId, c.Title, c.NumberOfSeats, c.DisplayTime, c.TicketPrice),
            (MovieState.PendingScreening s, IncreaseTicketPrice c) => (c.IncreaseBy + s.TicketPrice).OnSuccess(() => new TicketPriceIncreased(c.MovieId, c.IncreaseBy)),
            _ => "Invalid command for the current state"
        };
    }
}