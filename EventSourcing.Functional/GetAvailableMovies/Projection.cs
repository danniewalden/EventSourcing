using EventSourcing.Functional.Movie;

namespace EventSourcing.Functional.GetAvailableMovies;

public static class Projection
{
    private static ReadModel Apply(ReadModel readModel, MovieEvent @event) => @event switch
    {
        MovieAdded movieAdded => new ReadModel
        {
            Id = movieAdded.MovieId,
            Title = movieAdded.Title,
            NumberOfAvailableSeats = movieAdded.NumberOfSeats,
            DisplayTime = movieAdded.DisplayTime,
            TicketPrice = movieAdded.TicketPrice
        },
        _ => throw new InvalidOperationException($"{nameof(ReadModel)} doesn't know how to apply the {@event.GetType().Name} event")
    };

    public static ReadModel Apply(IEnumerable<MovieEvent> events) => events.Aggregate(new ReadModel(), Apply);
}