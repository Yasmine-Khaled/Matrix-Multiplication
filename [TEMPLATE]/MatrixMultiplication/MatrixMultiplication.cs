using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public static class MatrixMultiplication
    {
        #region YOUR CODE IS HERE

        static public int[,] MatrixSubtraction(int[,] M1, int[,] M2, int N)
        {
            int[,] res = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    res[i, j] = M1[i, j] - M2[i, j];
                }

            }
            return res;
        }
        static public int[,] MatrixAddition(int[,] M1, int[,] M2, int N)
        {
            int[,] res = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    res[i, j] = M1[i, j] + M2[i, j];
                }

            }
            return res;
        }
        public static int[,] NaiveMatrixMultiply(int[,] M1, int[,] M2, int n, int m, int p)
        {

            int[,] res = new int[n, p];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < m; k++)
                    {
                        sum += M1[i, k] * M2[k, j];
                    }
                    res[i, j] = sum;
                }
            }

            return res;
        }

        static public int[,] MatrixMultiply(int[,] M1, int[,] M2, int N)
        {

            if (N == 2)
            {
                int[,] result = new int[2, 2];

                result[0, 0] = (M1[0, 0] * M2[0, 0]) + (M1[0, 1] * M2[1, 0]);
                result[0, 1] = (M1[0, 0] * M2[0, 1]) + (M1[0, 1] * M2[1, 1]);
                result[1, 0] = (M1[1, 0] * M2[0, 0]) + (M1[1, 1] * M2[1, 0]);
                result[1, 1] = (M1[1, 0] * M2[0, 1]) + (M1[1, 1] * M2[1, 1]);

                return result;
            }

            else if (M1.GetLength(0) <= 64 || M1.GetLength(1) <= 64 || M2.GetLength(1) <= 64)
            {
                int n = M1.GetLength(0);
                int m = M1.GetLength(1);
                int p = M2.GetLength(1);

                return NaiveMatrixMultiply(M1, M2, n, m, p);
            }

            else
            {

                int[,] res = new int[N, N];
                //M1
                int[,] Y1 = new int[N / 2, N / 2];
                int[,] Y2 = new int[N / 2, N / 2];
                int[,] Y3 = new int[N / 2, N / 2];
                int[,] Y4 = new int[N / 2, N / 2];
                //M2
                int[,] Z1 = new int[N / 2, N / 2];
                int[,] Z2 = new int[N / 2, N / 2];
                int[,] Z3 = new int[N / 2, N / 2];
                int[,] Z4 = new int[N / 2, N / 2];

                for (int i = 0; i < N / 2; i++)
                {
                    for (int j = 0; j < N / 2; j++)
                    {
                        Y1[i, j] = M1[i, j];
                        Y2[i, j] = M1[i, j + (N / 2)];
                        Y3[i, j] = M1[i + (N / 2), j];
                        Y4[i, j] = M1[i + (N / 2), j + (N / 2)];
                        Z1[i, j] = M2[i, j];
                        Z2[i, j] = M2[i, j + (N / 2)];
                        Z3[i, j] = M2[i + (N / 2), j];
                        Z4[i, j] = M2[i + (N / 2), j + (N / 2)];
                    }

                }

                int[,] ret1 = MatrixAddition(Y1, Y4, N / 2);
                int[,] ret2 = MatrixAddition(Z1, Z4, N / 2);
                int[,] ret3 = MatrixAddition(Y3, Y4, N / 2);
                int[,] ret4 = MatrixSubtraction(Z2, Z4, N / 2);
                int[,] ret5 = MatrixSubtraction(Z3, Z1, N / 2);
                int[,] ret6 = MatrixAddition(Y1, Y2, N / 2);
                int[,] ret7 = MatrixSubtraction(Y3, Y1, N / 2);
                int[,] ret8 = MatrixAddition(Z1, Z2, N / 2);
                int[,] ret9 = MatrixSubtraction(Y2, Y4, N / 2);
                int[,] ret10 = MatrixAddition(Z3, Z4, N / 2);

                int[,] R1 = new int[N / 2, N / 2];
                int[,] R2 = new int[N / 2, N / 2];
                int[,] R3 = new int[N / 2, N / 2];
                int[,] R4 = new int[N / 2, N / 2];
                int[,] R5 = new int[N / 2, N / 2];
                int[,] R6 = new int[N / 2, N / 2];
                int[,] R7 = new int[N / 2, N / 2];

                Task t1 = Task.Run(() => {
                    R1 = MatrixMultiply(ret1, ret2, N / 2);
                });
                Task t2 = Task.Run(() => {
                    R2 = MatrixMultiply(ret3, Z1, N / 2);
                });
                Task t3 = Task.Run(() => {
                    R3 = MatrixMultiply(Y1, ret4, N / 2);
                });
                Task t4 = Task.Run(() => {
                    R4 = MatrixMultiply(Y4, ret5, N / 2);
                });

                Task t5 = Task.Run(() => {
                    R5 = MatrixMultiply(ret6, Z4, N / 2);
                });
                Task t6 = Task.Run(() => {
                    R6 = MatrixMultiply(ret7, ret8, N / 2);
                });
                Task t7 = Task.Run(() => {
                    R7 = MatrixMultiply(ret9, ret10, N / 2);
                });
                Task.WaitAll(t1, t2, t3, t4, t5, t6, t7);



                for (int i = 0; i < N / 2; i++)
                {
                    for (int j = 0; j < N / 2; j++)
                    {
                        res[i, j] = R1[i, j] + R4[i, j] - R5[i, j] + R7[i, j];
                        res[i, j + (N / 2)] = R3[i, j] + R5[i, j];
                        res[i + (N / 2), j] = R2[i, j] + R4[i, j];
                        res[i + (N / 2), j + (N / 2)] = R1[i, j] - R2[i, j] + R3[i, j] + R6[i, j];

                    }
                }

                return res;
            }
        }

        #endregion
    }
}
