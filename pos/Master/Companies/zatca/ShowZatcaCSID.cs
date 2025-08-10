using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
    }
}
