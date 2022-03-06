using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib
{
    internal readonly struct Line
    {
        readonly public Point Start;
        readonly public Point End;

        public Line(Point start, Point end)
        {
            Start = start;
            End = end;
        }
        public Line(int startX, int startY, int endX, int endY)
        {
            Start = new Point(startX, startY);
            End = new Point(endX, endY);
        }

        public bool Intersects(Line other)
        {
            float q = (Start.Y - other.Start.Y) * (other.End.X - other.Start.X) 
                    - (Start.X - other.Start.X) * (other.End.Y - other.Start.Y);
            float d = (End.X - Start.X) * (other.End.Y - other.Start.Y) 
                    - (End.Y - Start.Y) * (other.End.X - other.Start.X);

            if (ProjMath.AboutEqual(d,0))
            {
                return false;
            }

            float r = q / d;

            q = (Start.Y - other.Start.Y) * (End.X - Start.X) - (Start.X - other.Start.X) * (End.Y - Start.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }
    }
}
