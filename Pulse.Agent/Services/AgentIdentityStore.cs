using System.Text;

using Microsoft.Extensions.Options;

using Pulse.Agent.Models;


namespace Pulse.Agent.Services;

public class AgentIdentityStore
{

    private const int NonceSize = 12;
    private const int TagSize = 16;
    private const byte FormatVersion = 1;

    private readonly byte[] _key;
    private readonly string _filePath;
    private readonly string _directoryPath;

    public AgentIdentityStore(IOptions<AppSetting> settings)
    {
        _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                      "Pulse",
                                      "Agent");
        _filePath = Path.Combine(_directoryPath, "agent.json");

        var secret = settings.Value.Agent.IdentityEncryptionKey;
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new InvalidOperationException(
                "Missing configuration key 'Agent:IdentityEncryptionKey'.");
        }

        _key = SHA256.HashData(Encoding.UTF8.GetBytes(secret));
    }

    public async Task<AgentIdentity> LoadAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_filePath))
        {
            return null;
        }

        try
        {
            var protectedBytes = await File.ReadAllBytesAsync(_filePath, ct);
            var jsonBytes = Unprotect(protectedBytes);
            return JsonSerializer.Deserialize<AgentIdentity>(jsonBytes);
        }
        catch (Exception ex) when (ex is IOException or JsonException or CryptographicException)
        {
            // Corrupt/unreadable identity file -> treat as missing identity.
            return null;
        }
    }

    public async Task SaveAsync(AgentIdentity identity, CancellationToken ct = default)
    {
        Directory.CreateDirectory(_directoryPath);

        var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(identity);
        var protectedBytes = Protect(jsonBytes);

        var tempPath = $"{_filePath}.{Guid.NewGuid():N}.tmp";

        try
        {
            await File.WriteAllBytesAsync(tempPath, protectedBytes, ct);
            File.Move(tempPath, _filePath, overwrite: true);
        }
        finally
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
    }

    private byte[] Protect(byte[] plaintext)
    {
        var nonce = RandomNumberGenerator.GetBytes(NonceSize);
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[TagSize];

        using var aes = new AesGcm(_key, TagSize);
        aes.Encrypt(nonce, plaintext, ciphertext, tag);

        var output = new byte[1 + NonceSize + TagSize + ciphertext.Length];
        output[0] = FormatVersion;
        nonce.CopyTo(output.AsSpan(1, NonceSize));
        tag.CopyTo(output.AsSpan(1 + NonceSize, TagSize));
        ciphertext.CopyTo(output.AsSpan(1 + NonceSize + TagSize));

        return output;
    }

    private byte[] Unprotect(byte[] payload)
    {
        if (payload.Length < 1 + NonceSize + TagSize || payload[0] != FormatVersion)
        {
            throw new CryptographicException("Invalid identity file format.");
        }

        var nonce = payload.AsSpan(1, NonceSize);
        var tag = payload.AsSpan(1 + NonceSize, TagSize);
        var ciphertext = payload.AsSpan(1 + NonceSize + TagSize);

        var plaintext = new byte[ciphertext.Length];
        using var aes = new AesGcm(_key, TagSize);
        aes.Decrypt(nonce, ciphertext, tag, plaintext);

        return plaintext;
    }
}
