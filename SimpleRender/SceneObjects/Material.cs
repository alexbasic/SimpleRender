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
            DiffuseColor = new Vector4();
        }

        public Vector4 DiffuseColor { get; set; }
        public Vector4 AmbientColor { get; set; }
        public Vector4 SpecularColor { get; set; }
    }
}
