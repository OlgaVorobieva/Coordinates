namespace CoordinatesTest;

using Newtonsoft.Json;
using System.Text;
using Coordinates.DataTypes;
using Microsoft.AspNetCore.Mvc.Testing;

public class CoordinatesControllerTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // Create an instance of WebApplicationFactory and HttpClient 
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        // Dispose of the WebApplicationFactory and HttpClient
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetCoordinates_CountGreaterThanOne_ReturnsNonEmptyArray()
    {
        // Arrange
        var count = 5;

        // Act
        using (var response = await _client.GetAsync($"/coordinates?count={count}"))
        {
            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var coordinates = JsonConvert.DeserializeObject<object[]>(content);
            Assert.That(coordinates, Is.Not.Null);
            Assert.That(coordinates, Has.Length.EqualTo(count));
        }
    }

    [Test]
    public async Task GetCoordinates_CountLessThanOne_ReturnsBadRequest()
    {
        // Arrange
        var count = 0;

        // Act
        using (var response = await _client.GetAsync($"/coordinates?count={count}"))
        {
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.BadRequest));
        }
    }

    [Test]
    public async Task PostCoordinates_ArrayLengthGreaterThanOne_ReturnsNonZeroDistance()
    {
        // Arrange
        var coordinates = new[]
        {
            new Coordinate { Latitude = 40.7128, Longitude = -74.0060 },
            new Coordinate { Latitude = 34.0522, Longitude = -118.2437 }
        };
        var content = new StringContent(JsonConvert.SerializeObject(coordinates), Encoding.UTF8, "application/json");

        // Act
        using (var response = await _client.PostAsync("/coordinates", content))
        {
            // Assert
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<DistanceResult>(await response.Content.ReadAsStringAsync());
            Assert.Multiple(() =>
            {
                Assert.That(result.Meters, Is.GreaterThan(0));
                Assert.That(result.Miles, Is.GreaterThan(0));
            });
        }
    }

    [Test]
    [TestCaseSource(nameof(TestDataInvalidCoordinates))]
    public async Task PostCoordinates_ReturnsZeroDistance(Coordinate[] coordinates)
    {
        // Arrange
        var content = new StringContent(JsonConvert.SerializeObject(coordinates), Encoding.UTF8, "application/json");

        // Act
        using (var response = await _client.PostAsync("/coordinates", content))
        {
            // Assert
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<DistanceResult>(await response.Content.ReadAsStringAsync());
            Assert.Multiple(() =>
            {
                Assert.That(result.Meters, Is.EqualTo(0));
                Assert.That(result.Miles, Is.EqualTo(0));
            });
        }
    }

    private static IEnumerable<TestCaseData> TestDataInvalidCoordinates()
    {
        yield return new TestCaseData(new Coordinate[] { new() { Latitude = 40.7128, Longitude = -74.0060 } });
        yield return new TestCaseData(Array.Empty<Coordinate>());
        yield return new TestCaseData(null);
    }
}
