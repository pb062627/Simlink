namespace SIM_API_LINKS
{
    partial class UCOptionList
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
            this.grdOptionListGrid = new DevExpress.XtraGrid.GridControl();
            this.grdOptionView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colOptionNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.lblOptionList = new DevExpress.XtraEditors.LabelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.cboOption = new DevExpress.XtraEditors.LookUpEdit();
            this.btnNewOption = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdOptionListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOptionView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOption.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grdOptionListGrid
            // 
            this.grdOptionListGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdOptionListGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grdOptionListGrid.Location = new System.Drawing.Point(0, 37);
            this.grdOptionListGrid.MainView = this.grdOptionView;
            this.grdOptionListGrid.Margin = new System.Windows.Forms.Padding(4);
            this.grdOptionListGrid.Name = "grdOptionListGrid";
            this.grdOptionListGrid.Size = new System.Drawing.Size(609, 309);
            this.grdOptionListGrid.TabIndex = 2;
            this.grdOptionListGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdOptionView});
            // 
            // grdOptionView
            // 
            this.grdOptionView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colOptionNo,
            this.colVal});
            this.grdOptionView.GridControl = this.grdOptionListGrid;
            this.grdOptionView.Name = "grdOptionView";
            this.grdOptionView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.grdOptionView.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace;
            this.grdOptionView.OptionsEditForm.EditFormColumnCount = 1;
            this.grdOptionView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.grdOptionView.OptionsView.ShowGroupPanel = false;
            this.grdOptionView.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.grdDVView_InvalidRowException);
            this.grdOptionView.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdDVView_ValidateRow);
            this.grdOptionView.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.grdOptionView_RowUpdated);
            // 
            // colOptionNo
            // 
            this.colOptionNo.Caption = "Option No";
            this.colOptionNo.FieldName = "OptionNo";
            this.colOptionNo.Name = "colOptionNo";
            this.colOptionNo.Visible = true;
            this.colOptionNo.VisibleIndex = 0;
            this.colOptionNo.Width = 50;
            // 
            // colVal
            // 
            this.colVal.Caption = "Value";
            this.colVal.FieldName = "val";
            this.colVal.Name = "colVal";
            this.colVal.Visible = true;
            this.colVal.VisibleIndex = 1;
            this.colVal.Width = 200;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(440, 353);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(78, 43);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblOptionList
            // 
            this.lblOptionList.Location = new System.Drawing.Point(0, 7);
            this.lblOptionList.Margin = new System.Windows.Forms.Padding(4);
            this.lblOptionList.Name = "lblOptionList";
            this.lblOptionList.Size = new System.Drawing.Size(116, 16);
            this.lblOptionList.TabIndex = 4;
            this.lblOptionList.Text = "Selection option list:";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(527, 353);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(83, 43);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cboOption
            // 
            this.cboOption.Location = new System.Drawing.Point(136, 4);
            this.cboOption.Margin = new System.Windows.Forms.Padding(4);
            this.cboOption.Name = "cboOption";
            this.cboOption.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboOption.Properties.EditValueChanged += new System.EventHandler(this.cboOption_Properties_EditValueChanged);
            this.cboOption.Size = new System.Drawing.Size(366, 22);
            this.cboOption.TabIndex = 0;
            // 
            // btnNewOption
            // 
            this.btnNewOption.Location = new System.Drawing.Point(509, 2);
            this.btnNewOption.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNewOption.Name = "btnNewOption";
            this.btnNewOption.Size = new System.Drawing.Size(100, 23);
            this.btnNewOption.TabIndex = 5;
            this.btnNewOption.Text = "Create New";
            this.btnNewOption.Click += new System.EventHandler(this.btnNewOption_Click);
            // 
            // UCOptionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnNewOption);
            this.Controls.Add(this.cboOption);
            this.Controls.Add(this.lblOptionList);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdOptionListGrid);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCOptionList";
            this.Size = new System.Drawing.Size(614, 400);
            ((System.ComponentModel.ISupportInitialize)(this.grdOptionListGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOptionView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOption.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdOptionListGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView grdOptionView;
        private DevExpress.XtraGrid.Columns.GridColumn colOptionNo;
        private DevExpress.XtraGrid.Columns.GridColumn colVal;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.LabelControl lblOptionList;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.LookUpEdit cboOption;
        private DevExpress.XtraEditors.SimpleButton btnNewOption;
    }
}
