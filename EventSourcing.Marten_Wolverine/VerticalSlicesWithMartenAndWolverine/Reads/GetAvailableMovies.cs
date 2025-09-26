using EventSourcing.Marten_Wolverine.VerticalSlicesWithMartenAndWolverine.Writes.Movie;
using JasperFx.Events.Projections;
using JetBrains.Annotations;
using Marten;
using Marten.Events.Aggregation;
using Wolverine.Http;

namespace EventSourcing.Marten_Wolverine.VerticalSlicesWithMartenAndWolverine.Reads;

public static class GetAvailableMovies
{
    [PublicAPI]
    public record Response(Guid MovieId, string Title, int NumberOfAvailableSeats, double TicketPrice);

    [WolverineGet("/api/movies")]
    public static async Task<IEnumerable<Response>> Handle(Guid ownerId, IDocumentSession session) => await session.Query<Response>().ToListAsync();
}

[UsedImplicitly]
public class GetAvailableMoviesRegistrar : MartenRegistrar
{
    public override void Register(StoreOptions options, IServiceProvider serviceProvider)
    {
        options.Projections.Add<GetAvailableMoviesProjection>(ProjectionLifecycle.Inline);
        options.Schema.For<GetAvailableMovies.Response>().DocumentAlias("read_available_movies").Identity(p => p.MovieId);
    }
}

public class GetAvailableMoviesProjection : SingleStreamProjection<GetAvailableMovies.Response, Guid>
{
    public static GetAvailableMovies.Response Create(MovieAdded movieAdded) => new(movieAdded.MovieId, movieAdded.Title, movieAdded.NumberOfSeats, movieAdded.TicketPrice);

    public static GetAvailableMovies.Response Apply(GetAvailableMovies.Response state, TicketPriceIncreased ticketPriceIncreased) => state with
    {
        TicketPrice = state.TicketPrice + ticketPriceIncreased.Amount
    };

    public static bool Delete(ScreeningFinished _) => true;
}