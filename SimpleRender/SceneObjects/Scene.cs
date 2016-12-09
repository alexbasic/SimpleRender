using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.SceneObjects
{
    public class Scene
    {
        public ICamera Camera { get; set; }
        public ICollection<Object3D> Objects { get; set; }

        public void Render(Bitmap image) 
        {
        }
    }
}
