using System;
using System.Collections.Generic;
using System.Text;

namespace Lab5
{
    public static class Constants
    {
        public const double DoubleComparisonDelta = 0.0000001;
    }

    public class Matrix
    {
        public double[,] data;

        private int m;
        public int M { get => this.m; }

        private int n;
        public int N { get => this.n; }

        public int Length { get => this.data.Length; }

        public bool IsSquare { get => this.M == this.N; }

        public int amountOfOperations;

        public Matrix(int m, int n)
        {
            this.m = m;
            this.n = n;
            this.data = new double[m, n];
        }

        public Matrix(double[,] matrix)
        {
            this.m = matrix.GetLength(0);
            this.n = matrix.GetLength(1);
            this.data = new double[m, n];
            Array.Copy(matrix, this.data, matrix.Length);
        }

        public Matrix Copy()
        {
            var resData = new double[this.M, this.N];
            Array.Copy(this.data, resData, this.Length);
            var res = new Matrix(resData);
            return res;
        }

        public double this[int x, int y]
        {
            get
            {
                return this.data[x, y];
            }
            set
            {
                this.data[x, y] = value;
            }
        }

        public int GetLength(int i)
        {
            return this.data.GetLength(i);
        }

        public void ProcessFunctionOverData(Action<int, int> func)
        {
            for (var i = 0; i < this.M; i++)
            {
                for (var j = 0; j < this.N; j++)
                {
                    func(i, j);
                }
            }
        }

        public static Matrix operator *(Matrix matrix, double value)
        {
            var result = new Matrix(matrix.M, matrix.N);
            result.ProcessFunctionOverData((i, j) =>
                result[i, j] = matrix[i, j] * value);
            return result;
        }

        public static Matrix operator *(Matrix matrix, Matrix matrix2)
        {
            if (matrix.N != matrix2.M)
            {
                throw new ArgumentException("matrixes can not be multiplied");
            }
            var result = new Matrix(matrix.M, matrix2.N);
            result.ProcessFunctionOverData((i, j) => {
                for (var k = 0; k < matrix.N; k++)
                {
                    result[i, j] += matrix[i, k] * matrix2[k, j];
                }
            });
            return result;
        }

        public Matrix CreateIdentityMatrix(int n)
        {
            var result = new Matrix(n, n);
            for (var i = 0; i < n; i++)
            {
                result[i, i] = 1;
            }
            return result;
        }

        public static Matrix operator +(Matrix matrix, Matrix matrix2)
        {
            if (matrix.M != matrix2.M || matrix.N != matrix2.N)
            {
                throw new ArgumentException(
                    "matrixes dimensions should be equal");
            }
            var result = new Matrix(matrix.M, matrix.N);
            result.ProcessFunctionOverData((i, j) =>
                result[i, j] = matrix[i, j] + matrix2[i, j]);
            return result;
        }

        public static Matrix operator -(Matrix matrix, Matrix matrix2)
        {
            return matrix + (matrix2 * -1);
        }

        public Matrix CreateTransposeMatrix()
        {
            var result = new Matrix(this.N, this.M);
            result.ProcessFunctionOverData((i, j) => result[i, j] = this[j, i]);
            return result;
        }

        public static Matrix GetMinor(Matrix matrix, int n)
        {
            Matrix result = new Matrix(matrix.GetLength(0) - 1, matrix.GetLength(0) - 1);
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                for (int j = 0, col = 0; j < matrix.GetLength(1); j++)
                {
                    if (j == n)
                        continue;
                    result[i - 1, col] = matrix[i, j];
                    col++;
                }
            }
            return result;
        }

        public double DetRec(Matrix matrix)
        {
            if (matrix.Length == 1)
            {
                return matrix[0,0];
            }
            if (matrix.Length == 4)
            {
                amountOfOperations += 2;
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            double sign = 1, result = 0;
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                Matrix minor = Matrix.GetMinor(matrix, i);
                result += sign * matrix[0, i] * DetRec(minor);
                sign = -sign;
                amountOfOperations += 2;
            }
            return result;
        }

        private Matrix CreateMatrixWithoutColumn(int column)
        {
            if (column < 0 || column >= this.N)
            {
                throw new ArgumentException("invalid column index");
            }
            var result = new Matrix(this.M, this.N - 1);
            result.ProcessFunctionOverData((i, j) =>
                result[i, j] = j < column ? this[i, j] : this[i, j + 1]);
            return result;
        }

        private Matrix CreateMatrixWithoutRow(int row)
        {
            if (row < 0 || row >= this.M)
            {
                throw new ArgumentException("invalid row index");
            }
            var result = new Matrix(this.M - 1, this.N);
            result.ProcessFunctionOverData((i, j) =>
                result[i, j] = i < row ? this[i, j] : this[i + 1, j]);
            return result;
        }

        public Matrix CreateInvertibleMatrix()
        {
            if (this.M != this.N)
                return null;
            var determinant = DetRec(this);
            if (Math.Abs(determinant) < Constants.DoubleComparisonDelta)
                return null;

            var result = new Matrix(M, M);
            ProcessFunctionOverData((i, j) =>
            {
                result[i, j] = ((i + j) % 2 == 1 ? -1 : 1) *
                    CalculateMinor(i, j) / determinant;
                amountOfOperations += 2;
            });

            result = result.CreateTransposeMatrix();
            return result;
        }

        private double CalculateMinor(int i, int j)
        {
            return DetRec(CreateMatrixWithoutColumn(j)
                .CreateMatrixWithoutRow(i));
        }
    }
}
