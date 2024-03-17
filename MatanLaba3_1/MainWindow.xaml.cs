using System;
using System.Globalization;
using System.Windows;
using ScottPlot;

namespace MatanLaba3_1;

public partial class MainWindow
{
    private double[] _dataX;
    private double[] _dataY;
    private double[] _newX;
    // double[] dataX = { -2, 0, 1, 3, 5 };// начальные данные из таблицы
    // double[] dataY = { 7, 6, 10, 9, 10 };
    //double[] newX = {-1.25, -0.5, 0.2, 0.5, 1.2, 1.5, 2.2, 2.9, 3.5, 4.5};//массив для интерполяции Лангража
    // double[] newX = { 2, 3 };//массив для интерполяции Ньютона
    public MainWindow()
    {
        InitializeComponent();
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var inputX = DataX.Text.Split(',');
        var inputY = DataY.Text.Split(',');
        var inputInter = InterpolValuesX.Text.Split(','); 
        _newX = Array.ConvertAll(inputInter, s => double.Parse(s, CultureInfo.InvariantCulture));
        _dataX = Array.ConvertAll(inputX, double.Parse);
        _dataY = Array.ConvertAll(inputY, double.Parse);
        WpfPlot1.Plot.Clear();
        Generate();
    }

    private void Generate()
    {
        var newY = new double[_newX.Length];//массив для интерполяционных значений
        Func<double, double> fx = x => (19 * Math.Pow(x, 4) - 122 * Math.Pow(x, 3) + 31 * Math.Pow(x, 2) + 632*x+840) / 140;//функция лангража
        var coefficients1 = new LeastSquares().FitPolynomial(_dataX, _dataY, 1);
        var coefficients2 = new LeastSquares().FitPolynomial(_dataX, _dataY, 2);
        var coefficients3 = new LeastSquares().FitPolynomial(_dataX, _dataY, 3);
        var coefficients4 = new LeastSquares().FitPolynomial(_dataX, _dataY, 4);
        Func<double, double> firstDegree = x => coefficients1[0] + coefficients1[1] * x;
        Func<double, double> secondDegree = x => coefficients2[0] + coefficients2[1] * x + (coefficients2[2] * Math.Pow(x, 2));
        Func<double, double> thirdDegree = x => coefficients3[0] + coefficients3[1] * x + coefficients3[2] * Math.Pow(x, 2) + coefficients3[3] * Math.Pow(x, 3);
        Func<double, double> forthDegree = x => coefficients4[0] + coefficients4[1] * x + coefficients4[2] * Math.Pow(x, 2) + coefficients4[3] * Math.Pow(x, 3) + coefficients4[4]*Math.Pow(x, 4);
        var test = new LagrangeInterpolation();
        var test1 = new NewtonInterpolation();
        for (var i = 0; i < _newX.Length; i++)
        {
            if (Langrage.IsChecked == true)
            {
                newY[i] = test.Interpolate(_dataX, _dataY, _newX[i]);
            }
            else
            {
                newY[i] = test1.Interpolate(_dataX, _dataY, _newX[i]);
            }
        }

        if (LeastSquares.IsChecked == true)
        {
            var loadGraphFirstDegree = FuncLoad(firstDegree, -20, 20);
            var loadGraphSecondDegree = FuncLoad(secondDegree, -50, 50.7);
            var loadGraphThirdDegree = FuncLoad(thirdDegree, -50, 50.7);
            var loadGraphForthDegree = FuncLoad(forthDegree, -50, 50.7);
            WpfPlot1.Plot.Add.Scatter(loadGraphFirstDegree.Item1, loadGraphFirstDegree.Item2, Colors.Green);
            WpfPlot1.Plot.Add.Scatter(loadGraphSecondDegree.Item1, loadGraphSecondDegree.Item2, Colors.Blue);
            WpfPlot1.Plot.Add.Scatter(loadGraphThirdDegree.Item1, loadGraphThirdDegree.Item2, Colors.Red);
            WpfPlot1.Plot.Add.Scatter(loadGraphForthDegree.Item1, loadGraphForthDegree.Item2, Colors.Orange);
            WpfPlot1.Plot.Axes.SetLimits(-10, 10, 5, 12);
        }
        else
        {
            var loadGraphFx = FuncLoad(fx, -3, 5.7);//загрузка графика лангража
            WpfPlot1.Plot.Add.Scatter(loadGraphFx.Item1, loadGraphFx.Item2, Colors.Green);
            WpfPlot1.Plot.Axes.SetLimits(-12, 12, 2, 22);
            for(var i = 0; i<_newX.Length; i++)
            {
                var myScatter = WpfPlot1.Plot.Add.Scatter(_newX[i], newY[i], Colors.Orange);
                myScatter.MarkerSize = 15;
                Console.WriteLine($"Точки интерполяции: x:{_newX[i]:F3} y: {newY[i]:F3}");
            }
            for(var i = 0; i<_dataX.Length; i++)
            {
                var myScatter = WpfPlot1.Plot.Add.Scatter(_dataX[i], _dataY[i], Colors.Blue);
                myScatter.MarkerSize = 15;
                Console.WriteLine($"Набор точек: x:{_dataX[i]:F3} y: {_dataY[i]:F3}");
            } 
        }
        WpfPlot1.Refresh();
    }
    private Tuple<double[], double[]> FuncLoad(Func<double, double> fx, int start, double end)
    { 
        var x = new double[10001]; 
        var y = new double[10001];
        var step = (end - start) / (x.Length - 1);
        for (var i = 0; i < x.Length; i++)
        {
            x[i] = start + i * step;
            y[i] = fx(x[i]);
        }
        return new Tuple<double[], double[]>(x, y);
    }
    
}