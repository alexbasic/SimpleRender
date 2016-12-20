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
    }
}
