using Shared;

namespace EventSourcing.DCB.Movie;

public record TicketPrice
{
    private double Amount { get; }

    private TicketPrice(double amount) => Amount = amount;

    const double MaxTicketPrice = 500;

    public static TypeResult<TicketPrice> From(double amount) =>
        amount switch
        {
            < 0 => "Ticket price cannot be negative",
            > MaxTicketPrice => $"Ticket price exceeds our policy of maximum {MaxTicketPrice} per ticket",
            _ => new TicketPrice(amount)
        };

    public static implicit operator double(TicketPrice ticketPrice) => ticketPrice.Amount;
    public static TypeResult<TicketPrice> operator +(TicketPrice left, TicketPrice right) => From(left.Amount + right.Amount);
    public static TypeResult<TicketPrice> operator -(TicketPrice left, TicketPrice right) => From(left.Amount - right.Amount);
    public override string ToString() => $"${Amount:F2}";
}