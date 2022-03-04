using System;

namespace MapProjectorLib
{
    public class LoopParams
    {

        public int LoopCount = 1; //Number of Images to Output along given increments

        public double TiltIncr = 0; //(Degrees)
        public double TurnIncr = 0; //(Degrees)
        public double LatIncr = 0; //(Degrees)
        public double LongIncr = 0; //(Degrees)

        public double xIncr = 0; //Pixels
        public double yIncr = 0; //Pixels
        public double zIncr = 0; //Z (Zoom) Increment (Pixels)

        public double DateIncr = 0; //Days
        public double TimeIncr = 0; //Hours

        public override string ToString()
        {
            return string.Format(
@"Loop Params:

    Loop Count: {0}
    Tilt Incr: {1}
    Turn Incr: {2}
    Lat Incr: {3}
    Long Incr: {4}
    X Incr: {5}
    Y Incr: {6}
    Z Incr: {7}
    Date Incr: {8}
    Time Incr: {9}
",
LoopCount,
TiltIncr,
TurnIncr,
LatIncr,
LongIncr,
xIncr,
yIncr,
zIncr,
DateIncr,
TimeIncr
);
        }

    }
}
