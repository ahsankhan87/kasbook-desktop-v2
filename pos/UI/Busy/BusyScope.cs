using System;
using System.Windows.Forms;

namespace pos.UI.Busy
{
    /// <summary>
    /// Simple per-form busy indicator.
    /// Usage: using (BusyScope.Show(this, "Loading...")) { ... }
    /// </summary>
    public sealed class BusyScope : IDisposable
    {
        private readonly Form _owner;
        private readonly BusyForm _dlg;
        private readonly Control _previousActiveControl;
        private bool _disposed;

        private BusyScope(Form owner, BusyForm dlg, Control previousActiveControl)
        {
            _owner = owner;
            _dlg = dlg;
            _previousActiveControl = previousActiveControl;
        }

        public static BusyScope Show(Form owner, string message = "Loading…")
        {
            if (owner == null)
                owner = Form.ActiveForm;

            var prev = owner != null ? owner.ActiveControl : null;
            var dlg = new BusyForm(message);

            // IMPORTANT (MDI focus):
            // Disabling an MDI child can cause focus/activation to jump to the MDI parent/another child
            // (often the dashboard). So we DO NOT disable the owner.
            // Instead we show a non-activating owned tool window that sits on top of the current form.
            if (owner != null)
            {
                dlg.Show(owner);
                dlg.Location = new System.Drawing.Point(
                    owner.Left + (owner.Width - dlg.Width) / 2,
                    owner.Top + (owner.Height - dlg.Height) / 2);
            }
            else
            {
                dlg.Show();
            }

            return new BusyScope(owner, dlg, prev);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_dlg != null && !_dlg.IsDisposed)
            {
                _dlg.Close();
                _dlg.Dispose();
            }

            // Restore focus to the previous control (best effort)
            if (_owner != null && !_owner.IsDisposed)
            {
                try
                {
                    if (_previousActiveControl != null && !_previousActiveControl.IsDisposed)
                        _previousActiveControl.Focus();
                }
                catch
                {
                    // ignore
                }
            }
        }
    }
}
