using System;

namespace Trumpi.ObsControl
{
    public class Animation
    {
        public Animation(string[] components)
        {
            try
            {
                int index = 0;
                ItemName = components[index++];
                StartY = int.Parse(components[index++]);
                EndY = int.Parse(components[index++]);
                StartX = int.Parse(components[index++]);
                EndX = int.Parse(components[index++]);
                StartRotation = int.Parse(components[index++]);
                EndRotation = int.Parse(components[index++]);
                StartYScale = double.Parse(components[index++]);
                EndYScale = double.Parse(components[index++]);
                StartXScale = double.Parse(components[index++]);
                EndXScale = double.Parse(components[index]);
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        public string ItemName { get; set; }
        public int StartY { get; set; }
        public int EndY { get; set; }
        public int StartX { get; set; }
        public int EndX { get; set; }
        public int StartRotation { get; set; } = 0;
        public int EndRotation { get; set; } = 0;
        public double StartYScale { get; set; } = 1;
        public double EndYScale { get; set; } = 1;
        public double StartXScale { get; set; } = 1;
        public double EndXScale { get; set; } = 1;
    }
}