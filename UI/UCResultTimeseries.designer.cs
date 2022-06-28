namespace SIM_API_LINKS
{
    partial class UCResultTimseries
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
            this.grdResultTSGrid = new DevExpress.XtraGrid.GridControl();
            this.grdResultTSView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colResult_Label = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVarResultType_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colResultDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colElement_Label = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colElementLabel2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTS_StartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTS_StartHour = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTS_StartMin = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTS_Interval = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTS_Interval_Unit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBeginPeriodNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFunctionID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFunctionArgs = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsSecondary = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRefTS_ID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEvaluationGroup_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colElementIndex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnCSVTemplate = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportCSV = new DevExpress.XtraEditors.SimpleButton();
            this.lblEditableInfo = new DevExpress.XtraEditors.LabelControl();
            this.cboViewOption = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblViewerOption = new System.Windows.Forms.Label();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdResultTSGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdResultTSView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboViewOption.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grdResultTSGrid
            // 
            this.grdResultTSGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdResultTSGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdResultTSGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grdResultTSGrid.Location = new System.Drawing.Point(0, 32);
            this.grdResultTSGrid.MainView = this.grdResultTSView;
            this.grdResultTSGrid.Margin = new System.Windows.Forms.Padding(4);
            this.grdResultTSGrid.Name = "grdResultTSGrid";
            this.grdResultTSGrid.Size = new System.Drawing.Size(958, 314);
            this.grdResultTSGrid.TabIndex = 1;
            this.grdResultTSGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdResultTSView});
            // 
            // grdResultTSView
            // 
            this.grdResultTSView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colResult_Label,
            this.colVarResultType_FK,
            this.colResultDescription,
            this.colElement_Label,
            this.colElementLabel2,
            this.colTS_StartDate,
            this.colTS_StartHour,
            this.colTS_StartMin,
            this.colTS_Interval,
            this.colTS_Interval_Unit,
            this.colBeginPeriodNo,
            this.colFunctionID_FK,
            this.colFunctionArgs,
            this.colIsSecondary,
            this.colRefTS_ID_FK,
            this.colEvaluationGroup_FK,
            this.colElementIndex});
            this.grdResultTSView.GridControl = this.grdResultTSGrid;
            this.grdResultTSView.Name = "grdResultTSView";
            this.grdResultTSView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.grdResultTSView.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            this.grdResultTSView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.grdResultTSView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.grdResultTSView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.grdResultTSView_InitNewRow);
            this.grdResultTSView.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.grdDVView_InvalidRowException);
            this.grdResultTSView.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdDVView_ValidateRow);
            this.grdResultTSView.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.grdResultTSView_RowUpdated);
            // 
            // colResult_Label
            // 
            this.colResult_Label.Caption = "Label";
            this.colResult_Label.FieldName = "Result_Label";
            this.colResult_Label.Name = "colResult_Label";
            this.colResult_Label.Visible = true;
            this.colResult_Label.VisibleIndex = 0;
            // 
            // colVarResultType_FK
            // 
            this.colVarResultType_FK.Caption = "Result Data Type";
            this.colVarResultType_FK.FieldName = "VarResultType_FK";
            this.colVarResultType_FK.Name = "colVarResultType_FK";
            this.colVarResultType_FK.Visible = true;
            this.colVarResultType_FK.VisibleIndex = 1;
            // 
            // colResultDescription
            // 
            this.colResultDescription.Caption = "Description";
            this.colResultDescription.FieldName = "Result_Description";
            this.colResultDescription.Name = "colResultDescription";
            this.colResultDescription.Visible = true;
            this.colResultDescription.VisibleIndex = 2;
            // 
            // colElement_Label
            // 
            this.colElement_Label.Caption = "Element";
            this.colElement_Label.FieldName = "ElementID_FK";
            this.colElement_Label.Name = "colElement_Label";
            // 
            // colElementLabel2
            // 
            this.colElementLabel2.Caption = "Element Name";
            this.colElementLabel2.FieldName = "Element_Label";
            this.colElementLabel2.Name = "colElementLabel2";
            this.colElementLabel2.Visible = true;
            this.colElementLabel2.VisibleIndex = 13;
            // 
            // colTS_StartDate
            // 
            this.colTS_StartDate.Caption = "Start Date";
            this.colTS_StartDate.FieldName = "TS_StartDate";
            this.colTS_StartDate.Name = "colTS_StartDate";
            this.colTS_StartDate.Visible = true;
            this.colTS_StartDate.VisibleIndex = 3;
            // 
            // colTS_StartHour
            // 
            this.colTS_StartHour.Caption = "Start Hour";
            this.colTS_StartHour.FieldName = "TS_StartHour";
            this.colTS_StartHour.Name = "colTS_StartHour";
            this.colTS_StartHour.Visible = true;
            this.colTS_StartHour.VisibleIndex = 4;
            // 
            // colTS_StartMin
            // 
            this.colTS_StartMin.Caption = "Start Min";
            this.colTS_StartMin.FieldName = "TS_StartMin";
            this.colTS_StartMin.Name = "colTS_StartMin";
            this.colTS_StartMin.Visible = true;
            this.colTS_StartMin.VisibleIndex = 5;
            // 
            // colTS_Interval
            // 
            this.colTS_Interval.Caption = "TS Interval";
            this.colTS_Interval.FieldName = "TS_Interval";
            this.colTS_Interval.Name = "colTS_Interval";
            this.colTS_Interval.Visible = true;
            this.colTS_Interval.VisibleIndex = 6;
            // 
            // colTS_Interval_Unit
            // 
            this.colTS_Interval_Unit.Caption = "Interval Unit";
            this.colTS_Interval_Unit.FieldName = "TS_Interval_Unit";
            this.colTS_Interval_Unit.Name = "colTS_Interval_Unit";
            this.colTS_Interval_Unit.Visible = true;
            this.colTS_Interval_Unit.VisibleIndex = 7;
            // 
            // colBeginPeriodNo
            // 
            this.colBeginPeriodNo.Caption = "Start At Period";
            this.colBeginPeriodNo.FieldName = "BeginPeriodNo";
            this.colBeginPeriodNo.Name = "colBeginPeriodNo";
            this.colBeginPeriodNo.Visible = true;
            this.colBeginPeriodNo.VisibleIndex = 8;
            // 
            // colFunctionID_FK
            // 
            this.colFunctionID_FK.Caption = "Function";
            this.colFunctionID_FK.FieldName = "FunctionID_FK";
            this.colFunctionID_FK.Name = "colFunctionID_FK";
            this.colFunctionID_FK.Visible = true;
            this.colFunctionID_FK.VisibleIndex = 9;
            // 
            // colFunctionArgs
            // 
            this.colFunctionArgs.Caption = "Arguments";
            this.colFunctionArgs.FieldName = "FunctionArgs";
            this.colFunctionArgs.Name = "colFunctionArgs";
            this.colFunctionArgs.Visible = true;
            this.colFunctionArgs.VisibleIndex = 10;
            // 
            // colIsSecondary
            // 
            this.colIsSecondary.Caption = "Is Secondary";
            this.colIsSecondary.FieldName = "IsSecondary";
            this.colIsSecondary.Name = "colIsSecondary";
            this.colIsSecondary.Visible = true;
            this.colIsSecondary.VisibleIndex = 12;
            // 
            // colRefTS_ID_FK
            // 
            this.colRefTS_ID_FK.Caption = "Reference TS Label";
            this.colRefTS_ID_FK.FieldName = "RefTS_ID_FK";
            this.colRefTS_ID_FK.Name = "colRefTS_ID_FK";
            this.colRefTS_ID_FK.Visible = true;
            this.colRefTS_ID_FK.VisibleIndex = 11;
            // 
            // colEvaluationGroup_FK
            // 
            this.colEvaluationGroup_FK.Caption = "EvaluationGroup_FK";
            this.colEvaluationGroup_FK.FieldName = "EvaluationGroup_FK";
            this.colEvaluationGroup_FK.Name = "colEvaluationGroup_FK";
            // 
            // colElementIndex
            // 
            this.colElementIndex.Caption = "ElementIndex";
            this.colElementIndex.FieldName = "ElementIndex";
            this.colElementIndex.Name = "colElementIndex";
            // 
            // btnCSVTemplate
            // 
            this.btnCSVTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCSVTemplate.Enabled = false;
            this.btnCSVTemplate.Location = new System.Drawing.Point(736, 354);
            this.btnCSVTemplate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCSVTemplate.Name = "btnCSVTemplate";
            this.btnCSVTemplate.Size = new System.Drawing.Size(116, 41);
            this.btnCSVTemplate.TabIndex = 7;
            this.btnCSVTemplate.Text = "CSV Template";
            this.btnCSVTemplate.ToolTip = "Download import file template";
            this.btnCSVTemplate.Click += new System.EventHandler(this.btnCSVTemplate_Click);
            // 
            // btnImportCSV
            // 
            this.btnImportCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportCSV.Enabled = false;
            this.btnImportCSV.Location = new System.Drawing.Point(859, 354);
            this.btnImportCSV.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(99, 41);
            this.btnImportCSV.TabIndex = 6;
            this.btnImportCSV.Text = "Import...";
            this.btnImportCSV.Click += new System.EventHandler(this.btnImportCSV_Click);
            // 
            // lblEditableInfo
            // 
            this.lblEditableInfo.Location = new System.Drawing.Point(323, 4);
            this.lblEditableInfo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.lblEditableInfo.Name = "lblEditableInfo";
            this.lblEditableInfo.Size = new System.Drawing.Size(215, 16);
            this.lblEditableInfo.TabIndex = 12;
            this.lblEditableInfo.Text = "Grid is NOT editable in summary view";
            // 
            // cboViewOption
            // 
            this.cboViewOption.Location = new System.Drawing.Point(92, 3);
            this.cboViewOption.Name = "cboViewOption";
            this.cboViewOption.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboViewOption.Properties.Items.AddRange(new object[] {
            "Limited Detail",
            "Detailed"});
            this.cboViewOption.Size = new System.Drawing.Size(226, 22);
            this.cboViewOption.TabIndex = 11;
            this.cboViewOption.SelectedIndexChanged += new System.EventHandler(this.cboViewOption_SelectedIndexChanged);
            // 
            // lblViewerOption
            // 
            this.lblViewerOption.AutoSize = true;
            this.lblViewerOption.Location = new System.Drawing.Point(4, 6);
            this.lblViewerOption.Name = "lblViewerOption";
            this.lblViewerOption.Size = new System.Drawing.Size(87, 17);
            this.lblViewerOption.TabIndex = 10;
            this.lblViewerOption.Text = "View Option:";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(0, 354);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(91, 41);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // UCResultTimseries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lblEditableInfo);
            this.Controls.Add(this.cboViewOption);
            this.Controls.Add(this.lblViewerOption);
            this.Controls.Add(this.btnCSVTemplate);
            this.Controls.Add(this.btnImportCSV);
            this.Controls.Add(this.grdResultTSGrid);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCResultTimseries";
            this.Size = new System.Drawing.Size(1029, 500);
            ((System.ComponentModel.ISupportInitialize)(this.grdResultTSGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdResultTSView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboViewOption.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdResultTSGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView grdResultTSView;
        private DevExpress.XtraGrid.Columns.GridColumn colResult_Label;
        private DevExpress.XtraGrid.Columns.GridColumn colVarResultType_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colResultDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colElement_Label;
        private DevExpress.XtraEditors.SimpleButton btnCSVTemplate;
        private DevExpress.XtraEditors.SimpleButton btnImportCSV;
        private DevExpress.XtraGrid.Columns.GridColumn colTS_StartDate;
        private DevExpress.XtraGrid.Columns.GridColumn colTS_StartHour;
        private DevExpress.XtraGrid.Columns.GridColumn colTS_StartMin;
        private DevExpress.XtraGrid.Columns.GridColumn colTS_Interval;
        private DevExpress.XtraGrid.Columns.GridColumn colTS_Interval_Unit;
        private DevExpress.XtraGrid.Columns.GridColumn colBeginPeriodNo;
        private DevExpress.XtraGrid.Columns.GridColumn colFunctionID_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colFunctionArgs;
        private DevExpress.XtraGrid.Columns.GridColumn colRefTS_ID_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colIsSecondary;
        private DevExpress.XtraEditors.LabelControl lblEditableInfo;
        private DevExpress.XtraEditors.ComboBoxEdit cboViewOption;
        private System.Windows.Forms.Label lblViewerOption;
        private DevExpress.XtraGrid.Columns.GridColumn colElementLabel2;
        private DevExpress.XtraGrid.Columns.GridColumn colEvaluationGroup_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colElementIndex;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
    }
}
