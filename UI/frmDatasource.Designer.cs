namespace SIM_API_LINKS
{
    partial class frmDatasource
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
            this.ctrDBConnection = new SIM_API_LINKS.UCDatabaseSelection();
            this.SuspendLayout();
            // 
            // ctrDBConnection
            // 
            this.ctrDBConnection.Location = new System.Drawing.Point(6, 8);
            this.ctrDBConnection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ctrDBConnection.Name = "ctrDBConnection";
            this.ctrDBConnection.Size = new System.Drawing.Size(666, 262);
            this.ctrDBConnection.TabIndex = 0;
            this.ctrDBConnection.OnConnectDBClick += new SIM_API_LINKS.UCDatabaseSelection.ButtonClickedEventHandler(this.ctrDBConnection_OnConnectDBClick);
            this.ctrDBConnection.Load += new System.EventHandler(this.frmDatasource_Load);
            // 
            // frmDatasource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 267);
            this.Controls.Add(this.ctrDBConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDatasource";
            this.Text = "Select datasource";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDatasource_FormClosing);
            this.Load += new System.EventHandler(this.frmDatasource_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UCDatabaseSelection ctrDBConnection;
    }
}