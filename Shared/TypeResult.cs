using Dunet;

namespace Shared;

[Union]
public partial record TypeResult<T>
{
    public partial record Success(T Type);

    public partial record Failure(Error Error);

    public static implicit operator TypeResult<T>(string error) => new Failure(error);

    public T GetValueOrThrow() => UnwrapSuccess().Type;
}

public static class TypeResultExtensions
{
    public static DeciderResult OnSuccess<T>(this TypeResult<T> result, Func<Event> handler) => result.Match<DeciderResult>(p => handler(), failure => failure.Error);
}