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
using CH2M;
namespace SIM_API_LINKS
{
    public partial class frmCreateBatchFile : DevExpress.XtraEditors.XtraForm
    {
        private simlink _simlink;
        public frmCreateBatchFile(simlink simlink)
        {
            InitializeComponent();
            _simlink = simlink;
        }

        private void frmCreateBatchFile_Load(object sender, EventArgs e)
        {

        }

        private void chkDefaultLocation_CheckedChanged(object sender, EventArgs e)
        {
            bool blnIsEnabled = bool.Parse(chkDefaultLocation.EditValue.ToString());
            lblPath.Enabled = !blnIsEnabled;
            btnEditBrowsePath.Enabled = !blnIsEnabled;
        }

        private void btnEditBrowsePath_DoubleClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                btnEditBrowsePath.Text = folder.SelectedPath;
            }
        }
        /// <summary>
        /// Create run 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string strPath = (bool.Parse(chkDefaultLocation.EditValue.ToString()) == true ? "DEFAULT" : btnEditBrowsePath.Text);
                _simlink.CreateBatchFile_ByEval(_simlink.GetReferenceEvalID(), int.Parse(txtNoOfRun.Text), "swmm5.exe", strPath);
            }
        }

        private bool ValidateInput()
        {
            if (CommonUtilities.IsInteger(txtNoOfRun.Text) == false)
            {
                Commons.ShowMessage("Please make sure number of run is an integer value!");
                return false;
            }
            else
            {
                int intNoOfRun = int.Parse(txtNoOfRun.Text);
                if (intNoOfRun <=0)
                {
                    Commons.ShowMessage("Please make sure number of run is greater than zero!");
                    return false;
                }
            }
            if (bool.Parse(chkDefaultLocation.EditValue.ToString()) == false)
            {
                if (System.IO.Directory.Exists(btnEditBrowsePath.Text) == false)
                {
                    Commons.ShowMessage("Please select a valid folder!");
                    return false;
                }
                
            }
            return true;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}