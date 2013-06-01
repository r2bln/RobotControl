using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RobotControl
{
    public class Map
    {
        public int offsetX;
        public int offsetY;
        private int width;
        public int height;
        public MainView parent;
        private List<Robot> robotsList = new List<Robot>();
        private Bitmap robotBmp = new Bitmap(Resources.Robot);
        private int id;

        public Bitmap bgMap;
        
        
        public Map()
        {
            offsetX = 0;
            offsetY = 0;
        }

        public Map(int _offsetX, int _offsetY, int _width, int _height, MainView _parent)
        {
            parent = _parent;
            offsetX = _offsetX;
            offsetY = _offsetY;
            width = _width;
            height = _height;
            id = 0;
            bgMap = new Bitmap(width, height);
            for (int i = 0 ; i < width; i++)
                for (int j = 0; j < height; j++)
                    bgMap.SetPixel(i,j,Color.White);
        }

        public void Clear()
        {
            if (bgMap != null)
            {
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                        bgMap.SetPixel(i, j, Color.White);
            }
            parent.Invalidate();
        }

        public void AddRobot(Robot rb)
        {
            // Добавляем робота в список на карте
            robotsList.Add(rb);

            // Присваиваем ему id
            rb.id = id;
            id++;

            // Отписываемся в статус бар
            parent.StatusLabel1.Text = "Робот " + rb.id + " добавлен...";

            // Подписываемся на ивент дисконнекта
            rb.OnConnectionLost += rb_OnConnectionLost;

            // Подписываемся на ивент пришедших данных
            rb.OnDataRecievedExternal += rb_OnDataRecievedExternal;
        }

        void rb_OnDataRecievedExternal(Robot sender, EventArgs e)
        {
            parent.Invalidate();
        }

        void rb_OnConnectionLost(Robot sender, EventArgs e)
        {
            // Обрабатываем дисконнект робота

            parent.StatusLabel1.Text = "Соединение с роботом " + sender.id + " разорвано...";
            parent.Invalidate();
        }

        // Грязный хак. Оооочень.

        public void CheckSerialConnectons()
        {
            foreach (var robot in robotsList)
            {
                if (robot.connection[0] == 'C' || robot.connection[0] == 'c')
                {
                    try
                    {
                        robot.SendData(" ");
                    }
                    catch
                    {
                        robot.connectionFailed = true;
                    }
                }
            }
        }

        public void RemoveRobot()
        {
            foreach (var robot in robotsList)
            {
                if (robot.selected)
                {
                    robot.Disconnect();
                    robotsList.Remove(robot);
                    parent.listBox1.Items.Clear();
                    parent.StatusLabel1.Text = "Робот " + robot.id + " удален...";
                    break;
                }
            }
            parent.Invalidate();
        }

        public void Reconnect()
        {
            foreach (var robot in robotsList)
            {
                if (robot.selected)
                {
                    try
                    {
                        robot.ConnectIp();
                        robot.connectionFailed = false;
                        parent.Invalidate();
                        parent.Reconnect.Enabled = false;
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось переподключиться.");
                    }
                }
                    
            }
        }

        public void Click(MouseEventArgs e)
        {
            // Обрабатываем клик на поле.
            // Левая кнопка мыши - выделение
            if (e.Button == MouseButtons.Left)
            {
                foreach (var robot in robotsList)
                {
                    // Смотрим координаты мыши и всех роботов.
                    // Не попали ли в кого?
                    var actualX = robot.x + offsetX - robotBmp.Width / 2;
                    var actualY = height + offsetY - (robot.y + robotBmp.Height / 2);

                    if ((e.X > actualX && e.Y > actualY) &&
                        (e.X < actualX + robotBmp.Width && e.Y < actualY + robotBmp.Height))
                    {
                        // Попали, он выделен.
                        robot.selected = true;
                        parent.StatusLabel1.Text = "Робот " + robot.id + " выделен...";
                    }
                    else
                    {
                        // Не попали, он не выделен.
                        robot.selected = false; 
                        parent.listBox1.Items.Clear();
                    }
                }
            }

            // Правая кнопка мыши - движение.
            else
            {
                foreach (var robot in robotsList)
                {                    
                    if (robot.selected)
                    {
                        // Робот выделен - приказываем ему.
                        robot.gtX = e.X;
                        robot.gtY = e.Y;

                        // Отправляем посылку.
                        robot.SendData(e.X + "," + e.Y);
                    }
                }
            }

            parent.Invalidate();
        }


        // Основной Loop рисования вызывается по получении данных из сети, от устройств и ли пользователя.
        public void Update(Graphics canvas)
        {
            // Подкладываем полученную до этого карту под поле
            canvas.DrawImage(bgMap, offsetX, offsetY, width, height);

            foreach (var robot in robotsList)
            {
                robot.Draw(canvas, this);
            }
        }

        // Сниппет - поворачивает картинки

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
