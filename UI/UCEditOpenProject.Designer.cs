namespace CH2M
{
    partial class UCEditOpenProject
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
            this.grdProject = new DevExpress.XtraGrid.GridControl();
            this.viewProject = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProjectID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProjectName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProjectType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreatedOn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNote = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colModified = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnOpenEdit = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdProject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewProject)).BeginInit();
            this.SuspendLayout();
            // 
            // grdProject
            // 
            this.grdProject.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdProject.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdProject.Location = new System.Drawing.Point(7, 12);
            this.grdProject.MainView = this.viewProject;
            this.grdProject.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdProject.Name = "grdProject";
            this.grdProject.Size = new System.Drawing.Size(751, 267);
            this.grdProject.TabIndex = 0;
            this.grdProject.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.viewProject});
            // 
            // viewProject
            // 
            this.viewProject.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colProjectID,
            this.colProjectName,
            this.colProjectType,
            this.colCreatedOn,
            this.colNote,
            this.colModified});
            this.viewProject.GridControl = this.grdProject;
            this.viewProject.Name = "viewProject";
            this.viewProject.OptionsBehavior.Editable = false;
            this.viewProject.OptionsView.ShowGroupPanel = false;
            // 
            // colProjectID
            // 
            this.colProjectID.Caption = "ProjectID";
            this.colProjectID.FieldName = "ProjID";
            this.colProjectID.Name = "colProjectID";
            // 
            // colProjectName
            // 
            this.colProjectName.Caption = "Label";
            this.colProjectName.FieldName = "ProjLabel";
            this.colProjectName.Name = "colProjectName";
            this.colProjectName.Visible = true;
            this.colProjectName.VisibleIndex = 0;
            // 
            // colProjectType
            // 
            this.colProjectType.Caption = "Model Type";
            this.colProjectType.FieldName = "val";
            this.colProjectType.Name = "colProjectType";
            this.colProjectType.Visible = true;
            this.colProjectType.VisibleIndex = 1;
            // 
            // colCreatedOn
            // 
            this.colCreatedOn.Caption = "Date Create";
            this.colCreatedOn.FieldName = "DateCreated";
            this.colCreatedOn.Name = "colCreatedOn";
            this.colCreatedOn.Visible = true;
            this.colCreatedOn.VisibleIndex = 2;
            // 
            // colNote
            // 
            this.colNote.Caption = "Notes";
            this.colNote.FieldName = "ModelDescription";
            this.colNote.Name = "colNote";
            this.colNote.Visible = true;
            this.colNote.VisibleIndex = 3;
            // 
            // colModified
            // 
            this.colModified.Caption = "Date Last Modified";
            this.colModified.FieldName = "LastModified";
            this.colModified.Name = "colModified";
            this.colModified.Visible = true;
            this.colModified.VisibleIndex = 4;
            // 
            // btnOpenEdit
            // 
            this.btnOpenEdit.Location = new System.Drawing.Point(7, 287);
            this.btnOpenEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenEdit.Name = "btnOpenEdit";
            this.btnOpenEdit.Size = new System.Drawing.Size(114, 39);
            this.btnOpenEdit.TabIndex = 1;
            this.btnOpenEdit.Text = "Open/Edit";
            this.btnOpenEdit.Click += new System.EventHandler(this.btnOpenEdit_Click);
            // 
            // UCEditOpenProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnOpenEdit);
            this.Controls.Add(this.grdProject);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UCEditOpenProject";
            this.Size = new System.Drawing.Size(833, 590);
            ((System.ComponentModel.ISupportInitialize)(this.grdProject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewProject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnOpenEdit;
        private DevExpress.XtraGrid.Columns.GridColumn colProjectID;
        private DevExpress.XtraGrid.Columns.GridColumn colProjectName;
        private DevExpress.XtraGrid.Columns.GridColumn colProjectType;
        private DevExpress.XtraGrid.Columns.GridColumn colCreatedOn;
        private DevExpress.XtraGrid.Columns.GridColumn colNote;
        private DevExpress.XtraGrid.Columns.GridColumn colModified;
        public DevExpress.XtraGrid.GridControl grdProject;
        public DevExpress.XtraGrid.Views.Grid.GridView viewProject;
    }
}
