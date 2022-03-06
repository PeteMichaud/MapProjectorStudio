using System;

namespace MapProjectorLib
{
    internal static class ProjMath
    {
        public const double TwoPi = 2.0 * Math.PI;
        public const double OneOverPi = 1.0 / Math.PI;
        public const double TwoOverPi = 2.0 / Math.PI;
        public const double PiOverTwo = Math.PI / 2.0;
        public const double OneOverTwoPi = 1.0 / (2.0 * Math.PI);

        public const double SecondsPerDay = 86400;

        //

        // 16 minutes half-diameter + 34 minutes refraction

        // The earth
        public const double Eccentricity = 0.017;

        public const double
            PerihelionTime = -1.323859; // Time from equinox to perihelion

        public const double DaysInYear = 365.242;

        // It's orbit
        public static double Inclination = ToRadians(23.45);

        public static double
            Perihelion =
                ToRadians(12.25); // Angle from winter solstice to perihelion

        public static double
            EquinoxAngle =
                Math.PI / 2.0 - Perihelion; // Angle from equinox to perihelion

        // Sun declination for sunrise/sunset
        public static double SunriseAngle = ToRadians(-(16.0 + 34) / 60.0);

        public static double Cot(double theta)
        {
            return 1.0 / Math.Tan(theta);
        }

        public static double ToRadians(double deg)
        {
            return deg * Math.PI / 180.0;
        }

        public static double HoursToRadians(double hours)
        {
            return hours * Math.PI / 12.0;
        }

        public static double ToDegrees(double rad)
        {
            return rad * 180.0 / Math.PI;
        }

        public static double Hours(double rad)
        {
            return rad * 12.0 / Math.PI;
        }

        public static double Sqr(double x)
        {
            return x * x;
        }

        public static double Cube(double x)
        {
            return x * x * x;
        }

        public static double Frac(double x)
        {
            return x - Math.Floor(x);
        }

        public static bool FindRoot(
            double t0, double t1, double epsilon, ref double t,
            Func<double, double> projectionEquation)
        {
            var p0 = projectionEquation(t0);
            var p1 = projectionEquation(t1);

            if (p0 > p1)
            {
                var temp = t0;
                t0 = t1;
                t1 = temp;
                temp = p0;
                p0 = p1;
                p1 = temp;
            }

            ;

            if (p0 > 0 || p1 < 0)
                throw new ArgumentException(
                    $"No Root for {t0} {t1} {p0} {p1}");

            while (t1 - t0 > epsilon)
            {
                var t2 = (t0 + t1) / 2.0;
                var p2 = projectionEquation(t2);
                if (p2 <= 0.0)
                    t0 = t2;
                else
                    t1 = t2;
            }

            t = t0;
            return true;
        }

        // The angle the sun is round from perihelion at time m, measured from
        // perihelion.
        public static double PerihelionAngle(double m)
        {
            var e = Eccentricity;
            var r = m + 2 * e * Math.Sin(m) + 5 * Sqr(e) * Math.Sin(2 * m) / 4 +
                    Cube(e) * (-Math.Sin(m) / 4 + 13 * Math.Sin(3 * m) / 12);
            return r;
        }


        // Return the time, relative to the spring equinox, of the perihelion
        // Since this is constant, the result now in equations.h
        public static double GetPerihelionTime()
        {
            double theta0 = 0;
            var theta1 = 2 * Math.PI;
            const double epsilon = 1e-8;
            while (theta1 - theta0 > epsilon)
            {
                var theta2 = (theta0 + theta1) / 2.0;
                var p = PerihelionAngle(theta2) - EquinoxAngle;
                if (p <= 0.0)
                    theta0 = theta2;
                else
                    theta1 = theta2;
            }

            return -theta1;
        }


        // The value of the equation of time
        // mean time + equation = apparent time
        //computes the difference between apparent solar time and mean solar time
        public static double EquationOfTime(double date)
        {
            var p = Perihelion;
            var m = date - PerihelionTime;
            var s =
                -591.7 * Math.Sin(2 * (m + p))
                - 459.6 * Math.Sin(m) +
                +19.8 * Math.Sin(m + 2 * p)
                - 19.8 * Math.Sin(3 * m + 2 * p)
                - 12.8 * Math.Sin(4 * (m + p))
                - 4.8 * Math.Sin(2 * m)
                + 0.9 * Math.Sin(3 * m + 4 * p)
                - 0.9 * Math.Sin(5 * m + 4 * p)
                - 0.5 * Math.Sin(4 * m + 2 * p)
                - 0.4 * Math.Sin(6 * (m + p));

            return s; // seconds
        }

        // The angle the sun is round from the vernal equinox at date,
        // also measured from the vernal equinox.
        public static double SunAngle(double date)
        {
            // Date is measured from vernal equinox
            // Need time from perihelion to equinox

            // Add the time from the perihelion and equinox
            var t = date - PerihelionTime;
            // r is the angle of the sun, from the perihelion
            var r = PerihelionAngle(t);
            // So subtract the angle between perihelion and equinox
            return r - EquinoxAngle;
        }

        public static double SunHeight(double date)
        {
            return Math.Sin(SunAngle(date)) * Math.Sin(Inclination);
        }

        public static double SunDec(double date)
        {
            return Math.Asin(SunHeight(date));
        }

        public static double SunAltitude(double date, double phi, double time)
        {
            // Compute altitude of sun at the given date and time,
            // on the zero meridian at latitude phi
            var delta = Math.Asin(SunHeight(date)); // Declination of the sun

            // Rotate lat = phi, long = 0 round by time
            // Find x,y,z coords on the sphere - no need to use an ellipsoid, as the
            // tangents at a location are the same.
            var x = Math.Cos(time) * Math.Cos(phi);
            var y = Math.Sin(time) * Math.Cos(phi);
            var z = Math.Sin(phi);

            // The sun vector - the sun is at -x
            var sunx = -Math.Cos(delta);
            double suny = 0;
            var sunz = Math.Sin(delta);

            // Dot product to get angle to normal
            var alt = Math.PI / 2.0 - Math.Acos(x * sunx + y * suny + z * sunz);
            return alt;
        }

        public static double Sunrise(double date, double phi)
        {
            double t0 = 0;
            var t1 = Math.PI;
            const double epsilon = 1e-6;
            while (t1 - t0 > epsilon)
            {
                var t = (t1 + t0) / 2.0;
                var h = SunAltitude(date + t / DaysInYear, phi, t);

                if (h < SunriseAngle)
                    t0 = t;
                else
                    t1 = t;
            }

            return t0;
        }

        const double epsilon = 1E-6;
        public static bool AboutEqual(double d1, double d2)
        {
            return Math.Abs(d1 - d2) <= epsilon;
        }

    }
}