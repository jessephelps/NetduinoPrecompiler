using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using NetduinoCar.Models;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System;
using System.Collections;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Microsoft.SPOT.Net.NetworkInformation;
using Netduino.Shared.Sensors;
using Netduino.Shared.ICs;

namespace NetduinoCar
{
    public class Program
    {
        static SerialPort bluetooth;
        //static Tmp36 tempSensor;
        //static PhotoCell lightSensor;
        static HBridge hBridge;
        //static SecretLabs.NETMF.Hardware.PWM pwm;
        static Json.NETMF.JsonSerializer serializer;
        static ArrayList receivedBytes;

        public static void Main()
        {
            serializer = new Json.NETMF.JsonSerializer();
            receivedBytes = new ArrayList();

            bluetooth = new SerialPort("COM2", 115200);

            //tempSensor = new Tmp36(Pins.GPIO_PIN_A0);
            //lightSensor = new PhotoCell(Pins.GPIO_PIN_A1);

            hBridge = new HBridge(Pins.GPIO_PIN_D12, Pins.GPIO_PIN_D13, Pins.GPIO_PIN_D10, Pins.GPIO_PIN_D11);

            //pwm = new SecretLabs.NETMF.Hardware.PWM(Pins.GPIO_PIN_D5);
            //var period = (uint)26;
            //var duration = period / 2;
            //pwm.SetPulse(period, duration);

            bluetooth.Open();

            var commandmode = Encoding.UTF8.GetBytes("$$$");
            bluetooth.Write(commandmode, 0, commandmode.Length);

            System.Threading.Thread.Sleep(1000);
            var bytes = Encoding.UTF8.GetBytes("U,9600,N\n");
            bluetooth.Write(bytes, 0, bytes.Length);

            bluetooth.Close();
            bluetooth.BaudRate = 9600;
            bluetooth.Open();

            ////var timerCallback = new TimerCallback(TransmitStatusData);
            ////var timer = new Timer(timerCallback, null, 1000, 1000);

            SerialReceiver();
        }

        //static void TransmitStatusData(object obj)
        //{
        //    var displayData = new DisplayData() { Temp = tempSensor.GetTemperature() };
        //    var serial = serializer.Serialize(displayData);

        //    var tempStringBytes = Encoding.UTF8.GetBytes(serial + "\n");
        //    bluetooth.Write(tempStringBytes, 0, tempStringBytes.Length);
        //}

        static Direction direction;

        static void ProcessControlData(string controlData)
        {
            try
            {
                var test = serializer.Deserialize(controlData) as Hashtable;
                var newDir = (Direction)Convert.ToInt32(test["Direction"].ToString());

                if (newDir != direction)
                {
                    direction = newDir;

                    if (newDir == Direction.Forward)
                        hBridge.Forward();
                    else if (newDir == Direction.Back)
                        hBridge.Back();
                    else if (newDir == Direction.Left)
                        hBridge.Left();
                    else if (newDir == Direction.Right)
                        hBridge.Right();
                    else if (newDir == Direction.Stop)
                        hBridge.Stop();
                }

                //var displayData = new DisplayData() { Temp = tempSensor.GetTemperature(), Light = lightSensor.GetStatus().ToString() };
                //var serial = serializer.Serialize(displayData);

                //var tempStringBytes = Encoding.UTF8.GetBytes(serial + "\n");
                //bluetooth.Write(tempStringBytes, 0, tempStringBytes.Length);

            }
            catch (Exception)
            {

                //throw;
            }
        }

        static void SerialReceiver()
        {
            byte[] inBuffer = new byte[128];
            string t = "";

            while (true)
            {
                int count = bluetooth.Read(inBuffer, 0, inBuffer.Length);

                if (count > 0)
                {
                    char[] chars = Encoding.UTF8.GetChars(inBuffer);
                    t += new string(chars, 0, count);

                    if (t.IndexOf('\n') != -1)
                    {
                        ProcessControlData(t);
                        t = "";
                    }
                }
                Thread.Sleep(50);
            }
        }
    }
}
