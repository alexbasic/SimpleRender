using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleRender
{
    public class GCanvas : Form
    {
        private Graphics canvas;

        private Bitmap Image { get; set; }

        public Scene SceneObject { get; set; }

        public GCanvas()
        {
            InitComponents();
        }

        public void Drawing(object sender, EventArgs e)
        {
            Draw();
            canvas.Clear(Color.Black);
            canvas.DrawImage(Image, new Point(0, 0));
        }

        public void OnResizing(object sender, EventArgs e)
        {
            CreateImage();
            Drawing(sender, e);
        }

        private void InitComponents()
        {
            canvas = CreateGraphics();
            this.Shown += new EventHandler(this.Drawing);
            this.Load += new EventHandler(this.Drawing);
            this.Resize += new EventHandler(this.OnResizing);
            SceneObject = new Scene();
            SceneObject.Camera = new Camera();
            SceneObject.Camera.Wired = false;
            CreateImage();

            SceneObject.Objects = new List<Object3D>();
            SceneObject.Objects.Add(WaveForm.Load("Data\\african_head.obj"));
        }

        private void CreateImage()
        {
            canvas = CreateGraphics();
            Image = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        }

        private void Draw()
        {
            SceneObject.Render(Image);
        }
    }
}
