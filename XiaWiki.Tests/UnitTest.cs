using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XiaWiki.Core.Models;
using XiaWiki.Core.Repositories;
using XiaWiki.Infrastructure;

namespace XiaWiki.Tests;

public class UnitTest
{
    private readonly IServiceProvider _serviceProvider;

    public UnitTest()
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
    public async void GetLatestUpdates_HasData()
    {
        // Arrange
        var pageDetailRepository = _serviceProvider.GetRequiredService<IPageDetailRepository>();

        // Act
        var list = new List<PageDetail>();
        await foreach (var pageDetail in pageDetailRepository.GetLatestUpdatesAsync(12))
        {
            list.Add(pageDetail);
        }

        // Assert
        Assert.True(list.Count > 0);
    }

    [Fact]
    public async void GetRandomList_HasData()
    {
        // Arrange
        var pageDetailRepository = _serviceProvider.GetRequiredService<IPageDetailRepository>();

        // Act
        var list = new List<PageDetail>();
        await foreach (var pageDetail in pageDetailRepository.GetRandomListAsync(12))
        {
            list.Add(pageDetail);
        }

        // Assert
        Assert.True(list.Count > 0);
    }

}