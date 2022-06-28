namespace SIM_API_LINKS
{
    partial class frmElementList
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
            this.ctrElementList = new SIM_API_LINKS.UCElementList();
            this.SuspendLayout();
            // 
            // ctrElementList
            // 
            this.ctrElementList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrElementList.Location = new System.Drawing.Point(0, 0);
            this.ctrElementList.Name = "ctrElementList";
            this.ctrElementList.Size = new System.Drawing.Size(618, 300);
            this.ctrElementList.TabIndex = 0;
            this.ctrElementList.OnCloseDialog += new SIM_API_LINKS.UCElementList.ButtonClickedEventHandler(this.ctrElementList_OnCloseDialog);
            // 
            // frmElementList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 300);
            this.Controls.Add(this.ctrElementList);
            this.Name = "frmElementList";
            this.Text = "Element List";
            this.Load += new System.EventHandler(this.frmElementList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UCElementList ctrElementList;
    }
}