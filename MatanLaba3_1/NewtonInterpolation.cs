namespace MatanLaba3_1;

public class NewtonInterpolation
{
    public double Interpolate(double[] x, double[] y, double t)
    {
        var n = x.Length;
        var sum = 0d;
        for (var i = 0; i < n; i++)
        {
            var f = 0d;
            for (var j = 0; j <= i; j++)
            {
                var den = 1d;
                for (var k = 0; k <= i; k++)
                {
                    if (k != j)
                    {
                        den *= x[j] - x[k];
                    }
                }

                f += y[j] / den;
            }
            for(int k = 0; k < i; ++k)
                f *= t - x[k];
            sum += f;
        }

        return sum;
    }
}