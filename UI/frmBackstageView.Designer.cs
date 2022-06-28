namespace CH2M.SimLink.MainUI
{
    partial class frmBackstageView
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
            DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, null, true, true);
            this.ctrBackStage = new DevExpress.XtraBars.Ribbon.BackstageViewControl();
            this.backstageViewClientControl1 = new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.backstageViewClientControl2 = new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
            this.lblOpen = new DevExpress.XtraEditors.LabelControl();
            this.backstageViewClientControl3 = new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
            this.backstageViewClientControl6 = new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
            this.backstageViewClientControl4 = new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
            this.backstageViewClientControl5 = new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
            this.tabNewProject = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.tabOpenProject = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.tabScenarioGroup = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.tabDatasource = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.tabHelp = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.tabAbout = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
            this.ctrNewProject = new CH2M.UCCreateProject();
            this.ctrOpenProject = new CH2M.UCEditOpenProject();
            this.ctrDBConnection = new SIM_API_LINKS.UCDatabaseSelection();
            this.ctrBackStage.SuspendLayout();
            this.backstageViewClientControl1.SuspendLayout();
            this.backstageViewClientControl2.SuspendLayout();
            this.backstageViewClientControl6.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctrBackStage
            // 
            this.ctrBackStage.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.Yellow;
            this.ctrBackStage.Controls.Add(this.backstageViewClientControl1);
            this.ctrBackStage.Controls.Add(this.backstageViewClientControl2);
            this.ctrBackStage.Controls.Add(this.backstageViewClientControl3);
            this.ctrBackStage.Controls.Add(this.backstageViewClientControl6);
            this.ctrBackStage.Controls.Add(this.backstageViewClientControl4);
            this.ctrBackStage.Controls.Add(this.backstageViewClientControl5);
            this.ctrBackStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrBackStage.Items.Add(this.tabNewProject);
            this.ctrBackStage.Items.Add(this.tabOpenProject);
            this.ctrBackStage.Items.Add(this.tabScenarioGroup);
            this.ctrBackStage.Items.Add(this.tabDatasource);
            this.ctrBackStage.Items.Add(this.tabHelp);
            this.ctrBackStage.Items.Add(this.tabAbout);
            this.ctrBackStage.Location = new System.Drawing.Point(0, 0);
            this.ctrBackStage.Name = "ctrBackStage";
            this.ctrBackStage.SelectedTab = this.tabOpenProject;
            this.ctrBackStage.SelectedTabIndex = 1;
            this.ctrBackStage.Size = new System.Drawing.Size(745, 449);
            this.ctrBackStage.TabIndex = 0;
            this.ctrBackStage.Text = "backstageViewControl1";
            // 
            // backstageViewClientControl1
            // 
            this.backstageViewClientControl1.Controls.Add(this.labelControl14);
            this.backstageViewClientControl1.Controls.Add(this.ctrNewProject);
            this.backstageViewClientControl1.Location = new System.Drawing.Point(163, 0);
            this.backstageViewClientControl1.Name = "backstageViewClientControl1";
            this.backstageViewClientControl1.Size = new System.Drawing.Size(582, 449);
            this.backstageViewClientControl1.TabIndex = 0;
            // 
            // labelControl14
            // 
            this.labelControl14.Appearance.Font = new System.Drawing.Font("Tahoma", 24F);
            this.labelControl14.Appearance.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.labelControl14.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.labelControl14.Location = new System.Drawing.Point(0, 0);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(406, 58);
            this.labelControl14.TabIndex = 18;
            this.labelControl14.Text = "Create New Project";
            // 
            // backstageViewClientControl2
            // 
            this.backstageViewClientControl2.Controls.Add(this.ctrOpenProject);
            this.backstageViewClientControl2.Controls.Add(this.lblOpen);
            this.backstageViewClientControl2.Location = new System.Drawing.Point(163, 0);
            this.backstageViewClientControl2.Name = "backstageViewClientControl2";
            this.backstageViewClientControl2.Size = new System.Drawing.Size(582, 449);
            this.backstageViewClientControl2.TabIndex = 1;
            // 
            // lblOpen
            // 
            this.lblOpen.Appearance.Font = new System.Drawing.Font("Tahoma", 24F);
            this.lblOpen.Appearance.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblOpen.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblOpen.Location = new System.Drawing.Point(0, 3);
            this.lblOpen.Name = "lblOpen";
            this.lblOpen.Size = new System.Drawing.Size(274, 58);
            this.lblOpen.TabIndex = 18;
            this.lblOpen.Text = "Open Project";
            // 
            // backstageViewClientControl3
            // 
            this.backstageViewClientControl3.Location = new System.Drawing.Point(163, 0);
            this.backstageViewClientControl3.Name = "backstageViewClientControl3";
            this.backstageViewClientControl3.Size = new System.Drawing.Size(582, 449);
            this.backstageViewClientControl3.TabIndex = 2;
            // 
            // backstageViewClientControl6
            // 
            this.backstageViewClientControl6.Controls.Add(this.ctrDBConnection);
            this.backstageViewClientControl6.Location = new System.Drawing.Point(163, 0);
            this.backstageViewClientControl6.Name = "backstageViewClientControl6";
            this.backstageViewClientControl6.Size = new System.Drawing.Size(582, 449);
            this.backstageViewClientControl6.TabIndex = 5;
            // 
            // backstageViewClientControl4
            // 
            this.backstageViewClientControl4.Location = new System.Drawing.Point(163, 0);
            this.backstageViewClientControl4.Name = "backstageViewClientControl4";
            this.backstageViewClientControl4.Size = new System.Drawing.Size(582, 449);
            this.backstageViewClientControl4.TabIndex = 3;
            // 
            // backstageViewClientControl5
            // 
            this.backstageViewClientControl5.Location = new System.Drawing.Point(163, 0);
            this.backstageViewClientControl5.Name = "backstageViewClientControl5";
            this.backstageViewClientControl5.Size = new System.Drawing.Size(582, 449);
            this.backstageViewClientControl5.TabIndex = 4;
            // 
            // tabNewProject
            // 
            this.tabNewProject.Caption = "New Project";
            this.tabNewProject.ContentControl = this.backstageViewClientControl1;
            this.tabNewProject.Name = "tabNewProject";
            this.tabNewProject.Selected = false;
            this.tabNewProject.ItemPressed += new DevExpress.XtraBars.Ribbon.BackstageViewItemEventHandler(this.tabNewProject_ItemPressed);
            // 
            // tabOpenProject
            // 
            this.tabOpenProject.Caption = "Open Project";
            this.tabOpenProject.ContentControl = this.backstageViewClientControl2;
            this.tabOpenProject.Name = "tabOpenProject";
            this.tabOpenProject.Selected = true;
            this.tabOpenProject.ItemPressed += new DevExpress.XtraBars.Ribbon.BackstageViewItemEventHandler(this.tabOpenProject_ItemPressed);
            // 
            // tabScenarioGroup
            // 
            this.tabScenarioGroup.Caption = "Properties";
            this.tabScenarioGroup.ContentControl = this.backstageViewClientControl3;
            this.tabScenarioGroup.Name = "tabScenarioGroup";
            this.tabScenarioGroup.Selected = false;
            // 
            // tabDatasource
            // 
            this.tabDatasource.Caption = "Data Source Option";
            this.tabDatasource.ContentControl = this.backstageViewClientControl6;
            this.tabDatasource.Name = "tabDatasource";
            this.tabDatasource.Selected = false;
            // 
            // tabHelp
            // 
            this.tabHelp.Caption = "Help";
            this.tabHelp.ContentControl = this.backstageViewClientControl4;
            this.tabHelp.Name = "tabHelp";
            this.tabHelp.Selected = false;
            // 
            // tabAbout
            // 
            this.tabAbout.Caption = "About";
            this.tabAbout.ContentControl = this.backstageViewClientControl5;
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Selected = false;
            // 
            // ctrNewProject
            // 
            this.ctrNewProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrNewProject.Location = new System.Drawing.Point(0, 45);
            this.ctrNewProject.MySimlink = null;
            this.ctrNewProject.Name = "ctrNewProject";
            this.ctrNewProject.Size = new System.Drawing.Size(567, 404);
            this.ctrNewProject.TabIndex = 0;
            // 
            // ctrOpenProject
            // 
            this.ctrOpenProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrOpenProject.HideOpenButton = false;
            this.ctrOpenProject.IsOpenProject = true;
            this.ctrOpenProject.Location = new System.Drawing.Point(14, 49);
            this.ctrOpenProject.MySimlink = null;
            this.ctrOpenProject.Name = "ctrOpenProject";
            this.ctrOpenProject.Size = new System.Drawing.Size(581, 326);
            this.ctrOpenProject.TabIndex = 19;
            this.ctrOpenProject.OpenProjectButtonClick += new CH2M.UCEditOpenProject.ButtonClickedEventHandler(this.ctrOpenProject_OpenProjectButtonClick);
            // 
            // ctrDBConnection
            // 
            this.ctrDBConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrDBConnection.Location = new System.Drawing.Point(0, 0);
            this.ctrDBConnection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ctrDBConnection.Name = "ctrDBConnection";
            this.ctrDBConnection.Size = new System.Drawing.Size(582, 449);
            this.ctrDBConnection.TabIndex = 0;
            this.ctrDBConnection.OnConnectDBClick += new SIM_API_LINKS.UCDatabaseSelection.ButtonClickedEventHandler(this.ctrDBConnection_OnConnectDBClick);
            // 
            // frmBackstageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 449);
            this.Controls.Add(this.ctrBackStage);
            this.Name = "frmBackstageView";
            this.Text = "Backstage View";
            this.ctrBackStage.ResumeLayout(false);
            this.backstageViewClientControl1.ResumeLayout(false);
            this.backstageViewClientControl1.PerformLayout();
            this.backstageViewClientControl2.ResumeLayout(false);
            this.backstageViewClientControl2.PerformLayout();
            this.backstageViewClientControl6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.BackstageViewClientControl backstageViewClientControl1;
        private DevExpress.XtraBars.Ribbon.BackstageViewClientControl backstageViewClientControl2;
        private DevExpress.XtraBars.Ribbon.BackstageViewClientControl backstageViewClientControl3;
        private DevExpress.XtraBars.Ribbon.BackstageViewTabItem tabScenarioGroup;
        public DevExpress.XtraBars.Ribbon.BackstageViewControl ctrBackStage;
        private DevExpress.XtraBars.Ribbon.BackstageViewClientControl backstageViewClientControl4;
        private DevExpress.XtraBars.Ribbon.BackstageViewClientControl backstageViewClientControl5;
        private DevExpress.XtraBars.Ribbon.BackstageViewTabItem tabHelp;
        private DevExpress.XtraBars.Ribbon.BackstageViewTabItem tabAbout;
        private CH2M.UCCreateProject ctrNewProject;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.LabelControl lblOpen;
        public DevExpress.XtraBars.Ribbon.BackstageViewTabItem tabNewProject;
        public DevExpress.XtraBars.Ribbon.BackstageViewTabItem tabOpenProject;
        private DevExpress.XtraBars.Ribbon.BackstageViewClientControl backstageViewClientControl6;
        private SIM_API_LINKS.UCDatabaseSelection ctrDBConnection;
        public DevExpress.XtraBars.Ribbon.BackstageViewTabItem tabDatasource;
        private UCEditOpenProject ctrOpenProject;
    }
}