using XiaWiki.Shared.Extensions;

namespace XiaWiki.Tests;

public class UnitTests
{
    [Fact]
    public void ToBase64Url_ShouldBeOk()
    {
        // Arrange
        var str = "认识Arduino开发板和Arduino框架 18098505c8374b1694baed435d15c068/Untitled.png";

        // Act
        var base64 = str.ToBase64Url();
        var str1 = base64.FromBase64Url();

        // Assert
        Assert.Equal(str, str1);
    }
}
