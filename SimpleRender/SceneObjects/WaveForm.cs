using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.SceneObjects
{
    public class WaveForm
    {
        public static Object3D Load(string fileName)
        {
            Object3D result = new Object3D();
            result.Vertices = new List<Vertex>();
            result.Normals = new List<Vertex>();
            result.TextureVertices = new List<Vertex>();
            result.Faces = new List<Face>();
            int verticiseCounter = 0;
            int normalsCounter = 0;
            int textureVerticiseCounter = 0;
            int facesCounter = 0;
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().TrimStart();
                    string normalizedLine = System.Text.RegularExpressions.Regex.Replace(line, " +", " ");
                    var newNumberFormatInfo = new System.Globalization.NumberFormatInfo();
                    newNumberFormatInfo.NumberDecimalSeparator = ".";
                    if (line.StartsWith("v"))
                    {
                        string[] vectorElements = normalizedLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        ++verticiseCounter;
                        result.Vertices.Add(new Vertex
                        {
                            Number = verticiseCounter,
                            X = Single.Parse(vectorElements[1], newNumberFormatInfo),
                            Y = Single.Parse(vectorElements[2], newNumberFormatInfo),
                            Z = Single.Parse(vectorElements[3], newNumberFormatInfo)
                        });
                    }
                    if (line.StartsWith("vt"))
                    {
                        string[] vectorElements = normalizedLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        ++textureVerticiseCounter;
                        result.TextureVertices.Add(new Vertex
                        {
                            Number = textureVerticiseCounter,
                            X = Single.Parse(vectorElements[1], newNumberFormatInfo),
                            Y = Single.Parse(vectorElements[2], newNumberFormatInfo),
                            Z = Single.Parse(vectorElements[3], newNumberFormatInfo)
                        });
                    }
                    if (line.StartsWith("vn"))
                    {
                        string[] vectorElements = normalizedLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        ++normalsCounter;
                        result.Normals.Add(new Vertex
                        {
                            Number = normalsCounter,
                            X = Single.Parse(vectorElements[1], newNumberFormatInfo),
                            Y = Single.Parse(vectorElements[2], newNumberFormatInfo),
                            Z = Single.Parse(vectorElements[3], newNumberFormatInfo)
                        });
                    }
                    if (line.StartsWith("f"))
                    {
                        string[] faceElements = normalizedLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        string[] faceElementItems1 = faceElements[1].Split(new char[] { '/' }, StringSplitOptions.None);
                        string[] faceElementItems2 = faceElements[2].Split(new char[] { '/' }, StringSplitOptions.None);
                        string[] faceElementItems3 = faceElements[3].Split(new char[] { '/' }, StringSplitOptions.None);
                        ++facesCounter;
                        result.Faces.Add(new Face
                        {
                            Number = facesCounter,
                            Vertex1 = int.Parse(faceElementItems1[0]),
                            TextureVertex1 = int.Parse(faceElementItems1[1]),
                            NormalVertex1 = int.Parse(faceElementItems1[2]),
                            Vertex2 = int.Parse(faceElementItems2[0]),
                            TextureVertex2 = int.Parse(faceElementItems2[1]),
                            NormalVertex2 = int.Parse(faceElementItems2[2]),
                            Vertex3 = int.Parse(faceElementItems3[0]),
                            TextureVertex3 = int.Parse(faceElementItems3[1]),
                            NormalVertex3 = int.Parse(faceElementItems3[2])
                        });
                    }
                }
            }
            return result;
        }
    }
}
