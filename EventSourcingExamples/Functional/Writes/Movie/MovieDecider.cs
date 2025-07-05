namespace WebApplication1.Functional.Writes.Movie;

public static class MovieDecider
{
    public static DeciderResult<MovieEvent> Decide(MovieState state, object command)
    {
        return (state, command) switch
        {
            (MovieState.Screened, _) => new Error("Cannot apply commands to an already screened movie"),

            (MovieState.Initial, AddMovie c) => new MovieAdded(c.MovieId, c.Title, c.NumberOfSeats, c.DisplayTime, c.TicketPrice),

            (MovieState.PendingScreening s, IncreaseTicketPriceBy i) => s.TicketPrice.Increase(i.Amount)
                .Match<DeciderResult<MovieEvent>>(
                    success => new TicketPriceIncreased(s.Id, i.Amount),
                    failure => failure.Error),

            (MovieState.PendingScreening s, DecreaseTicketPriceBy i) => s.TicketPrice.Decrease(i.Amount).Match<DeciderResult<MovieEvent>>(
                success => new TicketPriceDecreased(s.Id, i.Amount),
                failure => failure.Error),
            
            _ => new Error("Invalid command for the current state")
        };
    }
}