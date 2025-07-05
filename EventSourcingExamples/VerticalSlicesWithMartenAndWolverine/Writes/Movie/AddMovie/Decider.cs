namespace WebApplication1.VerticalSlicesWithMartenAndWolverine.Writes.Movie.AddMovie;

public static class Decider
{
    public static DeciderResult<MovieEvent> Decide(Command command) => new MovieAdded(command.MovieId, command.Title, command.NumberOfSeats, command.DisplayTime, command.TicketPrice);
}