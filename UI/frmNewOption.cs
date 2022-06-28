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
using CH2M.SimLink.MainUI;

namespace SIM_API_LINKS
{
    public partial class frmNewOption : DevExpress.XtraEditors.XtraForm
    {
        private simlink _simlink;
        private string _strNewlyCreatedOption;

        public string NewlyCreatedOption
        {
            get { return _strNewlyCreatedOption; }
            set { _strNewlyCreatedOption = value; }
        }
        public frmNewOption(simlink simlink)
        {
            InitializeComponent();
            _simlink = simlink;
            // populate data to combo
            PopulateData2Combo();
        }
        /// <summary>
        /// Populate data to combo
        /// </summary>
        private void  PopulateData2Combo()
        {
            //#region Get New Val Method

            //lkpOperation.Properties.DataSource = _simlink.GetOperationLookUp();
            //lkpOperation.Properties.DisplayMember = "Operation";
            //lkpOperation.Properties.ValueMember = "Operation";

            //#endregion  Get New Val Method

            //#region Var Type FK

            //lkpVarType.Properties.DataSource = _simlink.GetVarTypeLookUp();
            //lkpVarType.Properties.DisplayMember = "FieldName";
            //lkpVarType.Properties.ValueMember = "VarType_FK";

            //#endregion Var Type FK
        }
        public string InputName
        {
            get { return txtValue.Text; }
        }
        private bool IsValidated()
        {
            if (txtValue.Text == "")
            {
                Commons.ShowMessage("Please enter the label!");
                return false;
            }
            if (_simlink.CheckOptionElementNameExists(txtValue.Text, true))
            {
                CH2M.Commons.ShowMessage("Name already exists! Please enter a new name.");
                return false;
            }
            //if (lkpOperation.ItemIndex<0)
            //{
            //    CH2M.Commons.ShowMessage("Please select operation.");
            //    return false;
            //}
            //if (lkpVarType.ItemIndex<0)
            //{
            //    CH2M.Commons.ShowMessage("Please select a var type.");
            //    return false;
            //}
            return true;
        }
        /// <summary>
        /// Inserting to option list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                //DataRowView rowView = lkpVarType.GetSelectedDataRow() as DataRowView;
                //DataRow row = rowView.Row;
                //int intVarTypeId = int.Parse(row["VarType_FK"].ToString());

                //DataRowView rowViewOperation = lkpOperation.GetSelectedDataRow() as DataRowView;
                //DataRow rowOps = rowViewOperation.Row;
                //string strOperation = rowOps["Operation"].ToString();

                _strNewlyCreatedOption = txtValue.Text;
                // create a new option
                _simlink.CreateNewOption(_simlink._nActiveProjID, txtValue.Text);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void frmNewOption_Load(object sender, EventArgs e)
        {

        }
    }
}