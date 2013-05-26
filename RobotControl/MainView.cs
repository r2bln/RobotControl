using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace RobotControl
{
    public partial class MainView : Form
    {
        private Map map;

        public MainView()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            map = new Map(32, 32, 500, 500, this);
        }

        private void MainView_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            map.Update(canvas);
        }

        private void newRobotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateRobotDialog createDialog = new CreateRobotDialog("serial");
            createDialog.map = map;
            createDialog.ShowDialog();
            Invalidate();
        }

        private void addRobotIPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateRobotDialog createDialog = new CreateRobotDialog("ip");
            createDialog.map = map;
            createDialog.ShowDialog();
            Invalidate();
        }

        private void MainView_MouseClick(object sender, MouseEventArgs e)
        {
            map.Click(e);
            //Invalidate();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            map.RemoveRobot();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            map.CheckSerialConnectons();
        }

        private void saveMapToolStripMenuItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog foo = new SaveFileDialog();
                foo.ShowDialog();
                map.bgMap.Save(foo.FileName, ImageFormat.Png);
                StatusLabel1.Text = "Карта сохранена";
            }
            catch
            {

            }
        }

        private void сlearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map.Clear();
            StatusLabel1.Text = "Карта очищена";
        }

        private void loadMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog foo = new OpenFileDialog();
                foo.ShowDialog();
                map.bgMap = new Bitmap(foo.FileName);
                StatusLabel1.Text = "Карта загружена";
            }
            catch {}
        }

        private void Reconnect_Click(object sender, EventArgs e)
        {
            map.Reconnect();
        }
    }
}
