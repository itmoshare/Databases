using System;
using System.Globalization;

namespace RestApi.Models
{
    public class Position
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Latitude} {Longitude}";
        }

        public static Position Parse(string str)
        {
            var t = str.Split(' ');
            return new Position
            {
                Latitude = double.Parse(t[0], CultureInfo.InvariantCulture),
                Longitude = double.Parse(t[1], CultureInfo.InvariantCulture)
            };
        }
    }
}
