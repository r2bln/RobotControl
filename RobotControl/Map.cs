using System;
using System.Collections.Generic;
using System.Drawing;

namespace RobotControl
{
    public class Map
    {
        private int offsetX;
        private int offsetY;
        private int width;
        private int height;
        public Bitmap bgMap;

        List<Robot> robotsList = new List<Robot>(); 
        Bitmap robotBmp = new Bitmap(Resources.Robot);
        

        public Map()
        {
            offsetX = 0;
            offsetY = 0;
        }

        public Map(int _offsetX, int _offsetY, int _width, int _height)
        {
            offsetX = _offsetX;
            offsetY = _offsetY;
            width = _width;
            height = _height;
            bgMap = new Bitmap(width, height);
            for (int i = 0 ; i < width; i++)
                for (int j = 0; j < height; j++)
                    bgMap.SetPixel(i,j,Color.White);
        }

        public void AddRobot(Robot rb)
        {
            robotsList.Add(rb);
        }

        public void Update(Graphics canvas)
        {
            canvas.DrawImage(bgMap, offsetX, offsetY, width, height);
            
            //canvas.FillRectangle(new SolidBrush(Color.White), offsetX, offsetY, width, height);
            
            foreach (var robot in robotsList)
            {
                var actualX = robot.x + offsetX - robotBmp.Width/2;
                var actualY = height + offsetY - (robot.y + robotBmp.Height/2);

                canvas.DrawImage(robotBmp, actualX, actualY);
                canvas.DrawString(robot.data, new Font(FontFamily.GenericSansSerif, 10), new SolidBrush(Color.Black), actualX + 32, actualY);
                canvas.DrawLine(new Pen(Color.Black, 2), actualX + robotBmp.Width/2 - 1, actualY - robot.obstRange, actualX + robotBmp.Width/2 + 1, actualY - robot.obstRange );
                
                // при повороте скорее всего работать не будет. Полярные координаты относительно телеги.
                try
                {
                    bgMap.SetPixel(actualX + robotBmp.Width / 2 - offsetX, actualY - robot.obstRange - offsetY, Color.Black);
                }
                catch 
                {
                    // Зашкалило.
                }
            }
        }
    }
}
