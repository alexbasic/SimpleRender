using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRender.Math
{
    public class Line
    {
        private double A;
        private double B;
        private double C;

        public Line(double coefficientA, double coefficientB, double coefficientC)
        {
            A = coefficientA;
            B = coefficientB;
            C = coefficientC;
        }

        public Line(double x0, double y0, double x1, double y1)
        {
            A = -(y1 - y0);
            B = x1 - x0;
            C = (y1 - y0) * x0 - (x1 - x0) * y0;
        }

        public double At(double x, double y)
        {
            return x * A + y * B + C;
        }

        public Vector2 Intersection(Line line)
        {
            var y = (line.A * C - A * line.C) / (A * line.B - line.A * B);
            var x = (A == 0.0d) ? ((line.B * C - B * line.C) / (line.A * B)) : (-(B / A) * y - (C / A));

            return new Vector2(x, y);
        }
    }
}
