using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.UI;

namespace pos.Master.Logs
{
    public partial class Logs : Form
    {
        public Logs()
        {
            InitializeComponent();
        }

        private void Logs_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            GridLogs.AutoGenerateColumns = false;
           
            GridLogs.DataSource =  POS.DLL.Log.GetAll();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel1, label21, panel2, GridLogs);
            label1.Font = AppTheme.FontLabel;
            label1.ForeColor = Color.White;
            label2.Font = AppTheme.FontLabel;
            label2.ForeColor = Color.White;
        }

        private void Logs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GridLogs.AutoGenerateColumns = false;

                GridLogs.DataSource = POS.DLL.Log.SearchRecordByDate(fromDate.Value.Date, toDate.Value.Date);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Log Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }
    }
}
