using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            var cvvMatrix = Math3D.GetPerspectiveMatrix(100, _halfScreenWidth / _halfscreenHeight, 0.3d, 1000d);
            foreach (var primitive in scene.Objects)
            {
                var rotationMatrix = Math3D.GetRotationMatrix(
                    primitive.Rotation.X,
                    primitive.Rotation.Y,
                    primitive.Rotation.Z);

                var translationMatrix = Math3D.GetTranslationMatrix(primitive.Position.X, primitive.Position.Y, primitive.Position.Z);

                var scaleMatrix = Math3D.GetScaleMatrix(1, 1, 1);
                var modelMatrix = translationMatrix * (rotationMatrix * scaleMatrix);
                var viewMatrix = Math3D.GetViewMatrix(new Vector3f(0f, 1.2f, -2f), new Vector3f(0, 0f, 0f));

                var transformMatrix = cvvMatrix * viewMatrix;// * modelMatrix;

                foreach (var triangle in primitive.Faces)
                {
                    var v1 = primitive.Vertices[triangle.Vertex1];
                    var v2 = primitive.Vertices[triangle.Vertex2];
                    var v3 = primitive.Vertices[triangle.Vertex3];

                    var worldCoord1 = modelMatrix * new Vector4(v1.X, v1.Y, v1.Z, 1);
                    var worldCoord2 = modelMatrix * new Vector4(v2.X, v2.Y, v2.Z, 1);
                    var worldCoord3 = modelMatrix * new Vector4(v3.X, v3.Y, v3.Z, 1);

                    var vector1 = transformMatrix * worldCoord1;
                    var vector2 = transformMatrix * worldCoord2;
                    var vector3 = transformMatrix * worldCoord3;

                    var decartvector1 = Math3D.ConvertToDecart(vector1);
                    var decartvector2 = Math3D.ConvertToDecart(vector2);
                    var decartvector3 = Math3D.ConvertToDecart(vector3);

                    Vector3f faceNormalInWorldCoord = Math3D.CalculateNormal(Math3D.ConvertToDecart(worldCoord1), Math3D.ConvertToDecart(worldCoord2), Math3D.ConvertToDecart(worldCoord3));

                    Vector3f faceNormalInProjectionCoord = SimpleRender.Math.Vector3f.CrossProductLeft((decartvector3 - decartvector1), (decartvector2 - decartvector1));
                    faceNormalInProjectionCoord = faceNormalInProjectionCoord.Normalize();
                    var viewDirection = new Vector4(0,0,-1, 1);
                    double intensity = Math3D.DotProduct(faceNormalInProjectionCoord, viewDirection);
                    if (intensity <= 0) continue;

                    //ambient = Ka,
//diffuse = Kd * cos(N, L),
//specular = Ks * pow(cos(R, V), Ns),
//intensity = ambient + amp * (diffuse + specular).

                    //-----------------
                    //http://www.gamedev.ru/code/articles/HLSL?page=4
                    //Lighting:
                    //Lambert (ambient lighting)
                    //Diffuse (diffuse lighting model)
                    //Phong (specular lighting model), Blinn (blinn specular lighting model)
                    //Sum of this

                    //Реалистичное освещение на основе Кука-Торренса

                    //-------------

                    var ligthSource = scene.LightSources.First();
                    var globalLightPosition = ligthSource.Position.Normalize();

                    double illuminationIntensity = Math3D.DotProduct(faceNormalInWorldCoord, globalLightPosition);
                    var diffuseColor = new Vector4(
                        primitive.Mategial.DiffuseColor.X * ligthSource.Color.X, 
                        primitive.Mategial.DiffuseColor.Y * ligthSource.Color.Y, 
                        primitive.Mategial.DiffuseColor.Z * ligthSource.Color.Z, 
                        1) 
                        * illuminationIntensity;

                    var sampleColor = scene.AmbientColor + diffuseColor * ligthSource.Intensity;

                        Draw3D.Triangle(
                            ConvertToScreenCoord0(decartvector1),
                            ConvertToScreenCoord0(decartvector2),
                            ConvertToScreenCoord0(decartvector3),
                            Image,
                            Color.FromArgb((int)(255 * Restrict(sampleColor.X)), (int)(255 * Restrict(sampleColor.Y)), (int)(255 * Restrict(sampleColor.Z))),
                            zBuffer);
                }
            }
        }

        private float Restrict(float x) 
        {
            if (x > 1f) return 1f;
            if (x < 0f) return 0f;
            return x;
        }

        private Vector3f ConvertToScreenCoord0(Vector3f decart)
        {
            var screenX = _halfScreenWidth + _halfScreenWidth * decart.X;
            var screenY = _halfscreenHeight - _halfscreenHeight * decart.Y;
            return new Vector3f((float)screenX, (float)screenY, decart.Z);
        }
    }
}
