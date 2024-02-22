namespace Coordinates.DataTypes
{
    public readonly struct DistanceResult(double meters, double miles)
    {
        public double Meters { get; } = meters;
        public double Miles { get; } = miles;
    }
}
