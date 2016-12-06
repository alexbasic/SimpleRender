using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace SimpleRender.Test
{
    [TestFixture]
    public class Draw2DTest
    {
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

        [Ignore("NotImplemented")]
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

        [Ignore("NotImplemented")]
        [Test]
        public void DrawTrianglesOutScreen()
        {
            var form = new TestForm();
            form.Paint += (object sender, PaintEventArgs e) =>
            {
                e.Graphics.Clear(Color.Black);

                Bitmap bmp = new Bitmap(320, 240);

                Draw2D.Triangle(new Point2D(-10, 120), new Point2D(170, -10), new Point2D(330, 119), bmp, Color.White);

                e.Graphics.DrawImage(bmp, 0, 0);
            };

            Application.Run(form);
        }
    }
}
