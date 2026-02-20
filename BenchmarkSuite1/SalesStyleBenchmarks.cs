using System;
using System.Reflection;
using System.Threading;
using BenchmarkDotNet.Attributes;
using POS.Core;
using pos;
using Microsoft.VSDiagnostics;

namespace pos.Benchmarks
{
    [CPUUsageDiagnoser]
    public class SalesStyleBenchmarks
    {
        private MethodInfo _styleMethod;
        [GlobalSetup]
        public void Setup()
        {
            if (string.IsNullOrEmpty(UsersModal.logged_in_lang))
            {
                UsersModal.logged_in_lang = "en-US";
            }
            _styleMethod = typeof(frm_sales).GetMethod("StyleSalesForm", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [Benchmark]
        public void ApplySalesStyle()
        {
            RunOnSta(() =>
            {
                using (var form = new frm_sales())
                {
                    _styleMethod?.Invoke(form, null);
                }
            });
        }

        [GlobalCleanup]
        public void Cleanup()
        {
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