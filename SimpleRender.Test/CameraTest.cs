using NUnit.Framework;
using SimpleRender.SceneObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleRender.Test
{
    [TestFixture]
    public class CameraTest
    {
        [Test]
        [STAThread]
        public void TestDrawCamera() 
        {
            var scene = new Scene();
            var cube = 
                new Object3D
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
                    }.ToArray(),
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
                    },
                    Position = new Vertex() { X = 0, Y = 0, Z = 1.25f}
            };
            scene.Objects = new List<Object3D>
            {
                cube
            };

            var form = new TestForm();
            form.ClientSize = new Size(320,240);
            var t = new Timer();

            t.Interval = 250;
            t.Tick += (object sender, EventArgs e) =>
            {
                cube.Rotation.Y += 0.07f;
                form.Refresh();
            };

            form.Paint += (object sender, PaintEventArgs e) =>
            {
                var camera = new Camera(form.ClientSize.Width, form.ClientSize.Height);
                camera.Render(scene);

                e.Graphics.Clear(Color.Black);

                e.Graphics.DrawImage(camera.Image, 0, 0);
            };

            form.Load += (s, e) => { t.Enabled = true; };

            Application.Run(form);
        }
    }
}
