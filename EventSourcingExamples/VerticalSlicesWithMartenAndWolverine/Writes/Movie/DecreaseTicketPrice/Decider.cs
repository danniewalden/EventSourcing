namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.DecreaseTicketPrice;

public static class Decider
{
    public static DeciderResult<MovieEvent> Decide(MovieState.PendingScreening state, Command command) =>
        state.TicketPrice.Decrease(command.Amount).Match<DeciderResult<MovieEvent>>(
            _ => new TicketPriceDecreased(state.Id, command.Amount),
            failure => failure.Error);
}