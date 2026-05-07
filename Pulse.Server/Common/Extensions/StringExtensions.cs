namespace Pulse.Server.Common.Extensions;

public static class StringExtensions
{
    public static string UrlNormalize(this string str)
    {
        return str.Trim().ToLowerInvariant().TrimEnd('/');
    }
}
