using System;
using System.Diagnostics;
using System.Windows.Forms;
using POS.Core.Accounts;
using pos.UI;

namespace pos.Accounts
{
    /// <summary>
    /// Progress dialog for import operations with cancel support
    /// </summary>
    public partial class ImportProgressForm : Form
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private bool _cancellationRequested;
        private int _totalRows;
        private int _processedRows;

        public bool CancellationRequested => _cancellationRequested;

        public ImportProgressForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Start the progress tracking
        /// </summary>
        public void StartProgress(string operation = "Processing import data...")
        {
            _stopwatch.Start();
            _cancellationRequested = false;
            btnCancel.Enabled = true;

            lblOperation.Text = operation;
            lblProgress.Text = "0%";
            lblElapsed.Text = "Elapsed: 00:00";
            lblEstimated.Text = "Remaining: --:--";
            progressBar.Value = 0;

            Application.DoEvents();
        }

        /// <summary>
        /// Update progress with current status
        /// </summary>
        public void UpdateProgress(ImportProgressEventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ImportProgressEventArgs>(UpdateProgress), args);
                return;
            }

            _totalRows = args.TotalRows;
            _processedRows = args.ProcessedRows;

            // Update operation text
            lblOperation.Text = args.CurrentOperation;

            // Update progress percentage
            int percent = args.PercentComplete;
            if (percent < 0) percent = 0;
            if (percent > 100) percent = 100;

            progressBar.Value = percent;
            lblProgress.Text = $"{percent}%";

            // Update elapsed time
            var elapsed = args.Elapsed;
            lblElapsed.Text = $"Elapsed: {elapsed.Minutes:D2}:{elapsed.Seconds:D2}";

            // Update estimated time
            if (args.Estimated.HasValue && args.Estimated.Value.TotalSeconds > 0)
            {
                var estimated = args.Estimated.Value;
                lblEstimated.Text = $"Remaining: {estimated.Minutes:D2}:{estimated.Seconds:D2}";
            }
            else
            {
                lblEstimated.Text = "Remaining: --:--";
            }

            // Enable/disable cancel button
            btnCancel.Enabled = args.CanCancel;

            Application.DoEvents();
        }

        /// <summary>
        /// Update progress with simple parameters
        /// </summary>
        public void UpdateProgress(int processedRows, int totalRows, string operation = null)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int, int, string>(UpdateProgress), processedRows, totalRows, operation);
                return;
            }

            _totalRows = totalRows;
            _processedRows = processedRows;

            if (!string.IsNullOrWhiteSpace(operation))
            {
                lblOperation.Text = operation;
            }

            int percent = totalRows > 0 ? (int)((double)processedRows / totalRows * 100) : 0;
            progressBar.Value = Math.Min(percent, 100);
            lblProgress.Text = $"{percent}%";

            var elapsed = _stopwatch.Elapsed;
            lblElapsed.Text = $"Elapsed: {elapsed.Minutes:D2}:{elapsed.Seconds:D2}";

            // Calculate estimated time
            if (processedRows > 0 && totalRows > processedRows)
            {
                var avgTimePerRow = elapsed.TotalSeconds / processedRows;
                var remainingRows = totalRows - processedRows;
                var estimatedSeconds = avgTimePerRow * remainingRows;
                var estimated = TimeSpan.FromSeconds(estimatedSeconds);

                lblEstimated.Text = $"Remaining: {estimated.Minutes:D2}:{estimated.Seconds:D2}";
            }
            else
            {
                lblEstimated.Text = "Remaining: --:--";
            }

            Application.DoEvents();
        }

        /// <summary>
        /// Complete the progress
        /// </summary>
        public void CompleteProgress(bool success, string message = null)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool, string>(CompleteProgress), success, message);
                return;
            }

            _stopwatch.Stop();

            progressBar.Value = 100;
            lblProgress.Text = "100%";

            if (!string.IsNullOrWhiteSpace(message))
            {
                lblOperation.Text = message;
            }
            else
            {
                lblOperation.Text = success ? "Import completed successfully!" : "Import failed";
            }

            btnCancel.Enabled = false;

            Application.DoEvents();
        }

        /// <summary>
        /// Handle cancel button click
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Are you sure you want to cancel the import?\n\nAll progress will be rolled back.",
                "Confirm Cancel",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _cancellationRequested = true;
                btnCancel.Enabled = false;
                lblOperation.Text = "Cancelling import...";
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Show progress form modeless
        /// </summary>
        public void ShowProgress()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ShowProgress));
                return;
            }

            Show();
            Application.DoEvents();
        }

        /// <summary>
        /// Close progress form
        /// </summary>
        public void CloseProgress()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(CloseProgress));
                return;
            }

            _stopwatch.Stop();
            Close();
        }

        /// <summary>
        /// Set operation text
        /// </summary>
        public void SetOperation(string operation)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SetOperation), operation);
                return;
            }

            lblOperation.Text = operation;
            Application.DoEvents();
        }

        /// <summary>
        /// Get elapsed time
        /// </summary>
        public TimeSpan GetElapsedTime()
        {
            return _stopwatch.Elapsed;
        }

        /// <summary>
        /// Check if operation should be cancelled
        /// </summary>
        public bool ShouldCancel()
        {
            Application.DoEvents();
            return _cancellationRequested;
        }
    }
}
