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
using CH2M.SimLink.MainUI;

namespace SIM_API_LINKS
{
    public partial class frmElementList : DevExpress.XtraEditors.XtraForm
    {
        private simlink _simlink;
        private int _intProjectID;

        public frmElementList(frmSimLinkMain frmMain)
        {
            InitializeComponent();
            ctrElementList.SimlinkMainDialog = frmMain;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="simlink"></param>
        /// <param name="intProjectID"></param>
        public frmElementList(simlink simlink, int intProjectID, frmSimLinkMain frmMain)
        {
            InitializeComponent();

            // TODO: Complete member initialization
            this._simlink = simlink;
            this._intProjectID = intProjectID;
            ctrElementList.SimlinkMainDialog = frmMain;

        }
        /// <summary>
        /// Load element list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmElementList_Load(object sender, EventArgs e)
        {
            ctrElementList.LoadElementListData(_simlink, _intProjectID);
        }

        private void ctrElementList_OnCloseDialog(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}