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
            form.ClientSize = new Size(300,300);

            var vertex1 = new Vector3f(0.5f, 0.5f, 0);
            var vertex2 = new Vector3f(5.1f, 0.9f, 0);
            var vertex3 = new Vector3f(4.8f, 4.9f, 0);

            form.Paint += (object sender, PaintEventArgs e) =>
            {
                e.Graphics.Clear(Color.Black);
                e.Graphics.SmoothingMode = SmoothingMode.None;
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

                Bitmap bmp = new Bitmap(15, 15);
                var zbuffer = new double[15 * 15];

                //Проверяем прямой порядок вершин по у
                Draw3D.SimpleRasterizeTriangle(vertex1, vertex2, vertex3, bmp, Color.Green, zbuffer);

                var imageScale = 20;

                DrawBitmap(e.Graphics, bmp, new Rectangle(0, 0, 300, 300));

                DrawGrid(new Pen(Color.Gray), e.Graphics, 15, 15, 300, 300);
                DrawBoundTriangle(new Pen(Color.Red), e.Graphics, vertex1, vertex2, vertex3, imageScale);
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

        private void DrawGrid(Pen pen, Graphics graphics, int cols, int rows, int canvasWidth, int canvasHeight)
        {
            var stepX = canvasHeight/cols;
            for (var col = 0; col < cols; col++)
            {
                var x = col * stepX;
                graphics.DrawLine(pen,x, 0, x,canvasHeight);
            }

            for (var row = 0; row < rows; row++)
            {
                var y = row * stepX;
                graphics.DrawLine(pen, 0, y, canvasWidth, y);
            }
        }

        private void DrawBoundTriangle(Pen pen, Graphics graphics, Vector3f vertex1, Vector3f vertex2, Vector3f vertex3, int imageScale)
        {
            graphics.DrawLine(pen, vertex1.X * imageScale, vertex1.Y * imageScale, vertex2.X * imageScale, vertex2.Y * imageScale);
            graphics.DrawLine(pen, vertex2.X * imageScale, vertex2.Y * imageScale, vertex3.X * imageScale, vertex3.Y * imageScale);
            graphics.DrawLine(pen, vertex3.X * imageScale, vertex3.Y * imageScale, vertex1.X * imageScale, vertex1.Y * imageScale);
        }

        private void DrawBitmap(Graphics graphics, Bitmap bmp, Rectangle destRect)
        {
            var stepX = destRect.Width / bmp.Width;
            var stepY = destRect.Height / bmp.Height;

            for (int h = 0; h < bmp.Height; h++)
            {
                for (int w = 0; w < bmp.Width; w++)
                {
                    var color = bmp.GetPixel(w, h);

                    var brush = new SolidBrush(color);

                    graphics.FillRectangle(brush, stepX * w, stepY * h, stepX, stepY);
                }
            }
        }
    }
}
