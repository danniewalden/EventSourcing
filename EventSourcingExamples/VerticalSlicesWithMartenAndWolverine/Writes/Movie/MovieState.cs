using Dunet;

namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie;

// State used to make decisions. (This is actually state AND projection - thats how "live" aggregations work in Marten)

[Union]
public partial record MovieState
{
    public required Guid Id { get; init; } // needs to be here so Marten and Wolverine knows which identifier to look for in the database
    public int Version { get; set; } // for optimistic concurrency in Marten - Marten will populate this for us - so lets keep a public setter

    public partial record Initial
    {
        public static Initial Instance = new Initial { Id = Guid.Empty };
    }

    public partial record Screened;

    public partial record PendingScreening
    {
        public required TicketPrice TicketPrice { get; init; }
    }

    public static MovieState Create(MovieAdded added) => new PendingScreening { Id = added.MovieId, TicketPrice = added.TicketPrice };

    public static MovieState Apply(MovieState state, TicketPriceIncreased increasedTicketPrice) => state switch
    {
        PendingScreening pendingScreening => pendingScreening with { TicketPrice = pendingScreening.TicketPrice + increasedTicketPrice.Amount },
        _ => state,
    };

    public static MovieState Apply(MovieState state, TicketPriceDecreased decreasedTicketPrice) => state switch
    {
        PendingScreening pendingScreening => pendingScreening with { TicketPrice = pendingScreening.TicketPrice - decreasedTicketPrice.Amount },
        _ => state
    };

    public static MovieState Apply(MovieState state, ScreeningFinished _) => state switch
    {
        Screened screened => screened,
        PendingScreening pendingScreening => new Screened { Id = pendingScreening.Id },
        _ => state,
    };
}