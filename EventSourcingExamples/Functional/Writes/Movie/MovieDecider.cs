namespace WebApplication1.Functional.Writes.Movie;

public static class MovieDecider
{
    public static DeciderResult<MovieEvent> Decide(MovieState state, object command)
    {
        return (state, command) switch
        {
            (MovieState.Screened, _) => new Error("Cannot apply commands to an already screened movie"),
            (MovieState.Initial, AddMovie c) => new MovieAdded(c.MovieId, c.Title, c.NumberOfSeats, c.DisplayTime, c.TicketPrice),
            
            _ => new Error("Invalid command for the current state")
        };
    }
}