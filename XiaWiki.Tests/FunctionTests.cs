using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XiaWiki.Core.Repositories;
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
        var pageDetailRepository = _serviceProvider.GetRequiredService<IPageDetailRepository>();

        // Act
        var list = pageDetailRepository.GetLatestUpdatesAsync(12).ToBlockingEnumerable().ToList();

        // Assert
        Assert.True(list.Count > 0);
    }

    [Fact]
    public void GetRandomList_HasData()
    {
        // Arrange
        var pageDetailRepository = _serviceProvider.GetRequiredService<IPageDetailRepository>();

        // Act
        var list = pageDetailRepository.GetRandomListAsync(12).ToBlockingEnumerable().ToList();

        // Assert
        Assert.True(list.Count > 0);
    }

    [Fact]
    public void SearchPageDetail_HasData()
    {
        // Arrange
        var pageDetailRepository = _serviceProvider.GetRequiredService<IPageDetailRepository>();

        // Act
        var list = pageDetailRepository.SearchAsync("Arduino").ToBlockingEnumerable().ToList();

        // Assert
        Assert.True(list.Count > 0);
    }

}