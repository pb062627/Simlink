namespace SIM_API_LINKS
{
    partial class UCResultSummary
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
            this.grdResultGrid = new DevExpress.XtraGrid.GridControl();
            this.grdResultView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colResult_Label = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVarResultType_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colResultDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colElement_Label = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsListVar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colImportResultDetail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnImportCSV = new DevExpress.XtraEditors.SimpleButton();
            this.btnCSV = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdResultGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdResultView)).BeginInit();
            this.SuspendLayout();
            // 
            // grdResultGrid
            // 
            this.grdResultGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdResultGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdResultGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grdResultGrid.Location = new System.Drawing.Point(0, 4);
            this.grdResultGrid.MainView = this.grdResultView;
            this.grdResultGrid.Margin = new System.Windows.Forms.Padding(4);
            this.grdResultGrid.Name = "grdResultGrid";
            this.grdResultGrid.Size = new System.Drawing.Size(1027, 442);
            this.grdResultGrid.TabIndex = 1;
            this.grdResultGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdResultView});
            // 
            // grdResultView
            // 
            this.grdResultView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colResult_Label,
            this.colVarResultType_FK,
            this.colResultDescription,
            this.colElement_Label,
            this.colIsListVar,
            this.colImportResultDetail});
            this.grdResultView.GridControl = this.grdResultGrid;
            this.grdResultView.Name = "grdResultView";
            this.grdResultView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.grdResultView.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            this.grdResultView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.grdResultView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.grdResultView_InitNewRow);
            this.grdResultView.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.grdDVView_InvalidRowException);
            this.grdResultView.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdDVView_ValidateRow);
            this.grdResultView.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.grdResultView_RowUpdated);
            // 
            // colResult_Label
            // 
            this.colResult_Label.Caption = "Result Label";
            this.colResult_Label.FieldName = "Result_Label";
            this.colResult_Label.Name = "colResult_Label";
            this.colResult_Label.Visible = true;
            this.colResult_Label.VisibleIndex = 0;
            // 
            // colVarResultType_FK
            // 
            this.colVarResultType_FK.Caption = "Var Result Type";
            this.colVarResultType_FK.FieldName = "VarResultType_FK";
            this.colVarResultType_FK.Name = "colVarResultType_FK";
            this.colVarResultType_FK.Visible = true;
            this.colVarResultType_FK.VisibleIndex = 1;
            // 
            // colResultDescription
            // 
            this.colResultDescription.Caption = "Result Description";
            this.colResultDescription.FieldName = "Result_Description";
            this.colResultDescription.Name = "colResultDescription";
            this.colResultDescription.Visible = true;
            this.colResultDescription.VisibleIndex = 2;
            // 
            // colElement_Label
            // 
            this.colElement_Label.Caption = "Element Label";
            this.colElement_Label.FieldName = "Element_Label";
            this.colElement_Label.Name = "colElement_Label";
            this.colElement_Label.Visible = true;
            this.colElement_Label.VisibleIndex = 3;
            // 
            // colIsListVar
            // 
            this.colIsListVar.Caption = "List Variable?";
            this.colIsListVar.FieldName = "IsListVar";
            this.colIsListVar.Name = "colIsListVar";
            this.colIsListVar.Visible = true;
            this.colIsListVar.VisibleIndex = 4;
            // 
            // colImportResultDetail
            // 
            this.colImportResultDetail.Caption = "Import Result Detail";
            this.colImportResultDetail.FieldName = "ImportResultDetail";
            this.colImportResultDetail.Name = "colImportResultDetail";
            this.colImportResultDetail.Visible = true;
            this.colImportResultDetail.VisibleIndex = 5;
            // 
            // btnImportCSV
            // 
            this.btnImportCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportCSV.Location = new System.Drawing.Point(920, 453);
            this.btnImportCSV.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(105, 43);
            this.btnImportCSV.TabIndex = 3;
            this.btnImportCSV.Text = "Import...";
            this.btnImportCSV.Click += new System.EventHandler(this.btnImportCSV_Click);
            // 
            // btnCSV
            // 
            this.btnCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCSV.Location = new System.Drawing.Point(800, 453);
            this.btnCSV.Margin = new System.Windows.Forms.Padding(4);
            this.btnCSV.Name = "btnCSV";
            this.btnCSV.Size = new System.Drawing.Size(112, 43);
            this.btnCSV.TabIndex = 3;
            this.btnCSV.Text = "CSV Template";
            this.btnCSV.ToolTip = "Download import file template";
            this.btnCSV.Click += new System.EventHandler(this.btnCSV_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(0, 453);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(101, 43);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // UCResultSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCSV);
            this.Controls.Add(this.btnImportCSV);
            this.Controls.Add(this.grdResultGrid);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCResultSummary";
            this.Size = new System.Drawing.Size(1029, 500);
            ((System.ComponentModel.ISupportInitialize)(this.grdResultGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdResultView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdResultGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView grdResultView;
        private DevExpress.XtraGrid.Columns.GridColumn colResult_Label;
        private DevExpress.XtraGrid.Columns.GridColumn colVarResultType_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colResultDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colElement_Label;
        private DevExpress.XtraEditors.SimpleButton btnImportCSV;
        private DevExpress.XtraEditors.SimpleButton btnCSV;
        private DevExpress.XtraGrid.Columns.GridColumn colIsListVar;
        private DevExpress.XtraGrid.Columns.GridColumn colImportResultDetail;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
    }
}
