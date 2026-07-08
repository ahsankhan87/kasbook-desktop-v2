namespace pos
{
    partial class frm_journal_entries
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_journal_entries));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.flowFooterButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_add_line = new System.Windows.Forms.Button();
            this.btn_new = new System.Windows.Forms.Button();
            this.btn_save_draft = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_post_new = new System.Windows.Forms.Button();
            this.btn_print = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelGridHost = new System.Windows.Forms.Panel();
            this.grid_journal = new System.Windows.Forms.DataGridView();
            this.colRowNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.account = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colAccountType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_center = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.debit_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.credit_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panelBalance = new System.Windows.Forms.Panel();
            this.lbl_line_count_title = new System.Windows.Forms.Label();
            this.lbl_line_count = new System.Windows.Forms.Label();
            this.lbl_balance_status = new System.Windows.Forms.Label();
            this.lbl_difference_title = new System.Windows.Forms.Label();
            this.txt_difference = new System.Windows.Forms.TextBox();
            this.lbl_cr_title = new System.Windows.Forms.Label();
            this.txt_cr_total = new System.Windows.Forms.TextBox();
            this.lbl_dr_title = new System.Windows.Forms.Label();
            this.txt_dr_total = new System.Windows.Forms.TextBox();
            this.grpVoucherInformation = new System.Windows.Forms.GroupBox();
            this.btn_load_template = new System.Windows.Forms.Button();
            this.btn_attachment = new System.Windows.Forms.Button();
            this.txt_narration = new System.Windows.Forms.TextBox();
            this.lblNarration = new System.Windows.Forms.Label();
            this.txt_reference_no = new System.Windows.Forms.TextBox();
            this.lblReferenceNo = new System.Windows.Forms.Label();
            this.cmb_voucher_type = new System.Windows.Forms.ComboBox();
            this.lblVoucherType = new System.Windows.Forms.Label();
            this.txt_entry_date = new System.Windows.Forms.DateTimePicker();
            this.lblVoucherDate = new System.Windows.Forms.Label();
            this.txt_invoice_no = new System.Windows.Forms.TextBox();
            this.lblVoucherNo = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.flowFooterButtons.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelGridHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_journal)).BeginInit();
            this.panelBalance.SuspendLayout();
            this.grpVoucherInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.panelFooter);
            this.panelMain.Controls.Add(this.panelContent);
            this.panelMain.Controls.Add(this.grpVoucherInformation);
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Name = "panelMain";
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelFooter.Controls.Add(this.flowFooterButtons);
            resources.ApplyResources(this.panelFooter, "panelFooter");
            this.panelFooter.Name = "panelFooter";
            // 
            // flowFooterButtons
            // 
            this.flowFooterButtons.Controls.Add(this.btn_add_line);
            this.flowFooterButtons.Controls.Add(this.btn_new);
            this.flowFooterButtons.Controls.Add(this.btn_save_draft);
            this.flowFooterButtons.Controls.Add(this.btn_save);
            this.flowFooterButtons.Controls.Add(this.btn_post_new);
            this.flowFooterButtons.Controls.Add(this.btn_print);
            this.flowFooterButtons.Controls.Add(this.btn_close);
            resources.ApplyResources(this.flowFooterButtons, "flowFooterButtons");
            this.flowFooterButtons.Name = "flowFooterButtons";
            // 
            // btn_add_line
            // 
            resources.ApplyResources(this.btn_add_line, "btn_add_line");
            this.btn_add_line.BackColor = System.Drawing.Color.White;
            this.btn_add_line.Name = "btn_add_line";
            this.btn_add_line.UseVisualStyleBackColor = false;
            this.btn_add_line.Click += new System.EventHandler(this.btn_add_line_Click);
            // 
            // btn_new
            // 
            resources.ApplyResources(this.btn_new, "btn_new");
            this.btn_new.BackColor = System.Drawing.Color.White;
            this.btn_new.Name = "btn_new";
            this.btn_new.UseVisualStyleBackColor = false;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_save_draft
            // 
            resources.ApplyResources(this.btn_save_draft, "btn_save_draft");
            this.btn_save_draft.BackColor = System.Drawing.Color.White;
            this.btn_save_draft.Name = "btn_save_draft";
            this.btn_save_draft.UseVisualStyleBackColor = false;
            this.btn_save_draft.Click += new System.EventHandler(this.btn_save_draft_Click);
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
            this.btn_save.BackColor = System.Drawing.Color.MidnightBlue;
            this.btn_save.ForeColor = System.Drawing.Color.White;
            this.btn_save.Name = "btn_save";
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_post_new
            // 
            resources.ApplyResources(this.btn_post_new, "btn_post_new");
            this.btn_post_new.BackColor = System.Drawing.Color.White;
            this.btn_post_new.Name = "btn_post_new";
            this.btn_post_new.UseVisualStyleBackColor = false;
            this.btn_post_new.Click += new System.EventHandler(this.btn_post_new_Click);
            // 
            // btn_print
            // 
            resources.ApplyResources(this.btn_print, "btn_print");
            this.btn_print.BackColor = System.Drawing.Color.White;
            this.btn_print.Name = "btn_print";
            this.btn_print.UseVisualStyleBackColor = false;
            this.btn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // btn_close
            // 
            resources.ApplyResources(this.btn_close, "btn_close");
            this.btn_close.BackColor = System.Drawing.Color.White;
            this.btn_close.Name = "btn_close";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.panelGridHost);
            this.panelContent.Controls.Add(this.panelBalance);
            resources.ApplyResources(this.panelContent, "panelContent");
            this.panelContent.Name = "panelContent";
            // 
            // panelGridHost
            // 
            this.panelGridHost.BackColor = System.Drawing.Color.White;
            this.panelGridHost.Controls.Add(this.grid_journal);
            resources.ApplyResources(this.panelGridHost, "panelGridHost");
            this.panelGridHost.Name = "panelGridHost";
            // 
            // grid_journal
            // 
            this.grid_journal.AllowUserToAddRows = false;
            this.grid_journal.AllowUserToDeleteRows = false;
            this.grid_journal.AllowUserToResizeRows = false;
            this.grid_journal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_journal.BackgroundColor = System.Drawing.Color.White;
            this.grid_journal.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            resources.ApplyResources(this.grid_journal, "grid_journal");
            this.grid_journal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRowNo,
            this.account,
            this.colAccountType,
            this.description,
            this.cost_center,
            this.debit_amount,
            this.credit_amount,
            this.colRemove});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid_journal.DefaultCellStyle = dataGridViewCellStyle8;
            this.grid_journal.EnableHeadersVisualStyles = false;
            this.grid_journal.GridColor = System.Drawing.Color.Gainsboro;
            this.grid_journal.MultiSelect = false;
            this.grid_journal.Name = "grid_journal";
            this.grid_journal.RowHeadersVisible = false;
            this.grid_journal.RowTemplate.Height = 30;
            this.grid_journal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grid_journal.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_journal_CellContentClick);
            this.grid_journal.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_journal_CellEndEdit);
            this.grid_journal.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_journal_CellFormatting);
            this.grid_journal.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grid_journal_CellValidating);
            this.grid_journal.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_journal_CurrentCellDirtyStateChanged);
            this.grid_journal.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grid_journal_DefaultValuesNeeded);
            this.grid_journal.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grid_journal_EditingControlShowing);
            this.grid_journal.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grid_journal_RowsAdded);
            this.grid_journal.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.grid_journal_RowsRemoved);
            this.grid_journal.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grid_journal_UserDeletingRow);
            this.grid_journal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_journal_KeyDown);
            // 
            // colRowNo
            // 
            this.colRowNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRowNo.FillWeight = 30F;
            resources.ApplyResources(this.colRowNo, "colRowNo");
            this.colRowNo.Name = "colRowNo";
            this.colRowNo.ReadOnly = true;
            this.colRowNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colRowNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // account
            // 
            this.account.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.account.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.account.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.account, "account");
            this.account.Name = "account";
            this.account.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colAccountType
            // 
            this.colAccountType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.colAccountType, "colAccountType");
            this.colAccountType.Name = "colAccountType";
            this.colAccountType.ReadOnly = true;
            this.colAccountType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAccountType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // description
            // 
            this.description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.description, "description");
            this.description.Name = "description";
            this.description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cost_center
            // 
            this.cost_center.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cost_center.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.cost_center.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.cost_center, "cost_center");
            this.cost_center.Name = "cost_center";
            this.cost_center.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // debit_amount
            // 
            this.debit_amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            this.debit_amount.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.debit_amount, "debit_amount");
            this.debit_amount.Name = "debit_amount";
            this.debit_amount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.debit_amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // credit_amount
            // 
            this.credit_amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            this.credit_amount.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.credit_amount, "credit_amount");
            this.credit_amount.Name = "credit_amount";
            this.credit_amount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.credit_amount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colRemove
            // 
            this.colRemove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            this.colRemove.DefaultCellStyle = dataGridViewCellStyle7;
            this.colRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.colRemove, "colRemove");
            this.colRemove.Name = "colRemove";
            this.colRemove.Text = "Remove";
            this.colRemove.UseColumnTextForButtonValue = true;
            // 
            // panelBalance
            // 
            this.panelBalance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelBalance.Controls.Add(this.lbl_line_count_title);
            this.panelBalance.Controls.Add(this.lbl_line_count);
            this.panelBalance.Controls.Add(this.lbl_balance_status);
            this.panelBalance.Controls.Add(this.lbl_difference_title);
            this.panelBalance.Controls.Add(this.txt_difference);
            this.panelBalance.Controls.Add(this.lbl_cr_title);
            this.panelBalance.Controls.Add(this.txt_cr_total);
            this.panelBalance.Controls.Add(this.lbl_dr_title);
            this.panelBalance.Controls.Add(this.txt_dr_total);
            resources.ApplyResources(this.panelBalance, "panelBalance");
            this.panelBalance.Name = "panelBalance";
            // 
            // lbl_line_count_title
            // 
            resources.ApplyResources(this.lbl_line_count_title, "lbl_line_count_title");
            this.lbl_line_count_title.Name = "lbl_line_count_title";
            // 
            // lbl_line_count
            // 
            resources.ApplyResources(this.lbl_line_count, "lbl_line_count");
            this.lbl_line_count.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lbl_line_count.Name = "lbl_line_count";
            // 
            // lbl_balance_status
            // 
            resources.ApplyResources(this.lbl_balance_status, "lbl_balance_status");
            this.lbl_balance_status.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbl_balance_status.Name = "lbl_balance_status";
            // 
            // lbl_difference_title
            // 
            resources.ApplyResources(this.lbl_difference_title, "lbl_difference_title");
            this.lbl_difference_title.Name = "lbl_difference_title";
            // 
            // txt_difference
            // 
            this.txt_difference.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txt_difference, "txt_difference");
            this.txt_difference.ForeColor = System.Drawing.Color.DarkRed;
            this.txt_difference.Name = "txt_difference";
            this.txt_difference.ReadOnly = true;
            // 
            // lbl_cr_title
            // 
            resources.ApplyResources(this.lbl_cr_title, "lbl_cr_title");
            this.lbl_cr_title.Name = "lbl_cr_title";
            // 
            // txt_cr_total
            // 
            this.txt_cr_total.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txt_cr_total, "txt_cr_total");
            this.txt_cr_total.ForeColor = System.Drawing.Color.MidnightBlue;
            this.txt_cr_total.Name = "txt_cr_total";
            this.txt_cr_total.ReadOnly = true;
            // 
            // lbl_dr_title
            // 
            resources.ApplyResources(this.lbl_dr_title, "lbl_dr_title");
            this.lbl_dr_title.Name = "lbl_dr_title";
            // 
            // txt_dr_total
            // 
            this.txt_dr_total.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.txt_dr_total, "txt_dr_total");
            this.txt_dr_total.ForeColor = System.Drawing.Color.MidnightBlue;
            this.txt_dr_total.Name = "txt_dr_total";
            this.txt_dr_total.ReadOnly = true;
            // 
            // grpVoucherInformation
            // 
            this.grpVoucherInformation.BackColor = System.Drawing.Color.White;
            this.grpVoucherInformation.Controls.Add(this.btn_load_template);
            this.grpVoucherInformation.Controls.Add(this.btn_attachment);
            this.grpVoucherInformation.Controls.Add(this.txt_narration);
            this.grpVoucherInformation.Controls.Add(this.lblNarration);
            this.grpVoucherInformation.Controls.Add(this.txt_reference_no);
            this.grpVoucherInformation.Controls.Add(this.lblReferenceNo);
            this.grpVoucherInformation.Controls.Add(this.cmb_voucher_type);
            this.grpVoucherInformation.Controls.Add(this.lblVoucherType);
            this.grpVoucherInformation.Controls.Add(this.txt_entry_date);
            this.grpVoucherInformation.Controls.Add(this.lblVoucherDate);
            this.grpVoucherInformation.Controls.Add(this.txt_invoice_no);
            this.grpVoucherInformation.Controls.Add(this.lblVoucherNo);
            resources.ApplyResources(this.grpVoucherInformation, "grpVoucherInformation");
            this.grpVoucherInformation.Name = "grpVoucherInformation";
            this.grpVoucherInformation.TabStop = false;
            // 
            // btn_load_template
            // 
            this.btn_load_template.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btn_load_template, "btn_load_template");
            this.btn_load_template.Name = "btn_load_template";
            this.btn_load_template.UseVisualStyleBackColor = false;
            this.btn_load_template.Click += new System.EventHandler(this.btn_load_template_Click);
            // 
            // btn_attachment
            // 
            this.btn_attachment.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btn_attachment, "btn_attachment");
            this.btn_attachment.Name = "btn_attachment";
            this.btn_attachment.UseVisualStyleBackColor = false;
            this.btn_attachment.Click += new System.EventHandler(this.btn_attachment_Click);
            // 
            // txt_narration
            // 
            resources.ApplyResources(this.txt_narration, "txt_narration");
            this.txt_narration.Name = "txt_narration";
            // 
            // lblNarration
            // 
            resources.ApplyResources(this.lblNarration, "lblNarration");
            this.lblNarration.Name = "lblNarration";
            // 
            // txt_reference_no
            // 
            resources.ApplyResources(this.txt_reference_no, "txt_reference_no");
            this.txt_reference_no.Name = "txt_reference_no";
            // 
            // lblReferenceNo
            // 
            resources.ApplyResources(this.lblReferenceNo, "lblReferenceNo");
            this.lblReferenceNo.Name = "lblReferenceNo";
            // 
            // cmb_voucher_type
            // 
            this.cmb_voucher_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_voucher_type.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_voucher_type, "cmb_voucher_type");
            this.cmb_voucher_type.Name = "cmb_voucher_type";
            // 
            // lblVoucherType
            // 
            resources.ApplyResources(this.lblVoucherType, "lblVoucherType");
            this.lblVoucherType.Name = "lblVoucherType";
            // 
            // txt_entry_date
            // 
            this.txt_entry_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.txt_entry_date, "txt_entry_date");
            this.txt_entry_date.Name = "txt_entry_date";
            // 
            // lblVoucherDate
            // 
            resources.ApplyResources(this.lblVoucherDate, "lblVoucherDate");
            this.lblVoucherDate.Name = "lblVoucherDate";
            // 
            // txt_invoice_no
            // 
            resources.ApplyResources(this.txt_invoice_no, "txt_invoice_no");
            this.txt_invoice_no.Name = "txt_invoice_no";
            this.txt_invoice_no.ReadOnly = true;
            // 
            // lblVoucherNo
            // 
            resources.ApplyResources(this.lblVoucherNo, "lblVoucherNo");
            this.lblVoucherNo.Name = "lblVoucherNo";
            // 
            // frm_journal_entries
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelMain);
            this.KeyPreview = true;
            this.Name = "frm_journal_entries";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_journal_entries_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_journal_entries_KeyDown);
            this.panelMain.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
            this.flowFooterButtons.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelGridHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_journal)).EndInit();
            this.panelBalance.ResumeLayout(false);
            this.panelBalance.PerformLayout();
            this.grpVoucherInformation.ResumeLayout(false);
            this.grpVoucherInformation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.FlowLayoutPanel flowFooterButtons;
        private System.Windows.Forms.Button btn_add_line;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Button btn_save_draft;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_post_new;
        private System.Windows.Forms.Button btn_print;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelGridHost;
        private System.Windows.Forms.DataGridView grid_journal;
        private System.Windows.Forms.Panel panelBalance;
        private System.Windows.Forms.GroupBox grpVoucherInformation;
        private System.Windows.Forms.Button btn_load_template;
        private System.Windows.Forms.Button btn_attachment;
        private System.Windows.Forms.TextBox txt_narration;
        private System.Windows.Forms.Label lblNarration;
        private System.Windows.Forms.TextBox txt_reference_no;
        private System.Windows.Forms.Label lblReferenceNo;
        private System.Windows.Forms.ComboBox cmb_voucher_type;
        private System.Windows.Forms.Label lblVoucherType;
        private System.Windows.Forms.DateTimePicker txt_entry_date;
        private System.Windows.Forms.Label lblVoucherDate;
        private System.Windows.Forms.TextBox txt_invoice_no;
        private System.Windows.Forms.Label lblVoucherNo;
        private System.Windows.Forms.Label lbl_line_count_title;
        private System.Windows.Forms.Label lbl_line_count;
        private System.Windows.Forms.Label lbl_balance_status;
        private System.Windows.Forms.Label lbl_difference_title;
        private System.Windows.Forms.TextBox txt_difference;
        private System.Windows.Forms.Label lbl_cr_title;
        private System.Windows.Forms.TextBox txt_cr_total;
        private System.Windows.Forms.Label lbl_dr_title;
        private System.Windows.Forms.TextBox txt_dr_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowNo;
        private System.Windows.Forms.DataGridViewComboBoxColumn account;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountType;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewComboBoxColumn cost_center;
        private System.Windows.Forms.DataGridViewTextBoxColumn debit_amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn credit_amount;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
    }
}