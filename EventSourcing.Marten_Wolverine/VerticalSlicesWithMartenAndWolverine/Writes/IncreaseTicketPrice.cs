using Dunet;
using EventSourcing.Marten_Wolverine.VerticalSlicesWithMartenAndWolverine.Writes.Movie;
using FluentValidation;
using Marten.Schema;
using Wolverine.Http;
using Wolverine.Marten;

namespace EventSourcing.Marten_Wolverine.VerticalSlicesWithMartenAndWolverine.Writes;

public static class IncreaseTicketPrice
{
    public record Request(double IncreaseBy);

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.IncreaseBy).MustBeValueObject(TicketPrice.Create);
            RuleFor(p => p.IncreaseBy).Must(p => true);
        }
    }

    public static (IResult, IncreaseTicketPriceState.PendingScreening) Validate([WriteAggregate] IncreaseTicketPriceState state)
    {
        if (state is IncreaseTicketPriceState.PendingScreening c) return (WolverineContinue.Result(), c);
        return (Results.ValidationProblem([], "Bad state"), null!);
    }

    [WolverinePost("/api/movies/{id:guid}/increase-price")]
    public static (IResult, AppendToStream) Handle(Guid id, Request request, IncreaseTicketPriceState.PendingScreening state)
    {
        return TicketPricePolicyIsViolated(state.CurrentTicketPrice, request.IncreaseBy, MaxTicketPrice)
            ? (Results.BadRequest($"The ticket price policy of max {MaxTicketPrice} is violated"), AppendToStream.NoOp)
            : (Results.NoContent(), new AppendToStream(id, new TicketPriceIncreased(id, TicketPrice.Create(request.IncreaseBy).GetValueOrThrow())));
    }

    public static readonly double MaxTicketPrice = 500;

    private static bool TicketPricePolicyIsViolated(double currentTicketPrice, double priceIncrease, double maxTicketPrice) => currentTicketPrice + priceIncrease >= maxTicketPrice;
}

[Union]
public partial record IncreaseTicketPriceState
{
    [Identity]
    public required Guid Id { get; init; }

    public partial record Initial;

    public partial record Other;

    public partial record PendingScreening
    {
        public required double CurrentTicketPrice { get; init; }
    }

    public static IncreaseTicketPriceState Create(MovieAdded movieAdded) => new PendingScreening
    {
        Id = movieAdded.MovieId,
        CurrentTicketPrice = movieAdded.TicketPrice
    };

    public static IncreaseTicketPriceState Apply(IncreaseTicketPriceState state, TicketPriceIncreased ticketPriceIncrease) => (state, ticketPriceIncrease) switch
    {
        (PendingScreening a, _) => a with { CurrentTicketPrice = a.CurrentTicketPrice + ticketPriceIncrease.Amount },
        _ => state // we dont know how to apply this
    };
}