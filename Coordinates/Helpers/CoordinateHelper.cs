namespace Coordinates.Helpers;
using Coordinates.DataTypes;
using System;

public static class CoordinateHelper
{
    private const int EarthRadiusMeters = 6371000; // Radius of the Earth in meters
    private const double CoefficientMetersToMiles = 0.000621371; // Coefficient to transform to Miles
    public const int MaxLatitude = 90;
    public const int MaxLongitude = 180;

    public static Coordinate[] GetRandomCoordinates(int count)
    {
        var coordinates = new Coordinate[count];
        for (int i = 0; i < count; i++)
        {
            coordinates[i] = GenerateRandomCoordinate();
        }
        return coordinates;
    }

    public static DistanceResult CalculateDistanse(Coordinate[]? coordinates)
    {
        if (coordinates is null)
        {
            return default;
        }
        //Calculate total distance between coordinates in meters
        double totalDistanceMeters = 0;
        for (int i = 0; i < coordinates.Length - 1; i++)
        {
            totalDistanceMeters += CalculateDistanceBetweenCoordinates(coordinates[i], coordinates[i + 1]);
        }

        // Convert meters to miles
        double totalDistanceMiles = totalDistanceMeters * CoefficientMetersToMiles;
        return new DistanceResult(totalDistanceMeters, totalDistanceMiles);
    }

   

    private static Coordinate GenerateRandomCoordinate()
    {
        Random _random = new();
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
