namespace WebApplication1.Functional.Writes.Movie;

public static class Projection
{
    private static MovieState Apply(MovieState state, MovieEvent @event) => (state, @event) switch
    {
        (MovieState.Initial _, MovieAdded movieAdded) =>  new MovieState.PendingScreening(movieAdded.MovieId, movieAdded.TicketPrice),
        _ => throw new InvalidOperationException($"{nameof(MovieState)} doesn't know how to apply the {@event.GetType().Name} event")
    };

    public static MovieState Apply(IEnumerable<MovieEvent> events) => events.Aggregate((MovieState)new MovieState.Initial(), Apply);
}