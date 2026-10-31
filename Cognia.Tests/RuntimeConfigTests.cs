using Cognia.API.Configuration;
using Xunit;

namespace Cognia.Tests;

public class RuntimeConfigTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void GetJwtSigningKey_UsesFallbackWhenBlankInDevelopment(string? value)
    {
        var result = RuntimeConfig.GetJwtSigningKey(value, isDevelopment: true);

        Assert.NotNull(result);
        Assert.True(result.Length >= 32, $"Expected at least 32 chars but got {result.Length}.");
    }

    [Fact]
    public void GetJwtSigningKey_ThrowsWhenBlankOutsideDevelopment()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            RuntimeConfig.GetJwtSigningKey(string.Empty, isDevelopment: false));

        Assert.Contains("Jwt:SigningKey", exception.Message);
    }
}
