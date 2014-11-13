using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;
using uPLibrary.Networking.M2Mqtt;
using System.Text;
using Netduino.Shared.Sensors;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;
using SecretLabs.NETMF.Hardware;

namespace Netduino.Micro
{
    public class Program
    {
        static MqttClient mqttClient;

        public static void Main()
        {
            var ni = NetworkInterface.GetAllNetworkInterfaces()[0];
            ni.EnableDhcp();
            //ni.RenewDhcpLease();


            string macAddress = "";

            // Create a character array for hexidecimal conversion.
            const string hexChars = "0123456789ABCDEF";

            // Loop through the bytes.
            for (int b = 0; b < 6; b++)
            {
                // Grab the top 4 bits and append the hex equivalent to the return string.
                macAddress += hexChars[ni.PhysicalAddress[b] >> 4];

                // Mask off the upper 4 bits to get the rest of it.
                macAddress += hexChars[ni.PhysicalAddress[b] & 0x0F];

                // Add the dash only if the MAC address is not finished.
                if (b < 5) macAddress += "-";
            }

            Debug.Print(macAddress);
            Debug.Print(ni.IPAddress);
            try
            {
                mqttClient = new MqttClient("opensensors.io");
                var conre = mqttClient.Connect("150", "jessephelps", "Jc4iTUlo");
            }
            catch (Exception ex)
            {
                
                throw;
            }

            

            var tempSensor = new Tmp36(Pins.GPIO_PIN_A0);
            //var lightSensor = new PhotoCell(Pins.GPIO_PIN_A1);
            //var piezoSensor = new AnalogInput(Pins.GPIO_PIN_A2);

            //while (true)
            //{
            //    if (piezoSensor.Read() > 100)
            //    {
            //        Debug.Print(piezoSensor.Read().ToString());
            //    }
            //}

            while (true)
            {
                var temp = tempSensor.GetTemperature();
              //  var light = lightSensor.GetQuantity();

                Debug.Print(temp.ToString());
                //Debug.Print(light.ToString());

                mqttClient.Publish("/users/jessephelps/temp", Encoding.UTF8.GetBytes(temp.ToString()));
                //mqttClient.Publish("/users/jessephelps/light", Encoding.UTF8.GetBytes(light.ToString()));
                Thread.Sleep(1000);
            }
        }

    }
}
