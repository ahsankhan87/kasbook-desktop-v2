using System;
using System.Threading;
using BenchmarkDotNet.Attributes;
using POS.Core;
using pos;
using Microsoft.VSDiagnostics;

namespace pos.Benchmarks
{
    [CPUUsageDiagnoser]
    public class SalesFormInitBenchmarks
    {
        [GlobalSetup]
        public void Setup()
        {
            if (string.IsNullOrEmpty(UsersModal.logged_in_lang))
            {
                UsersModal.logged_in_lang = "en-US";
            }
        }

        [Benchmark]
        public void CreateSalesForm()
        {
            RunOnSta(() =>
            {
                using (var form = new frm_sales())
                {
                }
            });
        }

        private static void RunOnSta(Action action)
        {
            Exception error = null;
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            if (error != null)
            {
                throw new InvalidOperationException("STA benchmark invocation failed.", error);
            }
        }
    }
}