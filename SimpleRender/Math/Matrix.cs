using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SimpleRender.Math
{
    public class Matrix
    {
        private double[,] _array { get; set; }

        public Matrix()
        {
            _array = new double[4, 4];
        }

        public Matrix(params double[] array)
        {
            _array = new double[4, 4];
            _array[0, 0] = array[0]; _array[1, 0] = array[1]; _array[2, 0] = array[2]; _array[3, 0] = array[3];
            _array[0, 1] = array[4]; _array[1, 1] = array[5]; _array[2, 1] = array[6]; _array[3, 1] = array[7];
            _array[0, 2] = array[8]; _array[1, 2] = array[9]; _array[2, 2] = array[10]; _array[3, 2] = array[11];
            _array[0, 3] = array[12]; _array[1, 3] = array[13]; _array[2, 3] = array[14]; _array[3, 3] = array[15];
        }

        public double this[int item]
        {
            get { return _array[ item % 4, item / 4]; }
            set
            {
                _array[item % 4, item / 4] = value;
            }
        }

        public double this[int coll, int row]
        {
            get
            {   
                return _array[coll, row];
            }
            set
            {
                _array[coll, row] = value;
            }
        }

        /// <summary>
        ///  multiply for vertical vector
        /// </summary>
        public static Vector4 operator *(Matrix m, Vector4 v)
        {
            return new Vector4
            {
                X = (float)(m[0, 0] * v.X + m[1, 0] * v.Y + m[2, 0] * v.Z + m[3, 0] * v.W),
                Y = (float)(m[0, 1] * v.X + m[1, 1] * v.Y + m[2, 1] * v.Z + m[3, 1] * v.W),
                Z = (float)(m[0, 2] * v.X + m[1, 2] * v.Y + m[2, 2] * v.Z + m[3, 2] * v.W),
                W = (float)(m[0, 3] * v.X + m[1, 3] * v.Y + m[2, 3] * v.Z + m[3, 3] * v.W),
            };
        }

        /// <summary>
        ///  multiply for horizontal vector
        /// </summary>
        public static Vector4 operator &(Vector4 v, Matrix m)
        {
            return new Vector4
            {
                X = (float)(m[0, 0] * v.X + m[0, 1] * v.Y + m[0, 2] * v.Z + m[0, 3] * v.W),
                Y = (float)(m[1, 0] * v.X + m[1, 1] * v.Y + m[1, 2] * v.Z + m[1, 3] * v.W),
                Z = (float)(m[2, 0] * v.X + m[2, 1] * v.Y + m[2, 2] * v.Z + m[2, 3] * v.W),
                W = (float)(m[3, 0] * v.X + m[3, 1] * v.Y + m[3, 2] * v.Z + m[3, 3] * v.W),
            };
        }
    }
}
