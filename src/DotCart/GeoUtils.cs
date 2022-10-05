namespace DotCart;
public static class GeoUtils
{
    public enum DistanceType
    {
        Miles = 0,
        Kilometers = 1
    }
    public static double DistanceTo(double sourceLat, double lat, double sourceLon, double lng, DistanceType dType)
    {
        var source = new GLatLng(sourceLat, sourceLon);
        return source.DistanceTo(lat, lng, dType);
    } // end DistanceTo
    public static double LongDistanceTo(double sourceLat, double lat, double sourceLon, double lng,
        DistanceType dType)
    {
        var source = new GLatLng(sourceLat, sourceLon);
        return source.LongDistanceTo(lat, lng, dType);
    } // end DistanceTo
    public static double LongRhumbDistanceTo(double sourceLat, double lat, double sourceLon, double lng,
        DistanceType dType)
    {
        var source = new GLatLng(sourceLat, sourceLon);
        return source.LongRhumbDistanceTo(lat, lng, dType);
    } // end DistanceTo
    public static double RhumbDistanceTo(double sourceLat, double lat, double sourceLon, double lng,
        DistanceType dType)
    {
        var source = new GLatLng(sourceLat, sourceLon);
        return source.RhumbDistanceTo(lat, lng, dType);
    }
    public static double RhumbBearingTo(double sourceLat, double lat, double sourceLon, double lng)
    {
        var source = new GLatLng(sourceLat, sourceLon);
        return source.RhumbBearingTo(lat, lng);
    }
    public static double BearingTo(double sourceLat, double lat, double sourceLon, double lng)
    {
        var source = new GLatLng(sourceLat, sourceLon);
        return source.RhumbBearingTo(lat, lng);
    }

    public record GLatLng
    {
        public const double EarthRadiusInMiles = 3956.0;
        public const double EarthRadiusInKilometers = 6367.0;
        public static double EarthCircInMiles = EarthRadiusInMiles * 2 * 3.14;
        public static double EarthCircInKilometers = EarthRadiusInKilometers * 2 * 3.14;
        private double _latitude;
        private double _longitude;
        public GLatLng(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }
        public double Latitude
        {
            get => _latitude;
            set => _latitude = value;
        }
        public double Longitude
        {
            get => _longitude;
            set => _longitude = value;
        }
        public double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public double RadianToDegree(double angle)
        {
            return 180.0 * angle / Math.PI;
        }
        public double DistanceTo(double lat, double lng, GeoUtils.DistanceType dType)
        {
            var r = dType == GeoUtils.DistanceType.Miles ? EarthRadiusInMiles : EarthRadiusInKilometers;
            var dLat = DegreeToRadian(lat) - DegreeToRadian(_latitude);
            var dLon = DegreeToRadian(lng) - DegreeToRadian(_longitude);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(DegreeToRadian(_latitude)) *
                Math.Cos(DegreeToRadian(lat)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = c * r;
            return Math.Round(distance, 2);
        } // end DistanceTo
        public double LongDistanceTo(double lat, double lng, GeoUtils.DistanceType dType)
        {
            var d = DistanceTo(lat, lng, dType);
            if (dType == GeoUtils.DistanceType.Miles)
                return EarthCircInMiles - d;
            return EarthCircInKilometers - d;
        }
        public double LongRhumbDistanceTo(double lat, double lng, GeoUtils.DistanceType dType)
        {
            var d = RhumbDistanceTo(lat, lng, dType);
            if (dType == GeoUtils.DistanceType.Miles)
                return EarthCircInMiles - d;
            return EarthCircInKilometers - d;
        }
        public double RhumbDistanceTo(double lat, double lng, GeoUtils.DistanceType dType)
        {
            var r = dType == GeoUtils.DistanceType.Miles ? EarthRadiusInMiles : EarthRadiusInKilometers;
            var lat1 = DegreeToRadian(_latitude);
            var lat2 = DegreeToRadian(lat);
            var dLat = DegreeToRadian(lat - _latitude);
            var dLon = DegreeToRadian(Math.Abs(lng - _longitude));

            var dPhi = Math.Log(Math.Tan(lat2 / 2 + Math.PI / 4) / Math.Tan(lat1 / 2 + Math.PI / 4));
            var q = Math.Cos(lat1);
            if (dPhi != 0) q = dLat / dPhi; // E-W line gives dPhi=0
            // if dLon over 180° take shorter rhumb across 180° meridian:
            if (dLon > Math.PI) dLon = 2 * Math.PI - dLon;
            var dist = Math.Sqrt(dLat * dLat + q * q * dLon * dLon) * r;
            return dist;
        } // end RhumbDistanceTo
        public double RhumbBearingTo(double lat, double lng)
        {
            var lat1 = DegreeToRadian(_latitude);
            var lat2 = DegreeToRadian(lat);
            var dLon = DegreeToRadian(lng - _longitude);

            var dPhi = Math.Log(Math.Tan(lat2 / 2 + Math.PI / 4) / Math.Tan(lat1 / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI) dLon = dLon > 0 ? -(2 * Math.PI - dLon) : 2 * Math.PI + dLon;
            var brng = Math.Atan2(dLon, dPhi);

            return (RadianToDegree(brng) + 360) % 360;
        } // end RhumbBearingTo
        public double BearingTo(double lat, double lng)
        {
            var lat1 = DegreeToRadian(_latitude);
            var lat2 = DegreeToRadian(lat);
            var dLon = DegreeToRadian(lng) - DegreeToRadian(_longitude);

            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            var brng = Math.Atan2(y, x);

            return (RadianToDegree(brng) + 360) % 360;
        } // end BearingTo



        public record GeoAngle
        {
            public bool IsNegative { get; set; }
            public int Degrees { get; set; }
            public int Minutes { get; set; }
            public int Seconds { get; set; }
            public int Milliseconds { get; set; }
            public static GeoAngle FromDouble(double angleInDegrees)
            {
                //ensure the value will fall within the primary range [-180.0..+180.0]
                while (angleInDegrees < -180.0)
                    angleInDegrees += 360.0;

                while (angleInDegrees > 180.0)
                    angleInDegrees -= 360.0;

                var result = new GeoAngle();

                //switch the value to positive
                result.IsNegative = angleInDegrees < 0;
                angleInDegrees = Math.Abs(angleInDegrees);

                //gets the degree
                result.Degrees = (int)Math.Floor(angleInDegrees);
                var delta = angleInDegrees - result.Degrees;

                //gets minutes and seconds
                var seconds = (int)Math.Floor(3600.0 * delta);
                result.Seconds = seconds % 60;
                result.Minutes = (int)Math.Floor(seconds / 60.0);
                delta = delta * 3600.0 - seconds;

                //gets fractions
                result.Milliseconds = (int)(1000.0 * delta);

                return result;
            }


            /// <summary>
            ///     Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                var degrees = IsNegative
                    ? -Degrees
                    : Degrees;

                return $"{degrees:000}°{Minutes:00}'{Seconds:00}\"";
            }


            /// <summary>
            ///     Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <param name="format">The format.</param>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            /// <exception cref="NotImplementedException"></exception>
            public string ToString(string format)
            {
                return format switch
                {
                    "NS" => $"{Degrees:000}°{Minutes:00}'{Seconds:00}\".{Milliseconds:000} {(IsNegative ? 'S' : 'N')}",
                    "WE" => $"{Degrees:000}°{Minutes:00}'{Seconds:00}\".{Milliseconds:000} {(IsNegative ? 'W' : 'E')}",
                    _ => throw new NotImplementedException()
                };
            }
        }
    }
}