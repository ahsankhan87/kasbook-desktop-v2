using System;
using System.Windows.Forms;
using POS.Core;

namespace pos.UI
{
    internal static class UiMessages
    {
        private static bool IsArabic => string.Equals(UsersModal.logged_in_lang, "ar-SA", StringComparison.OrdinalIgnoreCase);

        private static IWin32Window ResolveOwner()
        {
            if (Form.ActiveForm != null)
                return Form.ActiveForm;

            if (Application.OpenForms.Count > 0)
                return Application.OpenForms[Application.OpenForms.Count - 1];

            return null;
        }

        public static string T(string en, string ar)
        {
            return IsArabic ? ar : en;
        }

        public static void ShowInfo(string en, string ar, string captionEn = "Info", string captionAr = "„⁄·Ê„…")
        {
            var owner = ResolveOwner();
            if (owner != null)
                MessageBox.Show(owner, T(en, ar), T(captionEn, captionAr), MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(T(en, ar), T(captionEn, captionAr), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowWarning(string en, string ar, string captionEn = "Warning", string captionAr = " ‰»ÌÂ")
        {
            var owner = ResolveOwner();
            if (owner != null)
                MessageBox.Show(owner, T(en, ar), T(captionEn, captionAr), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show(T(en, ar), T(captionEn, captionAr), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError(string en, string ar, string captionEn = "Error", string captionAr = "Œÿ√")
        {
            var owner = ResolveOwner();
            if (owner != null)
                MessageBox.Show(owner, T(en, ar), T(captionEn, captionAr), MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(T(en, ar), T(captionEn, captionAr), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult ConfirmYesNo(string en, string ar, string captionEn = "Confirm", string captionAr = " √ﬂÌœ", MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2)
        {
            var owner = ResolveOwner();
            if (owner != null)
                return MessageBox.Show(owner, T(en, ar), T(captionEn, captionAr), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, defaultButton);

            return MessageBox.Show(T(en, ar), T(captionEn, captionAr), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, defaultButton);
        }

        public static DialogResult ConfirmYesNoCancel(string en, string ar, string captionEn = "Confirm", string captionAr = " √ﬂÌœ", MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button3)
        {
            var owner = ResolveOwner();
            if (owner != null)
                return MessageBox.Show(owner, T(en, ar), T(captionEn, captionAr), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, defaultButton);

            return MessageBox.Show(T(en, ar), T(captionEn, captionAr), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, defaultButton);
        }
    }
}
