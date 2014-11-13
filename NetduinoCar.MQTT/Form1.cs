using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace NetduinoCar.MQTT
{
    public partial class Form1 : Form
    {
        MqttClient mqttClient;
        
        public Form1()
        {
            InitializeComponent();
            var myModel = new PlotModel { Title = "Example 1" };

            var leftAxis = new LinearAxis();
            leftAxis.Position = AxisPosition.Left;
            leftAxis.Key = "Primary";
            leftAxis.Title = "Primary";

            var rightAxis = new LinearAxis();
            rightAxis.Position = AxisPosition.Right;
            rightAxis.Key = "Secondary";
            rightAxis.Title = "Secondary";

            myModel.Axes.Add(leftAxis);
            myModel.Axes.Add(rightAxis);

            var tempSeries = new LineSeries();
            tempSeries.Title = "Temperature";
            tempSeries.YAxisKey = "Primary";

            var lightSeries = new LineSeries();
            lightSeries.Title = "Light";
            lightSeries.YAxisKey = "Secondary";

            myModel.Series.Add(tempSeries);
            myModel.Series.Add(lightSeries);

            this.plot1.Model = myModel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mqttClient = new MqttClient("opensensors.io");
            mqttClient.MqttMsgPublishReceived += mqttClient_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            mqttClient.Connect("169", "jessephelps", "0JuwB0La");
            mqttClient.Subscribe(
                new string[] { "/users/jessephelps/temp", "/users/jessephelps/light" }, 
                new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }
            ); 
        }

        void mqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (e.Topic == "/users/jessephelps/temp")
            {
                var temp = Encoding.UTF8.GetString(e.Message);
                var series = (this.plot1.Model.Series[0] as LineSeries);

                series.Points.Add(new DataPoint(series.Points.Count, Convert.ToDouble(temp)));
            }
            else if (e.Topic == "/users/jessephelps/light")
            {
                var light = Encoding.UTF8.GetString(e.Message);
                var series = (this.plot1.Model.Series[1] as LineSeries);

                series.Points.Add(new DataPoint(series.Points.Count, Convert.ToInt32(light)));
            }
            else
            {
                throw new ArgumentException();
            }

            this.plot1.Model.InvalidatePlot(true);
        }
    }
}
