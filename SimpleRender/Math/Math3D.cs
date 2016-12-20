using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.Math
{
    using Math = System.Math;

    public static class Math3D
    {
        public static Matrix GetRotationMatrix(double angle_x, double angle_y, double angle_z) 
        {
            var A = Math.Cos(angle_x);
            var B = Math.Sin(angle_x);
            var C = Math.Cos(angle_y);
            var D = Math.Sin(angle_y);
            var E = Math.Cos(angle_z);
            var F = Math.Sin(angle_z);

            var AD = A * D;
            var BD = B * D;

            var mat = new Matrix();

            mat[0] = C * E;
            mat[1] = -C * F;
            mat[2] = -D;
            mat[4] = -BD * E + A * F;
            mat[5] = BD * F + A * E;
            mat[6] = -B * C;
            mat[8] = AD * E + B * F;
            mat[9] = -AD * F + B * E;
            mat[10] = A * C;

            mat[3] = mat[7] = mat[11] = mat[12] = mat[13] = mat[14] = 0;
            mat[15] = 1;

            return mat;
        }

        public static double Cotan(double x)
        {
            return 1 / Math.Tan(x);
        }
    }
}
