using Dunet;

namespace Shared;

[Union]
public partial record DeciderResult
{
    public partial record Success(Event Event);

    public partial record Failure(Error Error);
    
    public static implicit operator DeciderResult(string error) => new Failure(error);
}