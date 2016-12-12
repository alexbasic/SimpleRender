using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleRender.Drawing;
using SimpleRender.Math;

namespace SimpleRender.SceneObjects
{
    public interface ICamera
    {
        void Render();
    }

    public class Camera : ICamera
    {
        private double _halfScreenWidth;
        private double _halfscreenHeight;

        public Camera() 
        {
            var screenWidth = 1024;
            var screenHeight = 768;
            _halfScreenWidth = screenWidth / 2;
            _halfscreenHeight = screenHeight / 2;
        }

        public void Render()
        {
            //  
        }

        private Point2D ConvertToScreenCoord(Vector3f vector) 
        {
            var dist = 1d;
            var a = vector.X / (_halfScreenWidth);
            var b = vector.Y / (_halfscreenHeight);
            var c = (vector.Z + dist) / dist;
            var screenX = _halfScreenWidth + _halfScreenWidth * a / c;
            var screenY = _halfscreenHeight - _halfscreenHeight * b / c;
            return new Point2D((int)screenX, (int)screenY);
        }

        private Vector3f[] CalculatePQR(Vector3f target, Vector3f location, ) 
        {
            var p = target - location;
            Vector3f q = null;
if( p.X == 0 && p.Z == 0) { q = new Vector3f(0, 0, 1)} else {q = new Vector3f(p.Z, 0, -p.X)};
var r = p*q;//crossProduct(p, q);
4. Считаем lp = length(p) - длина p
5. Приводим r и q к длине 2*lp*tan(FOV/2)
        }
    }
}
