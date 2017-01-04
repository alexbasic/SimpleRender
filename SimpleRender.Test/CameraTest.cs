using NUnit.Framework;
using SimpleRender.SceneObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleRender.Math;

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
            var cube = new Cube() { Position = new Vector4() { X = 0, Y = 0, Z = 1.25f, W = 1d } };
            scene.Objects = new List<Object3D>
            {
                new Pyramid(){Position = new Vector4() { X = 0.7f, Y = 0, Z = 1.25f, W = 1d }},
                cube,
                new Pyramid(){Position = new Vector4() { X = -0.8f, Y = 0, Z = 3.5f, W = 1d }, Mategial
                = new Material{DiffuseColor = new Vector4{X = 1, Y = 1, Z = 0, W = 1 }}}
            };
            scene.LightSources =new List<LightSource>{new GlobalLightSource() { Color = new Vector4(1, 0.9f, 0.65f, 1), Intensity = 1f, Position = new Vector4(1, 1, -1, 1) 
            }};
            scene.AmbientColor = new Vector4(0, 0, 0.3f, 0);

            var form = new TestForm();
            form.ClientSize = new Size(320, 240);
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
