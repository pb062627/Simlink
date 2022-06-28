namespace SIM_API_LINKS
{
    partial class fmLinkRecordList
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
            this.cboLinkType = new DevExpress.XtraEditors.LookUpEdit();
            this.lblLinkRecord = new DevExpress.XtraEditors.LabelControl();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.grdLinkedRecordGrid = new DevExpress.XtraGrid.GridControl();
            this.grdLinkedRecordView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colLinkedRecord = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colApplyThreshold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colThreshold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsOver_Threshold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblInfo = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cboLinkType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLinkedRecordGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLinkedRecordView)).BeginInit();
            this.SuspendLayout();
            // 
            // cboLinkType
            // 
            this.cboLinkType.Location = new System.Drawing.Point(115, 10);
            this.cboLinkType.Margin = new System.Windows.Forms.Padding(4);
            this.cboLinkType.Name = "cboLinkType";
            this.cboLinkType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboLinkType.Size = new System.Drawing.Size(492, 22);
            this.cboLinkType.TabIndex = 6;
            this.cboLinkType.EditValueChanged += new System.EventHandler(this.cboLinkTable_Properties_EditValueChanged);
            // 
            // lblLinkRecord
            // 
            this.lblLinkRecord.Location = new System.Drawing.Point(13, 13);
            this.lblLinkRecord.Margin = new System.Windows.Forms.Padding(4);
            this.lblLinkRecord.Name = "lblLinkRecord";
            this.lblLinkRecord.Size = new System.Drawing.Size(94, 16);
            this.lblLinkRecord.TabIndex = 9;
            this.lblLinkRecord.Text = "Link Data Name:";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(437, 360);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(78, 43);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(524, 360);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(83, 43);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdLinkedRecordGrid
            // 
            this.grdLinkedRecordGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdLinkedRecordGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grdLinkedRecordGrid.Location = new System.Drawing.Point(13, 43);
            this.grdLinkedRecordGrid.MainView = this.grdLinkedRecordView;
            this.grdLinkedRecordGrid.Margin = new System.Windows.Forms.Padding(4);
            this.grdLinkedRecordGrid.Name = "grdLinkedRecordGrid";
            this.grdLinkedRecordGrid.Size = new System.Drawing.Size(594, 309);
            this.grdLinkedRecordGrid.TabIndex = 7;
            this.grdLinkedRecordGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdLinkedRecordView});
            // 
            // grdLinkedRecordView
            // 
            this.grdLinkedRecordView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colLinkedRecord,
            this.colApplyThreshold,
            this.colThreshold,
            this.colIsOver_Threshold});
            this.grdLinkedRecordView.GridControl = this.grdLinkedRecordGrid;
            this.grdLinkedRecordView.Name = "grdLinkedRecordView";
            this.grdLinkedRecordView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.grdLinkedRecordView.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Inplace;
            this.grdLinkedRecordView.OptionsEditForm.EditFormColumnCount = 1;
            this.grdLinkedRecordView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.grdLinkedRecordView.OptionsView.ShowGroupPanel = false;
            this.grdLinkedRecordView.ShowingPopupEditForm += new DevExpress.XtraGrid.Views.Grid.ShowingPopupEditFormEventHandler(this.grdLinkedRecordView_ShowingPopupEditForm);
            this.grdLinkedRecordView.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdOptionViewgrdLinkedRecordView_ValidateRow);
            this.grdLinkedRecordView.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.grdOptionViewgrdLinkedRecordView_RowUpdated);
            // 
            // colLinkedRecord
            // 
            this.colLinkedRecord.Caption = "Linked Record Label";
            this.colLinkedRecord.FieldName = "ID";
            this.colLinkedRecord.Name = "colLinkedRecord";
            this.colLinkedRecord.Visible = true;
            this.colLinkedRecord.VisibleIndex = 0;
            this.colLinkedRecord.Width = 211;
            // 
            // colApplyThreshold
            // 
            this.colApplyThreshold.Caption = "Apply Threshold";
            this.colApplyThreshold.FieldName = "ApplyThreshold";
            this.colApplyThreshold.Name = "colApplyThreshold";
            this.colApplyThreshold.Visible = true;
            this.colApplyThreshold.VisibleIndex = 1;
            this.colApplyThreshold.Width = 148;
            // 
            // colThreshold
            // 
            this.colThreshold.Caption = "Threshold";
            this.colThreshold.FieldName = "Threshold";
            this.colThreshold.Name = "colThreshold";
            this.colThreshold.Visible = true;
            this.colThreshold.VisibleIndex = 2;
            this.colThreshold.Width = 68;
            // 
            // colIsOver_Threshold
            // 
            this.colIsOver_Threshold.Caption = "IsOver Threshold";
            this.colIsOver_Threshold.FieldName = "IsOver_Threshold";
            this.colIsOver_Threshold.Name = "colIsOver_Threshold";
            this.colIsOver_Threshold.Visible = true;
            this.colIsOver_Threshold.VisibleIndex = 3;
            this.colIsOver_Threshold.Width = 149;
            // 
            // lblInfo
            // 
            this.lblInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblInfo.Location = new System.Drawing.Point(13, 374);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(253, 16);
            this.lblInfo.TabIndex = 11;
            this.lblInfo.Text = "Note: Threshold default value is -1.234";
            // 
            // fmLinkRecordList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 413);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.cboLinkType);
            this.Controls.Add(this.lblLinkRecord);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdLinkedRecordGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmLinkRecordList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Linked records list";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmLinkRecordList_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.cboLinkType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLinkedRecordGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLinkedRecordView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit cboLinkType;
        private DevExpress.XtraEditors.LabelControl lblLinkRecord;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraGrid.GridControl grdLinkedRecordGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView grdLinkedRecordView;
        private DevExpress.XtraGrid.Columns.GridColumn colLinkedRecord;
        private DevExpress.XtraGrid.Columns.GridColumn colApplyThreshold;
        private DevExpress.XtraGrid.Columns.GridColumn colThreshold;
        private DevExpress.XtraGrid.Columns.GridColumn colIsOver_Threshold;
        private DevExpress.XtraEditors.LabelControl lblInfo;
    }
}