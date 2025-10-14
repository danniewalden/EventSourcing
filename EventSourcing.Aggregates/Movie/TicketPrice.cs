namespace EventSourcing.Aggregates.Movie;

public record TicketPrice
{
    private double Amount { get; }

    private TicketPrice(double amount) => Amount = amount;

    public static TicketPrice From(double amount) =>
        amount switch
        {
            // should probably return a result instead of throwing exception... But i am in a rush
            < 0 => throw new InvalidOperationException("Ticket price cannot be negative"),
            > 500 => throw new InvalidOperationException("Ticket price exceeds our policy of maximum 500$ per ticket"),
            _ => new TicketPrice(amount)
        };

    public static implicit operator double(TicketPrice ticketPrice) => ticketPrice.Amount;
    public static implicit operator TicketPrice(double ticketPrice) => From(ticketPrice);
    public static TicketPrice operator +(TicketPrice left, TicketPrice right) => From(left.Amount + right.Amount);
    public static TicketPrice operator -(TicketPrice left, TicketPrice right) => From(left.Amount - right.Amount);
    public override string ToString() => $"${Amount:F2}";
}