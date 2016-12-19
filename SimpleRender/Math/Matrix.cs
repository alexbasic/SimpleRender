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
    }

    public class Vector4f : Vector3f
    {
        public double W { get; set; }
    }
}
