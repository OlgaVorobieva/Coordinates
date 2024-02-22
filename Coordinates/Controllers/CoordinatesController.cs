using Microsoft.AspNetCore.Mvc;
using Coordinates.DataTypes;
using System.Text.Json;

namespace Coordinates.Controllers
{
    [ApiController]
    public class CoordinatesController : ControllerBase
    {
        private const int EarthRadiusMeters = 6371000; // Radius of the Earth in meters
        private const double CoefficientMetersToMiles = 0.000621371; // Coefficient to transform to Miles
        private const int MaxLatitude = 90;
        private const int MaxLongitude = 180;

        private readonly Random _random = new();

        [HttpGet]
        [Route("coordinates")]
        public IActionResult GetCoordinates(int count)
        {
            if (count < 1)
            {
                return BadRequest("Count must be greater than zero.");
            }

            Coordinate[] coordinates = GetRandomCoordinates(count);

            return new JsonResult(coordinates, new JsonSerializerOptions { PropertyNamingPolicy = null });
        }


        [HttpPost]
        [Route("coordinates")]
        public IActionResult CalculateDistance([FromBody] Coordinate[]? coordinates = null)
        {
            if (coordinates == null || coordinates.Length < 2)
            {
                return new JsonResult(new DistanceResult(0, 0), new JsonSerializerOptions { PropertyNamingPolicy = null });
            }

            //Check coordinate values
            foreach (var coordinate in coordinates)
            {
                if (coordinate.Latitude > MaxLatitude || coordinate.Latitude < -MaxLatitude 
                    || coordinate.Longitude > MaxLongitude || coordinate.Longitude < -MaxLongitude) 
                {
                    return BadRequest("Provided coordinates have incorrect values");
                }
            }

            //Calculate total distance between coordinates in meters
            double totalDistanceMeters = 0;
            for (int i = 0; i < coordinates.Length - 1; i++)
            {
                totalDistanceMeters += CalculateDistanceBetweenCoordinates(coordinates[i], coordinates[i + 1]);
            }

            // Convert meters to miles
            double totalDistanceMiles = totalDistanceMeters * CoefficientMetersToMiles;

            return new JsonResult(new DistanceResult(totalDistanceMeters, totalDistanceMiles),
                  new JsonSerializerOptions { PropertyNamingPolicy = null });
        }

        private Coordinate[] GetRandomCoordinates(int count)
        {
            var coordinates = new Coordinate[count];
            for (int i = 0; i < count; i++)
            {
                coordinates[i] = GenerateRandomCoordinate();
            }
            return coordinates;
        }

        private Coordinate GenerateRandomCoordinate()
        {
            double latitude = Math.Round(_random.NextDouble() * 180 - MaxLatitude, 6); // Random latitude between -90 and 90 degree
            double longitude = Math.Round(_random.NextDouble() * 360 - MaxLongitude, 6); // Random longitude between -180 and 180 degree

            return new Coordinate(latitude, longitude);
        }

        private static double CalculateDistanceBetweenCoordinates(Coordinate coordinate1, Coordinate coordinate2)
        {
            // Using Haversine formula
            double dLatitude = (coordinate2.Latitude - coordinate1.Latitude) * Math.PI / 180;
            double dLongitude = (coordinate2.Longitude - coordinate1.Longitude) * Math.PI / 180;

            double a = Math.Sin(dLatitude / 2) * Math.Sin(dLatitude / 2) +
                    Math.Cos(coordinate1.Latitude * Math.PI / 180) * Math.Cos(coordinate2.Latitude * Math.PI / 180) *
                    Math.Sin(dLongitude / 2) * Math.Sin(dLongitude / 2);

            double distance = EarthRadiusMeters * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return distance;
        }
    }
}
