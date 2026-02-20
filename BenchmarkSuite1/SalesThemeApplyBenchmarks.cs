using System;
using System.Reflection;
using System.Threading;
using BenchmarkDotNet.Attributes;
using POS.Core;
using pos;
using pos.UI;
using Microsoft.VSDiagnostics;

namespace pos.Benchmarks
{
    [CPUUsageDiagnoser]
    public class SalesThemeApplyBenchmarks
    {
        private MethodInfo _applyMethod;
        [GlobalSetup]
        public void Setup()
        {
            if (string.IsNullOrEmpty(UsersModal.logged_in_lang))
            {
                UsersModal.logged_in_lang = "en-US";
            }

            var appThemeType = typeof(frm_sales).Assembly.GetType("pos.UI.AppTheme", throwOnError: false);
            _applyMethod = appThemeType?.GetMethod("Apply", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        }

        [Benchmark]
        public void ApplySalesTheme()
        {
            RunOnSta(() =>
            {
                using (var form = new frm_sales())
                {
                    _applyMethod?.Invoke(null, new object[] { form });
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