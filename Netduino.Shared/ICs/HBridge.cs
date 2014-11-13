using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Netduino.Shared.ICs
{
    public class HBridge
    {
        private OutputPort leftLogicPort1;
        private OutputPort leftLogicPort2;
        private OutputPort rightLogicPort1;
        private OutputPort rightLogicPort2;

        public HBridge(Cpu.Pin leftLogicPin1, Cpu.Pin leftLogicPin2, Cpu.Pin rightLogicPin1, Cpu.Pin rightLogicPin2)
        {
            leftLogicPort1 = new OutputPort (leftLogicPin1, false);
            leftLogicPort2 = new OutputPort(leftLogicPin2, false);
            rightLogicPort1 = new OutputPort(rightLogicPin1, false);
            rightLogicPort2 = new OutputPort(rightLogicPin2, false);
        }

        public void Stop()
        {
            leftLogicPort1.Write(false);
            leftLogicPort2.Write(false);
            rightLogicPort1.Write(false);
            rightLogicPort2.Write(false);
        }

        public void Forward()
        {
            Debug.Print("forward");
            leftLogicPort1.Write(true);
            leftLogicPort2.Write(false);
            rightLogicPort1.Write(true);
            rightLogicPort2.Write(false);
        }

        public void Back()
        {
            Debug.Print("back");
            leftLogicPort1.Write(false);
            leftLogicPort2.Write(true);
            rightLogicPort1.Write(false);
            rightLogicPort2.Write(true);
        }

        public void Right()
        {
            Debug.Print("right");
            leftLogicPort1.Write(true);
            leftLogicPort2.Write(false);
            rightLogicPort1.Write(false);
            rightLogicPort2.Write(true);
        }

        public void Left()
        {
            Debug.Print("left");
            leftLogicPort1.Write(false);
            leftLogicPort2.Write(true);
            rightLogicPort1.Write(true);
            rightLogicPort2.Write(false);
        }
    }
}
