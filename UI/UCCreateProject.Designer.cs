namespace CH2M
{
    partial class UCCreateProject
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblProjLabel = new DevExpress.XtraEditors.LabelControl();
            this.txtProjectName = new DevExpress.XtraEditors.TextEdit();
            this.lblModelType = new DevExpress.XtraEditors.LabelControl();
            this.cboModelType = new System.Windows.Forms.ComboBox();
            this.lblNote = new DevExpress.XtraEditors.LabelControl();
            this.txtNote = new DevExpress.XtraEditors.TextEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.lblExistingProject = new DevExpress.XtraEditors.LabelControl();
            this.txtFileLocation = new DevExpress.XtraEditors.ButtonEdit();
            this.lblFileLocation = new DevExpress.XtraEditors.LabelControl();
            this.ctrUCEdit = new CH2M.UCEditOpenProject();
            ((System.ComponentModel.ISupportInitialize)(this.txtProjectName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileLocation.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProjLabel
            // 
            this.lblProjLabel.Location = new System.Drawing.Point(26, 53);
            this.lblProjLabel.Name = "lblProjLabel";
            this.lblProjLabel.Size = new System.Drawing.Size(35, 16);
            this.lblProjLabel.TabIndex = 0;
            this.lblProjLabel.Text = "Label:";
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(105, 51);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(666, 22);
            this.txtProjectName.TabIndex = 1;
            // 
            // lblModelType
            // 
            this.lblModelType.Location = new System.Drawing.Point(26, 19);
            this.lblModelType.Name = "lblModelType";
            this.lblModelType.Size = new System.Drawing.Size(71, 16);
            this.lblModelType.TabIndex = 2;
            this.lblModelType.Text = "Model Type:";
            // 
            // cboModelType
            // 
            this.cboModelType.FormattingEnabled = true;
            this.cboModelType.Location = new System.Drawing.Point(105, 15);
            this.cboModelType.Name = "cboModelType";
            this.cboModelType.Size = new System.Drawing.Size(254, 24);
            this.cboModelType.TabIndex = 0;
            // 
            // lblNote
            // 
            this.lblNote.Location = new System.Drawing.Point(26, 120);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(31, 16);
            this.lblNote.TabIndex = 0;
            this.lblNote.Text = "Note:";
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(105, 115);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(666, 22);
            this.txtNote.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(680, 147);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 38);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblExistingProject
            // 
            this.lblExistingProject.Location = new System.Drawing.Point(26, 181);
            this.lblExistingProject.Name = "lblExistingProject";
            this.lblExistingProject.Size = new System.Drawing.Size(98, 16);
            this.lblExistingProject.TabIndex = 7;
            this.lblExistingProject.Text = "Existing Projects:";
            // 
            // txtFileLocation
            // 
            this.txtFileLocation.Location = new System.Drawing.Point(105, 83);
            this.txtFileLocation.Name = "txtFileLocation";
            this.txtFileLocation.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtFileLocation.Size = new System.Drawing.Size(666, 22);
            this.txtFileLocation.TabIndex = 2;
            this.txtFileLocation.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtFileLocation_ButtonClick);
            // 
            // lblFileLocation
            // 
            this.lblFileLocation.Location = new System.Drawing.Point(26, 88);
            this.lblFileLocation.Name = "lblFileLocation";
            this.lblFileLocation.Size = new System.Drawing.Size(60, 16);
            this.lblFileLocation.TabIndex = 0;
            this.lblFileLocation.Text = "Model file:";
            // 
            // ctrUCEdit
            // 
            this.ctrUCEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrUCEdit.HideOpenButton = true;
            this.ctrUCEdit.IsOpenProject = true;
            this.ctrUCEdit.Location = new System.Drawing.Point(16, 193);
            this.ctrUCEdit.Margin = new System.Windows.Forms.Padding(5);
            this.ctrUCEdit.MySimlink = null;
            this.ctrUCEdit.Name = "ctrUCEdit";
            this.ctrUCEdit.Size = new System.Drawing.Size(906, 361);
            this.ctrUCEdit.TabIndex = 6;
            // 
            // UCCreateProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtFileLocation);
            this.Controls.Add(this.lblExistingProject);
            this.Controls.Add(this.ctrUCEdit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cboModelType);
            this.Controls.Add(this.lblModelType);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.txtProjectName);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblFileLocation);
            this.Controls.Add(this.lblProjLabel);
            this.Name = "UCCreateProject";
            this.Size = new System.Drawing.Size(979, 575);
            ((System.ComponentModel.ISupportInitialize)(this.txtProjectName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNote.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileLocation.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblProjLabel;
        private DevExpress.XtraEditors.TextEdit txtProjectName;
        private DevExpress.XtraEditors.LabelControl lblModelType;
        private System.Windows.Forms.ComboBox cboModelType;
        private DevExpress.XtraEditors.LabelControl lblNote;
        private DevExpress.XtraEditors.TextEdit txtNote;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.LabelControl lblExistingProject;
        public UCEditOpenProject ctrUCEdit;
        private DevExpress.XtraEditors.ButtonEdit txtFileLocation;
        private DevExpress.XtraEditors.LabelControl lblFileLocation;
    }
}
