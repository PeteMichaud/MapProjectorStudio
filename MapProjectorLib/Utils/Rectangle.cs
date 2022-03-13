//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MapProjectorLib
//{
//    internal struct Rectangle
//    {
//        public Point Origin;
//        public int Width;
//        public int Height;

//        public int X => Origin.X;
//        public int Y => Origin.Y;

//        public Point TopLeft => Origin;
//        public Point TopRight => new Point(Origin.X + Width, Origin.Y);
//        public Point BottomLeft => new Point(Origin.X, Origin.Y + Height);
//        public Point BottomRight => new Point(Origin.X + Width, Origin.Y + Height);

//        public Line TopLine => new Line(TopLeft, TopRight);
//        public Line BottomLine => new Line(BottomLeft, BottomRight);
//        public Line LeftLine => new Line(TopLeft, BottomLeft);
//        public Line RightLine => new Line(TopRight, BottomRight);

//        public bool Intersects(Line line)
//        {
//            return Contains(line) ||
//                line.Intersects(TopLine) ||
//                line.Intersects(BottomLine) ||
//                line.Intersects(LeftLine) ||
//                line.Intersects(RightLine)
//            ;
//        }

//        public bool Contains(Point pt)
//        {
//            return pt.X >= Origin.X && pt.X <= Origin.X + Width
//                && pt.Y >= Origin.Y && pt.Y <= Origin.Y + Height;
//        }

//        public bool Contains(Line line)
//        {
//            return Contains(line.Start) && Contains(line.End);
//        }

//    }
//}
