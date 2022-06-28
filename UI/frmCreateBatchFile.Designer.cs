namespace SIM_API_LINKS
{
    partial class frmCreateBatchFile
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
            this.lblNoOfRun = new DevExpress.XtraEditors.LabelControl();
            this.txtNoOfRun = new DevExpress.XtraEditors.TextEdit();
            this.chkUnrunOnly = new DevExpress.XtraEditors.CheckEdit();
            this.chkDefaultLocation = new DevExpress.XtraEditors.CheckEdit();
            this.lblPath = new DevExpress.XtraEditors.LabelControl();
            this.btnEditBrowsePath = new DevExpress.XtraEditors.ButtonEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnCreate = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoOfRun.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUnrunOnly.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDefaultLocation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEditBrowsePath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNoOfRun
            // 
            this.lblNoOfRun.Location = new System.Drawing.Point(13, 15);
            this.lblNoOfRun.Name = "lblNoOfRun";
            this.lblNoOfRun.Size = new System.Drawing.Size(94, 16);
            this.lblNoOfRun.TabIndex = 0;
            this.lblNoOfRun.Text = "Number of runs:";
            // 
            // txtNoOfRun
            // 
            this.txtNoOfRun.EditValue = "1";
            this.txtNoOfRun.Location = new System.Drawing.Point(113, 12);
            this.txtNoOfRun.Name = "txtNoOfRun";
            this.txtNoOfRun.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtNoOfRun.Size = new System.Drawing.Size(81, 22);
            this.txtNoOfRun.TabIndex = 1;
            // 
            // chkUnrunOnly
            // 
            this.chkUnrunOnly.EditValue = true;
            this.chkUnrunOnly.Location = new System.Drawing.Point(246, 11);
            this.chkUnrunOnly.Name = "chkUnrunOnly";
            this.chkUnrunOnly.Properties.Caption = "Unrun only";
            this.chkUnrunOnly.Size = new System.Drawing.Size(123, 20);
            this.chkUnrunOnly.TabIndex = 2;
            // 
            // chkDefaultLocation
            // 
            this.chkDefaultLocation.EditValue = true;
            this.chkDefaultLocation.Location = new System.Drawing.Point(13, 43);
            this.chkDefaultLocation.Name = "chkDefaultLocation";
            this.chkDefaultLocation.Properties.Caption = "Default location";
            this.chkDefaultLocation.Size = new System.Drawing.Size(123, 20);
            this.chkDefaultLocation.TabIndex = 2;
            this.chkDefaultLocation.CheckedChanged += new System.EventHandler(this.chkDefaultLocation_CheckedChanged);
            // 
            // lblPath
            // 
            this.lblPath.Enabled = false;
            this.lblPath.Location = new System.Drawing.Point(77, 71);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(30, 16);
            this.lblPath.TabIndex = 3;
            this.lblPath.Text = "Path:";
            // 
            // btnEditBrowsePath
            // 
            this.btnEditBrowsePath.Enabled = false;
            this.btnEditBrowsePath.Location = new System.Drawing.Point(113, 69);
            this.btnEditBrowsePath.Name = "btnEditBrowsePath";
            this.btnEditBrowsePath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btnEditBrowsePath.Size = new System.Drawing.Size(358, 22);
            this.btnEditBrowsePath.TabIndex = 4;
            this.btnEditBrowsePath.DoubleClick += new System.EventHandler(this.btnEditBrowsePath_DoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(382, 108);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 36);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(281, 108);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(88, 36);
            this.btnCreate.TabIndex = 6;
            this.btnCreate.Text = "Create";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // frmCreateBatchFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 158);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnEditBrowsePath);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.chkDefaultLocation);
            this.Controls.Add(this.chkUnrunOnly);
            this.Controls.Add(this.txtNoOfRun);
            this.Controls.Add(this.lblNoOfRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCreateBatchFile";
            this.Text = "Create Batch Files";
            this.Load += new System.EventHandler(this.frmCreateBatchFile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtNoOfRun.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUnrunOnly.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDefaultLocation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEditBrowsePath.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblNoOfRun;
        private DevExpress.XtraEditors.TextEdit txtNoOfRun;
        private DevExpress.XtraEditors.CheckEdit chkUnrunOnly;
        private DevExpress.XtraEditors.CheckEdit chkDefaultLocation;
        private DevExpress.XtraEditors.LabelControl lblPath;
        private DevExpress.XtraEditors.ButtonEdit btnEditBrowsePath;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnCreate;
    }
}