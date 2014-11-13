using System;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware;

namespace Netduino.Shared.Sensors
{
    public enum TempUnits
    {
        Kelvin,
        Celsius,
        Fahrenheit
    }

    public class Tmp36
    {
        private AnalogInput tempSensor;

        public Tmp36(Microsoft.SPOT.Hardware.Cpu.Pin pin)
        {
            tempSensor = new AnalogInput(pin);
            tempSensor.SetRange(0, 1024);
        }

        public Double GetTemperature(TempUnits units = TempUnits.Fahrenheit)
        {
            // Read the temp sensor
            Double read = tempSensor.Read();

            // convert the reading to voltage
            Double voltage = read * 3.3 / 1024;

            // calculate Celsius from voltage
            Double tempC = (voltage - 0.5) * 100;

            if (units == TempUnits.Celsius)
            {
                return tempC;
            }

            if (units == TempUnits.Fahrenheit)
            {
                // calculate Fahrenheit from Celsius
                Double tempF = (tempC * 9 / 5) + 32;

                return tempF;
            }
            else if (units == TempUnits.Kelvin)
            {
                // calculate Kelvin from Celsius
                return tempC + 273.15;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
