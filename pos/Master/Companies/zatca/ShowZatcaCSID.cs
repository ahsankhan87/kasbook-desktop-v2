using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Master.Companies.zatca
{
    public partial class ShowZatcaCSID : Form
    {
        public ShowZatcaCSID()
        {
            InitializeComponent();
        }

        private void grid_fiscal_years_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (grid_zatca_csids.CurrentRow == null) return;
            string name = grid_zatca_csids.Columns[e.ColumnIndex].Name;
            if (name == "activate")
            {
                string id = grid_zatca_csids.CurrentRow.Cells["id"].Value.ToString();
                string mode = grid_zatca_csids.CurrentRow.Cells["mode"].Value.ToString();
                
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to activate", "Activate Zatca Environment", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ZatcaInvoiceGenerator.UpdateZatcaStatus(int.Parse(id));

                    MessageBox.Show("Record updated successfully.", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadZatcaCSIDGrid();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

                LoadZatcaCSIDGrid();
            }
        }

        private void ShowZatcaCSID_Load(object sender, EventArgs e)
        {
            LoadZatcaCSIDGrid();
        }

        protected void LoadZatcaCSIDGrid()
        {
            try
            {
                GeneralBLL objBLL = new GeneralBLL();
                grid_zatca_csids.AutoGenerateColumns = false;
                string keyword = "*";
                string table = "zatca_credentials";
                grid_zatca_csids.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            LoadZatcaCSIDGrid();
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            AutoGenerateCSID autoGenerateCSR = new AutoGenerateCSID();
            autoGenerateCSR.ShowDialog();
        }

        private void btn_generatePCSID_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_zatca_csids.CurrentRow == null) return;
                string id = grid_zatca_csids.CurrentRow.Cells["id"].Value.ToString();
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("Please select a valid CSID record.", "Generate PCSID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                GeneratePCSID generatePCSID = new GeneratePCSID(int.Parse(id));
                generatePCSID.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_renew_PCSID_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_zatca_csids.CurrentRow == null) return;
                string id = grid_zatca_csids.CurrentRow.Cells["id"].Value.ToString();
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("Please select a valid CSID record.", "Renew PCSID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                RenewPCSID renewPCSID = new RenewPCSID(int.Parse(id));
                renewPCSID.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            if (grid_zatca_csids.CurrentRow == null) return;
            string id = grid_zatca_csids.CurrentRow.Cells["id"].Value.ToString();
            string publicKey = grid_zatca_csids.CurrentRow.Cells["cert_base64"].Value.ToString(); 
            
            if (!string.IsNullOrEmpty(publicKey))
            {
                GetCertInfo(publicKey);

            }
        }
        private void GetCertInfo(string publicKey)
        {
            if (!string.IsNullOrEmpty(publicKey))
            {
                string pemText = publicKey.Trim();

                // Remove PEM headers
                string base64 = pemText
                    .Replace("-----BEGIN CERTIFICATE-----", "")
                    .Replace("-----END CERTIFICATE-----", "")
                    .Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();

                // Decode Base64
                byte[] certBytes = Convert.FromBase64String(base64);

                // Save to a .cer or .der file
                //File.WriteAllBytes(pemText, certBytes);

                // Optional: Load it into an X509Certificate2 object
                var certificate = new X509Certificate2(certBytes);

                // Print info
                string info = "";
                info = "Subject :" + certificate.Subject + "\n";
                info += "Issuer :" + certificate.Issuer + "\n";
                info += "Valid From :" + certificate.NotBefore + "\n";
                info += "Valid To :" + certificate.NotAfter + "\n";
                MessageBox.Show(info);

            }
        }

    }
}
