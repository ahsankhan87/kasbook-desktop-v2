using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.DLL;

namespace pos
{
    public partial class SalesByPaymentMethodForm : Form
    {
        private readonly SalesReportDLL _dal;

        public SalesByPaymentMethodForm()
        {
            InitializeComponent();
            _dal = new SalesReportDLL();
        }

        private async void SalesByPaymentMethodForm_Load(object sender, EventArgs e)
        {
            // Default: last 30 days
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;
            dtpStart.Checked = true;
            dtpEnd.Checked = true;

            await RefreshDataAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshDataAsync();
        }

        private async Task RefreshDataAsync()
        {
            DateTime? start = dtpStart.Checked ? dtpStart.Value.Date : (DateTime?)null;
            DateTime? end = dtpEnd.Checked ? dtpEnd.Value.Date : (DateTime?)null;

            try
            {
                UseWaitCursor = true;
                btnRefresh.Enabled = false;
                lblStatus.Text = "Loading...";

                var rows = await Task.Run(() => _dal.GetSalesByPaymentMethod(start, end));

                bindingSales.DataSource = rows;

                var txCountTotal = rows.Sum(r => r.TransactionCount);
                var amountTotal = rows.Sum(r => r.TotalAmount);
                lblStatus.Text = $"Rows: {rows.Count}, Transactions: {txCountTotal}, Total: {amountTotal:C2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error loading report", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Failed to load.";
            }
            finally
            {
                btnRefresh.Enabled = true;
                UseWaitCursor = false;
            }
        }
    }
}