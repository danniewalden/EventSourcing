using WebApplication1.Functional.Writes.Movie;

namespace WebApplication1.Functional.Reads.SeatSelection;

public record Movie(Guid Id, string Title, DateTimeOffset DisplayTime);

public static class MovieProjection
{
    public static Movie Apply(Movie state, object evt) => (state, evt) switch
    {
        (_, MovieAdded added) => new Movie(added.MovieId, added.Title, added.DisplayTime),
        _ => throw new InvalidOperationException($"{nameof(Movie)} doesn't know how to apply the {@evt.GetType().Name} event")
    };
    
    public static Movie Apply(IEnumerable<object> events) => events.Aggregate(new Movie(Guid.Empty, "", DateTimeOffset.MinValue), Apply);
}