using System;
using System.Drawing;
using System.Windows.Forms;

namespace pos.UI.Busy
{
    internal sealed class BusyForm : Form
    {
        private readonly Label _lbl;
        private readonly ProgressBar _bar;

        public BusyForm(string message)
        {
            Text = "Please wait";
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ControlBox = false;

            ClientSize = new Size(360, 120);

            _lbl = new Label
            {
                Dock = DockStyle.Top,
                Height = 55,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = string.IsNullOrWhiteSpace(message) ? "Loading…" : message
            };

            _bar = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 18,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30
            };

            var pad = new Panel { Dock = DockStyle.Fill };

            Controls.Add(pad);
            Controls.Add(_bar);
            Controls.Add(_lbl);
        }

        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_TOOLWINDOW = 0x00000080;
                const int WS_EX_NOACTIVATE = 0x08000000;

                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE;
                return cp;
            }
        }

        public void SetMessage(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SetMessage(message)));
                return;
            }

            _lbl.Text = string.IsNullOrWhiteSpace(message) ? "Loading…" : message;
        }
    }
}
