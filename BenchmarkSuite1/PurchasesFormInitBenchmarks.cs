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
    public class PurchasesFormInitBenchmarks
    {
        private MethodInfo _styleMethod;
        private MethodInfo _applyMethod;
        [GlobalSetup]
        public void Setup()
        {
            if (string.IsNullOrEmpty(UsersModal.logged_in_lang))
            {
                UsersModal.logged_in_lang = "en-US";
            }

            _styleMethod = typeof(frm_purchases).GetMethod("StylePurchasesForm", BindingFlags.Instance | BindingFlags.NonPublic);
            var appThemeType = typeof(frm_purchases).Assembly.GetType("pos.UI.AppTheme", throwOnError: false);
            _applyMethod = appThemeType?.GetMethod("Apply", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        }

        [Benchmark]
        public void CreatePurchasesForm()
        {
            RunOnSta(() =>
            {
                using (var form = new frm_purchases())
                {
                }
            });
        }

        [Benchmark]
        public void ApplyPurchasesStyle()
        {
            RunOnSta(() =>
            {
                using (var form = new frm_purchases())
                {
                    _styleMethod?.Invoke(form, null);
                }
            });
        }

        [Benchmark]
        public void ApplyPurchasesTheme()
        {
            RunOnSta(() =>
            {
                using (var form = new frm_purchases())
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