namespace SIM_API_LINKS
{
    partial class UCElementList
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
            this.grdElementListGrid = new DevExpress.XtraGrid.GridControl();
            this.grdElementView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colVarLabel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.lblLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtType = new DevExpress.XtraEditors.TextEdit();
            this.cboElementLabel = new DevExpress.XtraEditors.LookUpEdit();
            this.btnCreateNew = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.grdElementListGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdElementView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboElementLabel.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grdElementListGrid
            // 
            this.grdElementListGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdElementListGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grdElementListGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grdElementListGrid.Location = new System.Drawing.Point(19, 76);
            this.grdElementListGrid.MainView = this.grdElementView;
            this.grdElementListGrid.Margin = new System.Windows.Forms.Padding(4);
            this.grdElementListGrid.Name = "grdElementListGrid";
            this.grdElementListGrid.Size = new System.Drawing.Size(538, 226);
            this.grdElementListGrid.TabIndex = 2;
            this.grdElementListGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdElementView});
            // 
            // grdElementView
            // 
            this.grdElementView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVarLabel});
            this.grdElementView.GridControl = this.grdElementListGrid;
            this.grdElementView.Name = "grdElementView";
            this.grdElementView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.grdElementView.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            this.grdElementView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.grdElementView.OptionsView.ShowGroupPanel = false;
            this.grdElementView.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.grdDVView_InvalidRowException);
            this.grdElementView.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.grdDVView_ValidateRow);
            this.grdElementView.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.grdElementView_RowUpdated);
            // 
            // colVarLabel
            // 
            this.colVarLabel.Caption = "Var Label";
            this.colVarLabel.FieldName = "VarLabel";
            this.colVarLabel.Name = "colVarLabel";
            this.colVarLabel.Visible = true;
            this.colVarLabel.VisibleIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(454, 309);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(103, 43);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(342, 309);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 43);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblLabel
            // 
            this.lblLabel.AutoSize = true;
            this.lblLabel.Location = new System.Drawing.Point(12, 18);
            this.lblLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(145, 17);
            this.lblLabel.TabIndex = 3;
            this.lblLabel.Text = "Select Element Label:";
            this.lblLabel.Click += new System.EventHandler(this.lblLabel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Selected Element Type:";
            this.label1.Click += new System.EventHandler(this.lblLabel_Click);
            // 
            // txtType
            // 
            this.txtType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtType.Location = new System.Drawing.Point(179, 46);
            this.txtType.Margin = new System.Windows.Forms.Padding(4);
            this.txtType.Name = "txtType";
            this.txtType.Properties.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(378, 22);
            this.txtType.TabIndex = 1;
            // 
            // cboElementLabel
            // 
            this.cboElementLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboElementLabel.Location = new System.Drawing.Point(179, 14);
            this.cboElementLabel.Margin = new System.Windows.Forms.Padding(4);
            this.cboElementLabel.Name = "cboElementLabel";
            this.cboElementLabel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboElementLabel.Size = new System.Drawing.Size(264, 22);
            this.cboElementLabel.TabIndex = 0;
            this.cboElementLabel.EditValueChanged += new System.EventHandler(this.cboElementLabel_EditValueChanged);
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateNew.Location = new System.Drawing.Point(449, 13);
            this.btnCreateNew.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(105, 22);
            this.btnCreateNew.TabIndex = 6;
            this.btnCreateNew.Text = "Create New";
            this.btnCreateNew.Click += new System.EventHandler(this.btnCreateNew_Click);
            // 
            // UCElementList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCreateNew);
            this.Controls.Add(this.cboElementLabel);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLabel);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdElementListGrid);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCElementList";
            this.Size = new System.Drawing.Size(570, 356);
            ((System.ComponentModel.ISupportInitialize)(this.grdElementListGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdElementView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboElementLabel.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grdElementListGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView grdElementView;
        private DevExpress.XtraGrid.Columns.GridColumn colVarLabel;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtType;
        private DevExpress.XtraEditors.LookUpEdit cboElementLabel;
        private DevExpress.XtraEditors.SimpleButton btnCreateNew;
    }
}
