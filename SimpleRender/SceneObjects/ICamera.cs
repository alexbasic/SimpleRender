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

        private Point2D ConvertToScreenCoord1(Vector3f vector) 
        {
            var dist = 1d;
            var a = vector.X / (_halfScreenWidth);
            var b = vector.Y / (_halfscreenHeight);
            var c = (vector.Z + dist) / dist;
            var screenX = _halfScreenWidth + _halfScreenWidth * a / c;
            var screenY = _halfscreenHeight - _halfscreenHeight * b / c;
            return new Point2D((int)screenX, (int)screenY);
        }

        private Point2D ConvertToScreenCoord2(Vector3f vector)
        {
            var fov = 1d;
            var screenX = _halfScreenWidth + vector.X * fov / vector.Z;
            var screenY = _halfscreenHeight - vector.Y * fov / vector.Z;
            return new Point2D((int)screenX, (int)screenY);
        }

        private Vector3f[] CalculatePQR(Vector3f target, Vector3f location, double fov) 
        {
            var p = target - location;
            Vector3f q = null;
            if (p.X == 0 && p.Z == 0)
            {
                q = new Vector3f(0, 0, 1);
            }
            else
            {
                q = new Vector3f(p.Z, 0, -p.X);
            };
            var r = Vector3f.CrossProduct(p, q);
            var lp = p.Length();
            //5. Приводим r и q к длине
            //2*lp*System.Math.Tan(fov/2);
            return null;
        }

        private Matrix GetFrustum(double left, double right, double bottom, double top, double near, double far)
        {
            if (far < 0 || near < 0) throw new Exception("Far and near must be positive");
            var matrix = new Matrix(
                2*near/(right-left), 0,                   (right+left)/(right-left), 0,
                0,                   2*near/(top-bottom), (top+bottom)/(top-bottom), 0,
                0,                   0,                   -1*(far+near)/(far-near),  -1*(2*far*near)/(far-near),
                0,                   0,                   -1,                        0
                );
            return matrix;
        }

        //matrix to vector and screen coords
        /*
         Vec3<float> :: Vec3(Matrix m) : x(m[0][0]/m[3][0]), y(m[1][0]/m[3][0]), z(m[2][0]/m[3][0])
         */
    }
}
