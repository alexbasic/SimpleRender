using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender
{
    public class Draw3D
    {
        public static void Triangle(Vector3i t0, Vector3i t1, Vector3i t2, Bitmap image, Color color, int[] zbuffer)
        {
            if (t0.Y == t1.Y && t0.Y == t2.Y) return; // i dont care about degenerate triangles
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
                Vector3i A = (Vector3i)((Vector3f)t0 + (Vector3f)(t2 - t0) * alpha);
                Vector3i B = (Vector3i)(second_half ? (Vector3f)t1 + (Vector3f)(t2 - t1) * beta : (Vector3f)t0 + (Vector3f)(t1 - t0) * beta);
                if (A.X > B.X) Swap(ref A, ref B);
                for (int j = A.X; j <= B.X; j++)
                {
                    float phi = (float)(B.X == A.X ? 1.0 : (float)(j - A.X) / (float)(B.X - A.X));
                    Vector3i P = (Vector3i)((Vector3f)(A) + (Vector3f)(B - A) * phi);
                    int idx = P.X + P.Y * image.Width;
                    if (zbuffer[idx] < P.Z)
                    {
                        zbuffer[idx] = P.Z;
                        image.SetPixel(P.X, P.Y, color);
                    }
                }
            }
        }

        private static void Swap(ref Vector3f a, ref Vector3f b)
        {
            Vector3f tmp = a;
            a = b;
            b = tmp;
        }

        private static void Swap(ref Vector3i a, ref Vector3i b)
        {
            Vector3i tmp = a;
            a = b;
            b = tmp;
        }

    }
}
