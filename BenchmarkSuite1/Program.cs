using System;
using System.IO;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace BenchmarkSuite1
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var artifactsPath = Path.Combine(Path.GetTempPath(), "BenchmarkDotNet.Artifacts");
            var config = DefaultConfig.Instance.WithArtifactsPath(artifactsPath);
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
        }
    }
}
