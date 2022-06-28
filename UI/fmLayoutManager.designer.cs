namespace CH2M.SimLink.MainUI
{
    partial class fmLayoutManager
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
            this.lbLayouts = new DevExpress.XtraEditors.ListBoxControl();
            this.btnDeleteLayout = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveLayout = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnNewLayout = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.lbLayouts)).BeginInit();
            this.SuspendLayout();
            // 
            // lbLayouts
            // 
            this.lbLayouts.ItemAutoHeight = true;
            this.lbLayouts.Location = new System.Drawing.Point(12, 12);
            this.lbLayouts.Name = "lbLayouts";
            this.lbLayouts.Size = new System.Drawing.Size(113, 278);
            this.lbLayouts.TabIndex = 0;
            this.lbLayouts.SelectedIndexChanged += new System.EventHandler(this.lbLayouts_SelectedIndexChanged);
            this.lbLayouts.SelectedValueChanged += new System.EventHandler(this.lbLayouts_SelectedValueChanged);
            this.lbLayouts.DoubleClick += new System.EventHandler(this.lbLayouts_DoubleClick);
            // 
            // btnDeleteLayout
            // 
            this.btnDeleteLayout.Location = new System.Drawing.Point(131, 68);
            this.btnDeleteLayout.Name = "btnDeleteLayout";
            this.btnDeleteLayout.Size = new System.Drawing.Size(76, 22);
            this.btnDeleteLayout.TabIndex = 5;
            this.btnDeleteLayout.Text = "Delete Layout";
            this.btnDeleteLayout.Click += new System.EventHandler(this.btnDeleteLayout_Click);
            // 
            // btnSaveLayout
            // 
            this.btnSaveLayout.Location = new System.Drawing.Point(131, 12);
            this.btnSaveLayout.Name = "btnSaveLayout";
            this.btnSaveLayout.Size = new System.Drawing.Size(76, 22);
            this.btnSaveLayout.TabIndex = 6;
            this.btnSaveLayout.Text = "Save Layout";
            this.btnSaveLayout.Click += new System.EventHandler(this.btnSaveLayout_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(131, 268);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 22);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnNewLayout
            // 
            this.btnNewLayout.Location = new System.Drawing.Point(131, 40);
            this.btnNewLayout.Name = "btnNewLayout";
            this.btnNewLayout.Size = new System.Drawing.Size(76, 22);
            this.btnNewLayout.TabIndex = 7;
            this.btnNewLayout.Text = "New Layout";
            this.btnNewLayout.Click += new System.EventHandler(this.btnNewLayout_Click);
            // 
            // fmLayoutManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 300);
            this.Controls.Add(this.btnNewLayout);
            this.Controls.Add(this.lbLayouts);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSaveLayout);
            this.Controls.Add(this.btnDeleteLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "fmLayoutManager";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Layouts";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmLayoutManager_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmLayoutManager_FormClosed);
            this.Load += new System.EventHandler(this.fmLayoutManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lbLayouts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl lbLayouts;
        private DevExpress.XtraEditors.SimpleButton btnDeleteLayout;
        private DevExpress.XtraEditors.SimpleButton btnSaveLayout;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnNewLayout;
    }
}