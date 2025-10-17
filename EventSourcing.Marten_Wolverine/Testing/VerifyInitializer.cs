using System.Runtime.CompilerServices;

namespace EventSourcing.Marten_Wolverine.Testing;

public static class VerifyInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        UseProjectRelativeDirectory("Testing/Snapshots");
    }
}