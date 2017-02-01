using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SimpleRender.Math;

namespace SimpleRender.Drawing.Shaders
{
    public abstract class PixelShader
    {
        public Vector4 OutColor { get; set; }
        public double OutDepth { get; set; }

        public abstract void Compute();
    }

    public abstract class VertexShader
    {
        public Vector4 VertexCoord { get; set; }

        public abstract void Compute();
    }
}
