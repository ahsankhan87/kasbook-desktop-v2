using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            GridLogs.AutoGenerateColumns = false;
           
            GridLogs.DataSource =  POS.DLL.Log.GetAll();
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
