namespace MapProjectorLib
{
    public class LoopParams
    {
        public double DateIncr = 0; //Days
        public double LatIncr = 0; //(Degrees)
        public double LongIncr = 0; //(Degrees)

        public int LoopCount = 1; //Number of Images to Output along given increments

        public double TiltIncr = 0; //(Degrees)
        public double TimeIncr = 0; //Hours
        public double TurnIncr = 0; //(Degrees)

        public double xIncr = 0; //Pixels
        public double yIncr = 0; //Pixels
        public double zIncr = 0; //Z (Zoom) Increment (Pixels)

    }
}