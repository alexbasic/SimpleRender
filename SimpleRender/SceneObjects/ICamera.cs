﻿using System;
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
        void Render(Scene scene);
    }

    public class Camera : ICamera
    {
        private double _halfScreenWidth;
        private double _halfscreenHeight;

        private int _screenWidth;
        private int _screenHeight;

        public Bitmap Image { get; set; }

        public Camera(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _halfScreenWidth = screenWidth / 2;
            _halfscreenHeight = screenHeight / 2;
            Image = new Bitmap(screenWidth, screenHeight);
        }

        public void Render(Scene scene)
        {
            //TransformedVector = TranslationMatrix * RotationMatrix * ScaleMatrix * OriginalVector;
            //MVPmatrix = projection * view * model; 

            double[] zBuffer = new double[_screenWidth * _screenHeight];

            var rnd = new Random();
            var cvvMatrix = GetFrustumLeft(45, _halfScreenWidth / _halfscreenHeight, 0.5d, 10000d);
            foreach (var primitive in scene.Objects)
            {
                var rotationMatrix = Math3D.GetRotationMatrix(
                    primitive.Rotation.X,
                    primitive.Rotation.Y,
                    primitive.Rotation.Z);

                var translationMatrix = Math3D.GetTranslationMatrix(primitive.Position.X, primitive.Position.Y, primitive.Position.Z);

                var scaleMatrix = Math3D.GetScaleMatrix(1, 1, 1);
                var modelMatrix = translationMatrix * (rotationMatrix * scaleMatrix);
                var viewMatrix = Math3D.GetViewMatrix(new Vector3f(0, -0.8f, -0.4f), new Vector3f(0, -0.3f, 0.3f), new Vector3f(0, 1, 0));

                var transformMatrix = cvvMatrix * viewMatrix * modelMatrix;

                foreach (var triangle in primitive.Faces)
                {
                    var v1 = primitive.Vertices[triangle.Vertex1];
                    var v2 = primitive.Vertices[triangle.Vertex2];
                    var v3 = primitive.Vertices[triangle.Vertex3];

                    var vector1 = transformMatrix * new Vector4(v1.X, v1.Y, v1.Z, 1);
                    var vector2 = transformMatrix * new Vector4(v2.X, v2.Y, v2.Z, 1);
                    var vector3 = transformMatrix * new Vector4(v3.X, v3.Y, v3.Z, 1);

                    Vector3f n = SimpleRender.Math.Vector3f.CrossProductLeft((vector3 - vector1), (vector2 - vector1));
                    n = n.Normalize();
                    var viewDirection = new Vector3f(0,0,1);
                    double intensity = Math3D.DotProduct(n, viewDirection);
                    if (intensity <= 0) continue;

                    var lightDirection = new Vector3f(1,1,1);
                    lightDirection = lightDirection.Normalize();
                    double illuminationIntensity = Math3D.DotProduct(n, lightDirection);
                    if (illuminationIntensity < 0) illuminationIntensity = 0;
                    if (illuminationIntensity > 1) illuminationIntensity = 1d;
                        Draw3D.Triangle(
                            ConvertToScreenCoord01(ConvertToDecart(vector1)),
                            ConvertToScreenCoord01(ConvertToDecart(vector2)),
                            ConvertToScreenCoord01(ConvertToDecart(vector3)),
                            Image, 
                            //Color.FromArgb(rnd.Next((int)(255 * illuminationIntensity)), rnd.Next((int)(255 * illuminationIntensity)), 128),
                            Color.FromArgb((int)(255 * illuminationIntensity), (int)(255 * illuminationIntensity), 50),
                            zBuffer);
                }
            }
        }

        private Vector3f ConvertToDecart(Vector4 vector)
        {
            return new Vector3f(vector.X / (float)vector.W, vector.Y / (float)vector.W, vector.Z / (float)vector.W);
        }

        private Point2D ConvertToScreenCoord0(Vector3f decart)
        {
            var screenX = _halfScreenWidth + _halfScreenWidth * decart.X;
            var screenY = _halfscreenHeight - _halfscreenHeight * decart.Y;
            return new Point2D((int)screenX, (int)screenY);
        }

        private Vector3f ConvertToScreenCoord01(Vector3f decart)
        {
            var screenX = _halfScreenWidth + _halfScreenWidth * decart.X;
            var screenY = _halfscreenHeight - _halfscreenHeight * decart.Y;
            return new Vector3f((float)screenX, (float)screenY, decart.Z);
        }

        private Point2D ConvertToScreenCoord1(Vector3f vector)
        {
            var dist = 480d;
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

        //Creates viewport matrix
        /// <summary>
        /// Rightside-coordinate frustum
        /// </summary>
        private Matrix GetFrustumRight(double left, double right, double bottom, double top, double near, double far)
        {
            if (far < 0 || near < 0) throw new Exception("Far and near must be positive");
            var matrix = new Matrix(
                2 * near / (right - left), 0, (right + left) / (right - left), 0,
                0, 2 * near / (top - bottom), (top + bottom) / (top - bottom), 0,
                0, 0, -1 * (far + near) / (far - near), -1 * (2 * far * near) / (far - near),
                0, 0, -1, 0
                );
            return matrix;
        }

        /// <summary>
        /// Leftside-coordinate frustum
        /// </summary>
        /// <param name="fovy">fov  y in degrees</param>
        /// <param name="aspect">aspect = w div h</param>
        /// <param name="near">near</param>
        /// <param name="far">far</param>
        /// <returns>frustum matrix</returns>
        private Matrix GetFrustumLeft(double fovyInDegree, double aspect, double near, double far)
        {
            //2n/h = ctg(fovy/2)
            //aspect = w/h
            //2n/w = ctg(fovy/2)/aspect

            var fovy = fovyInDegree / System.Math.PI;

            if (far < 0 || near < 0) throw new Exception("Far and near must be positive");
            var matrix = new Matrix(
                Math3D.Cotan(fovy / 2) / aspect, 0, 0, 0,
                0, Math3D.Cotan(fovy / 2), 0, 0,
                0, 0, (far + near) / (far - near), 1,
                0, 0, (-2 * far * near) / (far - near), 0
                );
            return matrix;
        }
    }
}
