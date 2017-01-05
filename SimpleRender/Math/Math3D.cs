﻿using System;
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

        //TODO проверить правильность
        public static double Cotan(double x)
        {
            return 1 / Math.Tan(x);
        }

        //TODO Проверить правильность
        public static double ArcCotan(double x)
        {
            return Math.Atan(-x) + Math.PI / 2;
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

            var up = Vector3f.CrossProductLeft(new Vector3f(1.0f, 0.0f, 0.0f), zaxis);

            //camera right
            var xaxis = Vector3f.CrossProductLeft(up, zaxis).Normalize();
            //camera up
            var yaxis = Vector3f.CrossProductLeft(zaxis, xaxis).Normalize();
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

        public static double CalculateAngle(double size, double distance)
        {
            double radtheta = 2.0 * Math.Atan2(size / 2.0, distance);
            double degtheta = RadToDeg(radtheta);
            return degtheta;
        }

        public static double RadToDeg(double radtheta)
        {
            return (180.0d * radtheta) / Math.PI;
        }

        public static double DegToRad(double gradtheta)
        {
            return (Math.PI * gradtheta) / 180.0d;
        }

        /// <summary>
        /// Leftside-coordinate frustum
        /// </summary>
        /// <param name="fovy">fov  y in degrees</param>
        /// <param name="aspect">aspect = w div h</param>
        /// <param name="near">near</param>
        /// <param name="far">far</param>
        /// <returns>frustum matrix</returns>
        public static Matrix GetPerspectiveMatrix(double fovyInDegree, double aspect, double near, double far)
        {
            //2n/h = ctg(fovy/2)
            //aspect = w/h
            //2n/w = ctg(fovy/2)/aspect

            var fovy = Math3D.DegToRad(fovyInDegree);

            if (far < 0 || near < 0) throw new Exception("Far and near must be positive");
            //var matrix = new Matrix(
            //    Math3D.Cotan(fovy / 2) / aspect, 0, 0, 0,
            //    0, Math3D.Cotan(fovy / 2), 0, 0,
            //    0, 0, (far + near) / (far - near), 10,
            //    0, 0, (-2 * far * near) / (far - near), 0
            //    );

            var matrix = new Matrix(
                Math3D.Cotan(fovy / 2) / aspect, 0, 0, 0,
                0, Math3D.Cotan(fovy / 2), 0, 0,
                0, 0, (far + near) / (far - near), (-2 * far * near) / (far - near),
                0, 0, 1, 0
                );
            return matrix;
        }

        public static Matrix GetPerspectiveMatrix2(double projectionHeight, double aspect, double near, double far)
        {
            //TODO проверить это
            //var fovyInDegree = Math3D.ArcCotan((2*near/projectionHeight)*2);
            var fovyInDegree = Math3D.CalculateAngle(projectionHeight, near);

            var matrix = GetPerspectiveMatrix(fovyInDegree, aspect, near, far);
            return matrix;
        }

        public static Matrix GetFrustum(double left, double right, double bottom, double top, double near, double far)
        {
            var a = (right + left) / (right - left);
            var b = (top + bottom) / (top - bottom);
            var c = (far + near) / (far - near);
            var d = (-2 * far * near) / (far - near);

            var matrix = new Matrix(
                2 * near / (right - left), 0, a, 0,
                0, 2 * near / (top - bottom), b, 0,
                0, 0, c, d,
                0, 0, 1, 0
                );
            return matrix;
        }

        public static Vector3f ConvertToDecart(Vector4 vector)
        {
            return new Vector3f(vector.X / (float)vector.W, vector.Y / (float)vector.W, vector.Z / (float)vector.W);
        }

        internal static Vector3f CalculateNormal(Vector3f vector1, Vector3f vector2, Vector3f vector3)
        {
            return
                Vector3f.CrossProductLeft((vector3 - vector1), (vector2 - vector1));
        }
    }
}
