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

        public double this[int item]
        {
            get { return _array[item / 4, item % 4]; }
            set
            {
                _array[item / 4, item % 4] = value;
            }
        }

        public double this[int coll, int row]
        {
            get
            {   
                return _array[row, coll];
            }
            set
            {
                _array[row, coll] = value;
            }
        }
    }

    public class Vector4f : Vector3f
    {
        public double W { get; set; }
    }
}
