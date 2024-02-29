namespace Coordinates.DataTypes;

/// <summary>
/// Earth coordinates
/// </summary>
/// <param name="latitude"></param>
/// <param name="longitude"></param>
public readonly struct Coordinate(double latitude, double longitude)
{
    public double Latitude { get; init; } = latitude;
    public double Longitude { get; init; } = longitude;
}
