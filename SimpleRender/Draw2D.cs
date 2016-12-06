using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender
{
    public class Draw2D
    {
        public static void Line(Point2D t0, Point2D t1, Bitmap image, Color color)
        {
            Line(t0.X, t0.Y, t1.X, t1.Y, image, color);
        }

        public static void Triangle(Point2D t0, Point2D t1, Point2D t2, Bitmap image, Color color)
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
                    x2 = A.X + (sy - A.Y) * (B.X - A.X) / (B.Y - A.Y);
                else
                {
                    if (C.Y == B.Y)
                        x2 = B.X;
                    else
                        x2 = B.X + (sy - B.Y) * (C.X - B.X) / (C.Y - B.Y);
                }
                if (x1 > x2) { var tmp = x1; x1 = x2; x2 = tmp; }
                DrawHorizontalLine(image, sy, x1, x2, color);
            }
        }

        /// <summary>
        /// Алгоритм Брезенхема
        /// </summary>
        public static void Line(int x0, int y0, int x1, int y1, Bitmap image, Color color)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            var incx = Sign(dx);
            var incy = Sign(dy);
            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            int pdx;
            int pdy;
            int es;
            int el;

            if (dx > dy)
            {
                pdx = incx; pdy = 0;
                es = dy; el = dx;
            }
            else
            {
                pdx = 0; pdy = incy;
                es = dx; el = dy;
            }
            var x = x0;
            var y = y0;
            var err = el / 2;
            image.SetPixel(x, y, color);

            for (int t = 0; t < el; t++)
            {
                err -= es;
                if (err < 0)
                {
                    err += el;
                    x += incx;
                    y += incy;
                }
                else
                {
                    x += pdx;
                    y += pdy;
                }

                image.SetPixel(x, y, color);
            }
        }

        ///<summary>
        /// возвращает 0 если аргумент x равен нулю -1 если x меньше 0 и 1 если x больше 0
        ///</summary>
        private static int Sign(int x)
        {
            return (x > 0) ? 1 : (x < 0) ? -1 : 0;
        }

        private static void Swap(ref Point2D a, ref Point2D b)
        {
            Point2D tmp = a;
            a = b;
            b = tmp;
        }

        private static void DrawHorizontalLine(Bitmap image, int sy, int x1, int x2, Color color)
        {
            var px = x1;
            while (px <= x2)
            {
                image.SetPixel(px, sy, color);
                px++;
            }
        }
    }
}
