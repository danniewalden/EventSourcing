using Shouldly;

namespace Shared;

public static class DeciderResultExtensions
{
    public static T ShouldSucceed<T>(this DeciderResult result) where T : Event
    {
        result.ShouldBeOfType<DeciderResult.Success>("Expected successful result but got failure");
        return (T)((DeciderResult.Success)result).Event;
    }

    public static Error ShouldFail(this DeciderResult result)
    {
        result.ShouldBeOfType<DeciderResult.Failure>("Expected failure result but got success");
        return result.UnwrapFailure().Error;
    }
}