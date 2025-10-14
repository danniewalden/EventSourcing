using Shared;

namespace EventSourcing.Functional.Movie;

public record MovieEvent : Event;

public record MovieAdded(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, TicketPrice TicketPrice) : MovieEvent;
public record TicketPriceIncreased(Guid MovieId, TicketPrice IncreasedBy) : MovieEvent;

