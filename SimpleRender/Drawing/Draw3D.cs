using SimpleRender.Math;
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
            var triangleMinSize = float.Epsilon;
            // пропускаем рисование если треугольник ребром
            if (FCMP(t1.Y, t0.Y) && FCMP(t2.Y, t1.Y) || FCMP(t1.X, t0.X) && FCMP(t2.X, t1.X)) return;

            var A = new Vector3f(t0.X, t0.Y, t0.Z);
            var B = new Vector3f(t1.X, t1.Y, t1.Z);
            var C = new Vector3f(t2.X, t2.Y, t2.Z);

            // здесь сортируем вершины (A,B,C) по оси Y
            if (A.Y > B.Y) SwapValues(A, B);
            if (A.Y > C.Y) SwapValues(A, C);
            if (B.Y > C.Y) SwapValues(B, C);

            var Dx = A.X + (((B.Y - A.Y) * (C.X - A.X)) / (C.Y - A.Y));

            var dxLeft = (B.X - A.X) / (B.Y - A.Y);
            var dxRight = (C.X - A.X) / (C.Y - A.Y);
            var dxLeftNew = (C.X - B.X) / (C.Y - B.Y);

            var y0 = (int)System.Math.Ceiling(A.Y);
            var y1 = (int)System.Math.Ceiling(B.Y);
            var y2 = (int)System.Math.Ceiling(C.Y);

            var correctFirstTriangle = (System.Math.Ceiling(A.Y) - A.Y);
            var correctSecondTriangle = (System.Math.Ceiling(B.Y) - B.Y);

            var xLeft = A.X + (dxLeft * correctFirstTriangle);
            var xRight = A.X + (dxRight * correctFirstTriangle);

            var bottom = y1 - 1;

            if (bottom >= y0)
            {
                for (var sy = y0; sy <= bottom; sy++)
                {
                    DrawHorizontalLine(image, sy, xLeft, xRight, 0d, 0d, color, zbuffer);

                    xLeft += dxLeft;
                    xRight += dxRight;
                }
            }

            xLeft = B.X + (dxLeftNew * correctSecondTriangle);
            xRight = Dx + (dxRight * correctSecondTriangle);

            bottom = y2 - 1;

            if (bottom >= y1)
            {
                for (var sy = y1; sy <= bottom; sy++)
                {
                    DrawHorizontalLine(image, sy, xLeft, xRight, 0d, 0d, color, zbuffer);

                    xLeft += dxLeftNew;
                    xRight += dxRight;
                }
            }
        }

        private static bool FCMP(float a, float b)
        {
            return System.Math.Abs(b - a) <= float.Epsilon;
        }

        //public static void SimpleRasterizeTriangle_Old(Vector3f t0, Vector3f t1, Vector3f t2, Bitmap image, Color color, double[] zbuffer)
        //{
        //    // пропускаем рисование если треугольник ребром
        //    if (System.Math.Abs(t1.Y - t0.Y) <= float.Epsilon && System.Math.Abs(t2.Y - t0.Y) <= float.Epsilon) return;

        //    var A = t0;
        //    var B = t1;
        //    var C = t2;

        //    // здесь сортируем вершины (A,B,C) по оси Y
        //    if (A.Y > B.Y) Swap(ref A, ref B);
        //    if (A.Y > C.Y) Swap(ref A, ref C);
        //    if (B.Y > C.Y) Swap(ref B, ref C);

        //    var shiftY = System.Math.Ceiling(A.Y)-A.Y;

        //    for (float sy = A.Y; sy <= C.Y; sy++)
        //    {
        //        float x1 = A.X + (sy - A.Y) * (C.X - A.X) / (C.Y - A.Y);
        //        double z1 = A.Z + (sy - A.Y) * (C.Z - A.Z) / (C.Y - A.Y);
        //        float x2;
        //        double z2;
        //        if (sy < B.Y)
        //        {
        //            x2 = A.X + (sy - A.Y) * (B.X - A.X) / (B.Y - A.Y);
        //            z2 = A.Z + (sy - A.Y) * (B.Z - A.Z) / (B.Y - A.Y);
        //        }
        //        else
        //        {
        //            if ((int)C.Y == (int)B.Y)
        //            {
        //                x2 = (int)B.X;
        //                z2 = B.Z;
        //            }
        //            else
        //            {
        //                x2 = B.X + (sy - B.Y) * (C.X - B.X) / (C.Y - B.Y);
        //                z2 = B.Z + (sy - B.Y) * (C.Z - B.Z) / (C.Y - B.Y);
        //            }
        //        }
        //        if (x1 > x2)
        //        {
        //            float tmp = x1; x1 = x2; x2 = tmp;
        //            double tmpZ = z1; z1 = z2; z2 = tmpZ;
        //        }

        //        DrawHorizontalLine(image, (int)(sy+shiftY), (int)System.Math.Ceiling(x1), (int)System.Math.Ceiling(x2)-1, z1, z2, color, zbuffer);
        //    }
        //}

        private static void DrawHorizontalLine(Bitmap image, int sy, double left, double right, double z1, double z2, Color color, double[] zbuffer)
        {
            if (left > right)
            {
                var tmp = left; left = right; right = tmp;
            }
            int x1 = (int)(System.Math.Ceiling(left));
            int x2 = (int)(System.Math.Ceiling(right)-1);
            //if (x1 > x2) x2 = x1;

            var maxX = image.Width - 1;
            var minX = 0;
            var maxY = image.Height - 1;
            var minY = 0;

            //cut out of screen lines
            if (sy < minY || sy > maxY) return;
            if (x1 < minX)
            {
                //z1 = z1 + ((minX - x1) * (z2 - z1) / (x2 - x1));
                x1 = minX;
            }
            if (x2 > maxX)
            {
                //z2 = z1 + ((maxX - x1) * (z2 - z1) / (x2 - x1));
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

        private static void SwapValues(Vector3f a, Vector3f b)
        {
            var tmpX = b.X;
            var tmpY = b.Y;
            var tmpZ = b.Z;
            b.X = a.X;
            b.Y = a.Y;
            b.Z = a.Z;

            a.X = tmpX;
            a.Y = tmpY;
            a.Z = tmpZ;
        }

        //private static void Swap(ref Vector3f a, ref Vector3f b)
        //{
        //    var tmp = a;
        //    a = b;
        //    b = tmp;
        //}

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

        public static void RasterizeTraversalAabb(Vector4 vertex0, Vector4 vertex1, Vector4 vertex2, Bitmap image, Color color, double[] zbuffer, BindedMeshAttributes bindedAttributes, int index0, int index1, int index2)
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
                double pixel_center_y = ((double)y) + 0.5d;

                double first_x_center = ((double)boundingBox.Left) + 0.5d;

                double edge0_equation_value = edge0.At(first_x_center, pixel_center_y);
                double edge1_equation_value = edge1.At(first_x_center, pixel_center_y);
                double edge2_equation_value = edge2.At(first_x_center, pixel_center_y);

                for (int x = boundingBox.Left; x <= boundingBox.Right; ++x)
                {
                    double pixel_center_x = ((double)x) + 0.5d;

                    if (is_point_on_positive_halfspace_top_left(edge0_equation_value, edge0.A, edge0.B) &&
                        is_point_on_positive_halfspace_top_left(edge1_equation_value, edge1.A, edge1.B) &&
                        is_point_on_positive_halfspace_top_left(edge2_equation_value, edge2.A, edge2.B))
                    {

                        double area01 = Math3D.Triangle2dArea(vertex0.X, vertex0.Y, vertex1.X, vertex1.Y, pixel_center_x, pixel_center_y);
                        double area12 = Math3D.Triangle2dArea(vertex1.X, vertex1.Y, vertex2.X, vertex2.Y, pixel_center_x, pixel_center_y);

                        // Calculate barycentric coordinates
                        //
                        double b2 = area01 * triangle_area_inversed;
                        double b0 = area12 * triangle_area_inversed;
                        double b1 = 1.0f - b0 - b2;

                        // Process different attributes
                        //
                        SetBindPointsValuesFromBarycentric<Color4>(
                        bindedAttributes.color_attributes,
                        index0, index1, index2,
                        b0, b1, b2,
                        vertex0.W, vertex1.W, vertex2.W);

                        SetBindPointsValuesFromBarycentric<double>(
                            bindedAttributes.float_attributes,
                            index0, index1, index2,
                            b0, b1, b2,
                            vertex0.W, vertex1.W, vertex2.W);

                        SetBindPointsValuesFromBarycentric<Vector2>(
                            bindedAttributes.vector2f_attributes,
                            index0, index1, index2,
                            b0, b1, b2,
                            vertex0.W, vertex1.W, vertex2.W);

                        SetBindPointsValuesFromBarycentric<Vector3f>(
                            bindedAttributes.vector3f_attributes,
                            index0, index1, index2,
                            b0, b1, b2,
                            vertex0.W, vertex1.W, vertex2.W);

                        var pixel_coordinates = new Point2D(x, y);
                        Vector3f sample_point = new Vector3f((float)pixel_center_x, (float)pixel_center_y, 0.0f);
                        //process_rasterizing_stage_result(pixel_coordinates, sample_point, shader, target_texture);

                        //TODO доделать

                    }

                    edge0_equation_value += edge0.A;
                    edge1_equation_value += edge1.A;
                    edge2_equation_value += edge2.A;
                }
            }

        }

        private static void SetBindPointsValuesFromBarycentric<TAttr>(
        IEnumerable<BindedMeshAttributeInfo<TAttr>> binds,
		int index0, int index1, int index2,
		double b0, double b1, double b2,
		double z0_view_space_reciprocal, double z1_view_space_reciprocal, double z2_view_space_reciprocal)
	{
        //size_t binds_count{binds.size()};
        //for (size_t i{0}; i < binds_count; ++i)
        //{
        //    binded_mesh_attribute_info<TAttr> const& binded_attr = binds[i];
        //    std::vector<TAttr> const& binded_attr_data = binded_attr.info.get_data();
        //    std::vector<unsigned int> const& binded_attr_indices = binded_attr.info.get_indices();

        //    TAttr const& value0 = binded_attr_data[binded_attr_indices[index0]];
        //    TAttr const& value1 = binded_attr_data[binded_attr_indices[index1]];
        //    TAttr const& value2 = binded_attr_data[binded_attr_indices[index2]];

        //    if (binded_attr.info.get_interpolation_option() == attribute_interpolation_option::linear)
        //    {
        //        (*binded_attr.bind_point) = value0 * b0 + value1 * b1 + value2 * b2;
        //    }
        //    else if (binded_attr.info.get_interpolation_option() == attribute_interpolation_option::perspective_correct)
        //    {
        //        TAttr const value0_div_zview = value0 * z0_view_space_reciprocal;
        //        TAttr const value1_div_zview = value1 * z1_view_space_reciprocal;
        //        TAttr const value2_div_zview = value2 * z2_view_space_reciprocal;

        //        float const zview_reciprocal_interpolated = z0_view_space_reciprocal * b0 + z1_view_space_reciprocal * b1 + z2_view_space_reciprocal * b2;
        //        TAttr value_div_zview_interpolated = value0_div_zview * b0 + value1_div_zview * b1 + value2_div_zview * b2;

        //        (*binded_attr.bind_point) = value_div_zview_interpolated * (1.0f / zview_reciprocal_interpolated);
        //    }
        //}
}

        private static bool is_point_on_positive_halfspace_top_left(
        double edge_equation_value, double edge_equation_a, double edge_equation_b)
        {
            var esilon = double.Epsilon;
            // If we are on the edge, use top-left filling rule
            //
            if (System.Math.Abs(edge_equation_value) < esilon)
            {
                // edge_equation_value == 0.0f, thus point is on the edge,
                // and we use top-left rule to decide if point is inside a triangle
                //
                if (System.Math.Abs(edge_equation_a) < esilon)
                {
                    // edge.a == 0.0f, thus it's a horizontal edge and is either a top or a bottom one
                    //

                    // If normal y coordinate is pointing up - we are on the top edge
                    // Otherwise we are on the bottom edge
                    return edge_equation_b > 0.0f;
                }
                else
                {
                    // If normal x coordinate is pointing right - we are on the left edge
                    // Otherwise we are on the right edge
                    return edge_equation_a > 0.0f;
                }
            }
            else
            {
                // Check if we're on a positive halfplane
                return edge_equation_value > 0.0f;
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

    public class BindedMeshAttributeInfo<TAttr>
    {
		/** Attribute data */
		public MeshAttributeInfo<TAttr> info;

		/** Address of variable to put interpolated value into */
		TAttr bind_point;
    }

	/** Container for all the binds
	* @ingroup Rendering
	*/
	public class BindedMeshAttributes
	{
        public IEnumerable<BindedMeshAttributeInfo<Color4>> color_attributes;
        public IEnumerable<BindedMeshAttributeInfo<double>> float_attributes;
        public IEnumerable<BindedMeshAttributeInfo<Vector2>> vector2f_attributes;
        public IEnumerable<BindedMeshAttributeInfo<Vector3f>> vector3f_attributes;
}

    public class MeshAttributeInfo<TAttr>
    {
        
    }

    public class Color4
    {
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
        public double Alpha { get; set; }
    }
}
