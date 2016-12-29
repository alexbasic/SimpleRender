using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleRender.Math;

namespace SimpleRender.SceneObjects
{
    public class Material
    {
        public Material()
        {
            Color = new Vector4();
        }

        public Vector4 Color { get; set; }
    }
}
