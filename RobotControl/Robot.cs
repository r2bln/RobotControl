using System;
using System.IO.Ports;

namespace RobotControl
{
    public class Robot
    {
        public int x;
        public int y;
        public int obstRange;
        public string data;

        private double dist1;
        private double dist2;
        private double facing;
        private string portName;
        private SerialPort port;
        
        public Robot()
        {
            x = 0;
            y = 0;
            dist1 = 0;
            dist2 = 0;
            facing = 0;
        }

        ~Robot()
        {
            port.Close();
        }

        public Robot(int _x, int _y, double _dist1, double _dist2, double _facing, string _portName)
        {
            x = _x;
            y = _y;
            dist1 = _dist1;
            dist2 = _dist2;
            facing = _facing;
            portName = _portName;
            
            port = new SerialPort(portName, 9600, Parity.None);
            port.Open();
            port.DataReceived += port_DataReceived;
            
        }

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Посылка dist1,dist2,cm,action
            try
            {
                // Иногда приходит не целиком и валится. Хз почиму. Серийник же...
                data = port.ReadLine();
                var dataArr = data.Split(',');
                dist1 = Convert.ToDouble(dataArr[0]);
                dist2 = Convert.ToDouble(dataArr[1]);
                obstRange = Convert.ToInt32(dataArr[2]);
            }
            catch
            {

            }
        }
    }
}
