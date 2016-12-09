using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.SceneObjects
{
    public class Point2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point2D()
        {
        }

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point2D operator +(Point2D a, Point2D b)
        {
            return new Point2D( a.X - b.X, a.Y - b.Y );
        }

        public static Point2D operator -(Point2D a, Point2D b)
        {
            return new Point2D(a.X - b.X, a.Y - b.Y);
        }

        public static Point2D operator *(Point2D a, int b)
        {
            return new Point2D(a.X * b, a.Y * b);
        }

        public static Point2D operator /(Point2D a, int b)
        {
            return new Point2D(a.X / b, a.Y / b);
        }

        public static Point2D operator *(Point2D a, Single b)
        {
            return new Point2D((int)(a.X * b), (int)(a.Y * b));
        }

        public static Point2D operator /(Point2D a, Single b)
        {
            return new Point2D((int)(a.X / b), (int)(a.Y / b));
        }
    }
}
