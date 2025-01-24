using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_main_1 : Form
    {
        public frm_main_1()
        {
            InitializeComponent();
        }
        
        private void frm_main_1_Load(object sender, EventArgs e)
        {
            //frm_sales_obj.Show();
            
        }
        Form frm_customer_obj;
        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_customer_obj == null)
            {
                frm_customer_obj = new frm_customers();
                frm_customer_obj.MdiParent = this;
                frm_customer_obj.FormClosed += new FormClosedEventHandler(frm_customer_obj_FormClosed);
                frm_customer_obj.Show();
                //frm_customer_obj.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frm_customer_obj.Activate();
            }
            
        }

        void frm_customer_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_customer_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_supplier_obj;
        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_supplier_obj == null)
            {
                frm_supplier_obj = new frm_suppliers();
                frm_supplier_obj.MdiParent = this;
                frm_supplier_obj.FormClosed += new FormClosedEventHandler(frm_supplier_obj_FormClosed);
                //frm_cust.Dock = DockStyle.Fill;
                frm_supplier_obj.Show();
            }
            else
            {
                frm_supplier_obj.Activate();
            }
        }

        void frm_supplier_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_supplier_obj = null;
        }

        Form frm_employee_obj;
        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_employee_obj == null)
            {
                frm_employee_obj = new frm_employees();
                frm_employee_obj.MdiParent = this;
                frm_employee_obj.FormClosed += new FormClosedEventHandler(frm_employee_obj_FormClosed);
                //frm_cust.Dock = DockStyle.Fill;
                frm_employee_obj.Show();
            }
            else { frm_employee_obj.Activate(); }
        }

        void frm_employee_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_employees frm_employee_obj = new frm_employees();
            frm_employee_obj = null;
        }

        Form frm_products_obj;
        private void productsServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_products_obj == null)
            {
                frm_products_obj = new frm_products();
                frm_products_obj.MdiParent = this;
                //frm_cust.Dock = DockStyle.Fill;
                frm_products_obj.FormClosed += new FormClosedEventHandler(frm_products_obj_FormClosed);
                frm_products_obj.Show();
            }
            else
            {
                frm_products_obj.Activate();
            }
        }

        void frm_products_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_products_obj = null;
        }

        Form frm_purchases_obj;
        private void newTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_purchases_obj == null)
            {
                frm_purchases_obj = new frm_purchases();
                frm_purchases_obj.MdiParent = this;
                frm_purchases_obj.FormClosed += new FormClosedEventHandler(frm_purchases_obj_FormClosed);
                frm_purchases_obj.Show();
            }
            else
            {
                frm_purchases_obj.Activate();
            }
            
        }

        void frm_purchases_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_purchases_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_sales_obj;
        private void newTransactionToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (frm_sales_obj == null)
            {
                frm_sales_obj = new frm_sales();
                //frm_sales_obj.MdiParent = this;
                
                //frm_sales_obj.Dock = DockStyle.Fill;
                frm_sales_obj.FormClosed += new FormClosedEventHandler(frm_sales_obj_FormClosed);
                frm_sales_obj.WindowState = FormWindowState.Maximized;
                frm_sales_obj.Show();

            }
            else { frm_sales_obj.Activate();  }
            
        }

        void frm_sales_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_sales_obj = null;
            //throw new NotImplementedException();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Form frm_taxes_obj;
        private void taxesVATToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(frm_taxes_obj == null)
            {
                frm_taxes_obj = new frm_taxes();
                frm_taxes_obj.MdiParent = this;
                //frm_taxes_obj.Dock = DockStyle.Fill;
                frm_taxes_obj.FormClosed += new FormClosedEventHandler(frm_taxes_obj_FormClosed);
                frm_taxes_obj.Show();
                //frm_taxes_obj.WindowState = FormWindowState.Maximized;
            }
            else { frm_taxes_obj.Activate();  }
            
        }

        void frm_taxes_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_taxes_obj = null;
        }

        Form frm_categories_obj;
        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_categories_obj == null)
            {
                frm_categories_obj = new frm_categories();
                frm_categories_obj.MdiParent = this;
                frm_categories_obj.FormClosed += new FormClosedEventHandler(frm_categories_obj_FormClosed);
                frm_categories_obj.Show();
            }
            else
            {
                frm_categories_obj.Activate();
            }
            
        }

        void frm_categories_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_categories_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_units_obj;
        private void unitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(frm_units_obj == null)
            {
                frm_units_obj = new frm_units();
                frm_units_obj.MdiParent = this;
                frm_units_obj.FormClosed += new FormClosedEventHandler(frm_units_obj_FormClosed);
                frm_units_obj.Show();

            }
            else { frm_units_obj.Activate();  }
            
        }

        void frm_units_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_units_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_locations_obj;
        private void locationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_locations_obj == null)
            {
                frm_locations_obj = new frm_locations();
                frm_locations_obj.MdiParent = this;
                frm_locations_obj.FormClosed += new FormClosedEventHandler(frm_locations_obj_FormClosed);
                frm_locations_obj.Show();
            }
            else { frm_locations_obj.Activate();  }
            
        }

        void frm_locations_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_locations_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_brands_obj;
        private void brandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_brands_obj == null)
            {
                frm_brands_obj = new frm_brands();
                frm_brands_obj.MdiParent = this;
                frm_brands_obj.FormClosed += new FormClosedEventHandler(frm_brands_obj_FormClosed);
                frm_brands_obj.Show();
            }
            else
            {
                frm_brands_obj.Activate();
            }
            
        }

        void frm_brands_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_brands_obj = null; //throw new NotImplementedException();
        }

        Form frm_update_com_obj;
        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_update_com_obj == null)
            {
                frm_update_com_obj = new frm_updateCompany();
                frm_update_com_obj.MdiParent = this;
                frm_update_com_obj.FormClosed += new FormClosedEventHandler(frm_update_com_obj_FormClosed);
                frm_update_com_obj.Show();
            }
            else { frm_update_com_obj.Activate();  }
            
        }

        void frm_update_com_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_update_com_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_all_sales_obj;
        private void allTransactionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frm_all_sales_obj == null)
            {
                frm_all_sales_obj = new frm_all_sales();
                frm_all_sales_obj.MdiParent = this;
                frm_all_sales_obj.FormClosed += new FormClosedEventHandler(frm_all_sales_obj_FormClosed);
                frm_all_sales_obj.Show();
            }
            else { frm_all_sales_obj.Activate();  }
            
        }

        void frm_all_sales_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_all_sales_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_all_purchases_obj;
        private void allPurchasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_all_purchases_obj == null)
            {
                frm_all_purchases_obj = new frm_all_purchases();
                frm_all_purchases_obj.MdiParent = this;
                frm_all_purchases_obj.FormClosed += new FormClosedEventHandler(frm_all_purchases_obj_FormClosed);
                frm_all_purchases_obj.Show();
            }
            else { frm_all_purchases_obj.Activate();  }
            
        }

        void frm_all_purchases_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_all_purchases_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_sales_report_obj;
        private void salesReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(frm_sales_report_obj == null)
            {
                frm_sales_report_obj = new frm_SalesReport();
                frm_sales_report_obj.MdiParent = this;
                frm_sales_report_obj.FormClosed += new FormClosedEventHandler(frm_sales_report_obj_FormClosed);
                frm_sales_report_obj.Show();
            }
            else { frm_sales_report_obj.Activate();  }
            
        }

        void frm_sales_report_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_sales_report_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_purchase_report_obj;
        private void purchaseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_purchase_report_obj ==null)
            {
                frm_purchase_report_obj = new frm_PurchasesReport();
                frm_purchase_report_obj.MdiParent = this;
                frm_purchase_report_obj.FormClosed += new FormClosedEventHandler(frm_purchase_report_obj_FormClosed);
                frm_purchase_report_obj.Show();
            }
            else { frm_purchase_report_obj.Activate(); }
            
        }

        void frm_purchase_report_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_purchase_report_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_sales_return_obj;
        private void salesReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_sales_return_obj == null)
            {
                frm_sales_return_obj = new frm_sales_return();
                frm_sales_return_obj.MdiParent = this;
                frm_sales_return_obj.FormClosed += new FormClosedEventHandler(frm_sales_return_obj_FormClosed);
                frm_sales_return_obj.Show();
            }
            else { frm_sales_return_obj.Activate();  }
            
        }

        void frm_sales_return_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_sales_return_obj = null;
            //throw new NotImplementedException();
        }

        private void frm_main_1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        Form user_obj;
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (user_obj == null)
            {
                user_obj = new frm_users();
                user_obj.MdiParent = this;
                user_obj.FormClosed += new FormClosedEventHandler(user_obj_FormClosed);
                user_obj.Show();
            }
            else
            {
                user_obj.Activate();
            }
        }

        void user_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            user_obj = null;
            //throw new NotImplementedException();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login_obj = new Login();
            login_obj.Show();
        }

        Form branch_obj;
        private void branchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (branch_obj == null)
            {
                branch_obj = new frm_branches();
                branch_obj.MdiParent = this;
                branch_obj.FormClosed += new FormClosedEventHandler(branch_obj_FormClosed);
                branch_obj.Show();
            }
            else { branch_obj.Activate();  }
            
        }

        void branch_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            branch_obj = null;
            //throw new NotImplementedException();
        }

        Form group_obj;
        private void groupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (group_obj == null)
            {
                group_obj = new frm_groups();
                group_obj.MdiParent = this;
                group_obj.FormClosed += new FormClosedEventHandler(group_obj_FormClosed);
                group_obj.Show();
            }
            else { group_obj.Activate(); }
            
        }

        void group_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            group_obj = null;
            //throw new NotImplementedException();
        }

        Form account_obj;
        private void accountsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (account_obj == null)
            {
                account_obj = new frm_accounts();
                account_obj.MdiParent = this;
                account_obj.FormClosed += new FormClosedEventHandler(account_obj_FormClosed);
                account_obj.Show();
            }
            else { account_obj.Activate(); }
            
        }

        void account_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            account_obj = null;
            //throw new NotImplementedException();
        }

        Form journal_obj;
        private void journalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (journal_obj == null)
            {
                journal_obj = new frm_journal_entries();
                journal_obj.MdiParent = this;
                journal_obj.FormClosed += new FormClosedEventHandler(journal_obj_FormClosed);
                journal_obj.Show();
            }
            else
            {
                journal_obj.Activate();
            }
            
        }

        void journal_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            journal_obj = null;
            //throw new NotImplementedException();
        }

        Form daybook_obj;
        private void journalDaybookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (daybook_obj == null)
            {
                daybook_obj = new frm_journal_daybook();
                daybook_obj.MdiParent = this;
                daybook_obj.FormClosed +=new FormClosedEventHandler(daybook_obj_FormClosed);
                daybook_obj.Show();
            }
            else
            {
                daybook_obj.Activate();
            }
            
        }

        void daybook_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            daybook_obj = null;
            //throw new NotImplementedException();
        }

        Form account_report_obj;
        private void accountReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (account_report_obj == null)
            {
                account_report_obj = new frm_account_report();
                account_report_obj.MdiParent = this;
                account_report_obj.FormClosed += new FormClosedEventHandler(account_report_obj_FormClosed);
                account_report_obj.Show();
            }
            else
            {
                account_report_obj.Activate();
            }
            
        }

        void account_report_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            account_report_obj = null;
            //throw new NotImplementedException();
        }

        Form group_report_obj;
        private void groupReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (group_report_obj == null)
            {
                group_report_obj = new frm_group_report();
                group_report_obj.MdiParent = this;
                group_report_obj.FormClosed += new FormClosedEventHandler(group_report_obj_FormClosed);
                group_report_obj.Show();
            }
            else
            {
                group_report_obj.Activate();
            }
            
        }

        void group_report_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            group_report_obj = null;
            //throw new NotImplementedException();
        }

        Form trial_obj;
        private void trialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (trial_obj == null)
            {
                trial_obj = new frm_trialbalance_report();
                trial_obj.MdiParent = this;
                trial_obj.FormClosed += new FormClosedEventHandler(trial_obj_FormClosed);
                trial_obj.Show();
            }
            else
            {
                trial_obj.Activate();
            }
            
        }

        void trial_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            trial_obj = null;
            //throw new NotImplementedException();
        }

        Form customerWiseSalesReport;
        private void customerWiseSalesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(customerWiseSalesReport == null)
            {
                customerWiseSalesReport = new frm_customerWiseSalesReport();
                customerWiseSalesReport.MdiParent = this;
                customerWiseSalesReport.FormClosed += new FormClosedEventHandler(customerWiseSalesReport_FormClosed);
                customerWiseSalesReport.Show();
            }
            else
            {
                customerWiseSalesReport.Activate();
            }
        }

        void customerWiseSalesReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            customerWiseSalesReport = null;
            //throw new NotImplementedException();
        }

        Form frm_productWiseSalesReport;
        private void productWiseSalesSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_productWiseSalesReport == null)
            {
                frm_productWiseSalesReport = new frm_productWiseSalesReport();
                frm_productWiseSalesReport.MdiParent = this;
                frm_productWiseSalesReport.FormClosed += new FormClosedEventHandler(frm_productWiseSalesReport_FormClosed);
                frm_productWiseSalesReport.Show();
            }
            else
            {
                frm_productWiseSalesReport.Activate();
            }
        }

        void frm_productWiseSalesReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_productWiseSalesReport = null;
            //throw new NotImplementedException();
        }

        Form frm_categorySummary;
        private void categoryWiseSalesSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(frm_categorySummary == null)
            {
                frm_categorySummary = new frm_categoryWiseSalesReport();
                frm_categorySummary.MdiParent = this;
                frm_categorySummary.FormClosed += new FormClosedEventHandler(frm_categorySummary_FormClosed);
                frm_categorySummary.Show();
            }
            else
            {
                frm_categorySummary.Activate();
            }
        }

        void frm_categorySummary_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_categorySummary = null;
            //throw new NotImplementedException();
        }

        FrmDbBackup frm_backup;
        private void dBBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(frm_backup == null)
            {
                frm_backup = new FrmDbBackup();
                frm_backup.MdiParent = this;
                frm_backup.FormClosed += new FormClosedEventHandler(frm_backup_FormClosed);
                frm_backup.Show();
            }
            else
            {
                frm_backup.Activate();
            }
            
        }

        void frm_backup_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_backup = null;
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frm_customers frm_customers = new frm_customers();
            frm_customers.TopLevel = false;
            panel_main.Controls.Add(frm_customers);
            frm_customers.BringToFront();
            frm_customers.Show();
        }
        

        
    }
}
