﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotNumerics;
using StarMathLib;
using DotNum = DotNumerics.LinearAlgebra;
using MathDot = MathNet.Numerics.LinearAlgebra;

namespace TestEXE_for_StarMath
{
    internal class Program
    {
        private static void Main()
        {
            // testStackFunctions();
            // testLUfunctions();
            //benchMarkMatrixInversion();
            //compareSolvers_Inversion_to_GaussSeidel();
            checkEigen();
            Console.WriteLine("Press any key to close.");
            Console.ReadLine();
        }

        private static void testStackFunctions()
        {
            var A = new[,] { { 0.1, 0.2, 0.3 }, { 1, 2, 3 }, { 10, 20, 30 }, { 100, 200, 300 } };
            int i, j;
            StarMath.Max(A, out i, out j);
            Console.WriteLine(StarMath.MakePrintString(StarMath.JoinMatrixColumnsIntoVector(A)));
        }

        private static void testLUfunctions()
        {
            const int size = 250;
            var r = new Random();

            var A = new double[size, size];
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    A[i, j] = (200 * r.NextDouble()) - 100.0;
            Console.WriteLine("A =");
            Console.WriteLine(StarMath.MakePrintString(A));

            Console.WriteLine("Combined LU = ");
            Console.WriteLine(StarMath.LUDecomposition(A));

            double[,] L, U;
            StarMath.LUDecomposition(A, out L, out U);
            Console.WriteLine(" L = ");
            Console.WriteLine(StarMath.MakePrintString(L));
            Console.WriteLine(" U = ");
            Console.WriteLine(StarMath.MakePrintString(U));

            Console.WriteLine("L * U =");
            Console.WriteLine(StarMath.MakePrintString(StarMath.multiply(L, U)));

            var E = StarMath.subtract(A, StarMath.multiply(L, U));
            var error = StarMath.norm2(E);
            Console.WriteLine("error = " + error);
        }

        private static void benchMarkMatrixInversion()
        {
            var watch = new Stopwatch();
            double error;
            var results = new List<List<string>>();
            results.Add(new List<string>
                            {
                                "","",
                                "ALGlib Err",
                                "ALGlib Time",
                                "Dot Numerics Err",
                                "Dot Numerics Time",
                                "Dot NumericsClass Err",
                                "Dot NumericsClass Time",
                                "Math.Net Err",
                                "Math.Net Numerics Time",
                                "Math.NetClass Err",
                                "Math.NetClass Time",
                                "StArMath Err",
                                "StArMath Time"
                            });
            var r = new Random();

            var limits = new int[,] {{3,10,30,100,300,1000,3000},
            {50,50,30,30,20,10,3}};
            for (var index = 0; index < limits.GetLength(1); index++)
            {
                int size = limits[0, index];
                int numTrials = limits[1, index];

                for (var k = 0; k <= numTrials; k++)
                {
                    var A = new double[size, size];
                    for (var i = 0; i < size; i++)
                        for (var j = 0; j < size; j++)
                            A[i, j] = (200 * r.NextDouble()) - 100.0;
                    var result = new List<string> { size.ToString(), k.ToString() };

                    #region ALGlib

                    Console.WriteLine("\n\n\nALGlib: start invert check for matrix of size: " + size);

                    int info;
                    alglib.matinvreport rep;
                    watch.Restart();
                    var B = (double[,])A.Clone();
                    alglib.rmatrixinverse(ref B, out info, out rep);
                    watch.Stop();
                    recordResults(result, A, B, watch, k);

                    #endregion

                    #region Dot Numerics

                    Console.WriteLine("\n\n\nDot Numerics: start invert check for matrix of size: " + size);

                    var A_DN = new DotNum.Matrix(A);
                    watch.Restart();
                    var B_DN = A_DN.Inverse();
                    watch.Stop();
                    recordResults(result, A, B_DN.CopyToArray(), watch, k);

                    #endregion
                    #region Dot Numerics

                    Console.WriteLine("\n\n\nDot Numerics: start invert check for matrix of size: " + size);
                    watch.Restart();
                    A_DN = new DotNum.Matrix(A);
                    B_DN = A_DN.Inverse();
                    watch.Stop();
                    recordResults(result, A, B_DN.CopyToArray(), watch, k);

                    #endregion


                    #region Math.Net

                    Console.WriteLine("\n\n\nMath.Net: start invert check for matrix of size: " + size);

                    var A_MD = MathDot.Matrix.Create(A);
                    watch.Restart();
                    var B_MD = A_MD.Inverse();
                    watch.Stop();
                    recordResults(result, A, B_MD.CopyToArray(), watch, k);

                    #endregion

                    #region Math.Net

                    Console.WriteLine("\n\n\nMath.Net: start invert check for matrix of size: " + size);

                    watch.Restart();
                    A_MD = MathDot.Matrix.Create(A);
                    B_MD = A_MD.Inverse();
                    watch.Stop();

                    recordResults(result, A, B_MD.CopyToArray(), watch, k);

                    #endregion

                    #region StarMath

                    Console.WriteLine("\n\n\nSTARMATH: start invert check for matrix of size: " + size);

                    watch.Restart();
                    B = StarMath.inverse(A);
                    watch.Stop();
                    recordResults(result, A, B, watch, k);
                    #endregion
                    results.Add(result);
                }
            }
            SaveResultsToCSV("results.csv", results);


        }


        private static void checkEigen()
        {
            var r = new Random();
            var size = 44;
            var A = new double[size, size];
            for (var i = 0; i < size; i++)
                for (var j = i; j < size; j++)
                    A[i, j] = A[j, i] = (200 * r.NextDouble()) - 100.0;
            var eigenVectors = new double[size][];
            var λ = StarMath.GetEigenValuesAndVectors(A, out eigenVectors);
            //Console.WriteLine(StarMath.MakePrintString(ans[0]));
            for (int i = 0; i < size; i++)
            {
                var lhs = StarMath.multiply(A, eigenVectors[i]);
                var rhs = StarMath.multiply(λ[0][i], eigenVectors[i]);
                Console.WriteLine(StarMath.norm1(StarMath.subtract(lhs, rhs)));
            }
        }
        private static void compareSolvers_Inversion_to_GaussSeidel()
        {
            var watch = new Stopwatch();
            double error;
            var results = new List<List<string>>();

            var r = new Random();
            var fractionZeros = new double[] { 0.0, 0.3, 0.5, 0.8, 0.9, 0.95 };
            var matrixSize = new int[] { 10, 30, 100, 300, 800 };
            for (var i = 0; i < matrixSize.GetLength(0); i++)
            {
                for (int j = 0; j < fractionZeros.GetLength(0); j++)
                {
                    int size = matrixSize[i];
                    int numTrials = 10;
                    int numZeros = (int)(size * size * fractionZeros[j]);
                    for (var k = 0; k <= numTrials; k++)
                    {
                        var A = new double[size, size];
                        var b = new double[size];
                        for (var ii = 0; ii < size; ii++)
                        {
                            b[ii] = (200 * r.NextDouble()) - 100.0;
                            for (var jj = 0; jj < size; jj++)
                                A[ii, jj] = (200 * r.NextDouble()) - 100.0;
                        }
                        for (int l = 0; l < numZeros; l++)
                            A[r.Next(size), r.Next(size)] = 0.0;
                        var result = new List<string> { k.ToString(), size.ToString(), numZeros.ToString() };

                        watch.Restart();
                        /*
                        var x = StarMath.solveByInverse(A, b);
                        watch.Stop();
                        recordResults(result, A, x, b, watch);
                        watch.Restart();
                        x = StarMath.solveGaussSeidel(A, b);
                        watch.Stop();
                        recordResults(result, A, x, b, watch);


                        results.Add(result);
                         */
                    }
                }
            }
            SaveResultsToCSV("results.csv", results);
        }

        private static void recordResults(List<string> result, double[,] A, double[] x, double[] b, Stopwatch watch)
        {
            var error = StarMath.norm1(StarMath.subtract(b, StarMath.multiply(A, x))) / StarMath.norm1(b);
            result.Add(error.ToString());
            result.Add(watch.Elapsed.TotalMilliseconds.ToString());
        }

        private static void recordResults(List<string> result, double[,] A, double[,] invA, Stopwatch watch, int k)
        {
            if (k == 0) return; //it seems that the first time you call a new function there may be a delay. This is especially
            // true if the function is in another dll.
            var C = StarMath.subtract(StarMath.multiply(A, invA), StarMath.makeIdentity(A.GetLength(0)));
            var error = StarMath.norm2(C);
            Console.WriteLine("end invert, error = " + error);
            Console.WriteLine("time = " + watch.Elapsed);
            result.Add(error.ToString());
            result.Add(watch.Elapsed.TotalMilliseconds.ToString());
        }

        private static void SaveResultsToCSV(string path, List<List<string>> results)
        {
            var fs = new FileStream(path, FileMode.Create);
            var r = new StreamWriter(fs);
            foreach (var list in results)
            {
                string line = "";
                foreach (var s in list)
                    line += s + ",";
                line.Trim(',');
                r.WriteLine(line);
            }
            r.Close();
            fs.Close();
        }

    }
}

