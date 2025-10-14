namespace EventSourcing.Functional.Movie;

public static class Projection
{
    private static MovieState Apply(MovieState state, MovieEvent @event) => (state, @event) switch
    {
        (MovieState.Initial _, MovieAdded movieAdded) => new MovieState.PendingScreening(movieAdded.MovieId, movieAdded.TicketPrice),
        (MovieState.PendingScreening pendingScreening, TicketPriceIncreased priceIncreased) => pendingScreening with
        {
            TicketPrice = (pendingScreening.TicketPrice + priceIncreased.IncreasedBy).GetValueOrThrow()
        },
        _ => throw new InvalidOperationException($"{nameof(MovieState)} doesn't know how to apply the {@event.GetType().Name} event")
    };

    /// <summary>
    /// This is only for testing purposes.
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    public static MovieState Apply(params IEnumerable<MovieEvent> events) => events.Aggregate((MovieState)new MovieState.Initial(), Apply);
}