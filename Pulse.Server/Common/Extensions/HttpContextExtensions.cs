namespace Pulse.Server.Common.Extensions;

public static class HttpContextExtensions
{
    private const string BearerKeyword = "Bearer ";

    public static string GetApiKey(this HttpContext context)
    {
        string header = context.Request.Headers.Authorization.ToString();

        if (header.StartsWith(BearerKeyword, StringComparison.OrdinalIgnoreCase))
        {
            return header.Substring(BearerKeyword.Length);
        }

        return null;
    }
}
