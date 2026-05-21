using POS.BLL;
using POS.Core;
using pos.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace pos.Products.Suppression
{
    public partial class frm_stock_suppression_companies : Form
    {
        private readonly HashSet<int> _initialSelection;

        public List<int> SelectedBranchIds { get; private set; } = new List<int>();
        public string SelectedBranchText { get; private set; } = string.Empty;

        public frm_stock_suppression_companies(IEnumerable<int> selectedBranchIds)
        {
            _initialSelection = new HashSet<int>(selectedBranchIds ?? Enumerable.Empty<int>());
            InitializeComponent();
        }

        private void frm_stock_suppression_companies_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            LoadBranches();
        }

        private void LoadBranches()
        {
            var bll = new BranchesBLL();
            DataTable dt = bll.GetAll();

            gridBranches.Rows.Clear();
            if (dt == null)
                return;

            foreach (DataRow row in dt.Rows)
            {
                int id;
                if (!int.TryParse(Convert.ToString(row["id"]), out id))
                    continue;

                string name = Convert.ToString(row["name"]);
                bool selected = _initialSelection.Contains(id);

                gridBranches.Rows.Add(id, name, selected);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectedBranchIds.Clear();

            foreach (DataGridViewRow row in gridBranches.Rows)
            {
                if (row.IsNewRow)
                    continue;

                bool isSelected = row.Cells["colSelect"].Value != null && Convert.ToBoolean(row.Cells["colSelect"].Value);
                if (!isSelected)
                    continue;

                int id;
                if (!int.TryParse(Convert.ToString(row.Cells["colId"].Value), out id))
                    continue;

                SelectedBranchIds.Add(id);
            }

            if (SelectedBranchIds.Count == 0)
            {
                UiMessages.ShowWarning("Please select at least one branch.", "يرجى اختيار فرع واحد على الأقل.", captionEn: "Company Selection", captionAr: "اختيار الفرع");
                return;
            }

            var names = new List<string>();
            foreach (DataGridViewRow row in gridBranches.Rows)
            {
                if (row.IsNewRow)
                    continue;

                bool isSelected = row.Cells["colSelect"].Value != null && Convert.ToBoolean(row.Cells["colSelect"].Value);
                if (isSelected)
                {
                    names.Add(Convert.ToString(row.Cells["colName"].Value));
                }
            }

            SelectedBranchText = string.Join(", ", names);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
