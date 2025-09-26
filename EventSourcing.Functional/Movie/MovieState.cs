using Dunet;

namespace EventSourcing.Functional.Movie;

// State used to make decisions. (This is actually a projection)


[Union]
public partial record MovieState
{
    public partial record Initial;

    /// <summary>
    /// The state representing the movie that has been screened/showed in the cinema.
    /// </summary>
    public partial record Screened;

    /// <summary>
    /// The state representing the movie that is pending screening (to be screened in the future).
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="TicketPrice"></param>
    public partial record PendingScreening(Guid Id, TicketPrice TicketPrice);
}