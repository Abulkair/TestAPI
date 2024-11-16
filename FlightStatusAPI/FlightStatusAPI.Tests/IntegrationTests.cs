public class IntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;

    public IntegrationTests(WebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetFlights_ReturnsSuccess()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/flight?origin=NYC");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var flights = await response.Content.ReadAsAsync<List<FlightDto>>();
        Assert.NotEmpty(flights);
    }

    [Fact]
    public async Task AddFlight_ReturnsCreated()
    {
        // Arrange
        var flightDto = new FlightDto
        {
            Origin = "NYC",
            Destination = "LAX",
            Departure = DateTimeOffset.Now,
            Arrival = DateTimeOffset.Now.AddHours(5),
            Status = "InTime",
            Timezone = "UTC"
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/flight")
        {
            Content = new StringContent(JsonConvert.SerializeObject(flightDto), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
