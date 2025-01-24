using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using POS.DLL;

namespace pos
{
    public partial class FrmDbBackup : Form
    {
        SqlCommand cmd;
        SqlConnection sqlCon;
        //string conString = "Data Source=.; Initial Catalog=pos_db; User Id=ksa; Password=khybersoft;";
        
        public FrmDbBackup()
        {
            InitializeComponent();
            //sqlCon = new SqlConnection(conString);
            sqlCon = new SqlConnection(dbConnection.ConnectionString);
            sqlCon.Open();
        }

        private void FrmDbBackup_Load(object sender, EventArgs e)
        {
            LoadBackinfo();
            if (LinkLabel1.Text == string.Empty)
            {
                LinkLabel1.Text = "Click To Set Directory Path";
            }
        }

        private void LoadBackinfo()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            DataSet dsData = new DataSet();
            cmd = new SqlCommand("sp_DatabaseBackup", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ACTIONTYPE", "BACKUP_INFO");
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dsData);
            if (dsData.Tables.Count > 0)
            {
                if (dsData.Tables[0].Rows.Count > 0)
                {
                    LinkLabel1.Text = dsData.Tables[0].Rows[0]["LOCATION"].ToString();
                    txtNoOfFiles.Text = dsData.Tables[0].Rows[0]["NoOfFiles"].ToString();
                    txtSpan.Text = dsData.Tables[0].Rows[0]["DayInterval"].ToString();
                    txtDbName.Text = dsData.Tables[0].Rows[0]["DATABASENAME"].ToString();
                    linkLabel2.Text = dsData.Tables[0].Rows[0]["LOCATION"].ToString();
                    linkLabel3.Text = dsData.Tables[0].Rows[0]["DATABASENAME"].ToString() + "-" + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".bak";
                }
                if (dsData.Tables[1].Rows.Count > 0)
                {
                    lblLastBackupInfo.Text = string.Format("Last backup was taken {0} at {1} in location {2}.", dsData.Tables[1].Rows[0]["BackupType"].ToString(),
                        dsData.Tables[1].Rows[0]["BackupDate"].ToString(), dsData.Tables[1].Rows[0]["Location"].ToString());
                }
                else
                    lblLastBackupInfo.Text = "No Backups !!!";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (LinkLabel1.Text == "Click To Set Directory Path")
            {
                MessageBox.Show("Click To Set Directory Path", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (txtSpan.Text == string.Empty)
            {
                MessageBox.Show("Enter how many last backup files required ", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                int numFlag;
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                cmd = new SqlCommand("sp_DatabaseBackup", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTIONTYPE", "INSERT_BACKUP_INFO");
                cmd.Parameters.AddWithValue("@DatabaseName", txtDbName.Text); // Your Database Name
                cmd.Parameters.AddWithValue("@Location", LinkLabel1.Text);
                cmd.Parameters.AddWithValue("@DayInterval", txtSpan.Text);
                cmd.Parameters.AddWithValue("@SoftwareDate", Convert.ToDateTime(DateTimePicker1.Text));
                cmd.Parameters.AddWithValue("@NoOfFiles", txtNoOfFiles.Text);
                numFlag = cmd.ExecuteNonQuery();

                if (numFlag > 0)
                {
                    MessageBox.Show("Data saved successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBackinfo();
                }
                else
                {
                    MessageBox.Show("Data not saved. Plaese Try Again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            ProgressBarEx5.Value += 1;
            if (ProgressBarEx5.Value == 100)
            {
                ProgressBarEx5.Visible = false;
                Timer1.Stop();
                Panel1.Visible = false;
                ProgressBarEx5.Text = "Finished";
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FolderBrowserDialog1.ShowDialog();
            LinkLabel1.Text = FolderBrowserDialog1.SelectedPath;
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (linkLabel2.Text == string.Empty)
            {
                MessageBox.Show("Please Set Backup Setting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (linkLabel3.Text == string.Empty)
            {
                MessageBox.Show("Please Set Backup Setting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string filaPath;
                if (!linkLabel2.Text.EndsWith(@"\"))
                {
                    filaPath = linkLabel2.Text + @"\" + linkLabel3.Text;
                }
                else
                {
                    filaPath = linkLabel2.Text + linkLabel3.Text;
                }
                int numFlag;
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                cmd = new SqlCommand("sp_DatabaseBackup", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTIONTYPE", "DB_BACKUP");
                cmd.Parameters.AddWithValue("@DATABASE", txtDbName.Text); // Your Database Name
                cmd.Parameters.AddWithValue("@FILEPATH", filaPath);
                cmd.Parameters.AddWithValue("@BackupName", linkLabel3.Text);
                cmd.Parameters.AddWithValue("@SoftwareDate", Convert.ToDateTime(DateTimePicker1.Text));
                cmd.Parameters.AddWithValue("@Type", "Manually");
                numFlag = cmd.ExecuteNonQuery();
                DataTable dtLoc = new DataTable();
                cmd = new SqlCommand("sp_DatabaseBackup", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTIONTYPE", "REMOVE_LOCATION");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtLoc);
                for (int i = 0; i < dtLoc.Rows.Count; i++)
                {
                    string delLoc = dtLoc.Rows[i][0].ToString();
                    string filepath = delLoc;
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);

                    }
                }
                if (numFlag > 0)
                {
                    Panel1.Visible = true;
                    Panel1.Location = new Point(58, 138);
                    Panel1.Height = 117;
                    Panel1.Width = 446;
                    ProgressBarEx5.Visible = true;
                    ProgressBarEx5.Value = 0;
                    Timer1.Start();
                    LoadBackinfo();
                }
                else
                {
                    MessageBox.Show("Plaese Try Again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSpan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 46 || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
