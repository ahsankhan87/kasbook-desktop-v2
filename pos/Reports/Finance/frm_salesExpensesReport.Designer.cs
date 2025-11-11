using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS.Reports.Finance
{
    partial class frm_salesExpensesReport
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvReport;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Button btnGenerate;
        private Label lblFrom;
        private Label lblTo;
        private Label lblTotalSales;
        private Label lblTotalExpenses;
        private Label lblProfit;
        private TextBox txtTotalSales;
        private TextBox txtTotalExpenses;
        private TextBox txtProfit;
        private Button btnPrint;
        private Button btnExport;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvReport = new DataGridView();
            this.dtpFrom = new DateTimePicker();
            this.dtpTo = new DateTimePicker();
            this.btnGenerate = new Button();
            this.lblFrom = new Label();
            this.lblTo = new Label();
            this.lblTotalSales = new Label();
            this.lblTotalExpenses = new Label();
            this.lblProfit = new Label();
            this.txtTotalSales = new TextBox();
            this.txtTotalExpenses = new TextBox();
            this.txtProfit = new TextBox();
            this.btnPrint = new Button();
            this.btnExport = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();

            // dgvReport
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport.Location = new Point(12, 50);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.Size = (Size)new Point(900, 400);
            this.dgvReport.TabIndex = 0;

            // dtpFrom
            this.dtpFrom.Location = new Point(60, 15);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = (Size)new Point(200, 20);
            this.dtpFrom.TabIndex = 1;

            // dtpTo
            this.dtpTo.Location = new Point(300, 15);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = (Size)new Point(200, 20);
            this.dtpTo.TabIndex = 2;

            // btnGenerate
            this.btnGenerate.Location = new Point(520, 12);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = (Size)new Point(75, 25);
            this.btnGenerate.TabIndex = 3;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new EventHandler(this.btnGenerate_Click);

            // Labels
            this.lblFrom.Location = new Point(15, 18);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = (Size)new Point(40, 15);
            this.lblFrom.Text = "From:";

            this.lblTo.Location = new Point(270, 18);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = (Size)new Point(25, 15);
            this.lblTo.Text = "To:";

            // Summary Labels
            this.lblTotalSales.Location = new Point(12, 460);
            this.lblTotalSales.Name = "lblTotalSales";
            this.lblTotalSales.Size = (Size)new Point(80, 20);
            this.lblTotalSales.Text = "Total Sales:";

            this.lblTotalExpenses.Location = new Point(12, 490);
            this.lblTotalExpenses.Name = "lblTotalExpenses";
            this.lblTotalExpenses.Size = (Size)new Point(100, 20);
            this.lblTotalExpenses.Text = "Total Expenses:";

            this.lblProfit.Location = new Point(12, 520);
            this.lblProfit.Name = "lblProfit";
            this.lblProfit.Size = (Size)new Point(80, 20);
            this.lblProfit.Text = "Profit:";

            // Summary TextBoxes
            this.txtTotalSales.Location = new Point(120, 460);
            this.txtTotalSales.Name = "txtTotalSales";
            this.txtTotalSales.ReadOnly = true;
            this.txtTotalSales.Size = (Size)new Point(120, 20);
            this.txtTotalSales.TabIndex = 4;

            this.txtTotalExpenses.Location = new Point(120, 490);
            this.txtTotalExpenses.Name = "txtTotalExpenses";
            this.txtTotalExpenses.ReadOnly = true;
            this.txtTotalExpenses.Size = (Size)new Point(120, 20);
            this.txtTotalExpenses.TabIndex = 5;

            this.txtProfit.Location = new Point(120, 520);
            this.txtProfit.Name = "txtProfit";
            this.txtProfit.ReadOnly = true;
            this.txtProfit.Size = (Size)new Point(120, 20);
            this.txtProfit.TabIndex = 6;

            // Buttons
            this.btnPrint.Location = new Point(750, 460);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = (Size)new Point(75, 25);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);

            this.btnExport.Location = new Point(835, 460);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = (Size)new Point(75, 25);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);

            // Form
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(924, 551);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.txtProfit);
            this.Controls.Add(this.txtTotalExpenses);
            this.Controls.Add(this.txtTotalSales);
            this.Controls.Add(this.lblProfit);
            this.Controls.Add(this.lblTotalExpenses);
            this.Controls.Add(this.lblTotalSales);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.dgvReport);
            this.Name = "frm_salesExpensesReport";
            this.Text = "Sales and Expenses Report";
            this.Load += new EventHandler(this.frm_salesExpensesReport_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}