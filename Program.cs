
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Running;

public static class Program
{
    public static void Main(string[] args) 
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}