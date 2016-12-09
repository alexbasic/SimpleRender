using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.SceneObjects
{
    public class Face
    {
        public int Number { get; set; }
        public int Vertex1 { get; set; }
        public int Vertex2 { get; set; }
        public int Vertex3 { get; set; }

        public int TextureVertex1 { get; set; }
        public int TextureVertex2 { get; set; }
        public int TextureVertex3 { get; set; }

        public int NormalVertex1 { get; set; }
        public int NormalVertex2 { get; set; }
        public int NormalVertex3 { get; set; }
    }
}
