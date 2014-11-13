using System;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware;

namespace Netduino.Shared.Sensors
{
    public enum LightStatus
    {
        Dark,
        Dim,
        Moderate,
        Bright
    }

    public class PhotoCell
    {
        private AnalogInput photoSensor;

        public PhotoCell(Microsoft.SPOT.Hardware.Cpu.Pin pin)
        {
            photoSensor = new AnalogInput(pin);
        }

        public int GetQuantity()
        {
            return photoSensor.Read();
        }

        public LightStatus GetStatus()
        {
            LightStatus result;

            // Read the sensor state
            var val = photoSensor.Read();

            if (val <= 100)
                result = LightStatus.Dark;
            else if (val > 100 && val <= 250)
                result = LightStatus.Dim;
            else if (val > 250 && val <= 600)
                result = LightStatus.Moderate;
            else
                result = LightStatus.Bright;

            return result;
        }
    }
}
