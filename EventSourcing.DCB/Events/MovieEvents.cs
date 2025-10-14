using EventSourcing.DCB.Movie;
using Shared;

namespace EventSourcing.DCB.Events;

public record MovieEvent : Event;

public record MovieAdded(Guid MovieId, string Title, int NumberOfSeats, DateTimeOffset DisplayTime, TicketPrice TicketPrice) : MovieEvent;

public record TicketPriceIncreased(Guid MovieId, TicketPrice Amount) : MovieEvent;

public record TicketPriceDecreased(Guid MovieId, TicketPrice Amount) : MovieEvent;