using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.Math
{
    public class Vector3f
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3f()
        { }

        public Vector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        //implicit or explicit 
        public static explicit operator Vector3i(Vector3f v)
        {
            return new Vector3i { X = (int)v.X, Y = (int)v.Y, Z = (int)v.Z };
        }

        //public static implicit operator Vec3i(Vec3f v)
        //{
        //    return new Vec3i { X = (int)v.X, Y = (int)v.Y, Z = (int)v.Z };
        //}

        public static Vector3f operator +(Vector3f a, Vector3f b)
        {
            return new Vector3f { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
        }

        public static Vector3f operator -(Vector3f a, Vector3f b)
        {
            return new Vector3f { X = a.X - b.X, Y = a.Y - b.Y, Z = a.Z - b.Z };
        }

        public static Vector3f operator *(Vector3f a, float b)
        {
            return new Vector3f { X = a.X * b, Y = a.Y * b, Z = a.Z * b };
        }

        public static Vector3f CrossProduct(Vector3f a, Vector3f b)
        {
            //Для правой системы координат
            return new Vector3f(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
                );
        }

        public static Vector3f CrossProductLeft(Vector3f a, Vector3f b)
        {
            //Для левой системы координат
            return new Vector3f(
                a.Z * b.Y - a.Y * b.Z,
                a.X * b.Z - a.Z * b.X,
                a.Y * b.X - a.X * b.Y
                );
        }

        public double Length()
        {
            return System.Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public static Vector3f operator *(Matrix m, Vector3f v)
        {
            return new Vector3f
            {
                X = (float)(m[0, 0] * v.X + m[1, 0] * v.Y + m[2, 0] * v.Z + m[3, 0] * 1),
                Y = (float)(m[0, 1] * v.X + m[1, 1] * v.Y + m[2, 1] * v.Z + m[3, 1] * 1),
                Z = (float)(m[0, 2] * v.X + m[1, 2] * v.Y + m[2, 2] * v.Z + m[3, 2] * 1)
            };
        }
    }
}
