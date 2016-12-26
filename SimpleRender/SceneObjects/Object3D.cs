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
            Rotation = new Vector4();
        }


        
        public Vertex[] Vertices { get; set; }
        public ICollection<Vertex> TextureVertices { get; set; }
        public ICollection<Vertex> Normals { get; set; }
        public ICollection<Face> Faces { get; set; }
        public Material MategialObject { get; set; }
    }
}
