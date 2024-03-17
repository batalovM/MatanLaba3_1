namespace MatanLaba3_1;

public class LagrangeInterpolation
{
    public double Interpolate(double[] x, double[] y, double t)
    {
        var n = x.Length;
        var sum = 0d;
        for (var j = 0; j < n; j++)
        {
            var p1 = 1d;
            var p2 = 1d;
            for (var i = 0; i < n; i++)
            {
                if (i == j)
                {
                    p1 *= 1;
                    p2 *= 1;
                }
                else
                {
                    p1 *= t - x[i];
                    p2 *= x[j] - x[i];
                }
            }

            sum += y[j] * p1 / p2;
        }
        return sum;
    }
}