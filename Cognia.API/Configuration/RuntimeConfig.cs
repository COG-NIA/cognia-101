using System.Text;

namespace Cognia.API.Configuration;

public static class RuntimeConfig
{
    private const string DefaultMongoConnectionString = "mongodb://localhost:27017";
    private const string DefaultDevelopmentJwtSigningKey = "development-signing-key-that-is-long-enough!";

    public static string GetMongoConnectionString(string? configuredValue, bool isDevelopment)
    {
        var value = configuredValue?.Trim();
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        if (isDevelopment)
        {
            return DefaultMongoConnectionString;
        }

        throw new InvalidOperationException("MongoDb:ConnectionString must be configured.");
    }

    public static string GetJwtSigningKey(string? configuredValue, bool isDevelopment)
    {
        var value = configuredValue?.Trim();
        if (!string.IsNullOrWhiteSpace(value))
        {
            if (Encoding.UTF8.GetByteCount(value) < 32)
            {
                throw new InvalidOperationException("Jwt:SigningKey must contain at least 32 bytes.");
            }

            return value;
        }

        if (isDevelopment)
        {
            return DefaultDevelopmentJwtSigningKey;
        }

        throw new InvalidOperationException("Jwt:SigningKey must be configured.");
    }
}