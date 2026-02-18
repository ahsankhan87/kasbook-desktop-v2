namespace pos
{
    partial class FrmDbBackup
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
            this.components = new System.ComponentModel.Container();
            this.Label21 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.Label9 = new System.Windows.Forms.Label();
            this.ProgressBarEx5 = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDbName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.Label8 = new System.Windows.Forms.Label();
            this.LinkLabel1 = new System.Windows.Forms.LinkLabel();
            this.Label7 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.linkLabel2 = new System.Windows.Forms.Label();
            this.linkLabel3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpan = new System.Windows.Forms.TextBox();
            this.txtNoOfFiles = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblLastBackupInfo = new System.Windows.Forms.Label();
            this.Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label21
            // 
            this.Label21.BackColor = System.Drawing.Color.SteelBlue;
            this.Label21.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label21.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label21.ForeColor = System.Drawing.Color.White;
            this.Label21.Location = new System.Drawing.Point(0, 0);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(789, 25);
            this.Label21.TabIndex = 3;
            this.Label21.Text = "BACKUP  SETTINGS";
            this.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.SteelBlue;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.MistyRose;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MistyRose;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(611, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(28, 24);
            this.btnClose.TabIndex = 1284;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "X                           ";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Panel1
            // 
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel1.Controls.Add(this.Label9);
            this.Panel1.Controls.Add(this.ProgressBarEx5);
            this.Panel1.Location = new System.Drawing.Point(419, 218);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(446, 117);
            this.Panel1.TabIndex = 1354;
            this.Panel1.Visible = false;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Label9.Location = new System.Drawing.Point(9, 23);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(317, 23);
            this.Label9.TabIndex = 33;
            this.Label9.Text = "Database Backup Initialize, Please Wait...";
            // 
            // ProgressBarEx5
            // 
            this.ProgressBarEx5.Location = new System.Drawing.Point(12, 50);
            this.ProgressBarEx5.Name = "ProgressBarEx5";
            this.ProgressBarEx5.Size = new System.Drawing.Size(421, 18);
            this.ProgressBarEx5.TabIndex = 33;
            this.ProgressBarEx5.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(24, 332);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 23);
            this.label6.TabIndex = 1360;
            this.label6.Text = "Database Name";
            // 
            // txtDbName
            // 
            this.txtDbName.Location = new System.Drawing.Point(194, 117);
            this.txtDbName.Name = "txtDbName";
            this.txtDbName.Size = new System.Drawing.Size(289, 29);
            this.txtDbName.TabIndex = 1359;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(24, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 23);
            this.label5.TabIndex = 1358;
            this.label5.Text = "Database Name : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(24, 307);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 23);
            this.label4.TabIndex = 1356;
            this.label4.Text = "Backup Path";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(423, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 23);
            this.label3.TabIndex = 1355;
            this.label3.Text = "Date : ";
            // 
            // DateTimePicker1
            // 
            this.DateTimePicker1.Enabled = false;
            this.DateTimePicker1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateTimePicker1.Location = new System.Drawing.Point(492, 30);
            this.DateTimePicker1.Name = "DateTimePicker1";
            this.DateTimePicker1.Size = new System.Drawing.Size(147, 29);
            this.DateTimePicker1.TabIndex = 1348;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Label8.Location = new System.Drawing.Point(8, 273);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(196, 32);
            this.Label8.TabIndex = 1353;
            this.Label8.Text = "Manually Backup";
            // 
            // LinkLabel1
            // 
            this.LinkLabel1.AutoSize = true;
            this.LinkLabel1.Location = new System.Drawing.Point(173, 63);
            this.LinkLabel1.Name = "LinkLabel1";
            this.LinkLabel1.Size = new System.Drawing.Size(0, 23);
            this.LinkLabel1.TabIndex = 1352;
            this.LinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Label7.Location = new System.Drawing.Point(24, 63);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(172, 23);
            this.Label7.TabIndex = 1351;
            this.Label7.Text = "Save Directory Path : ";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSave.Location = new System.Drawing.Point(223, 156);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(89, 34);
            this.btnSave.TabIndex = 1349;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.BackColor = System.Drawing.Color.White;
            this.btnBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackup.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnBackup.Image = global::pos.Properties.Resources.database;
            this.btnBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBackup.Location = new System.Drawing.Point(496, 359);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(137, 33);
            this.btnBackup.TabIndex = 1350;
            this.btnBackup.Text = "Backup Database";
            this.btnBackup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBackup.UseVisualStyleBackColor = false;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.SteelBlue;
            this.label13.Dock = System.Windows.Forms.DockStyle.Right;
            this.label13.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(787, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(2, 417);
            this.label13.TabIndex = 1362;
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.Color.SteelBlue;
            this.label22.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label22.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(0, 440);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(787, 2);
            this.label22.TabIndex = 1363;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.SteelBlue;
            this.label20.Dock = System.Windows.Forms.DockStyle.Left;
            this.label20.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(0, 25);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(2, 415);
            this.label20.TabIndex = 1364;
            // 
            // Timer1
            // 
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FolderBrowserDialog1
            // 
            this.FolderBrowserDialog1.SelectedPath = "C:\\ProgramData\\PRM System\\Backup\\";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(134, 307);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(0, 23);
            this.linkLabel2.TabIndex = 1365;
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(134, 332);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(0, 23);
            this.linkLabel3.TabIndex = 1366;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(24, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(252, 23);
            this.label1.TabIndex = 1367;
            this.label1.Text = "Auto Backup Time Span (Days): ";
            // 
            // txtSpan
            // 
            this.txtSpan.Location = new System.Drawing.Point(223, 89);
            this.txtSpan.MaxLength = 2;
            this.txtSpan.Name = "txtSpan";
            this.txtSpan.Size = new System.Drawing.Size(57, 29);
            this.txtSpan.TabIndex = 1368;
            this.txtSpan.Text = "0";
            this.txtSpan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSpan_KeyPress);
            // 
            // txtNoOfFiles
            // 
            this.txtNoOfFiles.Location = new System.Drawing.Point(492, 89);
            this.txtNoOfFiles.MaxLength = 2;
            this.txtNoOfFiles.Name = "txtNoOfFiles";
            this.txtNoOfFiles.Size = new System.Drawing.Size(57, 29);
            this.txtNoOfFiles.TabIndex = 1370;
            this.txtNoOfFiles.Text = "5";
            this.txtNoOfFiles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSpan_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(347, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 23);
            this.label2.TabIndex = 1369;
            this.label2.Text = "No Of Backup To Keep : ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label10.Location = new System.Drawing.Point(8, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(160, 32);
            this.label10.TabIndex = 1371;
            this.label10.Text = "Backup Setup";
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.LightGray;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(42, 194);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(540, 2);
            this.label11.TabIndex = 1372;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label12.Location = new System.Drawing.Point(8, 196);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(188, 32);
            this.label12.TabIndex = 1373;
            this.label12.Text = "Last Backup Info";
            // 
            // lblLastBackupInfo
            // 
            this.lblLastBackupInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastBackupInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblLastBackupInfo.Location = new System.Drawing.Point(24, 223);
            this.lblLastBackupInfo.Name = "lblLastBackupInfo";
            this.lblLastBackupInfo.Size = new System.Drawing.Size(506, 45);
            this.lblLastBackupInfo.TabIndex = 1374;
            this.lblLastBackupInfo.Text = "Last backup was taken {0} at {1} in location {2}.";
            // 
            // FrmDbBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(789, 442);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.lblLastBackupInfo);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtNoOfFiles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtDbName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.DateTimePicker1);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.LinkLabel1);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.Label21);
            this.Controls.Add(this.txtSpan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmDbBackup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDbBackup";
            this.Load += new System.EventHandler(this.FrmDbBackup_Load);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label21;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.ProgressBar ProgressBarEx5;
        internal System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDbName;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Button btnBackup;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.LinkLabel LinkLabel1;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.Label label22;
        internal System.Windows.Forms.Label label20;
        internal System.Windows.Forms.Timer Timer1;
        internal System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1;
        private System.Windows.Forms.Label linkLabel2;
        private System.Windows.Forms.Label linkLabel3;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSpan;
        internal System.Windows.Forms.DateTimePicker DateTimePicker1;
        private System.Windows.Forms.TextBox txtNoOfFiles;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Label lblLastBackupInfo;
    }
}