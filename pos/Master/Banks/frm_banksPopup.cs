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

namespace pos.Master.Banks
{
    public partial class frm_banksPopup : Form
    {
        public string _bankIDPlusGLAccountID;

        public frm_banksPopup()
        {
            InitializeComponent();
        }
        private void frm_banksPopup_Load(object sender, EventArgs e)
        {
            get_banks_DDL();
        }
        public void get_banks_DDL()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "CONCAT(id,'+',GLAccountID) as id,name";
            string table = "pos_banks WHERE branch_id = " + UsersModal.logged_in_branch_id + "";

            DataTable banks_DDL = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = banks_DDL.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "";              // Set Column Value
            //banks_DDL.Rows.InsertAt(emptyRow, 0);

            cmb_banks.DisplayMember = "name";
            cmb_banks.ValueMember = "id";
            cmb_banks.DataSource = banks_DDL;

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            _bankIDPlusGLAccountID = cmb_banks.SelectedValue.ToString();
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
