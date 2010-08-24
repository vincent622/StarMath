﻿#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace StarMathLib
{
    public static partial class StarMath
    {
        #region Add, Subtract, and Multiply functions

        #region Multiplication of Scalars, Vectors, and Matrices

        #region Doubles-by-doubles

        /// <summary>
        ///   Multiplies all elements of a 1D double array with the double value.
        /// </summary>
        /// <param name = "a">The double value to be multiplied</param>
        /// <param name = "B">The matrix to be multiplied with</param>
        /// <returns>A 1D double array that contains the product</returns>
        public static double[] multiply(double a, double[] B)
        {
            // scale vector B by the amount of scalar B
            var size = B.GetLength(0);
            var c = new double[size];
            for (var i = 0; i != size; i++)
                c[i] = a*B[i];
            return c;
        }

        /// <summary>
        ///   Divides all elements of a 1D double array by the double value.
        /// </summary>
        /// <param name = "B">The vector to be divided</param>
        /// <param name = "a">The double value to be divided by, the divisor.</param>
        /// <returns>A 1D double array that contains the product</returns>
        public static double[] divide(double[] B, double a)
        {
            return multiply((1/a), B);
        }

        /// <summary>
        ///   Multiplies all elements of a 2D double array with a double value.
        /// </summary>
        /// <param name = "a">The double value to be multiplied</param>
        /// <param name = "B">The matrix to be multiplied with</param>
        /// <returns>A 2D double array that contains the product</returns>
        public static double[,] multiply(double a, double[,] B)
        {
            var c = new double[B.GetLength(0),B.GetLength(1)];
            for (var i = 0; i != B.GetLength(0); i++)
                for (var j = 0; j != B.GetLength(1); j++)
                    c[i, j] = a*B[i, j];
            return c;
        }

        /// <summary>
        ///   Divides all elements of a 2D double array by the double value.
        /// </summary>
        /// <param name = "B">The matrix to be divided</param>
        /// <param name = "a">The double value to be divided by, the divisor.</param>
        /// <returns>A 2D double array that contains the product</returns>
        public static double[,] divide(double[,] B, double a)
        {
            return multiply((1/a), B);
        }


        /// <summary>
        ///   The dot product of the two 1D double vectors A and B
        /// </summary>
        /// <param name = "A">1D double Array 1</param>
        /// <param name = "B">1D double Array 2</param>
        /// <returns>A double value that contains the dot product</returns>
        public static double multiplyDot(double[] A, double[] B)
        {
            // this is B dot term_i multiplication
            var size = A.GetLength(0);
            if (size != B.GetLength(0))
                throw new Exception("Matrix sizes do not match");
            var c = 0.0;
            for (var i = 0; i != size; i++)
                c += A[i]*B[i];
            return c;
        }

        /// <summary>
        ///   The cross product of the two 1D double vectors A and B
        /// </summary>
        /// <param name = "A">1D double Array 1</param>
        /// <param name = "B">1D double Array 2</param>
        /// <returns>A double value that contains the dot product</returns>
        public static double[] multiplyCross(double[] A, double[] B)
        {
            if ((A.GetLength(0) == 1) && (B.GetLength(0) == 1))
                return new[] {0.0};
            if ((A.GetLength(0) == 2) && (B.GetLength(0) == 2))
                return new[] {0.0, 0.0, multiplyCross2D(A, B)};
            if ((A.GetLength(0) == 3) && (B.GetLength(0) == 3))
                return multiplyCross3(A, B);
            if ((A.GetLength(0) == 7) && (B.GetLength(0) == 7))
                return multiplyCross7(A, B);
            throw new Exception("Cross product is only possible for vectors of length: 1, 3, or 7");
        }

        /// <summary>
        ///   The cross product of the two 1D double vectors A and B whic are of length, 2.
        /// </summary>
        /// <param name = "A">1D double Array 1</param>
        /// <param name = "B">1D double Array 2</param>
        /// <returns></returns>
        public static double multiplyCross2D(double[] A, double[] B)
        {
            if (((A.GetLength(0) == 2) && (B.GetLength(0) == 2))
                || ((A.GetLength(0) == 3) && (B.GetLength(0) == 3) && A[2] == 0.0 && B[2] == 0.0))
                return A[0]*B[1] - B[0]*A[1];
            throw new Exception("This cross product \"shortcut\" is only used with 2D vectors to get the single value in the,"
                                + "would be, Z-direction.");
        }

        private static double[] multiplyCross7(IList<double> A, IList<double> B)
        {
            return new[]
                       {
                           A[1]*B[3] - A[3]*B[1] + A[2]*B[6] - A[6]*B[2] + A[4]*B[5] - A[5]*B[4],
                           A[2]*B[4] - A[4]*B[2] + A[3]*B[0] - A[0]*B[3] + A[5]*B[6] - A[6]*B[5],
                           A[3]*B[5] - A[5]*B[3] + A[4]*B[1] - A[1]*B[4] + A[6]*B[0] - A[0]*B[6],
                           A[4]*B[6] - A[6]*B[4] + A[5]*B[2] - A[2]*B[5] + A[0]*B[1] - A[1]*B[0],
                           A[5]*B[0] - A[0]*B[5] + A[6]*B[3] - A[3]*B[6] + A[1]*B[2] - A[2]*B[1],
                           A[6]*B[1] - A[1]*B[6] + A[0]*B[4] - A[4]*B[0] + A[2]*B[3] - A[3]*B[2],
                           A[0]*B[2] - A[2]*B[0] + A[1]*B[5] - A[5]*B[1] + A[3]*B[4] - A[4]*B[3]
                       };
        }

        private static double[] multiplyCross3(IList<double> A, IList<double> B)
        {
            return new[]
                       {
                           A[1]*B[2] - B[1]*A[2],
                           A[2]*B[0] - B[2]*A[0],
                           A[0]*B[1] - B[0]*A[1]
                       };
        }

        /// <summary>
        ///   Product of each element of array-1 (1D double) with each element of array-2 (1D double)
        /// </summary>
        /// <param name = "A">1D double array - column vector (1 element per row)</param>
        /// <param name = "B">1D double array - row vector (1 element column)</param>
        /// <returns>2D double array product matrix, value of element [i,j] = A[i] * B[j]</returns>
        public static double[,] multiplyVectorsIntoAMatrix(double[] A, double[] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = B.GetLength(0);

            var C = new double[CRowSize,CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i]*B[j];
            return C;
        }

        /// <summary>
        ///   Product of two matrices (2D double)
        /// </summary>
        /// <param name = "A">2D double Array 1</param>
        /// <param name = "B">2D double Array 1</param>
        /// <returns>A 2D double array that is the product of the two matrices A and B</returns>
        public static double[,] multiply(double[,] A, double[,] B)
        {
            if (A.GetLength(1) != B.GetLength(0))
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");
            // this is B dot term_i multiplication
            var CRowSize = A.GetLength(0);
            var CColSize = B.GetLength(1);


            var C = new double[CRowSize,CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                {
                    C[i, j] = 0.0;
                    for (var k = 0; k != A.GetLength(1); k++)
                        C[i, j] += A[i, k]*B[k, j];
                }
            return C;
        }

        /// <summary>
        ///   Product of two matrices (2D double and 1D double)
        /// </summary>
        /// <param name = "A">2D double Array</param>
        /// <param name = "B">1D double array - column vector (1 element row)</param>
        /// <returns>A 1D double array that is the product of the two matrices A and B</returns>
        public static double[] multiply(double[,] A, double[] B)
        {
            // this is B dot term_i multiplication
            var ARowSize = A.GetLength(0);
            var AColSize = A.GetLength(1);
            if (AColSize != B.GetLength(0))
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");

            var C = new double[ARowSize];

            for (var i = 0; i != ARowSize; i++)
            {
                C[i] = 0.0;
                for (var j = 0; j != AColSize; j++)
                    C[i] += A[i, j]*B[j];
            }
            return C;
        }

        /// <summary>
        ///   Product of two matrices (1D double and 2D double)
        /// </summary>
        /// <param name = "A">1D double array - row vector (1 element column)</param>
        /// <param name = "B">2D double Array</param>
        /// <returns>A 1D double array that is the product of the two matrices A and B</returns>
        public static double[] multiply(double[] B, double[,] A)
        {
            // this is B dot term_i multiplication
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");

            var C = new double[CColSize];

            for (var i = 0; i != CColSize; i++)
            {
                C[i] = 0.0;
                for (var j = 0; j != CRowSize; j++)
                    C[i] += B[j]*A[j, i];
            }
            return C;
        }

        #endregion

        #region Doubles-by-ints and vice-versa

        /// <summary>
        ///   Multiplies all elements of a 2D int array with a double value.
        /// </summary>
        /// <param name = "a">The double value to be multiplied</param>
        /// <param name = "B">The matrix to be multiplied with</param>
        /// <returns>A 2D double array that contains the product</returns>
        public static double[,] multiply(double a, int[,] B)
        {
            var c = new double[B.GetLength(0),B.GetLength(1)];
            for (var i = 0; i != B.GetLength(0); i++)
                for (var j = 0; j != B.GetLength(1); j++)
                    c[i, j] = a*B[i, j];
            return c;
        }

        #endregion

        #endregion

        #region Add Vector-to-Vector and Matrix-to-Matrix

        /// <summary>
        ///   Adds arrays A and B
        /// </summary>
        /// <param name = "A">1D double array 1</param>
        /// <param name = "B">1D double array 2</param>
        /// <returns>1D double array that contains sum of vectros A and B</returns>
        public static double[] add(double[] A, double[] B)
        {
            // add vector A to vector B
            var size = A.GetLength(0);
            if (size != B.GetLength(0)) throw new Exception("Matrix sizes do not match");
            var c = new double[size];
            for (var i = 0; i != size; i++)
                c[i] = A[i] + B[i];
            return c;
        }

        /// <summary>
        ///   Adds arrays A and B
        /// </summary>
        /// <param name = "A">2D double array 1</param>
        /// <param name = "B">2D double array 2</param>
        /// <returns>2D double array that contains sum of vectros A and B</returns>
        public static double[,] add(double[,] A, double[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new double[CRowSize,CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }

        /// <summary>
        ///   Adds arrays A and B
        /// </summary>
        /// <param name = "A">2D double array 1</param>
        /// <param name = "B">2D int array 2</param>
        /// <returns>2D double array that contains sum of vectros A and B</returns>
        public static double[,] add(double[,] A, int[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new double[CRowSize,CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }

        /// <summary>
        ///   Adds arrays A and B
        /// </summary>
        /// <param name = "A">2D integer array 1</param>
        /// <param name = "B">2D integer array 2</param>
        /// <returns>2D integer array that contains sum of vectros A and B</returns>
        public static int[,] add(int[,] A, int[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new int[CRowSize,CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }

        #endregion

        #region Subtract Vector-to-Vector and Matrix-to-Matrix

        /// <summary>
        ///   Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend vector, A (1D double)</param>
        /// <param name = "B">The subtrahend vector, B (1D double)</param>
        /// <returns>Returns the difference vector, C (1D double)</returns>
        public static double[] subtract(double[] A, double[] B)
        {
            // add vector A to vector B
            var size = A.GetLength(0);
            if (size != B.GetLength(0))
                throw new Exception("Matrix sizes do not match");
            var c = new double[size];
            for (var i = 0; i != size; i++)
                c[i] = A[i] - B[i];
            return c;
        }

        /// <summary>
        ///   Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend matrix, A (2D double).</param>
        /// <param name = "B">The subtrahend matrix, B (2D double).</param>
        /// <returns>Returns the difference matrix, C (2D double)</returns>
        public static double[,] subtract(double[,] A, double[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new double[CRowSize,CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }

        /// <summary>
        ///   Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend matrix, A (2D integer).</param>
        /// <param name = "B">The subtrahend matrix, B (2D integer).</param>
        /// <returns>Returns the difference matrix, C (2D integer)</returns>
        public static int[,] subtract(int[,] A, int[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new int[CRowSize,CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }

        #endregion

        #region Sum

        /// <summary>
        ///   Sum up all the elements of a given matrix
        /// </summary>
        /// <param name = "B">Matrix (1D double) whose parameters need to be summed up</param>
        /// <returns>Returns the total (double) </returns>
        public static double sum(IEnumerable<double> B)
        {
            return B.Sum();
        }

        /// <summary>
        ///   Sum up all the elements of a given matrix
        /// </summary>
        /// <param name = "B">Matrix (1D int) whose parameters need to be summed up</param>
        /// <returns>Returns the total (int) </returns>
        public static double sum(IEnumerable<int> B)
        {
            return B.Sum();
        }

        /// <summary>
        ///   Sum up all the elements of a given matrix
        /// </summary>
        /// <param name = "B">Matrix (2D double) whose parameters need to be summed up</param>
        /// <returns>Returns the total (double) </returns>
        public static double sum(double[,] B)
        {
            var sum = 0.0;
            foreach (var element in B)
                sum = sum + element;
            return sum;
        }

        /// <summary>
        ///   Sum up all the elements of a given matrix
        /// </summary>
        /// <param name = "B">Matrix (2D double) whose parameters need to be summed up</param>
        /// <returns>Returns the total (int) </returns>
        public static double sum(int[,] B)
        {
            var sum = 0;
            foreach (var element in B)
                sum = sum + element;
            return sum;
        }

        #endregion

        #endregion
    }
}