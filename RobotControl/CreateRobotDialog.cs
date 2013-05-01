using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace RobotControl
{
    public partial class CreateRobotDialog : Form
    {
        public Map map;
        private Robot newRobot;
        public CreateRobotDialog()
        {
            InitializeComponent();
            var portNames = SerialPort.GetPortNames();
            foreach (var portName in portNames)
            {
                ports.Items.Add(portName);
            }
            ports.Text = ports.Items[0].ToString();
        }

        private void Create_Click(object sender, EventArgs e)
        {
            // Валидация!
            try
            {
                newRobot = new Robot(Convert.ToInt32(x.Text), Convert.ToInt32(y.Text),0,0, Convert.ToDouble(facing.Text), ports.SelectedItem.ToString());
                map.AddRobot(newRobot);
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Введены некорректные данные! Попробуйте еще раз.");
            }   
        }
    }
}
