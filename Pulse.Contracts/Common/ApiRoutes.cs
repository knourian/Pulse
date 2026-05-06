namespace Pulse.Contracts.Common;

#pragma warning disable CA1034 // Nested types should not be visible
public static class ApiRoutes
{
    public static class Agents
    {
        public const string Register = "/api/agents/register";
        public const string GetList = "/api/agents";
    }
    public static class Checks
    {
        public const string Get = "/api/checks";
    }
}
#pragma warning restore CA1034 // Nested types should not be visible