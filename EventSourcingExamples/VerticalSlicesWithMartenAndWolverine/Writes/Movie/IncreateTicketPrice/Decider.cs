namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.IncreateTicketPrice;

public static class Decider
{
    public static DeciderResult<MovieEvent> Decide(MovieState.PendingScreening state, Command command) =>
        state.TicketPrice.Increase(command.Amount)
            .Match<DeciderResult<MovieEvent>>(
                _ => new TicketPriceIncreased(state.Id, command.Amount),
                failure => failure.Error);
}