using System;

namespace NetduinoCar.Models
{
    public enum Direction
    {
        Stop,
        Forward,
        Back,
        Left,
        Right
    }

    public class ControlData
    {
        public Direction Direction { get; set; }
    }
}
