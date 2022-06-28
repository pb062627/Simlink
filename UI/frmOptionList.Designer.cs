namespace SIM_API_LINKS
{
    partial class frmOptionList
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
            this.ucOptionList1 = new SIM_API_LINKS.UCOptionList();
            this.SuspendLayout();
            // 
            // ucOptionList1
            // 
            this.ucOptionList1.Location = new System.Drawing.Point(4, 3);
            this.ucOptionList1.Margin = new System.Windows.Forms.Padding(4);
            this.ucOptionList1.Name = "ucOptionList1";
            this.ucOptionList1.SimlinkMainDialog = null;
            this.ucOptionList1.Size = new System.Drawing.Size(581, 451);
            this.ucOptionList1.TabIndex = 0;
            this.ucOptionList1.OnCloseDialog += new SIM_API_LINKS.UCOptionList.ButtonClickedEventHandler(this.ctrOptionList_OnCloseDialog);
            // 
            // frmOptionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 413);
            this.Controls.Add(this.ucOptionList1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptionList";
            this.Text = "Option List";
            this.Load += new System.EventHandler(this.frmOptionList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UCOptionList ucOptionList1;





    }
}