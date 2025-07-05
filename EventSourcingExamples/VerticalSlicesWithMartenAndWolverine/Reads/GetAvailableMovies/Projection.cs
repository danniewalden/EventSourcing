using JasperFx.Events.Projections;
using JetBrains.Annotations;
using Marten;
using Marten.Events.Aggregation;
using WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Reads.GetAvailableMovies;

// A marten projection that uses conventional methods to apply events
public class Projection : SingleStreamProjection<ReadModel, Guid>
{
    public Projection()
    {
        Name = "GetAvailableMovies";
    }

    public static ReadModel Create(MovieAdded movieAdded) => new(movieAdded.MovieId, movieAdded.Title, movieAdded.NumberOfSeats, movieAdded.DisplayTime, movieAdded.TicketPrice);
    public static ReadModel Apply(ReadModel readModel, TicketPriceIncreased ticketPriceIncreased) => readModel with { TicketPrice = readModel.TicketPrice + ticketPriceIncreased.Amount };
    public static ReadModel Apply(ReadModel readModel, TicketPriceDecreased ticketPriceDecreased) => readModel with { TicketPrice = readModel.TicketPrice - ticketPriceDecreased.Amount };
    public static bool ShouldDelete(ScreeningFinished _) => true;
}

[UsedImplicitly]
public class Registrar : MartenRegistrar
{
    public override void Register(StoreOptions options, IServiceProvider serviceProvider)
    {
        options.Projections.Add<Projection>(ProjectionLifecycle.Async);
        options.Schema.For<ReadModel>().DocumentAlias("read_available_movies");
    }
}