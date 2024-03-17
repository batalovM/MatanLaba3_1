using System;
using System.Collections.Generic;

namespace MatanLaba3_1;

public class GaussMethod
{ 
   private readonly double[,] _a;
   private readonly double[] _b;

   public GaussMethod(double[,] a, double[] b)
   {
      _a = a;
      _b = b;
   }
   private double[,] ExtendedMatrix(double[,] a, IReadOnlyList<double> b)//расширенная матрица
   {
      var n = a.GetLength(0); // Размерность системы
      var matrix = new double[n, n + 1]; // Расширенная матрица
      for (var i = 0; i < n; i++)// Заполнение расширенной матрицы
      {
         for (var j = 0; j < n; j++)
         {
            matrix[i, j] = a[i, j];
         }
         matrix[i, n] = b[i];
      }
      return matrix;
   }
   public double[] GaussWithMainElement()
   {
      var n = _a.GetLength(0);
      var x = new double[n];
      var matrix = ExtendedMatrix(_a, _b);
      for (var i = 0; i < n; i++)
      {
         // Прямой ход
         for (var j = i + 1; j < n; j++)
         {
            if (!(Math.Abs(matrix[i, i]) < Math.Abs(matrix[j, i]))) continue;
            for (var k = 0; k <= n; k++)
            {
               (matrix[i, k], matrix[j, k]) = (matrix[j, k], matrix[i, k]);
            }
         }
         // Приведение матрицы к треугольному виду
         for (var j = i + 1; j < n; j++)
         {
            var factor = matrix[j, i] / matrix[i, i];
            for (var k = i; k <= n; k++)
            {
               matrix[j, k] -= factor * matrix[i, k];
            }
         }
      }
      // Обратный ход
      for (var i = n - 1; i >= 0; i--)
      {
         x[i] = matrix[i, n];
         for (var j = i + 1; j < n; j++)
         {
            x[i] -= matrix[i, j] * x[j];
         }
         x[i] /= matrix[i, i];
      }

      return x;
   }
}