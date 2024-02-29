namespace Coordinates.Controllers;
using Microsoft.AspNetCore.Mvc;
using Coordinates.DataTypes;
using System.Text.Json;
using Coordinates.Helpers;

[ApiController]
public class CoordinatesController : ControllerBase
{
    private static readonly JsonSerializerOptions _jsonOption = new() { PropertyNamingPolicy = null };

    /// <summary>
    /// Generate <paramref name="count"/> coordinates.
    /// </summary>
    /// <param name="count">Number of generated coordinates.</param>
    /// <returns>Coordinates.</returns>
    [HttpGet]
    [Route("coordinates")]
    public IActionResult GetCoordinates(int count)
    {
        if (count < 1)
        {
            return BadRequest("Count must be greater than zero.");
        }

        Coordinate[] coordinates = CoordinateHelper.GetRandomCoordinates(count);

        return new JsonResult(coordinates, _jsonOption);
    }

    /// <summary>
    /// Generate distance between <paramref name="coordinates"/>.
    /// </summary>
    /// <param name="coordinates">Coordintares to calculate distance.</param>
    /// <returns>Sum of Distances between coordinates.</returns>
    [HttpPost]
    [Route("coordinates")]
    public IActionResult CalculateDistance([FromBody] Coordinate[]? coordinates = null)
    {
        if (coordinates is null or {Length: < 2 })
        {
            return new JsonResult(default (DistanceResult), _jsonOption);
        }

        //Check coordinate values
        foreach (var coordinate in coordinates)
        {
            if (coordinate is { Latitude: > CoordinateHelper.MaxLatitude or < -CoordinateHelper.MaxLatitude } ||
                coordinate is { Longitude: > CoordinateHelper.MaxLongitude or < -CoordinateHelper.MaxLongitude })
            {
                return BadRequest("Provided coordinates have incorrect values");
            }
        }

        DistanceResult distance = CoordinateHelper.CalculateDistanse(coordinates);
        return new JsonResult(distance, _jsonOption);
    }
}