using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender
{
    public class Object3D
    {
        public Vertex Position { get; set; }
        public List<Vertex> Vertices { get; set; }
        public List<Vertex> TextureVertices { get; set; }
        public List<Vertex> Normals { get; set; }
        public List<Face> Faces { get; set; }
        public Material MategialObject { get; set; }
    }
}
