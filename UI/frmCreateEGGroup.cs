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
    public partial class frmCreateEGGroup : DevExpress.XtraEditors.XtraForm
    {
        private simlink _simlink;
        CH2M.SimLink.MainUI.frmSimLinkMain _frmMain;
        /// <summary>
        /// Constructor
        /// </summary>
        public frmCreateEGGroup(simlink simlink, CH2M.SimLink.MainUI.frmSimLinkMain frmMain)
        {
            InitializeComponent();
            _simlink = simlink;
            _frmMain = frmMain;
            // load EG Groip to combobox
            LoadEGGroupToCombo(); 
        }

        /// <summary>
        /// Close dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Create EG group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (IsValidated()) // if it's valid
            {
                DataRowView rowView = cbeRefGroup.GetSelectedDataRow() as DataRowView;
                DataRow row = rowView.Row;
                int intRefGroupId = int.Parse(row["EvaluationID"].ToString());
                _simlink.CreateEG(_simlink._nActiveModelTypeID, "", txtLabel.Text, _simlink._nActiveProjID, intRefGroupId, memDescription.Text, _simlink._nActiveBaselineScenarioID);

                //_simlink.CreateEvaluationGroup(txtLabel.Text, memDescription.Text, strRefGroupId, _simlink._nActiveProjID, _simlink._nActiveModelTypeID, _simlink._nActiveBaselineScenarioID);
                Commons.ShowMessage("Successfully created evaluation group!", MessageBoxIcon.Information);
                this.Close(); // close dialog

                // refresh the EG into the current menu
                // get the selected evaluation group
                DataTable dtEG = _simlink.GetEGGroup(_simlink._nActiveProjID); // get the evalulation group

                // add to menu for the group
                _frmMain.AddEGToSelectMenu(dtEG);
            }
        }
        private void LoadEGGroupToCombo()
        {
            cbeRefGroup.Properties.DataSource = _simlink.GetEVGroupLookUp(_simlink._nActiveProjID.ToString());
            cbeRefGroup.Properties.ValueMember = "EvaluationID";
            cbeRefGroup.Properties.DisplayMember = "EvaluationLabel";

            cbeRefGroup.EditValue = "-1";
        }
        /// <summary>
        /// Is validated
        /// </summary>
        /// <returns></returns>
        private bool IsValidated()
        {
            if (txtLabel.Text =="")
            {
                Commons.ShowMessage("Please enter the label!");
                return false;
            }
            if (memDescription.Text =="")
            {
                Commons.ShowMessage("Please enter the description!");
                return false;
            }
            if (cbeRefGroup.ItemIndex <0)
            {
                Commons.ShowMessage("Please select EG Reference!");
                return false;
            }
            return true;
        }
    }
}