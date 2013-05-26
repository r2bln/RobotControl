using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RobotControl
{
    public class Map
    {
        private int offsetX;
        private int offsetY;
        private int width;
        private int height;
        private MainView parent;
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
                        robot.SendData("Go to " + e.X + " " + e.Y);
                    }
                }
            }

            parent.Invalidate();
        }


        // Основной Loop рисования вызывается постоянно каждую сек
        public void Update(Graphics canvas)
        {
            // Подкладываем полученную до этого карту под поле
            canvas.DrawImage(bgMap, offsetX, offsetY, width, height);
            
            foreach (var robot in robotsList)
            {
                // Координаты верхнего левого угла картинки робота
                var actualX = robot.x + offsetX - robotBmp.Width/2;
                var actualY = height + offsetY - (robot.y + robotBmp.Height/2);

                // Координаты центра робота
                var centerX = robot.x + offsetX;
                var centerY = height + offsetY - robot.y;

                // Координаты метки
                var markX = robot.obstRange * Math.Sin((robot.facing) * Math.PI / 180 ) + centerX;
                var markY = -robot.obstRange * Math.Cos((robot.facing) * Math.PI / 180 ) + centerY;

                // Поворачиваем робота
                var tmpBmp = RotateImageByAngle(robotBmp, (float)robot.facing); // Пашет

                // Рисуем робота и его центр
                canvas.DrawImage(tmpBmp, actualX, actualY);
                canvas.FillRectangle(new SolidBrush(Color.Red), centerX, centerY, 2, 2);

                // Если на робота кликнули - рисуем рамку

                if (robot.selected)
                {
                    canvas.DrawRectangle(new Pen(Color.Red), actualX, actualY, robotBmp.Width, robotBmp.Height);
                    parent.listBox1.Items.Clear();
                    parent.listBox1.Items.Add("Номер: " + robot.id);
                    parent.listBox1.Items.Add("X: " + robot.x.ToString());
                    parent.listBox1.Items.Add("Y: " + robot.y.ToString());
                    parent.listBox1.Items.Add("Дальномер: " + robot.obstRange.ToString());
                    parent.listBox1.Items.Add("Азимут: " + robot.facing.ToString());
                    parent.listBox1.Items.Add("Соединение: " + robot.connection);
                    if (!robot.connectionFailed)
                        parent.listBox1.Items.Add("Активно...");
                    else
                        parent.listBox1.Items.Add("РАЗОРВАНО!");
                }

                if (robot.connectionFailed)
                {
                    // Рисуем рамку вокруг робота и крест (Х_х)
                    canvas.DrawRectangle(new Pen(Color.Red), actualX, actualY, robotBmp.Width, robotBmp.Height);
                    canvas.DrawLine(new Pen(Color.Red), actualX, actualY, actualX + robotBmp.Width, actualY + robotBmp.Height);
                    canvas.DrawLine(new Pen(Color.Red), actualX + robotBmp.Width, actualY, actualX, actualY + robotBmp.Height);
                    parent.Reconnect.Enabled = true;
                }

                // Куда робот движется
                canvas.DrawLine(new Pen(Color.Red), centerX, centerY, robot.gtX, robot.gtY);

                // Выводим данные (так, для теста)              
                //canvas.DrawString(robot.data, new Font(FontFamily.GenericSansSerif, 10), new SolidBrush(Color.Black), actualX + 32, actualY);
                
                // В разумных ли приделах показания дальномера?
                if (robot.obstRange > 5 && robot.obstRange < 150)
                {
                    // Рисуем метку и линию до нее
                    canvas.FillRectangle(new SolidBrush(Color.Black), (float)markX, (float)markY, 2,2);
                    canvas.DrawLine(new Pen(Color.Red), centerX, centerY, (float)markX, (float)markY);
                
                    // Сохраняем метку на карте
                
                    try
                    {
                        bgMap.SetPixel((int)markX - offsetX, (int)markY - offsetY, Color.Black);
                    }
                    catch 
                    {
                        // Зашкалило.
                    }
                }
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
