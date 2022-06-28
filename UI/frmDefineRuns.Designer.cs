namespace SIM_API_LINKS
{
    partial class frmDefineRuns
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
            this.grdDefineRun = new DevExpress.XtraGrid.GridControl();
            this.grdViewScenario = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colScenarioID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colScenarioLabel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDNA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHasBeenRun = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colScenStart = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colScenEnd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDateEvaluated = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportScenarioFile = new DevExpress.XtraEditors.SimpleButton();
            this.btnDownloadSampleCSV = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnRunScenario = new DevExpress.XtraEditors.SimpleButton();
            this.btnPasteFromExcel = new DevExpress.XtraEditors.SimpleButton();
            this.lblInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdDefineRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewScenario)).BeginInit();
            this.SuspendLayout();
            // 
            // grdDefineRun
            // 
            this.grdDefineRun.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDefineRun.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdDefineRun.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdDefineRun.Location = new System.Drawing.Point(5, 4);
            this.grdDefineRun.MainView = this.grdViewScenario;
            this.grdDefineRun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdDefineRun.Name = "grdDefineRun";
            this.grdDefineRun.Size = new System.Drawing.Size(883, 334);
            this.grdDefineRun.TabIndex = 0;
            this.grdDefineRun.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewScenario});
            // 
            // grdViewScenario
            // 
            this.grdViewScenario.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colScenarioID,
            this.colScenarioLabel,
            this.colDNA,
            this.colHasBeenRun,
            this.colScenStart,
            this.colScenEnd,
            this.colDateEvaluated});
            this.grdViewScenario.GridControl = this.grdDefineRun;
            this.grdViewScenario.Name = "grdViewScenario";
            this.grdViewScenario.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.grdViewScenario.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            this.grdViewScenario.OptionsSelection.MultiSelect = true;
            this.grdViewScenario.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.grdViewScenario.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.grdViewScenario_SelectionChanged);
            this.grdViewScenario.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.grdViewScenario_InvalidRowException);
            this.grdViewScenario.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdViewScenario_ValidateRow);
            this.grdViewScenario.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.grdViewScenario_RowUpdated);
            // 
            // colScenarioID
            // 
            this.colScenarioID.Caption = "ID";
            this.colScenarioID.FieldName = "ScenarioID";
            this.colScenarioID.Name = "colScenarioID";
            this.colScenarioID.OptionsColumn.AllowEdit = false;
            this.colScenarioID.OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;
            this.colScenarioID.Visible = true;
            this.colScenarioID.VisibleIndex = 0;
            // 
            // colScenarioLabel
            // 
            this.colScenarioLabel.Caption = "Label";
            this.colScenarioLabel.FieldName = "ScenarioLabel";
            this.colScenarioLabel.Name = "colScenarioLabel";
            this.colScenarioLabel.Visible = true;
            this.colScenarioLabel.VisibleIndex = 1;
            // 
            // colDNA
            // 
            this.colDNA.Caption = "DNA";
            this.colDNA.FieldName = "DNA";
            this.colDNA.Name = "colDNA";
            this.colDNA.Visible = true;
            this.colDNA.VisibleIndex = 2;
            // 
            // colHasBeenRun
            // 
            this.colHasBeenRun.Caption = "Has been run";
            this.colHasBeenRun.FieldName = "HasBeenRun";
            this.colHasBeenRun.Name = "colHasBeenRun";
            this.colHasBeenRun.Visible = true;
            this.colHasBeenRun.VisibleIndex = 3;
            // 
            // colScenStart
            // 
            this.colScenStart.Caption = "Scenario Start";
            this.colScenStart.FieldName = "ScenStart";
            this.colScenStart.Name = "colScenStart";
            this.colScenStart.Visible = true;
            this.colScenStart.VisibleIndex = 4;
            // 
            // colScenEnd
            // 
            this.colScenEnd.Caption = "Scenario End";
            this.colScenEnd.FieldName = "ScenEnd";
            this.colScenEnd.Name = "colScenEnd";
            this.colScenEnd.Visible = true;
            this.colScenEnd.VisibleIndex = 5;
            // 
            // colDateEvaluated
            // 
            this.colDateEvaluated.Caption = "Date Evaluated";
            this.colDateEvaluated.FieldName = "DateEvaluated";
            this.colDateEvaluated.Name = "colDateEvaluated";
            this.colDateEvaluated.Visible = true;
            this.colDateEvaluated.VisibleIndex = 6;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(789, 346);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(99, 46);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnImportScenarioFile
            // 
            this.btnImportScenarioFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImportScenarioFile.Location = new System.Drawing.Point(177, 346);
            this.btnImportScenarioFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnImportScenarioFile.Name = "btnImportScenarioFile";
            this.btnImportScenarioFile.Size = new System.Drawing.Size(79, 46);
            this.btnImportScenarioFile.TabIndex = 1;
            this.btnImportScenarioFile.Text = "Import...";
            this.btnImportScenarioFile.ToolTip = "Import CSV file.";
            this.btnImportScenarioFile.Click += new System.EventHandler(this.btnImportScenarioFile_Click);
            // 
            // btnDownloadSampleCSV
            // 
            this.btnDownloadSampleCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDownloadSampleCSV.Location = new System.Drawing.Point(91, 346);
            this.btnDownloadSampleCSV.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDownloadSampleCSV.Name = "btnDownloadSampleCSV";
            this.btnDownloadSampleCSV.Size = new System.Drawing.Size(79, 46);
            this.btnDownloadSampleCSV.TabIndex = 1;
            this.btnDownloadSampleCSV.Text = "CSV";
            this.btnDownloadSampleCSV.ToolTip = "Download import file template";
            this.btnDownloadSampleCSV.Click += new System.EventHandler(this.btnDownloadSampleCSV_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(5, 346);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(79, 46);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.ToolTip = "Download import file template";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRunScenario
            // 
            this.btnRunScenario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunScenario.Location = new System.Drawing.Point(682, 346);
            this.btnRunScenario.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRunScenario.Name = "btnRunScenario";
            this.btnRunScenario.Size = new System.Drawing.Size(99, 46);
            this.btnRunScenario.TabIndex = 1;
            this.btnRunScenario.Text = "Run Scenario";
            this.btnRunScenario.ToolTip = "Run first selected scenario";
            this.btnRunScenario.Click += new System.EventHandler(this.btnRunScenario_Click);
            // 
            // btnPasteFromExcel
            // 
            this.btnPasteFromExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPasteFromExcel.Location = new System.Drawing.Point(262, 346);
            this.btnPasteFromExcel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPasteFromExcel.Name = "btnPasteFromExcel";
            this.btnPasteFromExcel.Size = new System.Drawing.Size(79, 46);
            this.btnPasteFromExcel.TabIndex = 1;
            this.btnPasteFromExcel.Text = "Paste";
            this.btnPasteFromExcel.ToolTip = "Paste data from Ms Excel...";
            this.btnPasteFromExcel.Click += new System.EventHandler(this.btnPasteFromExcel_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Tahoma", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(347, 361);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(62, 21);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "Info...";
            this.lblInfo.Visible = false;
            // 
            // frmDefineRuns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 399);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnDownloadSampleCSV);
            this.Controls.Add(this.btnRunScenario);
            this.Controls.Add(this.btnPasteFromExcel);
            this.Controls.Add(this.btnImportScenarioFile);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdDefineRun);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmDefineRuns";
            this.Text = "Scenario - Define Runs";
            this.Load += new System.EventHandler(this.frmDefineRuns_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDefineRuns_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdDefineRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewScenario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdDefineRun;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewScenario;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnImportScenarioFile;
        private DevExpress.XtraGrid.Columns.GridColumn colScenarioLabel;
        private DevExpress.XtraGrid.Columns.GridColumn colDNA;
        private DevExpress.XtraGrid.Columns.GridColumn colHasBeenRun;
        private DevExpress.XtraGrid.Columns.GridColumn colScenStart;
        private DevExpress.XtraGrid.Columns.GridColumn colScenEnd;
        private DevExpress.XtraGrid.Columns.GridColumn colDateEvaluated;
        private DevExpress.XtraGrid.Columns.GridColumn colScenarioID;
        private DevExpress.XtraEditors.SimpleButton btnDownloadSampleCSV;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnRunScenario;
        private DevExpress.XtraEditors.SimpleButton btnPasteFromExcel;
        private System.Windows.Forms.Label lblInfo;
    }
}