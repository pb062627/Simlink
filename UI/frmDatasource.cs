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
    public partial class frmDatasource : DevExpress.XtraEditors.XtraForm
    {
        frmSimLinkMain _frmMain;
        public frmDatasource(frmSimLinkMain frmMain)
        {
            _frmMain = frmMain;
            InitializeComponent();
        }

        private void frmDatasource_Load(object sender, EventArgs e)
        {
            ctrDBConnection.LoadExistingConfig(); // load the existing config
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

                // re-initiate on project opening
                _frmMain.ClearFloatingGrid();

            }
            if (ctrDBConnection.IsDBConnected)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
         }

        private void frmDatasource_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = (ctrDBConnection.IsDBConnected ? System.Windows.Forms.DialogResult.OK : System.Windows.Forms.DialogResult.No);
        }
    }
}