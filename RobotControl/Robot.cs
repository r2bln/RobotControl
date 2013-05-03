using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace RobotControl
{
    public class Robot
    {
        public int x;
        public int y;
        public int gtX;
        public int gtY;
        public int obstRange;
        public string data;
        public double facing;
        public bool selected;

        private byte[] buf;
        private int ipPort;
        private double dist1;
        private double dist2;
        private string address;
        private string portName;
        private SerialPort port;
        private Socket s;

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
            //port.Close();
        }

        public Robot(int _x, int _y, double _dist1, double _dist2, double _facing, string _portName)
        {
            x = _x;
            y = _y;
            dist1 = _dist1;
            dist2 = _dist2;
            facing = _facing;
            portName = _portName;
        }

        public Robot(int _x, int _y, double _dist1, double _dist2, double _facing, string _address, int _port)
        {
            x = _x;
            y = _y;
            dist1 = _dist1;
            dist2 = _dist2;
            facing = _facing;
            address = _address;
            ipPort = _port;
        }

        public void ConnectSerial()
        {
            port = new SerialPort(portName, 9600, Parity.None);
            port.Open();
            port.DataReceived += port_DataReceived;
        }

        public void ConnectIp()
        {
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(new IPEndPoint(IPAddress.Parse(address), ipPort));
            buf = new byte[32];
            s.BeginReceive(buf, 0, 32, SocketFlags.None, OnDataRecieved, null);
        }

        private void OnDataRecieved(IAsyncResult ar)
        {

            try
            {
                // Если сообщения будут приходить ОЧЕНЬ часто то буду слипаться и неправильно парситься.
                // Нужен СДЦ.

                data = Encoding.ASCII.GetString(buf);
                var dataArr = data.Split(',');
                x = Convert.ToInt32(dataArr[0]);
                y = Convert.ToInt32(dataArr[1]);
                obstRange = Convert.ToInt32(dataArr[2]);
                facing = Convert.ToDouble(dataArr[3]);
            }

            catch
            {
                // Посылка побилась
            }

            Array.Clear(buf, 0, 32);

            try
            {
                s.BeginReceive(buf, 0, 32, SocketFlags.None, OnDataRecieved, null);
            }
            catch
            {

            }
            
            
        }

        public void SendData(string msg)
        {
            s.Send(Encoding.ASCII.GetBytes(msg));
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
