using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib
{
    static class ProjMath
    {
        public const double TwoPi = 2.0 * Math.PI;
        public const double OneOverPi = 1.0 / Math.PI;
        public const double TwoOverPi = 2.0 / Math.PI;
        public const double PiOverTwo = Math.PI / 2.0;
        public const double OneOverTwoPi = 1.0 / (2.0 * Math.PI);

        public const double SecsPerDay = 86400;

        static public double Cot(double theta)
        {
            return 1.0 / Math.Tan(theta);
        }

        static public double ToRadians(double deg) { 
            return deg * Math.PI / 180.0; 
        }

        static public double HoursToRadians(double hours) { 
            return hours * Math.PI / 12.0; 
        }

        static public double ToDegrees(double rad) { 
            return rad * 180.0 / Math.PI; 
        }

        static public double Hours(double rad) { 
            return rad * 12.0 / Math.PI; 
        }

        static public double Sqr(double x) { 
            return x * x; 
        }

        static public double Cube(double x) {
            return x * x * x; 
        }

        static public double Frac(double x) { 
            return x - Math.Floor(x); 
        }

        //

        // 16 minutes half-diameter + 34 minutes refraction

        // The earth
        public const double Eccentricity = 0.017;

        // It's orbit
        static public double Inclination = ToRadians(23.45);
        static public double Perihelion = ToRadians(12.25);// Angle from winter solstice to perihelion
        static public double EquinoxAngle = Math.PI / 2.0 - Perihelion; // Angle from equinox to perihelion
        public const double PerihelionTime = -1.323859; // Time from equinox to perihelion

        // Sun declination for sunrise/sunset
        static public double SunriseAngle = ToRadians(-(16.0 + 34) / 60.0);
        public const double DaysInYear = 365.242;

        public static bool FindRoot(double t0, double t1, double epsilon, ref double t, Func<double, double> projectionEquation)
        {

            double p0 = projectionEquation(t0);
            double p1 = projectionEquation(t1);
          
            if (p0 > p1) {
                double temp;
                temp = t0; t0 = t1; t1 = temp;
                temp = p0; p0 = p1; p1 = temp;
            };

            if (p0 > 0 || p1< 0) {
                throw new ArgumentException(string.Format("No Root for", t0, t1, p0, p1));
            } 
            else
            {
                while (t1 - t0 > epsilon)
                {
                    double t2 = (t0 + t1) / 2.0;
                    double p2 = projectionEquation(t2);
                    if (p2 <= 0.0)
                    {
                        t0 = t2;
                    }
                    else
                    {
                        t1 = t2;
                    }
                }
                t = t0;
                return true;
            }
        }

        // The angle the sun is round from perihelion at time m, measured from
        // perihelion.
        public static double PerihelionAngle(double m)
        {
            double e = Eccentricity;
            double r = m + 2 * e * Math.Sin(m) + 5 * ProjMath.Sqr(e) * Math.Sin(2 * m) / 4 +
                       ProjMath.Cube(e) * (-Math.Sin(m) / 4 + 13 * Math.Sin(3 * m) / 12);
            return r;
        }


        // Return the time, relative to the spring equinox, of the perihelion
        // Since this is constant, the result now in equations.h
        public static double GetPerihelionTime()
        {
            double theta0 = 0;
            double theta1 = 2 * Math.PI;
            double epsilon = 1e-8;
            while (theta1 - theta0 > epsilon)
            {
                double theta2 = (theta0 + theta1) / 2.0;
                double p = PerihelionAngle(theta2) - EquinoxAngle;
                if (p <= 0.0)
                {
                    theta0 = theta2;
                }
                else
                {
                    theta1 = theta2;
                }
            }

            return -theta1;
        }


        // The value of the equation of time
        // mean time + equation = apparent time
        public static double EquationOfTime(double date)
        {
            double p = Perihelion;
            double m = date - PerihelionTime;
            double s =
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
            double t = date - PerihelionTime;
            // r is the angle of the sun, from the perihelion
            double r = PerihelionAngle(t);
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
            double delta = Math.Asin(SunHeight(date)); // Declination of the sun

            // Rotate lat = phi, long = 0 round by time
            // Find x,y,z coords on the sphere - no need to use an ellipsoid, as the
            // tangents at a location are the same.
            double x = Math.Cos(time) * Math.Cos(phi);
            double y = Math.Sin(time) * Math.Cos(phi);
            double z = Math.Sin(phi);

            // The sun vector - the sun is at -x
            double sunx = -Math.Cos(delta);
            double suny = 0;
            double sunz = Math.Sin(delta);

            // Dot product to get angle to normal
            double alt = Math.PI / 2.0 - Math.Acos(x * sunx + y * suny + z * sunz);
            return alt;
        }

        public static double Sunrise(double date, double phi)
        {
            double t0 = 0;
            double t1 = Math.PI;
            double epsilon = 1e-6;
            while (t1 - t0 > epsilon)
            {
                double t = (t1 + t0) / 2.0;
                double h = SunAltitude(date + t / DaysInYear, phi, t);

                if (h < SunriseAngle)
                {
                    t0 = t;
                }
                else
                {
                    t1 = t;
                }
            }
            return t0;
        }
    }
}
