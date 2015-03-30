using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender
{
    public class Scene
    {
        public Camera Camera { get; set; }
        public List<Object3D> Objects { get; set; }

        public void Render(Bitmap image) 
        {
            Camera.Render(this, image);
        }
    }
}
