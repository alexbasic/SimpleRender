using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleRender.Math;

namespace SimpleRender.SceneObjects
{
    public class LightSource : Primitive
    {
        public double Intensity { get; set; }

        public Vector4 Color { get; set; }
    }

    public class GlobalLightSource : LightSource
    {
    }
}
