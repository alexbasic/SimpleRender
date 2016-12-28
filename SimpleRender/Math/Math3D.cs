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

        public static Matrix GetScaleMatrix(double x, double y, double z)
        {
            return new Matrix(
                x, 0, 0, 0,
                0, y, 0, 0,
                0, 0, z, 0,
                0, 0, 0, 1);
        }

        public static Matrix GetTranslationMatrix(double x, double y, double z)
        {
            return new Matrix(
                1, 0, 0, x,
                0, 1, 0, y,
                0, 0, 1, z,
                0, 0, 0, 1);
        }

        public static double Cotan(double x)
        {
            return 1 / Math.Tan(x);
        }

        public static double DotProduct(Vector4 a, Vector4 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        }

        public static double DotProduct(Vector3f a, Vector3f b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Matrix GetViewMatrix(Vector3f position, Vector3f target)
        {
            //direction
            var zaxis = (position - target).Normalize();

            var up = Vector3f.CrossProduct(zaxis, new Vector3f(1.0f, 0.0f, 0.0f));

            //camera right
            var xaxis = Vector3f.CrossProduct(up, zaxis).Normalize();
            //camera up
            var yaxis = Vector3f.CrossProduct(zaxis, xaxis).Normalize();
            //var Minv = Matrix.Identity();
            //var Tr = Matrix.Identity();
            //for (int i = 0; i < 3; i++)
            //{
            //    Minv[0][i] = x[i];
            //    Minv[1][i] = y[i];
            //    Minv[2][i] = z[i];
            //    Tr[i][3] = -center[i];
            //}
            //var modelView = Minv * Tr;

            //return new Matrix(
            //    xaxis.X, yaxis.X, zaxis.X, 0,
            //    xaxis.Y, yaxis.Y, zaxis.Y, 0,
            //    xaxis.Z, yaxis.Z, zaxis.Z, 0,
            //    -Math3D.DotProduct(xaxis, eye), -Math3D.DotProduct(yaxis, eye), -Math3D.DotProduct(zaxis, eye), 1
            //    );

            var camRotationMatrix = new Matrix(
                    xaxis.X, xaxis.Y, xaxis.Z, 0,
                    yaxis.X, yaxis.Y, yaxis.Z, 0,
                    zaxis.X, zaxis.Y, zaxis.Z, 0,
                    0, 0, 0, 1
                );

            var camTranslationMatrix = new Matrix(
                    1, 0, 0, -position.X,
                    0, 1, 0, -position.Y,
                    0, 0, 1, -position.Z,
                    0, 0, 0, 1
                );

            return camRotationMatrix * camTranslationMatrix;
        }
    }
}
