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
    public partial class frmOptionList : DevExpress.XtraEditors.XtraForm
    {
        private simlink _simlink = null;
        private int _intProjectID;
        
        public frmOptionList(simlink simlink, int intProjectID, frmSimLinkMain frmMain)
        {
            InitializeComponent();
            
            // simlink
            _simlink = simlink;
            _intProjectID = intProjectID;

            ucOptionList1.SimlinkMainDialog = frmMain;
        }

        /// <summary>
        /// Optionlist initalisation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOptionList_Load(object sender, EventArgs e)
        {
           // load option list
           ucOptionList1.LoadOptionListData(_simlink, _intProjectID);
        }
        /// <summary>
        /// Closing dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrOptionList_OnCloseDialog(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}