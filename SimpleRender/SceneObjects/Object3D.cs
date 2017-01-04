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

    public class Pyramid : Object3D
    {
        public Pyramid()
        {
            Vertices = new List<Vertex>
                    {
                        new Vertex{Number = 0, X = -0.5f, Y = -0.5f, Z = -0.5f},
                        new Vertex{Number = 1, X = -0.5f, Y = -0.5f, Z = 0.5f},

                        new Vertex{Number = 2, X = 0f, Y = 0.5f, Z = 0f},

                        new Vertex{Number = 3, X = 0.5f, Y = -0.5f, Z = -0.5f},
                        new Vertex{Number = 4, X = 0.5f, Y = -0.5f, Z = 0.5f}
                    }.ToArray();
            Faces = new List<Face>
                    {
                        new Face{Vertex1 = 0, Vertex2 = 1, Vertex3 = 2},
                        new Face{Vertex1 = 3, Vertex2 = 2, Vertex3 = 4},

                        new Face{Vertex1 = 0, Vertex2 = 2, Vertex3 = 3},
                        new Face{Vertex1 = 1, Vertex2 = 4, Vertex3 = 2},

                        new Face{Vertex1 = 0, Vertex2 = 3, Vertex3 = 1},
                        new Face{Vertex1 = 3, Vertex2 = 4, Vertex3 = 1}
                    };
            Position = new Vector4();
            Mategial = new Material() { DiffuseColor = new Vector4(0, 1, 0, 1) };
        }
    }

    public class Cube : Object3D
    {
        public Cube()
        {
            Vertices = new List<Vertex>
                    {
                        new Vertex{Number = 0, X = -0.5f, Y = -0.5f, Z = -0.5f},
                        new Vertex{Number = 1, X = -0.5f, Y = 0.5f, Z = -0.5f},
                        new Vertex{Number = 2, X = -0.5f, Y = 0.5f, Z = 0.5f},
                        new Vertex{Number = 3, X = -0.5f, Y = -0.5f, Z = 0.5f},

                        new Vertex{Number = 4, X = 0.5f, Y = -0.5f, Z = -0.5f},
                        new Vertex{Number = 5, X = 0.5f, Y = 0.5f, Z = -0.5f},
                        new Vertex{Number = 6, X = 0.5f, Y = 0.5f, Z = 0.5f},
                        new Vertex{Number = 7, X = 0.5f, Y = -0.5f, Z = 0.5f},
                    }.ToArray();
            Faces = new List<Face>
                    {
                        new Face{Vertex1 = 0, Vertex2 = 2, Vertex3 = 1},
                        new Face{Vertex1 = 0, Vertex2 = 3, Vertex3 = 2},

                        new Face{Vertex1 = 4, Vertex2 = 5, Vertex3 = 6},
                        new Face{Vertex1 = 4, Vertex2 = 6, Vertex3 = 7},

                        new Face{Vertex1 = 0, Vertex2 = 1, Vertex3 = 5},
                        new Face{Vertex1 = 0, Vertex2 = 5, Vertex3 = 4},

                        new Face{Vertex1 = 3, Vertex2 = 7, Vertex3 = 2},
                        new Face{Vertex1 = 7, Vertex2 = 6, Vertex3 = 2},

                        new Face{Vertex1 = 1, Vertex2 = 6, Vertex3 = 5},
                        new Face{Vertex1 = 1, Vertex2 = 2, Vertex3 = 6},

                        new Face{Vertex1 = 0, Vertex2 = 4, Vertex3 = 3},
                        new Face{Vertex1 = 4, Vertex2 = 7, Vertex3 = 3},
                    };
            Position = new Vector4();
            Mategial = new Material() { DiffuseColor = new Vector4(1, 0, 0, 1) };
        }
    }
}
