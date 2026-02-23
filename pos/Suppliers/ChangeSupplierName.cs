using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Windows.Forms;

namespace pos.Suppliers
{
    public partial class ChangeSupplierName : Form
    {
        public string _invoiceNo;
        public string _supplierINvoiceNo = "";

        public ChangeSupplierName(string invoiceNo, string supplierINvoiceNo)
        {
            InitializeComponent();
            _invoiceNo = invoiceNo;
            _supplierINvoiceNo = supplierINvoiceNo;
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                PurchasesBLL purchasesBLL = new PurchasesBLL();
                int result = purchasesBLL.UpdateSupplierInPurchases(_invoiceNo, cmb_suppliers.SelectedValue.ToString(),txtSupplierInvoiceNo.Text);
                if (result > 0)
                {
                    MessageBox.Show("Supplier updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Supplier not updated", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void Get_Suppliers_DDL()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,first_name";
            string table = "pos_suppliers WHERE branch_id = " + UsersModal.logged_in_branch_id + "";

            DataTable DDL = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = banks_DDL.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "";              // Set Column Value
            //banks_DDL.Rows.InsertAt(emptyRow, 0);

            cmb_suppliers.DisplayMember = "first_name";
            cmb_suppliers.ValueMember = "id";
            cmb_suppliers.DataSource = DDL;

        }

        private void ChangeSupplierName_Load(object sender, EventArgs e)
        {
            lbl_invoice_no.Text = _invoiceNo;
            txtSupplierInvoiceNo.Text = _supplierINvoiceNo;
            this.ActiveControl = txtSupplierInvoiceNo;
            Get_Suppliers_DDL();
        }
    }
}
