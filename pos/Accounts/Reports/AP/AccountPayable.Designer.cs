﻿
namespace pos.Accounts.Reports.AP
{
    partial class AccountPayable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnSubmit = new System.Windows.Forms.Button();
            this.txt_to_date = new System.Windows.Forms.DateTimePicker();
            this.txt_from_date = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CmbCondition = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(978, 512);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(440, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "To";
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.Location = new System.Drawing.Point(811, 24);
            this.BtnSubmit.Name = "BtnSubmit";
            this.BtnSubmit.Size = new System.Drawing.Size(115, 55);
            this.BtnSubmit.TabIndex = 7;
            this.BtnSubmit.Text = "Submit (F3)";
            this.BtnSubmit.UseVisualStyleBackColor = true;
            this.BtnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // txt_to_date
            // 
            this.txt_to_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_to_date.Location = new System.Drawing.Point(471, 32);
            this.txt_to_date.Name = "txt_to_date";
            this.txt_to_date.Size = new System.Drawing.Size(156, 22);
            this.txt_to_date.TabIndex = 5;
            // 
            // txt_from_date
            // 
            this.txt_from_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_from_date.Location = new System.Drawing.Point(303, 32);
            this.txt_from_date.Name = "txt_from_date";
            this.txt_from_date.Size = new System.Drawing.Size(131, 22);
            this.txt_from_date.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CmbCondition);
            this.panel1.Controls.Add(this.BtnSubmit);
            this.panel1.Controls.Add(this.txt_from_date);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txt_to_date);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(978, 94);
            this.panel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.crystalReportViewer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 94);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(978, 512);
            this.panel2.TabIndex = 0;
            // 
            // CmbCondition
            // 
            this.CmbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCondition.FormattingEnabled = true;
            this.CmbCondition.Location = new System.Drawing.Point(98, 30);
            this.CmbCondition.Name = "CmbCondition";
            this.CmbCondition.Size = new System.Drawing.Size(153, 24);
            this.CmbCondition.TabIndex = 9;
            this.CmbCondition.SelectedIndexChanged += new System.EventHandler(this.CmbCondition_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Condition";
            // 
            // AccountPayable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 606);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "AccountPayable";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account Payable Report";
            this.Load += new System.EventHandler(this.AccountPayable_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AccountPayable_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnSubmit;
        private System.Windows.Forms.DateTimePicker txt_to_date;
        private System.Windows.Forms.DateTimePicker txt_from_date;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox CmbCondition;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}