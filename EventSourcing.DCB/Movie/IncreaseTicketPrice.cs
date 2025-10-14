using Dunet;
using EventSourcing.DCB.Events;
using Shared;

namespace EventSourcing.DCB.Movie;

public record IncreaseTicketPrice(Guid MovieId, TicketPrice IncreaseBy)
{
    public static DeciderResult Decide(IncreaseTicketPriceState state, IncreaseTicketPrice command) => (state) switch
    {
        (IncreaseTicketPriceState.PendingScreening s) => (command.IncreaseBy + s.CurrentTicketPrice).OnSuccess(() => new TicketPriceIncreased(command.MovieId, command.IncreaseBy)),
        _ => "Cannot increase ticket price when movie is not pending screening."
    };
}

/// <summary>
/// State & Projection for decision making of Increasing the ticket price 
/// </summary>
/// <param name="MovieId"></param>
/// <param name="CurrentTicketPrice"></param>
[Union]
public partial record IncreaseTicketPriceState
{
    public partial record Initial;

    public partial record Other;

    public partial record PendingScreening
    {
        public required Guid MovieId { get; init; }
        public required TicketPrice CurrentTicketPrice { get; init; }
    }

    public static IncreaseTicketPriceState Apply(params MovieEvent[] events) => events.Aggregate((IncreaseTicketPriceState)new Initial(), Apply);

    private static IncreaseTicketPriceState Apply(IncreaseTicketPriceState readModel, MovieEvent @event) => (readModel, @event) switch
    {
        (Initial _, MovieAdded movieAdded) => new PendingScreening
        {
            MovieId = movieAdded.MovieId,
            CurrentTicketPrice = movieAdded.TicketPrice
        },
        (PendingScreening pendingScreening, TicketPriceIncreased priceIncreased) => pendingScreening with
        {
            CurrentTicketPrice = (pendingScreening.CurrentTicketPrice + priceIncreased.Amount).GetValueOrThrow()
        },
        _ => readModel
    };
}