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
        public static void Line(int x0, int y0, int x1, int y1, Bitmap image, Color color)
        {
            bool steep = false;
            if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
                steep = true;
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }
            int dx = x1 - x0;
            int dy = y1 - y0;
            int derror2 = Math.Abs(dy) * 2;
            int error2 = 0;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                if (steep)
                {
                    if (y < image.Width - 1 && y > -1 && x < image.Height - 1 && x > -1)
                        image.SetPixel(y, x, color);
                }
                else
                {
                    if (y < image.Height - 1 && y > -1 && x < image.Width - 1 && x > -1)
                        image.SetPixel(x, y, color);
                }
                error2 += derror2;

                if (error2 > dx)
                {
                    y += (y1 > y0 ? 1 : -1);
                    error2 -= dx * 2;
                }
            }
        }

        public static void Line(Point2D t0, Point2D t1, Bitmap image, Color color)
        {
            Line(t0.X, t0.Y, t1.X, t1.Y, image, color);
        }

        public static void LineUnsafe(int x0, int y0, int x1, int y1, UnsafeBitmap image, Color color)
        {
            bool steep = false;
            if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
                steep = true;
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }
            int dx = x1 - x0;
            int dy = y1 - y0;
            int derror2 = Math.Abs(dy) * 2;
            int error2 = 0;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                if (steep)
                {
                    if (y < image.Bitmap.Width - 1 && y > -1 && x < image.Bitmap.Height - 1 && x > -1)
                        image.SetPixel(y, x, new PixelData() { red = color.R, green = color.G, blue = color.B });
                }
                else
                {
                    if (y < image.Bitmap.Height - 1 && y > -1 && x < image.Bitmap.Width - 1 && x > -1)
                        image.SetPixel(x, y, new PixelData() { red = color.R, green = color.G, blue = color.B });
                }
                error2 += derror2;

                if (error2 > dx)
                {
                    y += (y1 > y0 ? 1 : -1);
                    error2 -= dx * 2;
                }
            }
        }

        private static void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }

        private static void Swap(ref Point2D a, ref Point2D b)
        {
            Point2D tmp = a;
            a = b;
            b = tmp;
        }

        public static void Triangle(Point2D t0, Point2D t1, Point2D t2, Bitmap image, Color color)
        {
            if (t0.Y == t1.Y && t0.Y == t2.Y) return; // i dont care about degenerate triangles
            // sort the vertices, t0, t1, t2 lower-to-upper (bubblesort yay!)
            if (t0.Y > t1.Y) Swap(ref t0, ref t1);
            if (t0.Y > t2.Y) Swap(ref t0, ref t2);
            if (t1.Y > t2.Y) Swap(ref t1, ref t2);

            int total_height = t2.Y - t0.Y;
            for (int i = 0; i < total_height; i++)
            {
                bool second_half = i > t1.Y - t0.Y || t1.Y == t0.Y;
                int segment_height = second_half ? t2.Y - t1.Y : t1.Y - t0.Y;
                float alpha = (float)i / total_height;
                float beta = (float)(i - (second_half ? t1.Y - t0.Y : 0)) / segment_height; // be careful: with above conditions no division by zero here
                Point2D A = t0 + (t2 - t0) * alpha;
                Point2D B = second_half ? t1 + (t2 - t1) * beta : t0 + (t1 - t0) * beta;
                if (A.X > B.X) Swap(ref A, ref B);
                for (int j = A.X; j <= B.X; j++)
                {
                    int yy = t0.Y + i;
                    if (j > -1 && yy > -1 && j < image.Width && yy < image.Height)
                    {
                        image.SetPixel(j, yy, color); // attention, due to int casts t0.y+i != A.y
                    }
                }
            }
        }
    }

    public class Draw3D
    {
        public static void Triangle(Vec3i t0, Vec3i t1, Vec3i t2, Bitmap image, Color color, int[] zbuffer) {
    if (t0.Y==t1.Y && t0.Y==t2.Y) return; // i dont care about degenerate triangles
    if (t0.Y>t1.Y) Swap(ref t0, ref t1);
    if (t0.Y>t2.Y) Swap(ref t0, ref t2);
    if (t1.Y>t2.Y) Swap(ref t1, ref t2);
    int total_height = t2.Y-t0.Y;
    for (int i=0; i<total_height; i++) {
        bool second_half = i>t1.Y-t0.Y || t1.Y==t0.Y;
        int segment_height = second_half ? t2.Y-t1.Y : t1.Y-t0.Y;
        float alpha = (float)i/total_height;
        float beta  = (float)(i-(second_half ? t1.Y-t0.Y : 0))/segment_height; // be careful: with above conditions no division by zero here
        Vec3i A = (Vec3i)((Vec3f)t0 + (Vec3f)(t2 - t0) * alpha);
        Vec3i B = (Vec3i)(second_half ? (Vec3f)t1 + (Vec3f)(t2-t1)*beta : (Vec3f)t0 + (Vec3f)(t1-t0)*beta);
        if (A.X>B.X) Swap(ref A, ref B);
        for (int j=A.X; j<=B.X; j++) {
            float phi = (float)(B.X==A.X ? 1.0 : (float)(j-A.X)/(float)(B.X-A.X));
            Vec3i P = (Vec3i)((Vec3f)(A) + (Vec3f)(B-A)*phi);
            int idx = P.X+P.Y*image.Width;
            if (zbuffer[idx]<P.Z) {
                zbuffer[idx] = P.Z;
                image.SetPixel(P.X, P.Y, color);
            }
        }
    }
}

        private static void Swap(ref Vec3f a, ref Vec3f b)
        {
            Vec3f tmp = a;
            a = b;
            b = tmp;
        }

        private static void Swap(ref Vec3i a, ref Vec3i b)
        {
            Vec3i tmp = a;
            a = b;
            b = tmp;
        }

    }
public class Vec3i
{
    public int X {get;set;}
    public int Y {get;set;}
    public int Z {get;set;}

    public static explicit operator Vec3f(Vec3i v)
    {
        return new Vec3f { X = v.X, Y = v.Y, Z = v.Z };
    }

    //public static implicit operator Vec3f(Vec3i v)
    //{
    //    return new Vec3f { X = v.X, Y = v.Y, Z = v.Z };
    //}

    public static Vec3i operator +(Vec3i a, Vec3i b)
    {
        return new Vec3i { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
    }

    public static Vec3i operator -(Vec3i a, Vec3i b)
    {
        return new Vec3i { X = a.X - b.X, Y = a.Y - b.Y, Z = a.Z - b.Z };
    }

    public static Vec3i operator *(Vec3i a, int b)
    {
        return new Vec3i { X = a.X * b, Y = a.Y * b, Z = a.Z * b };
    }
}

public class Vec3f
{
    public float X {get;set;}
    public float Y {get;set;}
    public float Z {get;set;}

    //implicit or explicit 
    public static explicit operator Vec3i(Vec3f v)
    {
        return new Vec3i { X = (int)v.X, Y = (int)v.Y, Z = (int)v.Z};
    }

    //public static implicit operator Vec3i(Vec3f v)
    //{
    //    return new Vec3i { X = (int)v.X, Y = (int)v.Y, Z = (int)v.Z };
    //}

    public static Vec3f operator +(Vec3f a, Vec3f b)
    {
        return new Vec3f { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
    }

    public static Vec3f operator -(Vec3f a, Vec3f b)
    {
        return new Vec3f { X = a.X - b.X, Y = a.Y - b.Y, Z = a.Z - b.Z };
    }

    public static Vec3f operator *(Vec3f a, float b)
    {
        return new Vec3f { X = a.X * b, Y = a.Y * b, Z = a.Z * b };
    }
}

public class Vec2i
{
    public int X {get;set;}
    public int Y {get;set;}
}

public class Vec2f
{
    public float X {get;set;}
    public float Y {get;set;}
}
}
