namespace Pulse.Dashboard.Helpers;

public static class PasswordHasher
{
    private const int SaltSize = 16;

    private const int KeySize = 32;

    private const int Iterations = 100_000;

    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

        return string.Join('.', Iterations, Convert.ToHexString(salt), Convert.ToHexString(hash));
    }

    public static bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split('.');

        if (parts.Length != 3)
        {
            return false;
        }

        var iterations = int.Parse(parts[0]);

        var salt = Convert.FromHexString(parts[1]);

        var expectedHash = Convert.FromHexString(parts[2]);

        var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, Algorithm, expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}
