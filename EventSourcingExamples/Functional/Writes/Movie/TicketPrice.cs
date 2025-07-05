namespace WebApplication1.Functional.Writes.Movie;

public record TicketPrice
{
    public double Amount { get; }

    private TicketPrice(double amount) => Amount = amount;

    public static TicketPrice Create(double amount) =>
        amount switch
        {
            // should probably return a result instead of throwing exception... But i am in a rush
            < 0 => throw new InvalidOperationException("Ticket price cannot be negative"),
            > 500 => throw new InvalidOperationException("Ticket price exceeds our policy of maximum 500$ per ticket"),
            _ => new TicketPrice(amount)
        };
    
    public TypeResult<TicketPrice> Increase(double amount) => Amount + amount > 500 ? new Error("Ticket price exceeds our policy of maximum 500$ per ticket") : Create(Amount + amount);
    public TypeResult<TicketPrice> Decrease(double amount) => Amount - amount < 0 ? new Error("Ticket price cannot be negative") : Create(Amount + amount);

    public static implicit operator double(TicketPrice ticketPrice) => ticketPrice.Amount;
    public static implicit operator TicketPrice(double ticketPrice) => Create(ticketPrice);
    public static TicketPrice operator +(TicketPrice left, TicketPrice right) => Create(left.Amount + right.Amount);
    public static TicketPrice operator -(TicketPrice left, TicketPrice right) => Create(left.Amount - right.Amount);
    public override string ToString() => $"${Amount:F2}";
}