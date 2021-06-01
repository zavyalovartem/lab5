using System;
using System.Collections.Generic;
using System.Text;

namespace Lab5
{
    class SOLEInvertibleMatrix
    {
        public Matrix Solve(Matrix leftCoeffs, Matrix rightCoeffs)
        {
            if (leftCoeffs.M != rightCoeffs.N)
            {
                return null;
            }
            var invertible = leftCoeffs.CreateInvertibleMatrix();
            
            if (rightCoeffs.N > 1)
            {
                rightCoeffs = rightCoeffs.CreateTransposeMatrix();
            }

            var res = invertible * rightCoeffs;
            return res;
        }
    }
}
