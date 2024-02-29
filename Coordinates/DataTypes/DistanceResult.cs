namespace Coordinates.DataTypes;

/// <summary>
/// Distance in Meters and Miles
/// </summary>
/// <param name="meters"></param>
/// <param name="miles"></param>
public readonly struct DistanceResult(double meters = 0, double miles = 0)
{
    public double Meters { get; } = meters;
    public double Miles { get; } = miles;
}
