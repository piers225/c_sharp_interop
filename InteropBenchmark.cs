using System.Numerics;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

[DisassemblyDiagnoser]
public partial class InteropBenchMark
{
    private const int MatrixSize = 200; // Adjust for more complexity
    private double[] A, B, C;

    [GlobalSetup]
    public void Setup()
    {
        Random rand = new Random();
        A = new double[MatrixSize * MatrixSize];
        B = new double[MatrixSize * MatrixSize];
        C = new double[MatrixSize * MatrixSize];

        for (int i = 0; i < A.Length; i++)
        {
            A[i] = rand.NextDouble();
            B[i] = rand.NextDouble();
        }
    }

    [DllImport("/app/src/libmatrix_operations.so", CallingConvention = CallingConvention.Cdecl)]
    public static extern void MultiplyMatrices(int n, double[] A, double[] B, double[] C);

    [Benchmark]
    public void CSharpMatrixMultiplication()
    {
        for (int i = 0; i < MatrixSize; i++)
        {
            for (int j = 0; j < MatrixSize; j++)
            {
                C[i * MatrixSize + j] = 0;
                for (int k = 0; k < MatrixSize; k++)
                {
                    C[i * MatrixSize + j] += A[i * MatrixSize + k] * B[k * MatrixSize + j];
                }
            }
        }
    }

    [Benchmark]
    public void CMatrixMultiplication()
    {
        MultiplyMatrices(MatrixSize, A, B, C);
    }

    [Benchmark]
    public void CSharpMatrixMultiplicationSIMD()
    {
        int vectorSize = Vector<double>.Count;
        for (int i = 0; i < MatrixSize; i++)
        {
            for (int j = 0; j < MatrixSize; j++)
            {
                Vector<double> sumVector = Vector<double>.Zero;
                int k = 0;
                
                for (; k <= MatrixSize - vectorSize; k += vectorSize)
                {
                    var va = new Vector<double>(A, i * MatrixSize + k);
                    var vb = new Vector<double>(B, k * MatrixSize + j);
                    sumVector += va * vb;
                }

                double sum = Vector.Dot(sumVector, Vector<double>.One);

                for (; k < MatrixSize; k++)
                {
                    sum += A[i * MatrixSize + k] * B[k * MatrixSize + j];
                }

                C[i * MatrixSize + j] = sum;
            }
        }
    }
}