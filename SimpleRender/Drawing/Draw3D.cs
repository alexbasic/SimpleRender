﻿using SimpleRender.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleRender.SceneObjects;

namespace SimpleRender.Drawing
{
    public class Draw3D
    {
        public static void SimpleRasterizeTriangle(Vector3f t0, Vector3f t1, Vector3f t2, Bitmap image, Color color, double[] zbuffer)
        {
            // пропускаем рисование если треугольник ребром
            if ((int)t0.Y == (int)t1.Y && (int)t0.Y == (int)t2.Y) return;

            var A = t0;
            var B = t1;
            var C = t2;

            // здесь сортируем вершины (A,B,C) по оси Y
            if ((int)A.Y > (int)B.Y) Swap(ref A, ref B);
            if ((int)A.Y > (int)C.Y) Swap(ref A, ref C);
            if ((int)B.Y > (int)C.Y) Swap(ref B, ref C);

            for (int sy = (int)A.Y; sy <= (int)C.Y; sy++)
            {
                int x1 = (int)A.X + (sy - (int)A.Y) * ((int)C.X - (int)A.X) / ((int)C.Y - (int)A.Y);
                double z1 = A.Z + (sy - A.Y) * (C.Z - A.Z) / (C.Y - A.Y);
                int x2;
                double z2;
                if (sy < (int)B.Y)
                {
                    x2 = (int)A.X + (sy - (int)A.Y) * ((int)B.X - (int)A.X) / ((int)B.Y - (int)A.Y);
                    z2 = A.Z + (sy - A.Y) * (B.Z - A.Z) / (B.Y - A.Y);
                }
                else
                {
                    if ((int)C.Y == (int)B.Y)
                    {
                        x2 = (int)B.X;
                        z2 = B.Z;
                    }
                    else
                    {
                        x2 = (int)B.X + (sy - (int)B.Y) * ((int)C.X - (int)B.X) / ((int)C.Y - (int)B.Y);
                        z2 = B.Z + (sy - B.Y) * (C.Z - B.Z) / (C.Y - B.Y);
                    }
                }
                if (x1 > x2)
                {
                    int tmp = x1; x1 = x2; x2 = tmp;
                    double tmpZ = z1; z1 = z2; z2 = tmpZ;
                }

                DrawHorizontalLine(image, sy, x1, x2, z1, z2, color, zbuffer);
            }
        }

        private static void DrawHorizontalLine(Bitmap image, int sy, int x1, int x2, double z1, double z2, Color color, double[] zbuffer)
        {
            var frameWidth = image.Width;
            var maxX = image.Width - 1;
            var minX = 0;
            var maxY = image.Height - 1;
            var minY = 0;

            //cut out of screen lines
            if (sy < minY || sy > maxY) return;
            if (x1 < minX)
            {
                z1 = z1 + ((minX - x1) * (z2 - z1) / (x2 - x1));
                x1 = minX;
            }
            if (x2 > maxX)
            {
                z2 = z1 + ((maxX - x1) * (z2 - z1) / (x2 - x1));
                x2 = maxX;
            }

            var px = x1;
            while (px <= x2)
            {
                double z =/* double.MinValue*/1 + (z1 + ((px - x1) * (z2 - z1) / (x2 - x1)));

                SetPixel(image, px, sy, z, color, zbuffer);

                px++;
            }
        }

        private static void Swap(ref Vector3f a, ref Vector3f b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

        private static void SetPixel(Bitmap image, int x, int y, double z, Color color, double[] zbuffer)
        {
            var maxX = image.Width - 1;
            var minX = 0;
            var maxY = image.Height - 1;
            var minY = 0;

            //skip out of scren pixels
            if (x < minX || x > maxX || y < minY || y > maxY) return;

            if (zbuffer[x + y * image.Width] <= z)
            {
                zbuffer[x + y * image.Width] = z;
                image.SetPixel(x, y, color);
            }
        }

        public static void RasterizeTraversalAabb(Vector3f vertex0, Vector3f vertex1, Vector3f vertex2)
        {
            var edge0 = new Line(vertex1.X, vertex1.Y, vertex0.X, vertex0.Y);
            var edge1 = new Line(vertex2.X, vertex2.Y, vertex1.X, vertex1.Y);
            var edge2 = new Line(vertex0.X, vertex0.Y, vertex2.X, vertex2.Y);

            // Calculate triangle area on screen and inverse it
            double triangle_area_inversed = 1.0f /
                Math3D.Triangle2dArea(vertex0.X, vertex0.Y, vertex1.X, vertex1.Y, vertex2.X, vertex2.Y);

            // Construct triangle's bounding box
            Aabb boundingBox = new Aabb
            {
                Left = (int)Min(vertex0.X, vertex1.X, vertex2.X),
                Top = (int)Min(vertex0.Y, vertex1.Y, vertex2.Y),
                Right = (int)Max(vertex0.X, vertex1.X, vertex2.X),
                Bottom = (int)Max(vertex0.Y, vertex1.Y, vertex2.Y)
            };

            // Iterate over bounding box and check if pixel is inside the triangle
		//
		for (int y = boundingBox.Top; y <= boundingBox.Bottom; ++y)
		{
			double pixel_center_y = (double)(y) + 0.5d;

			float const first_x_center{static_cast<float>(bounding_box.from.x) + 0.5f};

			float edge0_equation_value{edge0.at(first_x_center, pixel_center_y)};
			float edge1_equation_value{edge1.at(first_x_center, pixel_center_y)};
			float edge2_equation_value{edge2.at(first_x_center, pixel_center_y)};

			for (unsigned int x{bounding_box.from.x}; x <= bounding_box.to.x; ++x)
			{
				float const pixel_center_x{static_cast<float>(x) + 0.5f};
            }
        }

        }

        private static double Min(double a, double b, double c)
        {
            var m = System.Math.Min(a, b);
            return System.Math.Min(m, c);
        }

        private static double Max(double a, double b, double c)
        {
            var m = System.Math.Max(a, b);
            return System.Math.Max(m, c);
        }
    }

    public struct Aabb
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
    }
}
