using SimpleRender.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.SceneObjects
{
    public class Object3D : Primitive
    {
        public Object3D()
        {
            Rotation = new Vector4(0, 0, 0, 1);
            Position = new Vector4(0, 0, 0, 1);
            Scale = new Vector4(1, 1, 1, 1);
            Mategial = new Material();
            Faces = new List<Face>();
            Normals = new List<Vertex>();
            TextureVertices = new List<Vertex>();
            Vertices = new Vertex[0];
        }

        public Vertex[] Vertices { get; set; }
        public ICollection<Vertex> TextureVertices { get; set; }
        public ICollection<Vertex> Normals { get; set; }
        public ICollection<Face> Faces { get; set; }

        public Material Mategial { get; set; }
    }
}
