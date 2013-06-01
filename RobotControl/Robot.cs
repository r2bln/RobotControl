using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Drawing;

namespace RobotControl
{
    public class Robot
    {
        public int x;
        public int y;
        public int gtX;
        public int gtY;
        public int id;
        public int obstRange;
        public string data;
        public double facing;
        public bool selected;
        public bool connectionFailed;
        public string connection;

        private byte[] buf;
        private int ipPort;
        private double dist1;
        private double dist2;
        private string address;
        private string portName;
        private SerialPort port;
        private Socket s;
        private Bitmap robotBmp = new Bitmap(Resources.Robot);

        // Делегат события OnConnectionlost
        public delegate void RobotDelegate(Robot sender, EventArgs e);

        // Создаем событие с этим делегатом
        public event RobotDelegate OnConnectionLost;
        public event RobotDelegate OnDataRecievedExternal;


        // Всякие конструкторы.
        public Robot()
        {
            x = 0;
            y = 0;
            dist1 = 0;
            dist2 = 0;
            facing = 0;
        }

        public Robot(int _x, int _y, double _dist1, double _dist2, double _facing, string _portName)
        {
            x = _x;
            y = _y;
            dist1 = _dist1;
            dist2 = _dist2;
            facing = _facing;
            portName = _portName;
            gtX = 32;
            gtY = 32;
            connection = portName;
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
            gtX = 32;
            gtY = 32;
            connection = address + ":" + ipPort.ToString();
        }

        public void Draw(Graphics canvas, Map map)
        {
            // Координаты верхнего левого угла картинки робота
            var actualX = x + map.offsetX - robotBmp.Width / 2;
            var actualY = map.height + map.offsetY - (y + robotBmp.Height / 2);

            // Координаты центра робота
            var centerX = x + map.offsetX;
            var centerY = map.height + map.offsetY - y;

            // Координаты метки
            var markX = obstRange * Math.Sin((facing) * Math.PI / 180) + centerX;
            var markY = -obstRange * Math.Cos((facing) * Math.PI / 180) + centerY;

            // Поворачиваем робота
            var tmpBmp = RotateImageByAngle(robotBmp, (float)facing); // Пашет

            // Рисуем робота и его центр
            canvas.DrawImage(tmpBmp, actualX, actualY);
            canvas.FillRectangle(new SolidBrush(Color.Red), centerX, centerY, 2, 2);

            // Если на робота кликнули - рисуем рамку и выводим данные о нем на форму

            if (selected)
            {
                canvas.DrawRectangle(new Pen(Color.Red), actualX, actualY, robotBmp.Width, robotBmp.Height);
                if (map.parent != null)
                {
                    map.parent.listBox1.Items.Clear();
                    map.parent.listBox1.Items.Add("Номер: " + id);
                    map.parent.listBox1.Items.Add("X: " + x.ToString());
                    map.parent.listBox1.Items.Add("Y: " + y.ToString());
                    map.parent.listBox1.Items.Add("Дальномер: " + obstRange.ToString());
                    map.parent.listBox1.Items.Add("Азимут: " + facing.ToString());
                    map.parent.listBox1.Items.Add("Соединение: " + connection);
                    if (!connectionFailed)
                        map.parent.listBox1.Items.Add("Активно...");
                    else
                        map.parent.listBox1.Items.Add("РАЗОРВАНО!");
                } 
            }

            if (connectionFailed)
            {
                // Рисуем рамку вокруг робота и крест (Х_х)
                canvas.DrawRectangle(new Pen(Color.Red), actualX, actualY, robotBmp.Width, robotBmp.Height);
                canvas.DrawLine(new Pen(Color.Red), actualX, actualY, actualX + robotBmp.Width, actualY + robotBmp.Height);
                canvas.DrawLine(new Pen(Color.Red), actualX + robotBmp.Width, actualY, actualX, actualY + robotBmp.Height);
                map.parent.Reconnect.Enabled = true;
            }

            // Куда робот движется
            canvas.DrawLine(new Pen(Color.Red), centerX, centerY, gtX, gtY);

            // В разумных ли приделах показания дальномера?
            if (obstRange > 5 && obstRange < 150)
            {
                // Рисуем метку и линию до нее
                canvas.FillRectangle(new SolidBrush(Color.Black), (float)markX, (float)markY, 2, 2);
                canvas.DrawLine(new Pen(Color.Red), centerX, centerY, (float)markX, (float)markY);

                // Сохраняем метку на карте

                try
                {
                    map.bgMap.SetPixel((int)markX - map.offsetX, (int)markY - map.offsetY, Color.Black);
                }
                catch
                {
                    // Зашкалило.
                }
            }
        }

        public void Disconnect()
        {
            // Вызывается при удалении робота.
            // Отключаемся.

            /* Я хуй знает почему это не работает....
            if (port != null && port.IsOpen)
            {
                port.Close();
            }
            */

            try
            {
                port.Close();
            }
            catch { }

            if (s != null && s.Connected)
            {
                s.Close();
            }
        }

        public void ConnectSerial()
        {
            // Открываем поледовательный порт к реальному роботу

            port = new SerialPort(portName, 9600, Parity.None);
            port.Open();

            // Подписываемся на событие приема данных

            port.DataReceived += port_DataReceived;
        }

        public void ConnectIp()
        {
            // Инитаем сокет
            // Подключаемся к виртуальному роботу (МБ все таки серьезней сделать сервак с покатушками?)
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(new IPEndPoint(IPAddress.Parse(address), ipPort));

            // Инитаем буфер для приема данных
            buf = new byte[32];

            // начинаем асинхронно ждать данных
            s.BeginReceive(buf, 0, 32, SocketFlags.None, OnDataRecieved, null);
        }

        private void OnDataRecieved(IAsyncResult ar)
        {
            // Пришли данные. Пытаемся распаковать и ждем дальше.
            // SocketType.Stream какаято муть иногда слипается ногда не доходит 

            OnDataRecievedExternal(this, null);

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
                // Слушаем дальше
                s.BeginReceive(buf, 0, 32, SocketFlags.None, OnDataRecieved, null);
            }
            catch
            {
                // Сокет таки не отвалился и все еще открыт, либо сервак лег
                // Кидаем ивент
                OnConnectionLost(this, null);
                connectionFailed = true;
                s.Close();
            }
        }

        public void SendData(string msg)
        {
            // Посылаем данные виртуальному роботу

            if (s != null && s.Connected)
            {
                try
                {
                    s.Send(Encoding.ASCII.GetBytes(msg));
                }
                catch
                {
                    OnConnectionLost(this, null);
                    connectionFailed = true;
                }
            }

            else if (port != null && port.IsOpen)
            {
                try
                {
                    port.WriteLine(msg);
                }
                catch
                {
                    OnConnectionLost(this, null);
                    connectionFailed = true;
                }
            }
        }

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Посылка dist1,dist2,cm,facing
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
                OnConnectionLost(this, null);
                connectionFailed = true;
            }
        }

        private static Bitmap RotateImageByAngle(System.Drawing.Image oldBitmap, float angle)
        {
            var newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height);
            var graphics = Graphics.FromImage(newBitmap);
            graphics.TranslateTransform((float)oldBitmap.Width / 2, (float)oldBitmap.Height / 2);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-(float)oldBitmap.Width / 2, -(float)oldBitmap.Height / 2);
            graphics.DrawImage(oldBitmap, new Point(0, 0));
            return newBitmap;
        }
    }
}
