using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;

namespace MFConsoleApplication1
{
    public class Program
    {
        static OutputPort led;

        public static void Main()
        {
            led = new OutputPort(Pins.ONBOARD_LED, false);
            bool interrupt = false;

            if (interrupt)
            {
                var button = new InterruptPort(Pins.ONBOARD_SW1, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
                button.OnInterrupt += button_OnInterrupt;
                while (true)
                {

                }
            }
            else
            {
                var inputbutton = new InputPort(Pins.ONBOARD_SW1, true, Port.ResistorMode.Disabled);

                while (true)
                {
                    led.Write(inputbutton.Read());
                }
            }
        }

        static void button_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (data2 == 1)
            {
                led.Write(true);
            }
            else if (data2 == 0)
                led.Write(false);
        }

    }
}
