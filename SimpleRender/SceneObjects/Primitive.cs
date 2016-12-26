using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleRender.Math;

namespace SimpleRender.SceneObjects
{
    public abstract class Primitive
    {
        public Vector4 Position { get; set; }
        public Vector4 Direction { get; set; }
        public Vector4 Rotation { get; set; }
        public Vector4 Scale { get; set; }
    }
}
