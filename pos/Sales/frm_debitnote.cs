using pos.Master.Companies.zatca;
using POS.BLL;
using POS.BLL.POS;
using POS.Core;
using POS.Core.POS;
using System;
using System.Data;
using System.Security.Principal;
using System.Windows.Forms;
using static jdk.nashorn.@internal.codegen.CompilerConstants;

namespace pos.Sales
{
    public partial class frm_debitnote : Form
    {
        private DataGridView dgvDebitNotes;

        public frm_debitnote()
        {
            InitializeComponent();
        }
        private void frm_debitnote_Load(object sender, EventArgs e)
        {
            // Initialize form components
            GetMaxInvoiceNo();
            Get_customers_dropdownlist();
            autoCompleteInvoice();
            LoadDebitNotes();
            ReasonDDL();

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate fields
            //if (string.IsNullOrWhiteSpace(cmb_customers.Text))
            //{
            //    MessageBox.Show("Please select a customer.");
            //    return;
            //}
            if (string.IsNullOrWhiteSpace(txtDebitNoteNumber.Text) ||
                string.IsNullOrWhiteSpace(txtReferenceInvoice.Text) ||
                string.IsNullOrWhiteSpace(txtAmount.Text) ||
                string.IsNullOrWhiteSpace(txtVATAmount.Text) ||
                string.IsNullOrWhiteSpace(txtTotalAmount.Text))
            {
                MessageBox.Show("Please fill all required fields.");
                return;
            }

            var debitNote = new DebitNoteModal
            {
                DebitNoteNumber = txtDebitNoteNumber.Text,
                IssueDate = dtpDate.Value,
                CustomerId = cmb_customers.SelectedValue == null ? 0 : int.Parse(cmb_customers.SelectedValue.ToString()),
                OriginalInvoiceId = txtReferenceInvoice.Text,
                Amount = decimal.Parse(txtAmount.Text),
                VATAmount = decimal.Parse(txtVATAmount.Text),
                TotalAmount = decimal.Parse(txtTotalAmount.Text),
                Reason = cmbReason.Text,
                InvoiceSubTypeCode = lbl_subtype_code.Text, // Assuming lbl_subtype_code is set based on the selected invoice type
                sale_type = lbl_saletype.Text, // Assuming lbl_saletype is set based on the selected sale type
            };

            var service = new DebitNoteBLL();
            service.CreateDebitNote(debitNote);
            // Assuming CreateDebitNote method handles the database insertion and ZATCA submission
            if (UsersModal.useZatcaEInvoice)
            {
                // Call ZATCA submission logic here
                DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                if (activeZatcaCredential == null)
                {
                    MessageBox.Show("No active ZATCA CSID/credentials found. Please configure them first.");
                }

                // Retrieve PCSID credentials from the database using the credentialId
                DataRow PCSID_dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(Convert.ToInt32(activeZatcaCredential["id"]));
                if (PCSID_dataRow == null)
                {
                    ZatcaHelper.SignDebitNoteToZatca(txtDebitNoteNumber.Text, txtReferenceInvoice.Text, Convert.ToDateTime(lbl_prevInvDate.Text));
                }
                else
                {
                    
                    ZatcaHelper.PCSID_SignDebitNoteToZatcaAsync(txtDebitNoteNumber.Text, txtReferenceInvoice.Text, Convert.ToDateTime(lbl_prevInvDate.Text));
                    
                }
            }
            
            // Show success message
            MessageBox.Show("Debit Note created successfully","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
            LoadDebitNotes();
            clearFields();
            GetMaxInvoiceNo();
        }

        private void clearFields()
        {
            txtDebitNoteNumber.Clear();
            dtpDate.Value = DateTime.Now;
            cmb_customers.SelectedIndex = 0;
            txtReferenceInvoice.Clear();
            txtAmount.Clear();
            txtVATAmount.Clear();
            txtTotalAmount.Clear();
            cmbReason.SelectedIndex = 0;
            lbl_subtype_code.Text = string.Empty;
            lbl_subtype_name.Text = string.Empty;
            lbl_saletype.Text = string.Empty;
        }

        private void ReasonDDL()
        {
            cmbReason.Items.Clear();
            cmbReason.Items.Add("Correction of previous invoice");
            cmbReason.Items.Add("Additional charges");
            cmbReason.Items.Add("Tax adjustment");
            cmbReason.Items.Add("Service fee");
            cmbReason.Items.Add("Other");

            cmbReason.SelectedIndex = 0;
        }

        private void btnSubmitZatca_Click(object sender, EventArgs e)
        {
            // Call your ZATCA submission logic here
            MessageBox.Show("Debit Note submitted to ZATCA.");
        }

        private void btnViewResponse_Click(object sender, EventArgs e)
        {
            if (gridDebitNotes.CurrentRow == null) return;
            string response = gridDebitNotes.CurrentRow.Cells["ZatcaMessage"].Value?.ToString();
            MessageBox.Show(response, "ZATCA Response");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDebitNotes();
        }

        private void LoadDebitNotes()
        {
            try
            {
                var service = new DebitNoteBLL();
                var debitNotes = service.GetAllDebitNotes(); // Implement this method to return a list
                //gridDebitNotes.AutoGenerateColumns = false;
                gridDebitNotes.DataSource = debitNotes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void GetMaxInvoiceNo()
        {
            SalesBLL salesBLL = new SalesBLL();
            txtDebitNoteNumber.Text = salesBLL.GetMaxDebitNoteInvoiceNo();

        }

        public void Get_customers_dropdownlist()
        {
            CustomerBLL customerBLL = new CustomerBLL();
            DataTable customers = customerBLL.GetAll();

            DataRow emptyRow = customers.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "";              // Set Column Value
            customers.Rows.InsertAt(emptyRow, 0);

            cmb_customers.DisplayMember = "first_name";
            cmb_customers.ValueMember = "id";
            cmb_customers.DataSource = customers;


        }

        public void autoCompleteInvoice()
        {
            try
            {
                txtReferenceInvoice.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtReferenceInvoice.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection coll = new AutoCompleteStringCollection();

                GeneralBLL invoicesBLL_obj = new GeneralBLL();
                string keyword = "TOP 1000 invoice_no ";
                string table = "pos_sales WHERE account = 'Sale' AND branch_id=" + UsersModal.logged_in_branch_id + " ORDER BY id desc";
                DataTable dt = invoicesBLL_obj.GetRecord(keyword, table);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        coll.Add(dr["invoice_no"].ToString());

                    }

                }

                txtReferenceInvoice.AutoCompleteCustomSource = coll;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            CalculateGrandTotal();
        }

        

        protected void CalculateGrandTotal()
        {
            decimal amount = string.IsNullOrEmpty(txtAmount.Text) ? 0 : Convert.ToDecimal(txtAmount.Text);
            decimal totalVAT = (amount * 15 / 100);
            txtVATAmount.Text = totalVAT.ToString();

            txtTotalAmount.Text = (amount + totalVAT).ToString();
        }


        private void txtReferenceInvoice_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GeneralBLL invoicesBLL_obj = new GeneralBLL();
                string keyword = "sale_date,sale_type,invoice_subtype_code, IIF(invoice_subtype_code = '02','Simplified','Standard') AS invoice_subtype ";
                string table = "pos_sales WHERE invoice_no = '" + txtReferenceInvoice.Text + "' AND branch_id=" + UsersModal.logged_in_branch_id + " ORDER BY id desc";
                DataTable dt = invoicesBLL_obj.GetRecord(keyword, table);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        lbl_subtype_code.Text = dr["invoice_subtype_code"].ToString();
                        lbl_subtype_name.Text = dr["invoice_subtype"].ToString();
                        lbl_saletype.Text = dr["sale_type"].ToString();
                        lbl_prevInvDate.Text = dr["sale_date"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}