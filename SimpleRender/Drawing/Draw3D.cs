using SimpleRender.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.Drawing
{
    public class Draw3D
    {
        public static void Triangle(Vector3f t0, Vector3f t1, Vector3f t2, Bitmap image, Color color, int[] zbuffer)
        {
        }

        public static void Triangle(Vector3f t0, Vector3f t1, Vector3f t2, Bitmap image, Color color, int[,] zbuffer)
        {
            // пропускаем рисование если треугольник ребром
            if (t0.Y == t1.Y && t0.Y == t2.Y) return;

            var A = t0;
            var B = t1;
            var C = t2;

            // здесь сортируем вершины (A,B,C) по оси Y
            if (A.Y > B.Y) Swap(ref A, ref B);
            if (A.Y > C.Y) Swap(ref A, ref C);
            if (B.Y > C.Y) Swap(ref B, ref C);

            for (var sy = A.Y; sy <= C.Y; sy++)
            {
                var x1 = A.X + (sy - A.Y) * (C.X - A.X) / (C.Y - A.Y);
                int x2;
                if (sy < B.Y)
                    x2 = (int)(A.X + (sy - A.Y) * (B.X - A.X) / (B.Y - A.Y));
                else
                {
                    if (C.Y == B.Y)
                        x2 = (int)B.X;
                    else
                        x2 = (int)(B.X + (sy - B.Y) * (C.X - B.X) / (C.Y - B.Y));
                }
                if (x1 > x2) { var tmp = x1; x1 = x2; x2 = (int)tmp; }
                DrawHorizontalLine(image, sy, x1, x2, z1, z2, color);
            }
        }

        private static void DrawHorizontalLine(Bitmap image, int sy, int x1, int x2, int z1, int z2, Color color)
        {
            var maxX = image.Width - 1;
            var minX = 0;
            var maxY = image.Height - 1;
            var minY = 0;

            //cut out of screen lines
            if (sy < minY || sy > maxY) return;
            if (x1 < minX) x1 = minX;
            if (x2 > maxX) x2 = maxX;

            var px = x1;
            while (px <= x2)
            {
                SetPixel(image, px, sy, color);
                px++;
            }
        }

        private static void Swap(ref Vector3f a, ref Vector3f b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

        private static void SetPixel(Bitmap image, int x, int y, Color color)
        {
            var maxX = image.Width - 1;
            var minX = 0;
            var maxY = image.Height - 1;
            var minY = 0;

            //skip out of scren pixels
            if (x < minX || x > maxX || y < minY || y > maxY) return;

            image.SetPixel(x, y, color);
        }
    }
}
