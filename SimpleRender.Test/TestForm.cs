using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework.Constraints;

namespace SimpleRender.Test
{
    public class TestForm : Form
    {
        public TestForm()
        {
            this.DoubleBuffered = true;
        }
    }
}
