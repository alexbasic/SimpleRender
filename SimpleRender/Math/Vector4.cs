using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.Math
{
    public class Vector4 : Vector3f
    {
        public Vector4()
        {
        }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public double W { get; set; }

        public static Vector4 operator *(Vector4 a, double b)
        {
            return new Vector4 { X = (float)(a.X * b), Y = (float)(a.Y * b), Z = (float)(a.Z * b), W = a.W * b };
        }
    }
}
