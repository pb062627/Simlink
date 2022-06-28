using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SIM_API_LINKS;
using CH2M;
using System.IO;
using SIM_API_LINKS.Properties;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;

namespace CH2M.SimLink.MainUI
{
    public partial class frmSimLinkMain : DevExpress.XtraEditors.XtraForm
    {
        frmBackstageView BackstageViewForm = null;
        public simlink _simlink = null;
        public frmSimLinkMain()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }
        /// <summary>
        /// clear floating grid
        /// </summary>
        public void ClearFloatingGrid()
        {
            dvGridEditor1.ClearGrid();
            ctrResultSummary.ClearGrid();
            ctrResultTimeseries.ClearGrid();
            ctrUCPerformance.ClearGrid();
            ctrEventResult.ClearGrid();
        }
        /// <summary>
        /// Re-intiate connection
        /// </summary>
        public void ReInitateConnection()
        {
            string strConnectionstring = SIM_API_LINKS.Properties.Settings.Default.ConnectionString;
            int intConnectionType = (SIM_API_LINKS.Properties.Settings.Default.DatabaseType == "OLEDB" ? 0 : 1);
            _simlink = new simlink();
            _simlink.InitializeModelLinkage(strConnectionstring, intConnectionType, false);
        }

        void OpenDropDownBackstageControl()
        {
            ribbonControl.ShowApplicationButtonContentControl();
            BackstageViewForm.Select();
            
        }
        private void ribbonControl_ApplicationButtonClick(object sender, EventArgs e)
        {
            BackstageViewForm.Load();
        }

        private void iNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenDropDownBackstageControl();
            BackstageViewForm.tabNewProject.Selected = true;
        }

        private void iOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenDropDownBackstageControl();
            BackstageViewForm.tabOpenProject.Selected = true;
        }
        private DevExpress.XtraBars.BarCheckItem _currentSelectedEG = null;
        public void AddEGToSelectMenu(DataTable dtEG)
        {
            barSelectEGGroup.ClearLinks();
            bool blnIsCheck = false;
            foreach(DataRow row in dtEG.Rows)
            {
                DevExpress.XtraBars.BarCheckItem item = new DevExpress.XtraBars.BarCheckItem();
                if (blnIsCheck==false)
                {
                    item.Checked = true;
                    blnIsCheck = true;
                    _currentSelectedEG = item;
                }
                item.Tag = row;
                item.Caption = row["EvaluationLabel"].ToString();
                item.ItemClick += SelectEvalGroup_ItemClick;
                barSelectEGGroup.AddItem(item);
            }
        }

        /// <summary>
        /// select evaluation group item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectEvalGroup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DevExpress.XtraBars.BarCheckItem myItem = (DevExpress.XtraBars.BarCheckItem)e.Item;
                DataRow rowVal = (DataRow)myItem.Tag;
                _simlink.UpdateActiveEG(rowVal);

                if (File.Exists(_simlink._sActiveModelLocation) == false)
                {
                    Commons.ShowMessage("Warning: Model file '" + _simlink._sActiveModelLocation + "' was not found!");
                }
                // update label
                barActiveEG.Caption = "Active Group: " + _simlink._strCurrentEGLabel + " (ID: " + (_simlink._nActiveEvalID == 0 ? "" : _simlink._nActiveEvalID.ToString() + ")");

                // intialise EG simlink
                _simlink.InitializeEG(_simlink._nActiveEvalID);

                // Load to DV grid on initializing
                dvGridEditor1.LoadDVData(_simlink, _simlink._nActiveModelTypeID);

                // load result summary
                ctrResultSummary.LoadResultData(_simlink);

                // load timeseries summary
                ctrResultTimeseries.LoadResultData(_simlink);
                // set active
                foreach (DevExpress.XtraBars.BarCheckItemLink eachItem in barSelectEGGroup.ItemLinks)
                {
                    if (eachItem.Item != myItem)
                    {
                        DevExpress.XtraBars.BarCheckItem thisItem = (DevExpress.XtraBars.BarCheckItem)eachItem.Item;
                        thisItem.Checked = false;
                    }
                }
                _currentSelectedEG = (DevExpress.XtraBars.BarCheckItem)e.Item; // update the current item
            }
            catch(Exception ex)
            {
                DevExpress.XtraBars.BarCheckItem thisItem = (DevExpress.XtraBars.BarCheckItem)e.Item;
                thisItem.Checked = false;
                if (_currentSelectedEG != null) _currentSelectedEG.Checked = true;
                Commons.ShowMessage("Error in selecting evaluation group!");
            }
        }

        private void btnOptionList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_simlink._nActiveProjID != -1)
            {
                frmOptionList frm = new frmOptionList(_simlink, _simlink._nActiveProjID, this);
                frm.ShowDialog();
            }
            else
            {
                CH2M.Commons.ShowMessage("Please open a project before accessing option list.", MessageBoxIcon.Exclamation);
            }
        }

        private void btnElementList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_simlink._nActiveProjID != -1)
            {
                frmElementList frm = new frmElementList(_simlink, _simlink._nActiveProjID, this);
                frm.ShowDialog();
            }
            else
            {
                CH2M.Commons.ShowMessage("Please open a project before accessing element list.", MessageBoxIcon.Exclamation);
            }
        }
        /// <summary>
        /// Define runs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnDefineRun_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_simlink._nActiveProjID !=-1 || _simlink._nActiveEvalID !=-1)
            {
                frmDefineRuns frmDefine = new frmDefineRuns(_simlink);
                frmDefine.Show();
            }
        }

        private void barMnuShowDVGrid_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetDockVisibility(dockDVGrid, barMnuShowDVGrid, "DV Editor Grid");
        }
        private void barMnuShowResultSummary_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetDockVisibility(dockResultSummary, barMnuShowResultSummary, "Result Summary");
        }
        private void barMnuShowResultTS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetDockVisibility(dockResultTimeseries, barMnuShowResultTS, "Result Timeseries");
        }
        /// <summary>
        /// Show/hide dock panel
        /// </summary>
        /// <param name="dock"></param>
        /// <param name="item"></param>
        /// <param name="strCaption"></param>
        private void SetDockVisibility(DevExpress.XtraBars.Docking.DockPanel dock, DevExpress.XtraBars.BarButtonItem item, string strCaption)
        {
            DevExpress.XtraBars.Docking.DockVisibility vis = dock.Visibility;
            if (vis == DevExpress.XtraBars.Docking.DockVisibility.Visible)
            {
                dock.HideImmediately();
                item.Caption = "Show " + strCaption;
            }
            else
            {
                dock.Show();
                item.Caption = "Hide " + strCaption;
            }
        }

        private void blciPanels_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.XtraBars.Docking.DockVisibility vis = dockDVGrid.Visibility;
            barMnuShowDVGrid.Caption = (vis == DevExpress.XtraBars.Docking.DockVisibility.Visible ? "Hide" : "Show") + " DV Editor Grid";

            vis = dockResultSummary.Visibility;
            barMnuShowResultSummary.Caption = (vis == DevExpress.XtraBars.Docking.DockVisibility.Visible ? "Hide" : "Show") + " Result Summary";

            vis = dockResultTimeseries.Visibility;
            barMnuShowResultTS.Caption = (vis == DevExpress.XtraBars.Docking.DockVisibility.Visible ? "Hide" : "Show") + " Result Timeseries";
        }

        private void barRunScenario_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Commons.ShowMessage("Run all scenario will be implemented here!");
            _simlink.ProcessEvaluationGroup(new string[0]); // run all scenarios when there is no scenario id do not exist
        }
        /// <summary>
        /// Create a new group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewGroup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCreateEGGroup frm = new frmCreateEGGroup(_simlink, this);
            frm.ShowDialog();
        }
        /// <summary>
        /// Reset layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbiResetLayout_ItemClick(object sender, ItemClickEventArgs e)
        {
            dockSimLinkMain.RestoreLayoutFromXml(SimLinkConstants.DockLayoutFilename);
        }
        public void SaveLayout(string layoutName)
        {
            if (string.IsNullOrWhiteSpace(layoutName))
                return;

            layoutName = layoutName.Trim();
            layoutName = String.Join("", layoutName.Split(Path.GetInvalidFileNameChars()));
            string layoutfile = Path.Combine(SimLinkConstants.USER_SETTINGS_FOLDER, layoutName + "_panel_layout.xml");
            dockSimLinkMain.SaveLayoutToXml(layoutfile);

            string ribbonLayoutfile = Path.Combine(SimLinkConstants.USER_SETTINGS_FOLDER, layoutName + "_ribbon_layout.xml");
            ribbonControl.SaveLayoutToXml(ribbonLayoutfile);

            Settings.Default.CurrentLayout = layoutName;

            if (Settings.Default.LayoutList.Contains(layoutName) == false)
                Settings.Default.LayoutList.Add(layoutName);

            Settings.Default.Save();
        }
        public void RestoreLayout(string layoutName)
        {
            if (string.IsNullOrWhiteSpace(layoutName))
                return;

            layoutName = layoutName.Trim();
            layoutName = String.Join("", layoutName.Split(Path.GetInvalidFileNameChars()));

            Settings.Default.CurrentLayout = layoutName;
            Settings.Default.Save();

            string layoutfile = Path.Combine(SimLinkConstants.USER_SETTINGS_FOLDER, layoutName + "_panel_layout.xml");
            // Check for invalid filename chars YW: this screw up thing comment out
            //if (File.Exists(layoutfile)) // check if file exists
            //{
            //    try
            //    {
            //        dmHydra.RestoreLayoutFromXml(layoutfile);
            //    }
            //    catch (Exception)
            //    {


            //    }
            //}

            string ribbonLayoutfile = Path.Combine(SimLinkConstants.USER_SETTINGS_FOLDER, layoutName + "_ribbon_layout.xml");
            //if (File.Exists(ribbonLayoutfile)) // check if file exists YW: this screw up thing comment out
            //{
            //    try
            //    {
            //        ribbonControl.RestoreLayoutFromXml(ribbonLayoutfile);
            //    }
            //    catch (Exception)
            //    {


            //    }
            //}
        }

        /// <summary>
        /// Deletes the layout of the panels and ribbon to the currently selected layout name
        /// </summary>
        public void DeleteLayout(string layoutName)
        {
            if (string.IsNullOrWhiteSpace(layoutName))
                return;

            layoutName = layoutName.Trim();
            layoutName = String.Join("", layoutName.Split(Path.GetInvalidFileNameChars()));
            string layoutfile = Path.Combine(SimLinkConstants.USER_SETTINGS_FOLDER, layoutName + "_panel_layout.xml");
            if (File.Exists(layoutfile))
                File.Delete(layoutfile);

            string ribbonLayoutfile = Path.Combine(SimLinkConstants.USER_SETTINGS_FOLDER, layoutName + "_ribbon_layout.xml");
            if (File.Exists(ribbonLayoutfile))

                if (Settings.Default.LayoutList.Contains(layoutName) == false)
                    Settings.Default.LayoutList.Remove(layoutName);

            Settings.Default.Save();
        }
        private void barCheckItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var barCheckItem = new BarCheckItem();
            barCheckItem = e.Item as BarCheckItem;

            DockPanel pane = dockSimLinkMain.Panels[barCheckItem.Name.ToString()];
            pane.Visibility = barCheckItem.Checked ? DockVisibility.Visible : DockVisibility.Hidden;
        }
        private void blciLayouts_Popup(object sender, EventArgs e)
        {
            SetupLayoutMenu();
        }
        /// <summary>
        /// Sets up the Panel check menu items depending on the panels visibility 
        /// </summary>
        private void SetupPanelPopup()
        {
            // setup the panel check menu items  
            blciPanels.BeginUpdate();

            blciPanels.ItemLinks.Clear();
            foreach (DockPanel pane in dockSimLinkMain.Panels)
            {
                if ((!pane.Tabbed) && (pane.Count == 0)) // only show if not a tabbed or container panel 
                {
                    //blciPanels.AddItem(pane.Name);
                    var barCheckItem = new BarCheckItem { Caption = string.Format(pane.Text) };
                    barCheckItem.Name = pane.Name;
                    barCheckItem.Checked = (pane.Visibility == DockVisibility.Visible);
                    barCheckItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barCheckItem_ItemClick);

                    blciPanels.ItemLinks.Add(barCheckItem);
                }
            }

            blciPanels.EndUpdate();
        }
        private void SetupLayoutMenu()
        {
            // setup the panel check menu items  
            blciLayouts.BeginUpdate();

            blciLayouts.ItemLinks.Clear();

            bool defaultFound = false;

            // add the list of stored layouts to the Layout selector combo;
            foreach (string str in Settings.Default.LayoutList)
            {
                // if the layout file exists then add it to the Layout list
                string layoutfile = Path.Combine(SimLinkConstants.USER_SETTINGS_FOLDER, str + "_panel_layout.xml");
                if (File.Exists(layoutfile))
                {
                    var barButtonItem = new BarButtonItem { Caption = string.Format(str) };
                    barButtonItem.Name = str;
                    barButtonItem.ButtonStyle = BarButtonStyle.Check;
                    barButtonItem.GroupIndex = 10;
                    barButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.layoutButtonItem_ItemClick);

                    blciLayouts.ItemLinks.Add(barButtonItem);
                    if (str.Equals(Settings.Default.CurrentLayout))
                        barButtonItem.Down = true;

                    if (str.Equals("Default"))
                        defaultFound = true;
                }
            }
            // make sure there is always a "Default" option in the layout list
            if (defaultFound == false)
            {
                var barButtonItem = new BarButtonItem { Caption = string.Format("Default") };
                barButtonItem.Name = "Default";
                barButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.layoutButtonItem_ItemClick);

                blciLayouts.ItemLinks.Insert(0, barButtonItem);
                SaveLayout("Default");
            }

            // Add the manage layouts button
            var bbi = new BarButtonItem { Caption = string.Format("Manage Layouts") };
            bbi.Name = "ManageLayouts";
            bbi.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiManageLayout_ItemClick);

            BarItemLink bil = blciLayouts.ItemLinks.Add(bbi);
            bil.BeginGroup = true;
            

            // Add the Save layouts button
            var bbiSave = new BarButtonItem { Caption = string.Format("Save Layout") };
            bbiSave.Name = "SaveLayout";
            bbiSave.ImageIndex = -1;
            bbiSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiSaveLayout_ItemClick);
            blciLayouts.ItemLinks.Add(bbiSave);

            // Add the Reset  layouts button
            var bbiReset = new BarButtonItem { Caption = string.Format("Reset Layout") };
            bbiReset.Name = "ResetLayout";
            bbiReset.ImageIndex = -1;
            bbiReset.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbiResetLayout_ItemClick);
            blciLayouts.ItemLinks.Add(bbiReset);

            blciLayouts.EndUpdate();
        }
        /// <summary>
        /// Enable or disable items if project is not open
        /// </summary>
        /// <param name="blnIsEnabled"></param>
        public void EnableOrDisableBarItems(bool blnIsEnabled)
        {
            string[] astrSkipThisButton = new string[] { "iNew", "iOpen", "iClose", "iFind", 
                "iSaveAs", "iExit", "iHelp", "iAbout", "iResetLayout" };
            foreach (BarItem item in ribbonControl.Manager.Items)
            {
                if (!astrSkipThisButton.Contains(item.Name))
                {
                    item.Enabled = blnIsEnabled;
                }
            }
        }
        private void layoutButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var barButtonItem = new BarButtonItem();
            barButtonItem = e.Item as BarButtonItem;

            RestoreLayout(barButtonItem.Name.ToString());
        }
        private void bbiSaveLayout_ItemClick(object sender, ItemClickEventArgs e)
        {
            string layout = fmLayoutManager.ShowPrompt("Please enter layout name...", "Save Layout", Settings.Default.CurrentLayout);
            SaveLayout(layout);
        }
        private void bbiManageLayout_ItemClick(object sender, ItemClickEventArgs e)
        {
            fmLayoutManager fm = new fmLayoutManager(this);
            fm.ShowDialog();
            
        }
        //bbiManageLayout_ItemClick
        private void frmSimLinkMain_Load(object sender, EventArgs e)
        {
            //string strConnectionstring = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Application.StartupPath + "\\SimLink2.0_NewHaven_LOCAL.mdb";
            ReInitateConnection(); // initiate connection

            BackstageViewForm = new frmBackstageView(_simlink, this);
            // change the Application Button (tab) from a default icon to text
            ribbonControl.ApplicationButtonDropDownControl = BackstageViewForm.ctrBackStage;

            if (_simlink._dbContext.CurrentDBConnection.State == ConnectionState.Closed || _simlink.IsValidConnection == false)
            {
                //ribbonControl.ShowApplicationButtonContentControl();
                //BackstageViewForm.tabDatasource.Selected = true;
                frmDatasource frm = new frmDatasource(this);
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BackstageViewForm.InitiateControls(_simlink, this);
                }
            }

            //ribbonControl.ApplicationButtonText = HydraUIConstants.TEXT_PROJECT;

            // Restore the currently selected layout
            RestoreLayout(Settings.Default.CurrentLayout);
            // save the default panel layout if it does not exist
            if (!Directory.Exists(SimLinkConstants.USER_SETTINGS_FOLDER))
                Directory.CreateDirectory(SimLinkConstants.USER_SETTINGS_FOLDER);

            //if (! File.Exists(FloodModellerUIConstants.DOCKING_LAYOUT_FILE))
            dockSimLinkMain.SaveLayoutToXml(SimLinkConstants.DEFAULT_DOCKING_LAYOUT_FILE);

            // save the default ribbon layout if it doen't exist
            //if (! File.Exists(FloodModellerUIConstants.RIBBON_LAYOUT_FILE))
            ribbonControl.SaveLayoutToXml(SimLinkConstants.DEFAULT_RIBBON_LAYOUT_FILE);

            EnableOrDisableBarItems(false); // disable
        }

        private void frmSimLinkMain_Shown(object sender, EventArgs e)
        {
            dockSimLinkMain.SaveLayoutToXml(SimLinkConstants.DockLayoutFilename);
            SetupPanelPopup();
            SetupLayoutMenu();
        }

        private void barBtnCreateBatch_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmCreateBatchFile frm = new frmCreateBatchFile(_simlink);
            frm.ShowDialog();
        }


        //public ItemClickEventHandler bbiManageLayout_ItemClick { get; set; }
    }
}