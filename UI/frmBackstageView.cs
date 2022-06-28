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

namespace CH2M.SimLink.MainUI
{
    public partial class frmBackstageView : DevExpress.XtraEditors.XtraForm
    {
        private DevExpress.XtraSplashScreen.SplashScreenManager ssmOpenProject;
        private simlink _simLink;
        private frmSimLinkMain _frmMain;

        #region Properties
        public simlink SimLink
        {
            get { return _simLink; }
            set { _simLink = value; }
        }
        #endregion Properties

        #region Constructor
        public frmBackstageView(simlink simLink, frmSimLinkMain frmMainSimlink)
        {
            InitializeComponent();

            InitiateControls(simLink, frmMainSimlink); // intiate control
        }
        #endregion Constructor

        public void InitiateControls(simlink sim, frmSimLinkMain frmMain)
        {
            _frmMain = frmMain;
            _simLink = sim;
            if (_simLink._dbContext.CurrentDBConnection.State == ConnectionState.Open)
            {
                ctrOpenProject.LoadProject(_simLink);
                ctrNewProject.InitiateControl(_simLink);
                ctrDBConnection.LoadExistingConfig(); // load the existing config
            }
        }
        #region Private Methods
        /// <summary>
        /// Load back stage view
        /// </summary>
        public void Load()
        {
            this.ssmOpenProject = new DevExpress.XtraSplashScreen.SplashScreenManager();
        }
        #endregion Private Methods

        #region Form Event Handler
        /// <summary>
        /// Open a project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabOpenProject_ItemPressed(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            if (_simLink._dbContext.CurrentDBConnection.State == ConnectionState.Closed) return;
            ctrOpenProject.LoadProject(_simLink);
        }
        /// <summary>
        /// Create a new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabNewProject_ItemPressed(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            if (_simLink._dbContext.CurrentDBConnection.State == ConnectionState.Closed) return;
            ctrNewProject.InitiateControl(_simLink);
        }
        #endregion Form Event Handler

        /// <summary>
        /// Open project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrOpenProject_OpenProjectButtonClick(object sender, EventArgs e)
        {
            try
            {
                int intDBType = _frmMain._simlink._dbContext.GetDBTypeAsInt();
                _frmMain.ribbonControl.HideApplicationButtonContentControl();
                foreach (int intSelRow in ctrOpenProject.viewProject.GetSelectedRows())
                {
                    DataRowView rowView = (DataRowView)ctrOpenProject.viewProject.GetRow(intSelRow);
                    int intProjectID = int.Parse(rowView.Row["projid"].ToString());
                    SIM_API_LINKS.Project project = (Project)_simLink.LoadSelectedProject(intProjectID); // load selected project

                    int nActiveEvalID = _frmMain._simlink.GetEvalIDFromProj(intProjectID);  // return the first EG associated with the proj

                    if (project.ModelTypeID == 1)
                    {
                        //note from met: the previou code did not declare the object of type swm5022_link, and so was not calling the derived class functions.
                        swmm5022_link swmm = new swmm5022_link();
                        // load eg simlink specification
                        // this may be duplicating new code on UI side... not sure yet.
                        swmm.InitializeModelLinkage(_frmMain._simlink._dbContext._sConnectionString, intDBType, false); //todo: handle connection string properly as well as 3rd htcondor arg
                        swmm.InitializeEG(nActiveEvalID);         //todo: replace with properly retrieved EG



                        // copy object properties
                        // not sure the logic of this        CopyObjectProperties(swmm);

                        // update the main form modelling package//
                        _frmMain._simlink = swmm;        //    _simLink;
                    }
                    else if (project.ModelTypeID == CommonUtilities._nModelTypeID_EPANET)
                    {
                        EPANET_link epanet = new EPANET_link();

                        // copy object properties
                        //          CopyObjectProperties(_simLink);

                        epanet.InitializeModelLinkage(_frmMain._simlink._dbContext._sConnectionString, intDBType, false); //todo: handle connection string properly as well as 3rd htcondor arg
                        epanet.InitializeEG(nActiveEvalID);         //todo: replace with properly retrieved EG

                        // update the main form modelling package//
                        _frmMain._simlink = epanet;        //    _simLink;
                    }

                    // if (_simLink._dbContext == null)
                    // {
                    //     // do not understand this- SWMM object gets overwritten to reg simlink.... 
                    //_simLink = _frmMain._simlink; // update new simlink   ??
                    //     //seems to cause fail
                    // // get to compile    _simLink._dbContext = _frmMain._simlink._dbContext; // is this what we want?  --> yes, but causes error in last line of this function.

                    // }
                    // get the selected evaluation group
                    DataTable dtEG = _simLink.GetEGGroup(intProjectID); // get the evalulation group

                    _frmMain.barActiveEG.Caption = "Active Group: " + _simLink._strCurrentEGLabel + " (ID: " + (_simLink._nActiveEvalID == 0 ? "" : _simLink._nActiveEvalID.ToString() + ")");
                    _frmMain.barActiveProject.Caption = "Active project: " + project.ProjectName;

                    // add to menu for the group
                    _frmMain.AddEGToSelectMenu(dtEG);

                    // Load to DV grid on initializing
                    ReInitiateOnProjectOpening();

                    _frmMain.EnableOrDisableBarItems(true);

                }
            }
            catch(Exception ex)
            {
                Commons.ShowMessage("Error opening project '" + ex.Source + ": " + ex.Message);
            }
        }
        /// <summary>
        /// Copy the existing object properties to the new simlink
        /// Update: take Simlink object
        /// MET still don't understand why we have this.
        /// </summary>
        /// 
        //met changed for temp debugging purposes 
        // this comment should not go into master branch (nor code change below)

        private void CopyObjectProperties(simlink mySimLink)
        {
            mySimLink._nActiveBaselineScenarioID = _frmMain._simlink._nActiveBaselineScenarioID;
            mySimLink._nActiveEvalID = _frmMain._simlink._nActiveEvalID;
            mySimLink._nActiveModelTypeID = _frmMain._simlink._nActiveModelTypeID;
            mySimLink._nActiveProjID = _frmMain._simlink._nActiveProjID;
            mySimLink._nActiveReferenceEG_BaseScenarioID = _frmMain._simlink._nActiveReferenceEG_BaseScenarioID;
            mySimLink._nActiveReferenceEvalID = _frmMain._simlink._nActiveReferenceEvalID;
            mySimLink._nActiveScenarioID = _frmMain._simlink._nActiveScenarioID;
            mySimLink._nDB_WriteLevel = _frmMain._simlink._nDB_WriteLevel;
            mySimLink._nPerformanceID_Objective = _frmMain._simlink._nPerformanceID_Objective;
            mySimLink._sTS_GroupID = _frmMain._simlink._sTS_GroupID;
            mySimLink._tsRepo = _frmMain._simlink._tsRepo;
            mySimLink._sDNA_Delimiter = _frmMain._simlink._sDNA_Delimiter;  

            if (_simLink._dbContext == null)
            {
                _simLink._dbContext = _frmMain._simlink._dbContext;
            }
        }
        private void ReInitiateOnProjectOpening()
        {
            _frmMain.dvGridEditor1.LoadDVData(_simLink, _simLink._nActiveModelTypeID);
            _frmMain.ctrResultSummary.LoadResultData(_simLink);
            _frmMain.ctrResultTimeseries.LoadResultData(_simLink);
            _frmMain.ctrUCPerformance.LoadPerformance(_simLink);
            _frmMain.ctrEventResult.LoadEventResult(_simLink);
        }

        /// <summary>
        /// When successfully connected then use a new connectio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrDBConnection_OnConnectDBClick(object sender, EventArgs e)
        {
            if (ctrDBConnection.IsDBConnected)
            {
                _frmMain.ReInitateConnection(); // reinitiate connection on main form

                _simLink = _frmMain._simlink;
                
                // hide the back stage view
                _frmMain.ribbonControl.HideApplicationButtonContentControl();

                // re-initiate on project opening
                _frmMain.ClearFloatingGrid();
            }
        }
    }
}