using Dunet;
using EventSourcing.DCB.Events;
using Shared;

namespace EventSourcing.DCB;

public record IncreaseTicketPrice(Guid MovieId, double Amount)
{
    private const double MaxTicketPrice = 500;

    public static DeciderResult<MovieEvent> Decide(IncreaseTicketPriceState state, IncreaseTicketPrice command)
    {
        if (state is not IncreaseTicketPriceState.PendingScreening pendingScreening) return "Cannot increase ticket price when movie is not pending screening.";
        if (command.Amount <= 0) return "Please provide a positive number to increase the ticket price";
        if (TicketPricePolicyIsViolated(pendingScreening.CurrentTicketPrice, command.Amount, MaxTicketPrice)) return $"The ticket price policy of max {MaxTicketPrice} is violated";
        return new TicketPriceIncreased(command.MovieId, command.Amount);
    }

    private static bool TicketPricePolicyIsViolated(double currentTicketPrice, double priceIncrease, double maxTicketPrice) => currentTicketPrice + priceIncrease >= maxTicketPrice;
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
        public required double CurrentTicketPrice { get; init; }
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
            CurrentTicketPrice = pendingScreening.CurrentTicketPrice + priceIncreased.Amount
        },
        _ => readModel
    };
}