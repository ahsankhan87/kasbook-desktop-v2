using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using pos;
using Microsoft.VSDiagnostics;

namespace pos.Benchmarks
{
    [CPUUsageDiagnoser]
    public class LoginStyleBenchmarks
    {
        private Login _form;
        private MethodInfo _styleMethod;
        [GlobalSetup]
        public void Setup()
        {
            _form = new Login();
            _styleMethod = typeof(Login).GetMethod("StyleLoginForm", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [Benchmark]
        public void ApplyLoginStyle()
        {
            _styleMethod?.Invoke(_form, null);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _form?.Dispose();
        }
    }
}