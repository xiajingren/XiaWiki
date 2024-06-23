using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XiaWiki.Core.Repositories;
using XiaWiki.Core.Services;
using XiaWiki.Infrastructure;

namespace XiaWiki.Tests;

public class FunctionTests
{
    private readonly IServiceProvider _serviceProvider;

    public FunctionTests()
    {
        var serviceCollection = new ServiceCollection();

        var configData = new Dictionary<string, string?>
        {
            // { "Runtime:Workspace", "" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        serviceCollection.AddWiki(configuration);

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [Fact]
    public void GetLatestUpdates_HasData()
    {
        // Arrange
        var pageLiteService = _serviceProvider.GetRequiredService<IPageLiteService>();

        // Act
        var list = pageLiteService.GetLatestUpdatesAsync(12).ToBlockingEnumerable().ToList();

        // Assert
        Assert.True(list.Count > 0);
    }

    [Fact]
    public void GetRandomList_HasData()
    {
        // Arrange
        var pageLiteService = _serviceProvider.GetRequiredService<IPageLiteService>();

        // Act
        var list = pageLiteService.GetRandomListAsync(12).ToBlockingEnumerable().ToList();

        // Assert
        Assert.True(list.Count > 0);
    }

    [Fact]
    public void SearchPageLite_HasData()
    {
        // Arrange
        var pageLiteService = _serviceProvider.GetRequiredService<IPageLiteService>();

        // Act
        var list = pageLiteService.SearchAsync("故乡").ToBlockingEnumerable().ToList();

        // Assert
        Assert.True(list.Count > 0);
    }

}