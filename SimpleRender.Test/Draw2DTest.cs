using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;
using SimpleRender.Drawing;
using SimpleRender.Math;
using SimpleRender.SceneObjects;

namespace SimpleRender.Test
{
    [TestFixture]
    public class Draw2DTest
    {
        [Test]
        public void PricisionTest()
        {
            double x = 0.0d;
            Assert.IsTrue(x == 0d);

            x = 0.9d;
            Assert.IsFalse(x == 0d);

            x = Double.Epsilon;
            Assert.IsFalse(x == 0d);

            x = 0.0000000000000000000000000001d;
            Assert.IsFalse(x == 0d);

            x = Double.Epsilon - Double.Epsilon;
            Assert.IsTrue(x == 0d);

            float x2 = 0.0f;
            Assert.IsTrue(x2 == 0.0f);

            x = Double.Epsilon;
            Assert.IsTrue(x2 == 0);
        }

        [Test]
        public void DrawLines() 
        {
            var form = new TestForm();
            form.Paint += (object sender, PaintEventArgs e) =>
            {

                e.Graphics.Clear(Color.Black);

                Bitmap bmp = new Bitmap(320, 240);
                Draw2D.Line(0, 0, 319, 0, bmp, Color.White);
                Draw2D.Line(0, 0, 319, 10, bmp, Color.White);
                Draw2D.Line(0, 0, 319, 239, bmp, Color.White);
                Draw2D.Line(0, 0, 10, 239, bmp, Color.White);
                Draw2D.Line(0, 0, 0, 239, bmp, Color.White);
                Draw2D.Line(319, 180, 0, 120, bmp, Color.White);

                e.Graphics.DrawImage(bmp, 0, 0);
            };

            Application.Run(form);
        }

        //[Ignore("NotImplemented")]
        [Test]
        public void DrawLinesOutScreen()
        {
            var form = new TestForm();
            form.Paint += (object sender, PaintEventArgs e) =>
            {

                e.Graphics.Clear(Color.Black);

                Bitmap bmp = new Bitmap(320, 240);
                Draw2D.Line(-10, -10, 400, 0, bmp, Color.White);
                Draw2D.Line(-10, -10, 319, 10, bmp, Color.White);
                Draw2D.Line(-10, -10, -10, 400, bmp, Color.White);
                Draw2D.Line(319, 180, 0, 120, bmp, Color.White);

                e.Graphics.DrawImage(bmp, 0, 0);
            };

            Application.Run(form);
        }

        [Test]
        public void DrawTriangles()
        {
            var form = new TestForm();
            form.Paint += (object sender, PaintEventArgs e) =>
            {
                e.Graphics.Clear(Color.Black);

                Bitmap bmp = new Bitmap(320, 240);

                //Проверяем прямой порядок вершин по у
                Draw2D.Triangle(new Point2D(60,60), new Point2D(10,120), new Point2D(150,120), bmp, Color.White);
                Draw2D.Triangle(new Point2D(220, 60), new Point2D(200, 120), new Point2D(319, 120), bmp, Color.Orange);

                //обратный порядок вершин по Y
                Draw2D.Triangle(new Point2D(10, 200), new Point2D(180, 180), new Point2D(70, 140), bmp, Color.Green);

                e.Graphics.DrawImage(bmp, 0, 0);
            };

            Application.Run(form);
        }

        //[Ignore("NotImplemented")]
        [Test]
        public void DrawTrianglesOutScreen()
        {
            var form = new TestForm();
            form.Paint += (object sender, PaintEventArgs e) =>
            {
                e.Graphics.Clear(Color.Black);

                Bitmap bmp = new Bitmap(320, 240);

                Draw2D.Triangle(new Point2D(-40, 120), new Point2D(170, -50), new Point2D(370, 119), bmp, Color.White);

                e.Graphics.DrawImage(bmp, 0, 0);
            };

            Application.Run(form);
        }

        [Test]
        public void DrawTriangles_SubpixelAccuracity()
        {
            var form = new TestForm();
            form.ClientSize = new Size(640,480);

            var vertex1 = new Vector3f(148.5f, 20, 0);
            var vertex2 = new Vector3f(10, 119.0f, 0);

            form.Paint += (object sender, PaintEventArgs e) =>
            {
                e.Graphics.Clear(Color.Black);
                e.Graphics.SmoothingMode = SmoothingMode.None;
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

                Bitmap bmp = new Bitmap(320, 240);
                var zbuffer = new double[320 * 240];

                //Проверяем прямой порядок вершин по у
                Draw3D.SimpleRasterizeTriangle(vertex1, vertex2, new Vector3f(150.3f, 120.1f, 0), bmp, Color.White, zbuffer);
                Draw3D.SimpleRasterizeTriangle(new Vector3f(220, 60, 0), new Vector3f(200, 120, 0), new Vector3f(319, 120, 0), bmp, Color.Orange, zbuffer);

                //обратный порядок вершин по Y
                Draw3D.SimpleRasterizeTriangle(new Vector3f(10, 200, 0), new Vector3f(180, 180, 0), new Vector3f(70, 140, 0), bmp, Color.Green, zbuffer);

                e.Graphics.DrawImage(bmp, 0, 0);
            };

            form.KeyDown += (object sender, KeyEventArgs e) =>
            {
                if (e.KeyCode == Keys.A) vertex1.X -= 0.05f;
                if (e.KeyCode == Keys.D) vertex1.X += 0.05f;

                if (e.KeyCode == Keys.W) vertex2.Y -= 0.05f;
                if (e.KeyCode == Keys.S) vertex2.Y += 0.05f;

                form.Refresh();
            };

            Application.Run(form);
        }
    }
}
