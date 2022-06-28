namespace SIM_API_LINKS
{
    partial class UCPerformance
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
            this.grdPerformanceGrid = new DevExpress.XtraGrid.GridControl();
            this.grdPerformanceView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colPerformance_Label = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPF_Type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPF_FunctionType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCategoryID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLinkTableCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnLinkCodeClick = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.colResultFunctionKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFunctionID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsObjective = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSQN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFunctionArgs = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDV_ID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOptionID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colApplyThreshold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colThreshold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsOverThreshold001 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComponentApplyThreshold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComponentThreshold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComponentIsOver_Threshold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblViewerOption = new System.Windows.Forms.Label();
            this.cboViewOption = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnCSV = new DevExpress.XtraEditors.SimpleButton();
            this.btnImportCSV = new DevExpress.XtraEditors.SimpleButton();
            this.linkNewTableCode = new DevExpress.XtraEditors.HyperLinkEdit();
            this.lblEditableInfo = new DevExpress.XtraEditors.LabelControl();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdPerformanceGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPerformanceView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLinkCodeClick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboViewOption.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkNewTableCode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grdPerformanceGrid
            // 
            this.grdPerformanceGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPerformanceGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdPerformanceGrid.Location = new System.Drawing.Point(7, 64);
            this.grdPerformanceGrid.MainView = this.grdPerformanceView;
            this.grdPerformanceGrid.Name = "grdPerformanceGrid";
            this.grdPerformanceGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.btnLinkCodeClick});
            this.grdPerformanceGrid.Size = new System.Drawing.Size(731, 246);
            this.grdPerformanceGrid.TabIndex = 2;
            this.grdPerformanceGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdPerformanceView});
            // 
            // grdPerformanceView
            // 
            this.grdPerformanceView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPerformance_Label,
            this.colPF_Type,
            this.colPF_FunctionType,
            this.colCategoryID_FK,
            this.colLinkTableCode,
            this.colResultFunctionKey,
            this.colFunctionID_FK,
            this.colIsObjective,
            this.colSQN,
            this.colFunctionArgs,
            this.colDV_ID_FK,
            this.colOptionID_FK,
            this.colApplyThreshold,
            this.colThreshold,
            this.colIsOverThreshold001,
            this.colComponentApplyThreshold,
            this.colComponentThreshold,
            this.colComponentIsOver_Threshold});
            this.grdPerformanceView.GridControl = this.grdPerformanceGrid;
            this.grdPerformanceView.Name = "grdPerformanceView";
            this.grdPerformanceView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.grdPerformanceView.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            this.grdPerformanceView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.grdPerformanceView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.grdPerformanceView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.grdPerformanceView_RowClick);
            this.grdPerformanceView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.grdPerformanceView_InitNewRow);
            this.grdPerformanceView.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.grdPerformanceView_InvalidRowException);
            this.grdPerformanceView.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdPerformanceView_ValidateRow);
            this.grdPerformanceView.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.grdPerformanceView_RowUpdated);
            // 
            // colPerformance_Label
            // 
            this.colPerformance_Label.Caption = "Label";
            this.colPerformance_Label.FieldName = "Performance_Label";
            this.colPerformance_Label.Name = "colPerformance_Label";
            this.colPerformance_Label.Visible = true;
            this.colPerformance_Label.VisibleIndex = 0;
            // 
            // colPF_Type
            // 
            this.colPF_Type.Caption = "Type";
            this.colPF_Type.FieldName = "PF_Type";
            this.colPF_Type.Name = "colPF_Type";
            this.colPF_Type.Visible = true;
            this.colPF_Type.VisibleIndex = 1;
            // 
            // colPF_FunctionType
            // 
            this.colPF_FunctionType.Caption = "Function Type";
            this.colPF_FunctionType.FieldName = "PF_FunctionType";
            this.colPF_FunctionType.Name = "colPF_FunctionType";
            this.colPF_FunctionType.Visible = true;
            this.colPF_FunctionType.VisibleIndex = 4;
            // 
            // colCategoryID_FK
            // 
            this.colCategoryID_FK.Caption = "Category";
            this.colCategoryID_FK.FieldName = "CategoryID_FK";
            this.colCategoryID_FK.Name = "colCategoryID_FK";
            this.colCategoryID_FK.Visible = true;
            this.colCategoryID_FK.VisibleIndex = 2;
            // 
            // colLinkTableCode
            // 
            this.colLinkTableCode.Caption = "Linked Data (LD)";
            this.colLinkTableCode.ColumnEdit = this.btnLinkCodeClick;
            this.colLinkTableCode.FieldName = "LinkTableCode";
            this.colLinkTableCode.Name = "colLinkTableCode";
            this.colLinkTableCode.Visible = true;
            this.colLinkTableCode.VisibleIndex = 3;
            // 
            // btnLinkCodeClick
            // 
            this.btnLinkCodeClick.AutoHeight = false;
            this.btnLinkCodeClick.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btnLinkCodeClick.Name = "btnLinkCodeClick";
            this.btnLinkCodeClick.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // colResultFunctionKey
            // 
            this.colResultFunctionKey.Caption = "Function on Linked Data";
            this.colResultFunctionKey.FieldName = "ResultFunctionKey";
            this.colResultFunctionKey.Name = "colResultFunctionKey";
            this.colResultFunctionKey.Visible = true;
            this.colResultFunctionKey.VisibleIndex = 12;
            // 
            // colFunctionID_FK
            // 
            this.colFunctionID_FK.Caption = "Function";
            this.colFunctionID_FK.FieldName = "FunctionID_FK";
            this.colFunctionID_FK.Name = "colFunctionID_FK";
            this.colFunctionID_FK.Visible = true;
            this.colFunctionID_FK.VisibleIndex = 5;
            // 
            // colIsObjective
            // 
            this.colIsObjective.Caption = "Objective";
            this.colIsObjective.FieldName = "IsObjective";
            this.colIsObjective.Name = "colIsObjective";
            this.colIsObjective.Visible = true;
            this.colIsObjective.VisibleIndex = 6;
            // 
            // colSQN
            // 
            this.colSQN.Caption = "Sequence";
            this.colSQN.FieldName = "SQN";
            this.colSQN.Name = "colSQN";
            this.colSQN.Visible = true;
            this.colSQN.VisibleIndex = 7;
            // 
            // colFunctionArgs
            // 
            this.colFunctionArgs.Caption = "Arguments";
            this.colFunctionArgs.FieldName = "FunctionArgs";
            this.colFunctionArgs.Name = "colFunctionArgs";
            this.colFunctionArgs.Visible = true;
            this.colFunctionArgs.VisibleIndex = 8;
            // 
            // colDV_ID_FK
            // 
            this.colDV_ID_FK.Caption = "DV Key";
            this.colDV_ID_FK.FieldName = "DV_ID_FK";
            this.colDV_ID_FK.Name = "colDV_ID_FK";
            this.colDV_ID_FK.Visible = true;
            this.colDV_ID_FK.VisibleIndex = 9;
            // 
            // colOptionID_FK
            // 
            this.colOptionID_FK.Caption = "Options";
            this.colOptionID_FK.FieldName = "OptionID_FK";
            this.colOptionID_FK.Name = "colOptionID_FK";
            this.colOptionID_FK.Visible = true;
            this.colOptionID_FK.VisibleIndex = 10;
            // 
            // colApplyThreshold
            // 
            this.colApplyThreshold.Caption = "Apply Threshold";
            this.colApplyThreshold.FieldName = "ApplyThreshold";
            this.colApplyThreshold.Name = "colApplyThreshold";
            this.colApplyThreshold.Visible = true;
            this.colApplyThreshold.VisibleIndex = 13;
            // 
            // colThreshold
            // 
            this.colThreshold.Caption = "Threshold Value";
            this.colThreshold.FieldName = "Threshold";
            this.colThreshold.Name = "colThreshold";
            this.colThreshold.Visible = true;
            this.colThreshold.VisibleIndex = 11;
            // 
            // colIsOverThreshold001
            // 
            this.colIsOverThreshold001.Caption = "Trig. If Over Thres";
            this.colIsOverThreshold001.FieldName = "IsOver_Threshold";
            this.colIsOverThreshold001.Name = "colIsOverThreshold001";
            this.colIsOverThreshold001.Visible = true;
            this.colIsOverThreshold001.VisibleIndex = 14;
            // 
            // colComponentApplyThreshold
            // 
            this.colComponentApplyThreshold.Caption = "Apply Thres (Linked Values)";
            this.colComponentApplyThreshold.FieldName = "ComponentApplyThreshold";
            this.colComponentApplyThreshold.Name = "colComponentApplyThreshold";
            this.colComponentApplyThreshold.Visible = true;
            this.colComponentApplyThreshold.VisibleIndex = 15;
            // 
            // colComponentThreshold
            // 
            this.colComponentThreshold.Caption = "Threshold (Linked Values)";
            this.colComponentThreshold.FieldName = "ComponentThreshold";
            this.colComponentThreshold.Name = "colComponentThreshold";
            this.colComponentThreshold.Visible = true;
            this.colComponentThreshold.VisibleIndex = 16;
            // 
            // colComponentIsOver_Threshold
            // 
            this.colComponentIsOver_Threshold.Caption = "Trig. If Over Thres (Linked Values)";
            this.colComponentIsOver_Threshold.FieldName = "ComponentIsOver_Threshold";
            this.colComponentIsOver_Threshold.Name = "colComponentIsOver_Threshold";
            this.colComponentIsOver_Threshold.Visible = true;
            this.colComponentIsOver_Threshold.VisibleIndex = 17;
            // 
            // lblViewerOption
            // 
            this.lblViewerOption.AutoSize = true;
            this.lblViewerOption.Location = new System.Drawing.Point(7, 38);
            this.lblViewerOption.Name = "lblViewerOption";
            this.lblViewerOption.Size = new System.Drawing.Size(85, 17);
            this.lblViewerOption.TabIndex = 3;
            this.lblViewerOption.Text = "View Option:";
            // 
            // cboViewOption
            // 
            this.cboViewOption.Location = new System.Drawing.Point(95, 35);
            this.cboViewOption.Name = "cboViewOption";
            this.cboViewOption.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboViewOption.Properties.Items.AddRange(new object[] {
            "Limited Detail",
            "Detailed"});
            this.cboViewOption.Size = new System.Drawing.Size(226, 22);
            this.cboViewOption.TabIndex = 5;
            this.cboViewOption.SelectedIndexChanged += new System.EventHandler(this.cboViewOption_SelectedIndexChanged);
            // 
            // btnCSV
            // 
            this.btnCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCSV.Location = new System.Drawing.Point(515, 318);
            this.btnCSV.Name = "btnCSV";
            this.btnCSV.Size = new System.Drawing.Size(112, 43);
            this.btnCSV.TabIndex = 6;
            this.btnCSV.Text = "CSV Template";
            this.btnCSV.ToolTip = "Download import file template";
            this.btnCSV.Click += new System.EventHandler(this.btnCSV_Click);
            // 
            // btnImportCSV
            // 
            this.btnImportCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportCSV.Location = new System.Drawing.Point(633, 318);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(105, 43);
            this.btnImportCSV.TabIndex = 7;
            this.btnImportCSV.Text = "Import...";
            this.btnImportCSV.Click += new System.EventHandler(this.btnImportCSV_Click);
            // 
            // linkNewTableCode
            // 
            this.linkNewTableCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkNewTableCode.Location = new System.Drawing.Point(7, 3);
            this.linkNewTableCode.Name = "linkNewTableCode";
            this.linkNewTableCode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.linkNewTableCode.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.linkNewTableCode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.linkNewTableCode.Size = new System.Drawing.Size(787, 20);
            this.linkNewTableCode.TabIndex = 8;
            this.linkNewTableCode.ToolTipTitle = "Click here to add new linked data";
            this.linkNewTableCode.Click += new System.EventHandler(this.linkNewTableCode_Click);
            // 
            // lblEditableInfo
            // 
            this.lblEditableInfo.Location = new System.Drawing.Point(326, 38);
            this.lblEditableInfo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.lblEditableInfo.Name = "lblEditableInfo";
            this.lblEditableInfo.Size = new System.Drawing.Size(215, 16);
            this.lblEditableInfo.TabIndex = 9;
            this.lblEditableInfo.Text = "Grid is NOT editable in summary view";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(7, 316);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(97, 45);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // UCPerformance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lblEditableInfo);
            this.Controls.Add(this.linkNewTableCode);
            this.Controls.Add(this.btnCSV);
            this.Controls.Add(this.btnImportCSV);
            this.Controls.Add(this.cboViewOption);
            this.Controls.Add(this.lblViewerOption);
            this.Controls.Add(this.grdPerformanceGrid);
            this.Name = "UCPerformance";
            this.Size = new System.Drawing.Size(798, 486);
            ((System.ComponentModel.ISupportInitialize)(this.grdPerformanceGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdPerformanceView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLinkCodeClick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboViewOption.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkNewTableCode.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdPerformanceGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView grdPerformanceView;
        private DevExpress.XtraGrid.Columns.GridColumn colPerformance_Label;
        private DevExpress.XtraGrid.Columns.GridColumn colPF_Type;
        private DevExpress.XtraGrid.Columns.GridColumn colCategoryID_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colLinkTableCode;
        private DevExpress.XtraGrid.Columns.GridColumn colPF_FunctionType;
        private DevExpress.XtraGrid.Columns.GridColumn colFunctionID_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colIsObjective;
        private DevExpress.XtraGrid.Columns.GridColumn colSQN;
        private DevExpress.XtraGrid.Columns.GridColumn colFunctionArgs;
        private DevExpress.XtraGrid.Columns.GridColumn colDV_ID_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colOptionID_FK;
        private System.Windows.Forms.Label lblViewerOption;
        private DevExpress.XtraEditors.ComboBoxEdit cboViewOption;
        private DevExpress.XtraEditors.SimpleButton btnCSV;
        private DevExpress.XtraEditors.SimpleButton btnImportCSV;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnLinkCodeClick;
        private DevExpress.XtraEditors.HyperLinkEdit linkNewTableCode;
        private DevExpress.XtraGrid.Columns.GridColumn colThreshold;
        private DevExpress.XtraEditors.LabelControl lblEditableInfo;
        private DevExpress.XtraGrid.Columns.GridColumn colResultFunctionKey;
        private DevExpress.XtraGrid.Columns.GridColumn colApplyThreshold;
        private DevExpress.XtraGrid.Columns.GridColumn colIsOverThreshold001;
        private DevExpress.XtraGrid.Columns.GridColumn colComponentApplyThreshold;
        private DevExpress.XtraGrid.Columns.GridColumn colComponentThreshold;
        private DevExpress.XtraGrid.Columns.GridColumn colComponentIsOver_Threshold;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
    }
}
