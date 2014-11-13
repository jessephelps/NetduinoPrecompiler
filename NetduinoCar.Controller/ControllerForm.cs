using NetduinoCar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetduinoCar.Controller
{
    public partial class ControllerForm : Form
    {
        //PrivateFontCollection pfc;

        Direction direction = Direction.Stop;

        public ControllerForm()
        {
            InitializeComponent();
            //pfc = new PrivateFontCollection();
            //pfc.AddFontFile("font\\icomoon.ttf");
            serialPort1.Open();
        }

        private void ControllerForm_Load(object sender, EventArgs e)
        {
            //button1.Font = new Font(pfc.Families[0], 16, FontStyle.Regular);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var controlData = new ControlData() { Direction = direction };

            var transmit = Newtonsoft.Json.JsonConvert.SerializeObject(controlData);

            serialPort1.WriteLine(transmit);
        }

        public void updateTemp(double temp)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<double>(updateTemp), temp);
            }
            else
            {
                tempTextBox.Text = temp.ToString();
            }
        }

        public void updateLight(string light)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(updateLight), light);
            }
            else
            {
                lightsTextBox.Text = light;
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var buffer = serialPort1.ReadLine();
                var displaydata = Newtonsoft.Json.JsonConvert.DeserializeObject<DisplayData>(buffer);
                updateTemp(displaydata.Temp);
                updateLight(displaydata.Light);
            }
            catch (Exception)
            {
            }
        }

        private void fwdButton_Click(object sender, EventArgs e)
        {
            direction = Direction.Forward;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            direction = Direction.Stop;
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            direction = Direction.Left;
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            direction = Direction.Right;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            direction = Direction.Back;
        }
    }
}
