using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.Math
{
    public class Vector3i
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public static explicit operator Vector3f(Vector3i v)
        {
            return new Vector3f { X = v.X, Y = v.Y, Z = v.Z };
        }

        //public static implicit operator Vec3f(Vec3i v)
        //{
        //    return new Vec3f { X = v.X, Y = v.Y, Z = v.Z };
        //}

        public static Vector3i operator +(Vector3i a, Vector3i b)
        {
            return new Vector3i { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
        }

        public static Vector3i operator -(Vector3i a, Vector3i b)
        {
            return new Vector3i { X = a.X - b.X, Y = a.Y - b.Y, Z = a.Z - b.Z };
        }

        public static Vector3i operator *(Vector3i a, int b)
        {
            return new Vector3i { X = a.X * b, Y = a.Y * b, Z = a.Z * b };
        }
    }
}
