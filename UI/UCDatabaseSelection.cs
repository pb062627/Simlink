using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;
using CH2M;

namespace SIM_API_LINKS
{
    public partial class UCDatabaseSelection : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void ButtonClickedEventHandler(object sender, EventArgs e);
        public event ButtonClickedEventHandler OnConnectDBClick;
        private bool _blnCanConnect;

        /// <summary>
        /// check if db is connected
        /// </summary>
        public bool IsDBConnected
        {
            get {return _blnCanConnect;}
        }
        public UCDatabaseSelection()
        {
            InitializeComponent();
            
        }
        /// <summary>
        /// Load existing config
        /// </summary>
        public void LoadExistingConfig()
        {
            string strConnectionString = Properties.Settings.Default.ConnectionString;
            if (Properties.Settings.Default.DatabaseType == "OLEDB")
            {
                string strDBPath = "";
                string[] astrVal = strConnectionString.Split(';');
                foreach(string strVal in astrVal)
                {
                    if (strVal.IndexOf("Data Source=")>=0)
                    {
                        strDBPath = strVal.Split('=')[1];
                    }
                }
                radMsAccess.Checked = true;
                txtAccessDB.Text = strDBPath;
            }
            else // sql server
            {
                radSQLServer.Checked = true;
                string[] astrVal = strConnectionString.Split(';');
                DataTable dbltables = new DataTable();
                dbltables.Columns.Add("database_name", typeof(string));
                string strFoundDBName = "";
                foreach (string strVal in astrVal)
                {
                    string strServer = ""; string strDB = "";
                    if (strVal.IndexOf("Server=") >= 0)
                    {
                        strServer = strVal.Split('=')[1];
                    }
                    else if (strVal.IndexOf("Database=") >= 0)
                    {
                        strDB = strVal.Split('=')[1];
                        DataRow row = dbltables.NewRow();
                        row["database_name"] = strDB;
                        dbltables.Rows.Add(row); // add new row
                        strFoundDBName = strDB;
                    }
                    if (strServer != "")
                    {
                        cboSelServer.Properties.Items.Add(strServer);
                    }
                }
                dbltables.AcceptChanges();
                cboDatabases.Properties.DataSource = null;
                cboDatabases.Properties.DataSource = dbltables;
                cboDatabases.Properties.ValueMember = "database_name";
                cboDatabases.Properties.DisplayMember = "database_name";
                cboDatabases.EditValue = strFoundDBName;
            }
        }

        /// <summary>
        /// server selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radSQLServer_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (radSQLServer.Checked)
                {
                    SqlDataSourceEnumerator dse = SqlDataSourceEnumerator.Instance;
                    DataTable dt = SqlDataSourceEnumerator.Instance.GetDataSources();
                    cboSelServer.Properties.Items.Clear();
                    foreach (DataRow dr in dt.Rows)
                    {
                        string strItem = dr["ServerName"].ToString() + "\\" + dr["InstanceName"].ToString();
                        cboSelServer.Properties.Items.Add(strItem);
                    }
                    if (cboSelServer.Properties.Items.Count > 0) cboSelServer.SelectedIndex = 0;
                }
                cboSelServer.Enabled = radSQLServer.Checked;
                cboDatabases.Enabled = radSQLServer.Checked;
                lblSelectServer.Enabled = radSQLServer.Checked;
                lblSelectDB.Enabled = radSQLServer.Checked;
                Cursor.Current = Cursors.Default;
            }
            catch
            {
                Commons.ShowMessage("SQL Server might not exist in the current machine");
            }
        }
        /// <summary>
        /// server selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void server_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string servername = cboSelServer.Text;
                string constring = "Server=" + servername + ";Database=master;Trusted_Connection=True;";
                SqlConnection con = new SqlConnection(constring);

                con.Open();
                DataTable dbltables = con.GetSchema("Databases");

                con.Close();
                cboDatabases.Properties.DataSource = dbltables;
                cboDatabases.Properties.DisplayMember = dbltables.Columns["database_name"].ToString();
            }
            catch(Exception ex)
            {
                Commons.ShowMessage("Error connecting to db " + ex.Source + ": " + ex.Message);
            }
        }
        /// <summary>
        /// Ms access selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radMsAccess_CheckedChanged(object sender, EventArgs e)
        {
            lblMsAccess.Enabled = radMsAccess.Checked;
            txtAccessDB.Enabled = radMsAccess.Checked;
            btnBrowseFile.Enabled = radMsAccess.Checked;
        }
        /// <summary>
        /// Browse selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Select Ms Access File (*.mdb;*.accdb)|*.mdb;*.accdb";
            open.Title = "Select SimLink Ms Access database file";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtAccessDB.Text = open.FileName;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (OnConnectDBClick != null)
            {
                string strCNN ="";
                _blnCanConnect = false;
                if (radMsAccess.Checked)
                {
                    strCNN = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + txtAccessDB.Text;
                    try
                    {
                        OleDbConnection cn = new OleDbConnection(strCNN);
                        cn.Open();
                        _blnCanConnect = true;
                    }
                    catch(Exception ex)
                    {
                        _blnCanConnect = false;
                        Commons.ShowMessage("Error connecting to database '" + ex.Source + ": " + ex.Message);

                    }
                }
                else
                {
                    strCNN = "Server=" + cboSelServer.Text + ";Database=" + cboDatabases.Text + ";Trusted_Connection=True;";
                    try
                    {
                        SqlConnection cn = new SqlConnection(strCNN);
                        cn.Open();
                        _blnCanConnect = true;
                    }
                    catch (Exception ex)
                    {
                        _blnCanConnect = false;
                        Commons.ShowMessage("Error connecting to database '" + ex.Source + ": " + ex.Message);
                    }
                }
                if (_blnCanConnect)
                {
                    Properties.Settings.Default.ConnectionString = strCNN;
                    Properties.Settings.Default.DatabaseType = (radMsAccess.Checked ? "OLEDB" : "SQLServer");
                    Properties.Settings.Default.Save(); // save new config

                    Commons.ShowMessage("Successfully connected to database!", MessageBoxIcon.Information);
                }
                OnConnectDBClick(this, e);
            }
        }
        /// <summary>
        /// Key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboSelServer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                server_SelectedIndexChanged(sender, new EventArgs());
            }
        }
    }
}