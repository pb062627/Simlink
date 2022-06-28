namespace SIM_API_LINKS
{
    partial class UCDVGridEditor
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
            this.grdDVGrid = new DevExpress.XtraGrid.GridControl();
            this.grdDVView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDVLabel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVarType_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOption = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOption_MIN = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOption_MAX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGetNewValMethod = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFunctionID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFunctionArgs = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsListVar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSkipMinVal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colElementID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPrimaryDV_ID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSecondaryDVLabel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperation = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsSpecialCase = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsTS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXModelID_FK = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsqn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.grdLKPVarType = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.grdLKPVarTypeView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnImportCSV = new DevExpress.XtraEditors.SimpleButton();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.cboFilter = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnCSVTemplate = new DevExpress.XtraEditors.SimpleButton();
            this.lblEditableInfo = new DevExpress.XtraEditors.LabelControl();
            this.cboViewOption = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDVGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDVView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLKPVarType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLKPVarTypeView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFilter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboViewOption.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grdDVGrid
            // 
            this.grdDVGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDVGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdDVGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grdDVGrid.Location = new System.Drawing.Point(7, 41);
            this.grdDVGrid.MainView = this.grdDVView;
            this.grdDVGrid.Margin = new System.Windows.Forms.Padding(4);
            this.grdDVGrid.Name = "grdDVGrid";
            this.grdDVGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.grdLKPVarType});
            this.grdDVGrid.Size = new System.Drawing.Size(771, 265);
            this.grdDVGrid.TabIndex = 0;
            this.grdDVGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdDVView});
            // 
            // grdDVView
            // 
            this.grdDVView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDVLabel,
            this.colVarType_FK,
            this.colOption,
            this.colOption_MIN,
            this.colOption_MAX,
            this.colGetNewValMethod,
            this.colFunctionID_FK,
            this.colFunctionArgs,
            this.colIsListVar,
            this.colSkipMinVal,
            this.colElementID_FK,
            this.colPrimaryDV_ID_FK,
            this.colSecondaryDVLabel,
            this.colOperation,
            this.colIsSpecialCase,
            this.colIsTS,
            this.colXModelID_FK,
            this.colsqn});
            this.grdDVView.GridControl = this.grdDVGrid;
            this.grdDVView.Name = "grdDVView";
            this.grdDVView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.grdDVView.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            this.grdDVView.OptionsSelection.MultiSelect = true;
            this.grdDVView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.grdDVView.EditFormPrepared += new DevExpress.XtraGrid.Views.Grid.EditFormPreparedEventHandler(this.grdDVView_EditFormPrepared);
            this.grdDVView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.grdDVView_InitNewRow);
            this.grdDVView.HiddenEditor += new System.EventHandler(this.grdDVView_HiddenEditor);
            this.grdDVView.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdDVView_ValidateRow);
            this.grdDVView.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.grdDVView_RowUpdated);
            this.grdDVView.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.grdDVView_ValidatingEditor);
            // 
            // colDVLabel
            // 
            this.colDVLabel.Caption = "Label";
            this.colDVLabel.FieldName = "DV_Label";
            this.colDVLabel.Name = "colDVLabel";
            this.colDVLabel.Visible = true;
            this.colDVLabel.VisibleIndex = 0;
            // 
            // colVarType_FK
            // 
            this.colVarType_FK.Caption = "Type";
            this.colVarType_FK.FieldName = "VarType_FK";
            this.colVarType_FK.Name = "colVarType_FK";
            this.colVarType_FK.Visible = true;
            this.colVarType_FK.VisibleIndex = 1;
            // 
            // colOption
            // 
            this.colOption.Caption = "Option";
            this.colOption.FieldName = "OptionID";
            this.colOption.Name = "colOption";
            this.colOption.Visible = true;
            this.colOption.VisibleIndex = 16;
            // 
            // colOption_MIN
            // 
            this.colOption_MIN.Caption = "Min Option";
            this.colOption_MIN.FieldName = "Option_MIN";
            this.colOption_MIN.Name = "colOption_MIN";
            this.colOption_MIN.Visible = true;
            this.colOption_MIN.VisibleIndex = 2;
            // 
            // colOption_MAX
            // 
            this.colOption_MAX.Caption = "Max Option";
            this.colOption_MAX.FieldName = "Option_MAX";
            this.colOption_MAX.Name = "colOption_MAX";
            this.colOption_MAX.Visible = true;
            this.colOption_MAX.VisibleIndex = 3;
            // 
            // colGetNewValMethod
            // 
            this.colGetNewValMethod.Caption = "New Val Method";
            this.colGetNewValMethod.FieldName = "GetNewValMethod";
            this.colGetNewValMethod.Name = "colGetNewValMethod";
            this.colGetNewValMethod.Visible = true;
            this.colGetNewValMethod.VisibleIndex = 4;
            // 
            // colFunctionID_FK
            // 
            this.colFunctionID_FK.Caption = "Function";
            this.colFunctionID_FK.FieldName = "FunctionID_FK";
            this.colFunctionID_FK.Name = "colFunctionID_FK";
            this.colFunctionID_FK.Visible = true;
            this.colFunctionID_FK.VisibleIndex = 11;
            // 
            // colFunctionArgs
            // 
            this.colFunctionArgs.Caption = "Function Args";
            this.colFunctionArgs.FieldName = "FunctionArgs";
            this.colFunctionArgs.Name = "colFunctionArgs";
            this.colFunctionArgs.Visible = true;
            this.colFunctionArgs.VisibleIndex = 12;
            // 
            // colIsListVar
            // 
            this.colIsListVar.Caption = "List Variable?";
            this.colIsListVar.FieldName = "IsListVar";
            this.colIsListVar.Name = "colIsListVar";
            this.colIsListVar.Visible = true;
            this.colIsListVar.VisibleIndex = 13;
            // 
            // colSkipMinVal
            // 
            this.colSkipMinVal.Caption = "Skip Min Val Option";
            this.colSkipMinVal.FieldName = "SkipMinVal";
            this.colSkipMinVal.Name = "colSkipMinVal";
            this.colSkipMinVal.Visible = true;
            this.colSkipMinVal.VisibleIndex = 7;
            // 
            // colElementID_FK
            // 
            this.colElementID_FK.Caption = "Elements";
            this.colElementID_FK.FieldName = "ElementID";
            this.colElementID_FK.Name = "colElementID_FK";
            this.colElementID_FK.Visible = true;
            this.colElementID_FK.VisibleIndex = 5;
            // 
            // colPrimaryDV_ID_FK
            // 
            this.colPrimaryDV_ID_FK.Caption = "Primary DV Label";
            this.colPrimaryDV_ID_FK.FieldName = "PrimaryDV_ID_FK";
            this.colPrimaryDV_ID_FK.Name = "colPrimaryDV_ID_FK";
            this.colPrimaryDV_ID_FK.Visible = true;
            this.colPrimaryDV_ID_FK.VisibleIndex = 6;
            // 
            // colSecondaryDVLabel
            // 
            this.colSecondaryDVLabel.Caption = "Secondary DV Label";
            this.colSecondaryDVLabel.FieldName = "SecondaryDV_Key";
            this.colSecondaryDVLabel.Name = "colSecondaryDVLabel";
            this.colSecondaryDVLabel.Visible = true;
            this.colSecondaryDVLabel.VisibleIndex = 10;
            // 
            // colOperation
            // 
            this.colOperation.Caption = "Operation";
            this.colOperation.FieldName = "Operation";
            this.colOperation.Name = "colOperation";
            this.colOperation.Visible = true;
            this.colOperation.VisibleIndex = 9;
            // 
            // colIsSpecialCase
            // 
            this.colIsSpecialCase.Caption = "Special Case?";
            this.colIsSpecialCase.FieldName = "IsSpecialCase";
            this.colIsSpecialCase.Name = "colIsSpecialCase";
            this.colIsSpecialCase.Visible = true;
            this.colIsSpecialCase.VisibleIndex = 8;
            // 
            // colIsTS
            // 
            this.colIsTS.Caption = "Is Time Series?";
            this.colIsTS.FieldName = "IsTS";
            this.colIsTS.Name = "colIsTS";
            this.colIsTS.Visible = true;
            this.colIsTS.VisibleIndex = 14;
            // 
            // colXModelID_FK
            // 
            this.colXModelID_FK.Caption = "XModel ID";
            this.colXModelID_FK.FieldName = "XModelID_FK";
            this.colXModelID_FK.Name = "colXModelID_FK";
            this.colXModelID_FK.Visible = true;
            this.colXModelID_FK.VisibleIndex = 15;
            // 
            // colsqn
            // 
            this.colsqn.Caption = "sqn";
            this.colsqn.FieldName = "SQN";
            this.colsqn.Name = "colsqn";
            // 
            // grdLKPVarType
            // 
            this.grdLKPVarType.AutoHeight = false;
            this.grdLKPVarType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.grdLKPVarType.Name = "grdLKPVarType";
            this.grdLKPVarType.View = this.grdLKPVarTypeView;
            // 
            // grdLKPVarTypeView
            // 
            this.grdLKPVarTypeView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.grdLKPVarTypeView.Name = "grdLKPVarTypeView";
            this.grdLKPVarTypeView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.grdLKPVarTypeView.OptionsView.ShowGroupPanel = false;
            // 
            // btnImportCSV
            // 
            this.btnImportCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportCSV.Location = new System.Drawing.Point(689, 317);
            this.btnImportCSV.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(87, 41);
            this.btnImportCSV.TabIndex = 1;
            this.btnImportCSV.Text = "Import...";
            this.btnImportCSV.Click += new System.EventHandler(this.btnImportCSV_Click);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(4, 11);
            this.lblFilter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(41, 17);
            this.lblFilter.TabIndex = 2;
            this.lblFilter.Text = "Filter:";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(4, 317);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(87, 41);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // cboFilter
            // 
            this.cboFilter.Location = new System.Drawing.Point(216, 8);
            this.cboFilter.Margin = new System.Windows.Forms.Padding(4);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboFilter.Properties.Items.AddRange(new object[] {
            "Show All",
            "Primary",
            "Secondary"});
            this.cboFilter.Size = new System.Drawing.Size(160, 22);
            this.cboFilter.TabIndex = 4;
            this.cboFilter.SelectedIndexChanged += new System.EventHandler(this.cboFilter_SelectedIndexChanged);
            // 
            // btnCSVTemplate
            // 
            this.btnCSVTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCSVTemplate.Location = new System.Drawing.Point(580, 317);
            this.btnCSVTemplate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCSVTemplate.Name = "btnCSVTemplate";
            this.btnCSVTemplate.Size = new System.Drawing.Size(102, 41);
            this.btnCSVTemplate.TabIndex = 5;
            this.btnCSVTemplate.Text = "CSV Template";
            this.btnCSVTemplate.ToolTip = "Download import file template";
            this.btnCSVTemplate.Click += new System.EventHandler(this.btnCSVTemplate_Click);
            // 
            // lblEditableInfo
            // 
            this.lblEditableInfo.Location = new System.Drawing.Point(382, 11);
            this.lblEditableInfo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.lblEditableInfo.Name = "lblEditableInfo";
            this.lblEditableInfo.Size = new System.Drawing.Size(178, 16);
            this.lblEditableInfo.TabIndex = 13;
            this.lblEditableInfo.Text = "Grid is editable in detailed view";
            // 
            // cboViewOption
            // 
            this.cboViewOption.Location = new System.Drawing.Point(49, 8);
            this.cboViewOption.Margin = new System.Windows.Forms.Padding(4);
            this.cboViewOption.Name = "cboViewOption";
            this.cboViewOption.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboViewOption.Properties.Items.AddRange(new object[] {
            "Limited Detailed View",
            "Detailed View"});
            this.cboViewOption.Size = new System.Drawing.Size(160, 22);
            this.cboViewOption.TabIndex = 4;
            this.cboViewOption.SelectedIndexChanged += new System.EventHandler(this.cboViewOption_SelectedIndexChanged);
            // 
            // UCDVGridEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblEditableInfo);
            this.Controls.Add(this.btnCSVTemplate);
            this.Controls.Add(this.cboViewOption);
            this.Controls.Add(this.cboFilter);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnImportCSV);
            this.Controls.Add(this.grdDVGrid);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCDVGridEditor";
            this.Size = new System.Drawing.Size(837, 465);
            ((System.ComponentModel.ISupportInitialize)(this.grdDVGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDVView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLKPVarType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLKPVarTypeView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFilter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboViewOption.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdDVGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView grdDVView;
        private DevExpress.XtraGrid.Columns.GridColumn colDVLabel;
        private DevExpress.XtraGrid.Columns.GridColumn colVarType_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colOption_MIN;
        private DevExpress.XtraGrid.Columns.GridColumn colOption_MAX;
        private DevExpress.XtraGrid.Columns.GridColumn colGetNewValMethod;
        private DevExpress.XtraGrid.Columns.GridColumn colElementID_FK;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit grdLKPVarType;
        private DevExpress.XtraGrid.Views.Grid.GridView grdLKPVarTypeView;
        private DevExpress.XtraEditors.SimpleButton btnImportCSV;
        private System.Windows.Forms.Label lblFilter;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraGrid.Columns.GridColumn colPrimaryDV_ID_FK;
        private DevExpress.XtraEditors.ComboBoxEdit cboFilter;
        private DevExpress.XtraEditors.SimpleButton btnCSVTemplate;
        private DevExpress.XtraGrid.Columns.GridColumn colOperation;
        private DevExpress.XtraGrid.Columns.GridColumn colSecondaryDVLabel;
        private DevExpress.XtraGrid.Columns.GridColumn colFunctionID_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colFunctionArgs;
        private DevExpress.XtraGrid.Columns.GridColumn colIsListVar;
        private DevExpress.XtraGrid.Columns.GridColumn colSkipMinVal;
        private DevExpress.XtraGrid.Columns.GridColumn colIsSpecialCase;
        private DevExpress.XtraGrid.Columns.GridColumn colIsTS;
        private DevExpress.XtraGrid.Columns.GridColumn colXModelID_FK;
        private DevExpress.XtraGrid.Columns.GridColumn colsqn;
        private DevExpress.XtraGrid.Columns.GridColumn colOption;
        private DevExpress.XtraEditors.LabelControl lblEditableInfo;
        private DevExpress.XtraEditors.ComboBoxEdit cboViewOption;
    }
}
