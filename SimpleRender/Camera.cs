using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender
{
    public class Camera
    {
        public bool Wired { get; set; }

        private int[] zbuffer;

        public void Render(Scene SceneObject, Bitmap OutputBuffer)
        {
            if (Wired)
            {
                WireRender(SceneObject, OutputBuffer);
            }
            else
            {
                SolidRender(SceneObject, OutputBuffer);
            }
        }

        private void SolidRender(Scene SceneObject, Bitmap OutputBuffer)
        {
            Point2D[] t0 = new Point2D[] { new Point2D(10, 70), new Point2D(50, 160), new Point2D(70, 80) };
            Point2D[] t1 = new Point2D[] { new Point2D(180, 50), new Point2D(150, 1), new Point2D(70, 180) };
            Point2D[] t2 = new Point2D[] { new Point2D(180, 150), new Point2D(120, 160), new Point2D(130, 180) };

            Draw2D.Triangle(t0[0], t0[1], t0[2], OutputBuffer, Color.Red);
            Draw2D.Triangle(t1[0], t1[1], t1[2], OutputBuffer, Color.White);
            Draw2D.Triangle(t2[0], t2[1], t2[2], OutputBuffer, Color.Green);
        }

        private void WireRender(Scene SceneObject, Bitmap OutputBuffer)
        {
            zbuffer = new int[OutputBuffer.Width * OutputBuffer.Height];
            //!!Stub Begin!!
            foreach (var mdl in SceneObject.Objects)
            {
                var width = OutputBuffer.Width;
                var height = OutputBuffer.Height;
                for (int i = 1; i < mdl.Faces.Count; i++)
                {
                    Face face = mdl.Faces.Single(x => x.Number == i);
                    Vertex[] verticies = new Vertex[]
                    {
                        mdl.Vertices.Single(x => x.Number == face.Vertex1),
                        mdl.Vertices.Single(x => x.Number == face.Vertex2),
                        mdl.Vertices.Single(x => x.Number == face.Vertex3)
                    };
                    for (int j = 0; j < 3; j++)
                    {
                        Vertex v0 = verticies[j];
                        Vertex v1 = verticies[(j + 1) % 3];
                        int x0 = (int)((v0.X + 1.0) * width / 2.0);
                        int y0 = (int)((v0.Y + 1.0) * height / 2.0);
                        int x1 = (int)((v1.X + 1.0) * width / 2.0);
                        int y1 = (int)((v1.Y + 1.0) * height / 2.0);
                        Draw2D.Line(x0, y0, x1, y1, OutputBuffer, Color.White);
                    }
                }
            }
            //!!Stub End!!
        }
    }
}
