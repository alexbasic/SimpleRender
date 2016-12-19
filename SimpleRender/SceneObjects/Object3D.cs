using SimpleRender.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.SceneObjects
{
    public class Object3D
    {
        public Vertex Position { get; set; }
        public Vector3f Rotation { get; set; }
        public Vertex[] Vertices { get; set; }
        public ICollection<Vertex> TextureVertices { get; set; }
        public ICollection<Vertex> Normals { get; set; }
        public ICollection<Face> Faces { get; set; }
        public Material MategialObject { get; set; }
    }
}
