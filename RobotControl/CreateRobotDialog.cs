using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace RobotControl
{
    public partial class CreateRobotDialog : Form
    {
        public Map map;
        private Robot newRobot;
        private string type;
        public CreateRobotDialog(string _type)
        {
            InitializeComponent();
            type = _type;
            switch (_type)
            {
                case "serial":
                    connection.Text = "Порт";
                    var portNames = SerialPort.GetPortNames();
                    foreach (var portName in portNames)
                    {
                        ports.Items.Add(portName);
                    }
                    ports.Text = ports.Items[0].ToString();
                    break;

                case "ip":
                    connection.Text = "Адрес:порт";
                    ports.Text = "127.0.0.1:666";
                    ports.Items.Add("127.0.0.1:666");
                    ports.Items.Add("192.168.1.117:666");
                    break;
            }
        }

        private void Create_Click(object sender, EventArgs e)
        {
            // Валидация!
            try
            {
                switch (type)
                {
                    case "serial":
                        newRobot = new Robot(Convert.ToInt32(x.Text), Convert.ToInt32(y.Text),0,0, Convert.ToDouble(facing.Text), ports.SelectedItem.ToString());
                        newRobot.ConnectSerial();
                        map.AddRobot(newRobot);
                        this.Close();
                        break;

                    case "ip":
                        var address = ports.Text.Split(':');
                        newRobot = new Robot(Convert.ToInt32(x.Text), Convert.ToInt32(y.Text), 0, 0, 0, address[0], Convert.ToInt32(address[1]));
                        newRobot.ConnectIp();
                        map.AddRobot(newRobot);
                        this.Close();
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Введены некорректные данные! Попробуйте еще раз.");
            }   
        }
    }
}
