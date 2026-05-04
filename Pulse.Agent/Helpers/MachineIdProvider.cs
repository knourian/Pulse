namespace Pulse.Agent.Helpers;

public static class MachineIdProvider
{
    public static string Get()
    {
        var raw = $"{Environment.MachineName}-{Environment.OSVersion}";
        return Convert.ToBase64String(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(raw)
            ));
    }
}
