namespace EventSourcing.Marten_Wolverine.Features;

public static class HttpResponseMessageExtensions
{
    public static async Task Verify(this HttpResponseMessage response)
    {
        await VerifyJson(response.Content.ReadAsStreamAsync());
    }
}