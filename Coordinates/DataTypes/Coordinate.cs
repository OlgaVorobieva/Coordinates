﻿namespace Coordinates.DataTypes
{
    public struct Coordinate(double latitude, double longitude)
    {
        public double Latitude { get; set; } = latitude;
        public double Longitude { get; set; } = longitude;
    }
}
