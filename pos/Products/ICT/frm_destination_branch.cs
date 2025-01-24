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

namespace pos.Products.ICT
{
    public partial class frm_destination_branch : Form
    {
        public int _branch_id;
        public frm_destination_branch()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_destination_branch_Load(object sender, EventArgs e)
        {
            get_branches_DDL();
        }
        public void get_branches_DDL()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "pos_branches WHERE id <> "+ UsersModal.logged_in_branch_id+"";

            DataTable branches_DDL = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = branches_DDL.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "";              // Set Column Value
            //branches_DDL.Rows.InsertAt(emptyRow, 0);

            cmb_branches.DisplayMember = "name";
            cmb_branches.ValueMember = "id";
            cmb_branches.DataSource = branches_DDL;

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            _branch_id = int.Parse(cmb_branches.SelectedValue.ToString());
            this.Close();
        }
    }
}
