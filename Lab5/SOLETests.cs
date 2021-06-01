using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab5
{
    [TestFixture]
    class SOLETests
    {
        [Test]
        public void testSolving()
        {
            double[,] left =
{
                { 3, -2, 4},
                { 3, 4, -2},
                {2, -1, -1 }
            };
            double[,] right =
            {
                {21, 9, 10}
            };
            var leftMatrix = new Matrix(left);
            var rightMatrix = new Matrix(right);
            var solver = new SOLEInvertibleMatrix();
            var actual = solver.Solve(leftMatrix, rightMatrix);
            for (var i = 0; i < actual.data.Length; i++)
            {
                actual.data[i,0] = Math.Round(actual.data[i,0]);
            }
            var expected = new double[,] { { 5}, { -1 }, { 1 } };
            Assert.AreEqual(expected, actual.data);
        }
    }
}
