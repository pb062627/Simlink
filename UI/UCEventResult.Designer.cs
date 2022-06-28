namespace SIM_API_LINKS
{
    partial class UCEventResult
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
            this.grdEventGrid = new DevExpress.XtraGrid.GridControl();
            this.grdEventView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colResultTS_or_Event_ID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInterEvent_Threshold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEventFunctionID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colThreshold_Inst = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsOver_Threshold_Inst = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colThreshold_Cumulative = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsOver_Threshold_Cumulative = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCalcValueInExcessOfThreshold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCategoryID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnCSV = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportCSV = new DevExpress.XtraEditors.SimpleButton();
            this.Delete = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdEventGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdEventView)).BeginInit();
            this.SuspendLayout();
            // 
            // grdEventGrid
            // 
            this.grdEventGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdEventGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdEventGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdEventGrid.Location = new System.Drawing.Point(3, 4);
            this.grdEventGrid.MainView = this.grdEventView;
            this.grdEventGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdEventGrid.Name = "grdEventGrid";
            this.grdEventGrid.Size = new System.Drawing.Size(696, 206);
            this.grdEventGrid.TabIndex = 2;
            this.grdEventGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdEventView});
            // 
            // grdEventView
            // 
            this.grdEventView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colResultTS_or_Event_ID_FK,
            this.colInterEvent_Threshold,
            this.colEventFunctionID,
            this.colThreshold_Inst,
            this.colIsOver_Threshold_Inst,
            this.colThreshold_Cumulative,
            this.colIsOver_Threshold_Cumulative,
            this.colCalcValueInExcessOfThreshold,
            this.colCategoryID_FK});
            this.grdEventView.GridControl = this.grdEventGrid;
            this.grdEventView.Name = "grdEventView";
            this.grdEventView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.grdEventView.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            this.grdEventView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.grdEventView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.grdEventView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.grdEventView_InitNewRow);
            this.grdEventView.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.grdPerformanceView_InvalidRowException);
            this.grdEventView.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdPerformanceView_ValidateRow);
            this.grdEventView.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.grdPerformanceView_RowUpdated);
            // 
            // colResultTS_or_Event_ID_FK
            // 
            this.colResultTS_or_Event_ID_FK.Caption = "Timeseries Label";
            this.colResultTS_or_Event_ID_FK.FieldName = "ResultTS_or_Event_ID_FK";
            this.colResultTS_or_Event_ID_FK.Name = "colResultTS_or_Event_ID_FK";
            this.colResultTS_or_Event_ID_FK.Visible = true;
            this.colResultTS_or_Event_ID_FK.VisibleIndex = 0;
            // 
            // colInterEvent_Threshold
            // 
            this.colInterEvent_Threshold.Caption = "Time between Events (sec)";
            this.colInterEvent_Threshold.FieldName = "InterEvent_Threshold";
            this.colInterEvent_Threshold.Name = "colInterEvent_Threshold";
            this.colInterEvent_Threshold.Visible = true;
            this.colInterEvent_Threshold.VisibleIndex = 1;
            // 
            // colEventFunctionID
            // 
            this.colEventFunctionID.Caption = "Calc On Timeseries";
            this.colEventFunctionID.FieldName = "EventFunctionID";
            this.colEventFunctionID.Name = "colEventFunctionID";
            this.colEventFunctionID.Visible = true;
            this.colEventFunctionID.VisibleIndex = 6;
            // 
            // colThreshold_Inst
            // 
            this.colThreshold_Inst.Caption = "Instantaneous Threshold";
            this.colThreshold_Inst.FieldName = "Threshold_Inst";
            this.colThreshold_Inst.Name = "colThreshold_Inst";
            this.colThreshold_Inst.Visible = true;
            this.colThreshold_Inst.VisibleIndex = 2;
            // 
            // colIsOver_Threshold_Inst
            // 
            this.colIsOver_Threshold_Inst.Caption = "Trig. if Over Thres (inst.)";
            this.colIsOver_Threshold_Inst.FieldName = "IsOver_Threshold_Inst";
            this.colIsOver_Threshold_Inst.Name = "colIsOver_Threshold_Inst";
            this.colIsOver_Threshold_Inst.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.colIsOver_Threshold_Inst.Visible = true;
            this.colIsOver_Threshold_Inst.VisibleIndex = 3;
            // 
            // colThreshold_Cumulative
            // 
            this.colThreshold_Cumulative.Caption = "Cumulative Threshold";
            this.colThreshold_Cumulative.FieldName = "Threshold_Cumulative";
            this.colThreshold_Cumulative.Name = "colThreshold_Cumulative";
            this.colThreshold_Cumulative.Visible = true;
            this.colThreshold_Cumulative.VisibleIndex = 4;
            // 
            // colIsOver_Threshold_Cumulative
            // 
            this.colIsOver_Threshold_Cumulative.Caption = "Trig. If Over Thres (Cumulative)";
            this.colIsOver_Threshold_Cumulative.FieldName = "IsOver_Threshold_Cumulative";
            this.colIsOver_Threshold_Cumulative.Name = "colIsOver_Threshold_Cumulative";
            this.colIsOver_Threshold_Cumulative.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.colIsOver_Threshold_Cumulative.Visible = true;
            this.colIsOver_Threshold_Cumulative.VisibleIndex = 5;
            // 
            // colCalcValueInExcessOfThreshold
            // 
            this.colCalcValueInExcessOfThreshold.Caption = "Calc. Value In Excess of Threshold";
            this.colCalcValueInExcessOfThreshold.FieldName = "CalcValueInExcessOfThreshold";
            this.colCalcValueInExcessOfThreshold.Name = "colCalcValueInExcessOfThreshold";
            this.colCalcValueInExcessOfThreshold.Visible = true;
            this.colCalcValueInExcessOfThreshold.VisibleIndex = 7;
            // 
            // colCategoryID_FK
            // 
            this.colCategoryID_FK.Caption = "CategoryID_FK";
            this.colCategoryID_FK.FieldName = "CategoryID_FK";
            this.colCategoryID_FK.Name = "colCategoryID_FK";
            // 
            // btnCSV
            // 
            this.btnCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCSV.Location = new System.Drawing.Point(474, 218);
            this.btnCSV.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCSV.Name = "btnCSV";
            this.btnCSV.Size = new System.Drawing.Size(112, 43);
            this.btnCSV.TabIndex = 8;
            this.btnCSV.Text = "CSV Template";
            this.btnCSV.ToolTip = "Download import file template";
            this.btnCSV.Click += new System.EventHandler(this.btnCSV_Click);
            // 
            // btnImportCSV
            // 
            this.btnImportCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportCSV.Location = new System.Drawing.Point(594, 218);
            this.btnImportCSV.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(105, 43);
            this.btnImportCSV.TabIndex = 9;
            this.btnImportCSV.Text = "Import...";
            this.btnImportCSV.Click += new System.EventHandler(this.btnImportCSV_Click);
            // 
            // Delete
            // 
            this.Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Delete.Location = new System.Drawing.Point(3, 218);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(82, 43);
            this.Delete.TabIndex = 10;
            this.Delete.Text = "Delete";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // UCEventResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.btnCSV);
            this.Controls.Add(this.btnImportCSV);
            this.Controls.Add(this.grdEventGrid);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UCEventResult";
            this.Size = new System.Drawing.Size(742, 377);
            ((System.ComponentModel.ISupportInitialize)(this.grdEventGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdEventView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdEventGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView grdEventView;
        private DevExpress.XtraGrid.Columns.GridColumn colResultTS_or_Event_ID_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colInterEvent_Threshold;
        private DevExpress.XtraGrid.Columns.GridColumn colThreshold_Inst;
        private DevExpress.XtraGrid.Columns.GridColumn colIsOver_Threshold_Inst;
        private DevExpress.XtraGrid.Columns.GridColumn colThreshold_Cumulative;
        private DevExpress.XtraGrid.Columns.GridColumn colIsOver_Threshold_Cumulative;
        private DevExpress.XtraEditors.SimpleButton btnCSV;
        private DevExpress.XtraEditors.SimpleButton btnImportCSV;
        private DevExpress.XtraGrid.Columns.GridColumn colEventFunctionID;
        private DevExpress.XtraGrid.Columns.GridColumn colCalcValueInExcessOfThreshold;
        private DevExpress.XtraGrid.Columns.GridColumn colCategoryID_FK;
        private DevExpress.XtraEditors.SimpleButton Delete;
    }
}
