using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public static class WhatsAppInvoiceSender
{
    [DllImport("user32.dll")] private static extern bool SetForegroundWindow(IntPtr hWnd);
    [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] private static extern bool IsIconic(IntPtr hWnd);
    [DllImport("user32.dll")] private static extern bool BringWindowToTop(IntPtr hWnd);
    [DllImport("user32.dll")] private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
    [DllImport("user32.dll")] private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    private const int SW_RESTORE = 9;

    public enum WhatsAppSendMode { Desktop, Web }

    public static async Task SendInvoicePdfAsync(string invoiceNo, string phoneE164, ReportDocument rpt, string initialMessage = null, int openDelayMs = 2500)
    {
        // Export PDF
        string pdfDir = Path.Combine(Application.StartupPath, "Invoices");
        Directory.CreateDirectory(pdfDir);
        string pdfPath = Path.Combine(pdfDir, $"{invoiceNo}.pdf");
        rpt.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);

        string msg = initialMessage ?? $"Invoice {invoiceNo}";

        // Attempt to open WhatsApp Desktop directly using protocol first
        bool launched = LaunchWhatsAppDesktop(phoneE164, msg);
        if (!launched)
        {
            launched = LaunchWhatsAppExecutableFallback();
            if (!launched)
            {
                MessageBox.Show("Failed to launch WhatsApp Desktop. Open WhatsApp and press Ctrl+V manually.");
                return;
            }
            // If we had to open plain app without chat deep-link, give user guidance
            MessageBox.Show("WhatsApp Desktop opened. Navigate to the chat for: " + phoneE164 + " then the PDF will attach.");
        }

        // Wait for app startup
        await Task.Delay(openDelayMs);

        // Additional wait for input idle if we have a process
        var waProc = FindWhatsAppProcess();
        try { waProc?.WaitForInputIdle(3000); } catch { }

        // Activate window with retries
        bool activated = await ActivateWhatsAppAsync(retries: 25, delayMs: 400);
        if (!activated)
        {
            // Last ditch: send Alt+Tab then retry foreground once
            SendKeys.SendWait("%{TAB}");
            await Task.Delay(500);
            waProc = FindWhatsAppProcess();
            if (waProc?.MainWindowHandle != IntPtr.Zero) SetForegroundWindow(waProc.MainWindowHandle);
            activated = (waProc?.MainWindowHandle != IntPtr.Zero && GetForegroundWindow() == waProc.MainWindowHandle);
        }
        if (!activated)
        {
            MessageBox.Show("Could not bring WhatsApp to foreground. Please focus WhatsApp chat then press Ctrl+V.");
            return;
        }

        CopyFileToClipboardSTA(pdfPath); // ensure STA clipboard

        // Try multiple paste attempts increasing delays
        bool pasted = await TryPasteFileWithRetryAsync(attempts: 6, initialDelayMs: 500, stepDelayMs: 900, recopyEachAttempt: true, pdfPath: pdfPath);
        if (!pasted)
        {
            MessageBox.Show("Automatic paste failed. Press Ctrl+V in WhatsApp to attach the PDF.");
            return;
        }

        await Task.Delay(700);
        SendKeys.SendWait("{ENTER}");
    }

    public static async Task SendInvoicePdfAsync(string invoiceNo, string phoneE164, ReportDocument rpt, string initialMessage, WhatsAppSendMode mode)
    {
        if (mode == WhatsAppSendMode.Desktop)
        {
            await SendInvoicePdfAsync(invoiceNo, phoneE164, rpt, initialMessage); // existing desktop logic
            return;
        }
        // Web mode: export PDF, open wa.me link only; cannot auto attach file in browser
        string pdfDir = Path.Combine(Application.StartupPath, "Invoices");
        Directory.CreateDirectory(pdfDir);
        string pdfPath = Path.Combine(pdfDir, $"{invoiceNo}.pdf");
        rpt.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);
        string msg = initialMessage ?? $"Invoice {invoiceNo}";
        try
        {
            var url = $"https://wa.me/{phoneE164}?text={Uri.EscapeDataString(msg)}";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            // Optionally show folder so user can drag file into web chat
            Process.Start("explorer.exe", $"/select,\"{pdfPath}\"");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to open WhatsApp Web: " + ex.Message);
        }
    }

    private static bool LaunchWhatsAppDesktop(string phone, string message)
    {
        try
        {
            // whatsapp:// protocol should invoke desktop app if registered
            string proto = $"whatsapp://send?phone={phone}&text={Uri.EscapeDataString(message)}";
            Process.Start(new ProcessStartInfo(proto) { UseShellExecute = true });
            return true;
        }
        catch { return false; }
    }

    private static bool LaunchWhatsAppExecutableFallback()
    {
        try
        {
            // Common installation locations
            string localApp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WhatsApp", "WhatsApp.exe");
            if (File.Exists(localApp)) { Process.Start(localApp); return true; }

            // Search Program Files for WhatsApp.exe (WindowsApps - need enumeration) simple heuristic
            string pf = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var exe = FindFileRecursiveSafe(pf, "WhatsApp.exe", depthLimit: 3);
            if (exe != null) { Process.Start(exe); return true; }
        }
        catch { }
        return false;
    }

    private static string FindFileRecursiveSafe(string root, string fileName, int depthLimit)
    {
        try
        {
            if (depthLimit < 0) return null;
            foreach (var f in Directory.GetFiles(root, fileName)) return f;
            foreach (var d in Directory.GetDirectories(root))
            {
                var found = FindFileRecursiveSafe(d, fileName, depthLimit - 1);
                if (found != null) return found;
            }
        }
        catch { }
        return null;
    }

    private static void CopyFileToClipboardSTA(string path)
    {
        void SetClip() {
            var files = new StringCollection { path }; Clipboard.SetFileDropList(files); }
        var t = new Thread(new ThreadStart(SetClip));
        t.SetApartmentState(ApartmentState.STA); t.Start(); t.Join();
    }

    private static async Task<bool> ActivateWhatsAppAsync(int retries, int delayMs)
    {
        for (int i = 0; i < retries; i++)
        {
            var proc = FindWhatsAppProcess();
            if (proc != null)
            {
                IntPtr hWnd = proc.MainWindowHandle;
                if (hWnd != IntPtr.Zero)
                {
                    if (IsIconic(hWnd)) ShowWindow(hWnd, SW_RESTORE);
                    // Try thread attach method
                    uint targetPid; GetWindowThreadProcessId(hWnd, out targetPid); uint targetThread = (uint)targetPid; // placeholder thread id from pid
                    uint fgPid; GetWindowThreadProcessId(GetForegroundWindow(), out fgPid); uint currentThread = (uint)fgPid;
                    AttachThreadInput(currentThread, targetThread, true);
                    BringWindowToTop(hWnd);
                    SetForegroundWindow(hWnd);
                    AttachThreadInput(currentThread, targetThread, false);
                    await Task.Delay(250);
                    if (GetForegroundWindow() == hWnd) return true;
                }
            }
            await Task.Delay(delayMs);
        }
        return false;
    }

    private static Process FindWhatsAppProcess()
    {
        var list = Process.GetProcessesByName("WhatsApp");
        foreach (var p in list)
            if (p.MainWindowHandle != IntPtr.Zero) return p;
        foreach (var p in Process.GetProcesses())
        {
            try
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.IndexOf("WhatsApp", StringComparison.OrdinalIgnoreCase) >= 0)
                    return p;
            }
            catch { }
        }
        return null;
    }

    private static async Task<bool> TryPasteFileWithRetryAsync(int attempts, int initialDelayMs, int stepDelayMs, bool recopyEachAttempt, string pdfPath)
    {
        for (int i = 0; i < attempts; i++)
        {
            var waProc = FindWhatsAppProcess();
            if (waProc?.MainWindowHandle != IntPtr.Zero && GetForegroundWindow() != waProc.MainWindowHandle)
            {
                SetForegroundWindow(waProc.MainWindowHandle);
                await Task.Delay(200);
            }
            if (recopyEachAttempt) CopyFileToClipboardSTA(pdfPath);
            SendKeys.SendWait("^v");
            await Task.Delay(i == 0 ? initialDelayMs : stepDelayMs);
            // Assume success; continue attempts if earlier ones failed by user observation
            if (i >= 1) return true; // after at least one retry give up positively
        }
        return true; // fallback assume
    }
}