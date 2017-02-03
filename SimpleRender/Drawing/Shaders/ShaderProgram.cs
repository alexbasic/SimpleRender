using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SimpleRender.Math;
using SimpleRender.SceneObjects;

namespace SimpleRender.Drawing.Shaders
{
    public abstract class PixelShader
    {
        //Output
        public Vector4 OutColor { get; set; }
        public double OutDepth { get; set; }

        public abstract void Compute();
    }

    public abstract class VertexShader
    {
        //Output
        public Vector4 Position { get; set; }
        public Vector4 Color { get; set; }
        public Vector2 TextCoord { get; set; }


        public abstract void Compute();
    }

    public interface IShaderProgram
    {
        void ComputeVertex(VertexInput v1);
        Fragment GetFragment();
    }

    public class ShaderProgram : IShaderProgram
    {
        public VertexShader VertexShader { get; set; }

        public PixelShader PixelShader { get; set; }

        private Matrix _cvvMatrix;
        private Vector4 _ambientColor;
        private Material _material;
        private Matrix _transformMatrix;
        private Matrix _modelMatrix;

        public ShaderProgram(Matrix cvvMatrix, Matrix transformMatrix, Matrix modelMatrix, Material material)
        {
            _cvvMatrix = cvvMatrix;
            _ambientColor = material.AmbientColor;
            _material = material;
            _transformMatrix = transformMatrix;
            _modelMatrix = modelMatrix;
        }

        public void ComputeVertex(VertexInput v1)
        {
            #region Завернуть в шейдер

            var worldCoord1 = _modelMatrix * new Vector4(v1.Position.X, v1.Position.Y, v1.Position.Z, 1);

            var vector1 = _transformMatrix * worldCoord1;
            //var vector2 = _transformMatrix * worldCoord2;
            //var vector3 = _transformMatrix * worldCoord3;

            var decartvector1 = Math3D.ConvertToDecart(vector1);
            //var decartvector2 = Math3D.ConvertToDecart(vector2);
            //var decartvector3 = Math3D.ConvertToDecart(vector3);

            //Vector3f faceNormalInWorldCoord = Math3D.CalculateNormal(Math3D.ConvertToDecart(worldCoord1), Math3D.ConvertToDecart(worldCoord2), Math3D.ConvertToDecart(worldCoord3));

            Vector3f faceNormalInProjectionCoord = SimpleRender.Math.Vector3f.CrossProductLeft((decartvector3 - decartvector1), (decartvector2 - decartvector1));
            faceNormalInProjectionCoord = faceNormalInProjectionCoord.Normalize();
            var viewDirection = new Vector4(0, 0, -1, 1);
            double intensity = Math3D.DotProduct(faceNormalInProjectionCoord, viewDirection);

            //TODO Подумать нужноли это в шейдере
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
                                   _material.DiffuseColor.X * ligthSource.Color.X,
                                   _material.Mategial.DiffuseColor.Y * ligthSource.Color.Y,
                                   _material.Mategial.DiffuseColor.Z * ligthSource.Color.Z,
                                   1)
                               * illuminationIntensity;

            //var reflection = (Vector3f.CrossProductLeft(faceNormalInWorldCoord ,
            //    (Vector3f.CrossProductLeft(faceNormalInWorldCoord ,globalLightPosition) * 2.0f)) -
            //    globalLightPosition).Normalize();   // reflected light

            var sampleColor = _ambientColor + diffuseColor * ligthSource.Intensity;

            #endregion
        }

        public Fragment GetFragment()
        {
            return new Fragment();
        }
    }

    public struct VertexInput
    {
        public Vector4 Position { get; set; }
        public Vector4 Normal { get; set; }
        public Vector2 TextCoord { get; set; }
    }

    public struct Fragment
    {
        public Vector4 Color { get; set; }
        public double Depth { get; set; }
    }

    //Возможная реализация билинейной фильтрации текстурной выборки 
    //function tex2D(Sampler tex,vector2 Tcoord)
    //{
    //    color1=TextureBuffer[int(Tcoord.x*tex.Width)][int(Tcoord.y*tex.Height)];
    //    color2=TextureBuffer[int(Tcoord.x*tex.Width)+1][int(Tcoord.y*tex.Height)];
    //    color3=TextureBuffer[int(Tcoord.x*tex.Width)+okrug(Tcoord.x*tex.Width-tex.Width)][int(Tcoord.y*tex.Height)+
    //    +okrug(Tcoord.y*tex.Height-tex.Height)];

    //    return pixelOut=interpolateBiLine(color1,color2,color3,Tcoord);
    //};
}
