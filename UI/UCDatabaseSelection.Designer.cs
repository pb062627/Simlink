namespace SIM_API_LINKS
{
    partial class UCDatabaseSelection
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
            this.radMsAccess = new System.Windows.Forms.RadioButton();
            this.radSQLServer = new System.Windows.Forms.RadioButton();
            this.lblMsAccess = new System.Windows.Forms.Label();
            this.txtAccessDB = new DevExpress.XtraEditors.TextEdit();
            this.btnBrowseFile = new DevExpress.XtraEditors.SimpleButton();
            this.lblSelectServer = new System.Windows.Forms.Label();
            this.lblSelectDB = new System.Windows.Forms.Label();
            this.btnConnect = new DevExpress.XtraEditors.SimpleButton();
            this.cboSelServer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboDatabases = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccessDB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSelServer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDatabases.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // radMsAccess
            // 
            this.radMsAccess.AutoSize = true;
            this.radMsAccess.Checked = true;
            this.radMsAccess.Location = new System.Drawing.Point(14, 15);
            this.radMsAccess.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radMsAccess.Name = "radMsAccess";
            this.radMsAccess.Size = new System.Drawing.Size(177, 21);
            this.radMsAccess.TabIndex = 0;
            this.radMsAccess.TabStop = true;
            this.radMsAccess.Text = "Use Ms Access Database";
            this.radMsAccess.UseVisualStyleBackColor = true;
            this.radMsAccess.CheckedChanged += new System.EventHandler(this.radMsAccess_CheckedChanged);
            // 
            // radSQLServer
            // 
            this.radSQLServer.AutoSize = true;
            this.radSQLServer.Location = new System.Drawing.Point(14, 100);
            this.radSQLServer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radSQLServer.Name = "radSQLServer";
            this.radSQLServer.Size = new System.Drawing.Size(124, 21);
            this.radSQLServer.TabIndex = 0;
            this.radSQLServer.Text = "Use SQL Server";
            this.radSQLServer.UseVisualStyleBackColor = true;
            this.radSQLServer.CheckedChanged += new System.EventHandler(this.radSQLServer_CheckedChanged);
            // 
            // lblMsAccess
            // 
            this.lblMsAccess.AutoSize = true;
            this.lblMsAccess.Location = new System.Drawing.Point(64, 47);
            this.lblMsAccess.Name = "lblMsAccess";
            this.lblMsAccess.Size = new System.Drawing.Size(108, 17);
            this.lblMsAccess.TabIndex = 1;
            this.lblMsAccess.Text = "Select database:";
            // 
            // txtAccessDB
            // 
            this.txtAccessDB.Location = new System.Drawing.Point(174, 44);
            this.txtAccessDB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAccessDB.Name = "txtAccessDB";
            this.txtAccessDB.Size = new System.Drawing.Size(440, 22);
            this.txtAccessDB.TabIndex = 0;
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Location = new System.Drawing.Point(616, 43);
            this.btnBrowseFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(36, 25);
            this.btnBrowseFile.TabIndex = 1;
            this.btnBrowseFile.Text = "...";
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // lblSelectServer
            // 
            this.lblSelectServer.AutoSize = true;
            this.lblSelectServer.Enabled = false;
            this.lblSelectServer.Location = new System.Drawing.Point(64, 133);
            this.lblSelectServer.Name = "lblSelectServer";
            this.lblSelectServer.Size = new System.Drawing.Size(91, 17);
            this.lblSelectServer.TabIndex = 1;
            this.lblSelectServer.Text = "Select server:";
            // 
            // lblSelectDB
            // 
            this.lblSelectDB.AutoSize = true;
            this.lblSelectDB.Enabled = false;
            this.lblSelectDB.Location = new System.Drawing.Point(64, 166);
            this.lblSelectDB.Name = "lblSelectDB";
            this.lblSelectDB.Size = new System.Drawing.Size(108, 17);
            this.lblSelectDB.TabIndex = 1;
            this.lblSelectDB.Text = "Select database:";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(547, 196);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(105, 38);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cboSelServer
            // 
            this.cboSelServer.Location = new System.Drawing.Point(177, 127);
            this.cboSelServer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboSelServer.Name = "cboSelServer";
            this.cboSelServer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSelServer.Size = new System.Drawing.Size(475, 22);
            this.cboSelServer.TabIndex = 2;
            this.cboSelServer.SelectedIndexChanged += new System.EventHandler(this.server_SelectedIndexChanged);
            this.cboSelServer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboSelServer_KeyDown);
            // 
            // cboDatabases
            // 
            this.cboDatabases.Location = new System.Drawing.Point(176, 159);
            this.cboDatabases.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboDatabases.Name = "cboDatabases";
            this.cboDatabases.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDatabases.Size = new System.Drawing.Size(475, 22);
            this.cboDatabases.TabIndex = 3;
            // 
            // UCDatabaseSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboDatabases);
            this.Controls.Add(this.cboSelServer);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnBrowseFile);
            this.Controls.Add(this.lblSelectDB);
            this.Controls.Add(this.txtAccessDB);
            this.Controls.Add(this.lblSelectServer);
            this.Controls.Add(this.lblMsAccess);
            this.Controls.Add(this.radSQLServer);
            this.Controls.Add(this.radMsAccess);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UCDatabaseSelection";
            this.Size = new System.Drawing.Size(659, 246);
            ((System.ComponentModel.ISupportInitialize)(this.txtAccessDB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSelServer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDatabases.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radMsAccess;
        private System.Windows.Forms.RadioButton radSQLServer;
        private System.Windows.Forms.Label lblMsAccess;
        private DevExpress.XtraEditors.TextEdit txtAccessDB;
        private DevExpress.XtraEditors.SimpleButton btnBrowseFile;
        private System.Windows.Forms.Label lblSelectServer;
        private System.Windows.Forms.Label lblSelectDB;
        private DevExpress.XtraEditors.SimpleButton btnConnect;
        private DevExpress.XtraEditors.ComboBoxEdit cboSelServer;
        private DevExpress.XtraEditors.LookUpEdit cboDatabases;
    }
}