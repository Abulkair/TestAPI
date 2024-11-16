using Moq;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

public class FlightServiceTests
{
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly Mock<IMemoryCache> _mockCache;
    private readonly FlightService _flightService;

    public FlightServiceTests()
    {
        _mockContext = new Mock<ApplicationDbContext>();
        _mockCache = new Mock<IMemoryCache>();
        _flightService = new FlightService(_mockContext.Object, _mockCache.Object);
    }

    [Fact]
    public void GetFlights_ReturnsCachedFlights_WhenCacheExists()
    {
        // Arrange
        var flights = new List<Flight>
        {
            new Flight { ID = 1, Origin = "ALA", Destination = "NQZ" }
        };

        _mockCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out flights)).Returns(true);

        // Act
        var result = _flightService.GetFlights("ALA", "NQZ");

        // Assert
        Assert.Single(result);
        Assert.Equal("ALA", result.First().Origin);
    }

    // Добавьте другие тесты для методов AddFlight и UpdateFlight
}
