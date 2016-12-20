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
            get { return _array[item % 4, item / 4]; }
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

        public static Matrix operator *(Matrix a, Matrix b)
        {
            //row count from a
            //col count from b

            //col 1
            var x1 = a[0, 0] * b[0, 0] + a[1, 0] * b[0, 1] + a[2, 0] * b[0, 2] + a[3, 0] * b[0, 3];
            var x2 = a[0, 1] * b[0, 0] + a[1, 1] * b[0, 1] + a[2, 1] * b[0, 2] + a[3, 1] * b[0, 3];
            var x3 = a[0, 2] * b[0, 0] + a[1, 2] * b[0, 1] + a[2, 2] * b[0, 2] + a[3, 2] * b[0, 3];
            var x4 = a[0, 3] * b[0, 0] + a[1, 3] * b[0, 1] + a[2, 3] * b[0, 2] + a[3, 3] * b[0, 3];

            //col 2
            var x5 = a[0, 0] * b[1, 0] + a[1, 0] * b[1, 1] + a[2, 0] * b[1, 2] + a[3, 0] * b[1, 3];
            var x6 = a[0, 1] * b[1, 0] + a[1, 1] * b[1, 1] + a[2, 1] * b[1, 2] + a[3, 1] * b[1, 3];
            var x7 = a[0, 2] * b[1, 0] + a[1, 2] * b[1, 1] + a[2, 2] * b[1, 2] + a[3, 2] * b[1, 3];
            var x8 = a[0, 3] * b[1, 0] + a[1, 3] * b[1, 1] + a[2, 3] * b[1, 2] + a[3, 3] * b[1, 3];

            //col 3
            var x9 = a[0, 0] * b[2, 0] + a[1, 0] * b[2, 1] + a[2, 0] * b[2, 2] + a[3, 0] * b[2, 3];
            var x10 = a[0, 1] * b[2, 0] + a[1, 1] * b[2, 1] + a[2, 1] * b[2, 2] + a[3, 1] * b[2, 3];
            var x11 = a[0, 2] * b[2, 0] + a[1, 2] * b[2, 1] + a[2, 2] * b[2, 2] + a[3, 2] * b[2, 3];
            var x12 = a[0, 3] * b[2, 0] + a[1, 3] * b[2, 1] + a[2, 3] * b[2, 2] + a[3, 3] * b[2, 3];

            //col 4
            var x13 = a[0, 0] * b[3, 0] + a[1, 0] * b[3, 1] + a[2, 0] * b[3, 2] + a[3, 0] * b[3, 3];
            var x14 = a[0, 1] * b[3, 0] + a[1, 1] * b[3, 1] + a[2, 1] * b[3, 2] + a[3, 1] * b[3, 3];
            var x15 = a[0, 2] * b[3, 0] + a[1, 2] * b[3, 1] + a[2, 2] * b[3, 2] + a[3, 2] * b[3, 3];
            var x16 = a[0, 3] * b[3, 0] + a[1, 3] * b[3, 1] + a[2, 3] * b[3, 2] + a[3, 3] * b[3, 3];

            return new Matrix(
                x1, x5, x9,  x13,
                x2, x6, x10, x14,
                x3, x7, x11, x15,
                x4, x8, x12, x16
                );
        }
    }
}
