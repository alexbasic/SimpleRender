using System;
using System.Drawing;
using System.Linq;
using SimpleRender.Drawing;
using SimpleRender.Drawing.Shaders;
using SimpleRender.Math;

namespace SimpleRender.SceneObjects
{
    public class Renderer
    {
        private double _halfScreenWidth;
        private double _halfscreenHeight;

        private int _screenWidth;
        private int _screenHeight;

        public Bitmap FrameBuffer { get; set; }
        private double[] _zBuffer;

        public IShaderProgram ShaderProgram { get; set; }

        public Renderer(int screenWidth, int screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _halfScreenWidth = (int)((screenWidth - 1) / 2);
            _halfscreenHeight = (int)((screenHeight - 1) / 2);
            FrameBuffer = new Bitmap(screenWidth, screenHeight);
            _zBuffer = new double[_screenWidth * _screenHeight];
        }

        public void Render(Scene scene)
        {
            //TransformedVector = TranslationMatrix * RotationMatrix * ScaleMatrix * OriginalVector;
            //MVPmatrix = projection * view * model; 

            ClearBuffers();

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

                var transformMatrix = cvvMatrix * viewMatrix;

                ShaderProgram = new ShaderProgram(cvvMatrix, transformMatrix, modelMatrix, primitive.Mategial);

                foreach (var triangle in primitive.Faces)
                {
                    DrawTriangle(
                        primitive.Vertices[triangle.Vertex1],
                        primitive.Vertices[triangle.Vertex2],
                        primitive.Vertices[triangle.Vertex3]);
                }
            }
        }

        private void DrawTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            //var v1 = primitive.Vertices[triangle.Vertex1];
            //var v2 = primitive.Vertices[triangle.Vertex2];
            //var v3 = primitive.Vertices[triangle.Vertex3];

            //TODO Здесь нужен вызов фрагментного шейдера
            //shader.Set(v1,v2,v3) shader.Compute()

            Vector3f faceNormal = Math3D.CalculateNormal(Math3D.ConvertToDecart(v1), Math3D.ConvertToDecart(v2), Math3D.ConvertToDecart(v3));

            var worldCoord1 = _modelMatrix * new Vector4(v1.Position.X, v1.Position.Y, v1.Position.Z, 1);
            var worldCoord2 = _modelMatrix * new Vector4(v2.X, v2.Y, v2.Z, 1);
            var worldCoord3 = _modelMatrix * new Vector4(v3.X, v3.Y, v3.Z, 1);

            ShaderProgram.ComputeVertex(new VertexInput{Position = v1, Normal = (Vector4)faceNormal});
            ShaderProgram.ComputeVertex(vi2);
            ShaderProgram.ComputeVertex(vi3);

            Draw3D.SimpleRasterizeTriangle(
                ConvertToScreenCoord0(decartvector1),
                ConvertToScreenCoord0(decartvector2),
                ConvertToScreenCoord0(decartvector3),
                FrameBuffer,
                Color.FromArgb((int)(255 * Restrict(sampleColor.X)), (int)(255 * Restrict(sampleColor.Y)), (int)(255 * Restrict(sampleColor.Z))),
                _zBuffer);    
        }

        private void ClearBuffers()
        {
            var graphics = Graphics.FromImage(FrameBuffer);
            graphics.Clear(Color.Black);

            for (var x = 0; x < _zBuffer.Length; x++)
            {
                _zBuffer[x] = 0d;
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