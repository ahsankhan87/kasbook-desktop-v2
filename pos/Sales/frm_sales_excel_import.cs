using pos.UI;
using System;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_sales_excel_import : Form
    {
        private readonly Action _importAction;
        private readonly Action _downloadTemplateAction;

        public frm_sales_excel_import(
            Action importAction,
            Action downloadTemplateAction,
            string windowTitle = null,
            string heading = null,
            string details = null,
            string stepsTitle = null,
            string steps = null)
        {
            _importAction = importAction;
            _downloadTemplateAction = downloadTemplateAction;
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(windowTitle))
                Text = windowTitle;
            if (!string.IsNullOrWhiteSpace(heading))
                lblTitle.Text = heading;
            if (!string.IsNullOrWhiteSpace(details))
                lblDetails.Text = details;
            if (!string.IsNullOrWhiteSpace(stepsTitle))
                groupBoxSteps.Text = stepsTitle;
            if (!string.IsNullOrWhiteSpace(steps))
                lblSteps.Text = steps;

            AppendOptionalFieldsHint();
        }

        private void AppendOptionalFieldsHint()
        {
            const string detailsHint = "\r\nQty and Price are optional. If omitted, Qty = 1 and the saved product price will be used.";
            const string stepsHint = "\r\n5. You can also import only product codes; missing Qty and Price will default automatically.";

            if (!lblDetails.Text.Contains("Qty and Price are optional"))
                lblDetails.Text += detailsHint;

            if (!lblSteps.Text.Contains("missing Qty and Price will default automatically"))
                lblSteps.Text += stepsHint;
        }

        private void frm_sales_excel_import_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            _importAction?.Invoke();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            _downloadTemplateAction?.Invoke();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
