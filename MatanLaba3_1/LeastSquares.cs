using System;

namespace MatanLaba3_1;

public class LeastSquares
{
    public double[] FitPolynomial(double[] x, double[] y, int degree)
    {
        var n = x.Length;
        var m = degree + 1;

        var a = new double[m, m];
        var b = new double[m];

        for (var i = 0; i < m; i++)
        {
            for (var j = 0; j < m; j++)
            {
                for (var k = 0; k < n; k++)
                {
                    a[i, j] += Math.Pow(x[k], i + j);
                }
            }

            for (var k = 0; k < n; k++)
            {
                b[i] += y[k] * Math.Pow(x[k], i);
            }
        }

        var result = new GaussMethod(a, b).GaussWithMainElement();

        return result;
    }
}