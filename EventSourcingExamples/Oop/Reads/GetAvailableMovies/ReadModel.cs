using WebApplication1.Oop.Writes.Movie;

namespace WebApplication1.Oop.Reads.GetAvailableMovies;

public record ReadModel
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public int NumberOfAvailableSeats { get; private set; }
    public DateTimeOffset DisplayTime { get; private set; }
    public double TicketPrice { get; private set; }

    public void Apply(MovieAdded movieAdded)
    {
        Id = movieAdded.MovieId;
        Title = movieAdded.Title;
        NumberOfAvailableSeats = movieAdded.NumberOfSeats;
        DisplayTime = movieAdded.DisplayTime;
        TicketPrice = movieAdded.TicketPrice;
    }


    public void Apply(TicketPriceIncreased ticketPriceIncreased) => TicketPrice += ticketPriceIncreased.Amount;
    public void Apply(TicketPriceDecreased ticketPriceDecreased) => TicketPrice -= ticketPriceDecreased.Amount;

    public void Apply(IEnumerable<MovieEvent> events)
    {
        foreach (var movieEvent in events)
        {
            switch (movieEvent)
            {
                case MovieAdded movieAdded:
                    Apply(movieAdded);
                    break;
                case TicketPriceIncreased ticketPriceIncreased:
                    Apply(ticketPriceIncreased);
                    break;
                case TicketPriceDecreased ticketPriceDecreased:
                    Apply(ticketPriceDecreased);
                    break;
            }
        }
    }
}