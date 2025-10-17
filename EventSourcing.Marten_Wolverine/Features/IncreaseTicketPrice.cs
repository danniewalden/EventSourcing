using Dunet;
using EventSourcing.Marten_Wolverine.Events;
using EventSourcing.Marten_Wolverine.Plumbing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;
using Wolverine.Marten;

namespace EventSourcing.Marten_Wolverine.Features;

public static class IncreaseTicketPrice
{
    public record Request(double IncreaseBy);

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.IncreaseBy).MustBeValueObject(TicketPrice.From);
        }
    }

    public static (ProblemDetails, IncreaseTicketPriceState.PendingScreening) Validate([WriteAggregate] IncreaseTicketPriceState state)
    {
        if (state is IncreaseTicketPriceState.PendingScreening c) return (WolverineContinue.NoProblems, c);
        return (new ProblemDetails
        {
            Detail = "Invalid State",
            Status = 400
        }, null!);
    }

    [WolverinePost("/api/movies/{id:guid}/increase-price")]
    public static (IResult, AppendToStream) Handle(Guid id, Request request, IncreaseTicketPriceState.PendingScreening state)
    {
        var toIncrease = request.IncreaseBy;

        return TicketPrice.From(state.CurrentTicketPrice + toIncrease)
            .Match<(IResult, AppendToStream)>(
                success: _ => (Results.NoContent(), new AppendToStream(id, new TicketPriceIncreased(id, toIncrease))),
                failure: error => (Results.Problem(new ProblemDetails { Detail = error.Error, Status = 400 }), AppendToStream.NoOp));
    }
}

[Union]
public partial record IncreaseTicketPriceState
{
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

    public static IncreaseTicketPriceState Apply(IncreaseTicketPriceState state, TicketPriceDecreased ticketPriceDecrease) => (state, ticketPriceIncrease: ticketPriceDecrease) switch
    {
        (PendingScreening a, _) => a with { CurrentTicketPrice = a.CurrentTicketPrice - ticketPriceDecrease.Amount },
        _ => state // we dont know how to apply this
    };
}