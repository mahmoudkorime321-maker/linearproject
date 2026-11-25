using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class GaussianSolver
    {
        public static List<MatrixStep> Solve(double[,] inputMatrix, bool toRref = true)
        {
            var steps = new List<MatrixStep>();
            int rows = inputMatrix.GetLength(0);
            int cols = inputMatrix.GetLength(1);

            double[,] matrix = new double[rows, cols];
            Array.Copy(inputMatrix, matrix, inputMatrix.Length);

            steps.Add(new MatrixStep(matrix, "Initial matrix as entered."));

            int lead = 0;
            for (int r = 0; r < rows; r++)
            {
                if (cols <= lead) break;

                int i = r;
                while (Math.Abs(matrix[i, lead]) < 1e-10)
                {
                    i++;
                    if (rows == i)
                    {
                        i = r;
                        lead++;
                        if (cols == lead)
                        {
                            string earlyForm = toRref ? "RREF" : "REF";
                            string earlyStatus = AnalyzeSolution(matrix);
                            steps.Add(new MatrixStep(matrix, $"Reached {earlyForm} — {earlyStatus}"));
                            return steps;
                        }
                    }
                }

                // 1. Swapping rows
                if (i != r)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        double temp = matrix[r, j];
                        matrix[r, j] = matrix[i, j];
                        matrix[i, j] = temp;
                    }
                    steps.Add(new MatrixStep(matrix, $"Swapped Row {r + 1} with Row {i + 1}", r, lead));
                }

                // 2. Scaling pivot row so pivot = 1
                double div = matrix[r, lead];
                if (Math.Abs(div - 1.0) > 1e-10 && Math.Abs(div) > 1e-10)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        matrix[r, j] /= div;
                        if (Math.Abs(matrix[r, j]) < 1e-10) matrix[r, j] = 0;
                    }
                    steps.Add(new MatrixStep(matrix, $"Divided Row {r + 1} by {div:0.##} to make pivot = 1", r, lead));
                }

                // 3. Eliminating entries in the pivot column
                int startRow = toRref ? 0 : r + 1;
                for (int j = startRow; j < rows; j++)
                {
                    if (j != r)
                    {
                        double mult = matrix[j, lead];
                        if (Math.Abs(mult) > 1e-10)
                        {
                            for (int k = 0; k < cols; k++)
                            {
                                matrix[j, k] -= mult * matrix[r, k];
                                if (Math.Abs(matrix[j, k]) < 1e-10) matrix[j, k] = 0;
                            }
                            steps.Add(new MatrixStep(matrix, $"Row {j + 1} = Row {j + 1} - ({mult:0.##}) × Row {r + 1}", j, lead));
                        }
                    }
                }

                lead++;
            }

            string formName = toRref ? "RREF" : "REF";
            string solutionStatus = AnalyzeSolution(matrix);
            steps.Add(new MatrixStep(matrix, $"Reached {formName} — {solutionStatus}"));

            return steps;
        }

        private static string AnalyzeSolution(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (cols < 2)
                return "Done.";

            int numVars = cols - 1; // Last column is the constants column (b)
            int pivotCount = 0;

            for (int r = 0; r < rows; r++)
            {
                bool allCoeffsZero = true;
                for (int c = 0; c < numVars; c++)
                {
                    if (Math.Abs(matrix[r, c]) > 1e-10)
                    {
                        allCoeffsZero = false;
                        break;
                    }
                }

                if (allCoeffsZero)
                {
                    if (Math.Abs(matrix[r, cols - 1]) > 1e-10)
                        return "No Solution (Inconsistent system: 0 = non-zero).";
                }
                else
                {
                    pivotCount++;
                }
            }

            if (pivotCount < numVars)
                return $"Infinite Solutions ({numVars - pivotCount} free variable(s)).";

            return "Unique Solution.";
        }
    }
}