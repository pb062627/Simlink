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
    public partial class frmNewElement : DevExpress.XtraEditors.XtraForm
    {
        private simlink _simlink;
        private string _strNewlyCreatedElement = "";

        public string NewlyCreatedElement
        {
            get { return _strNewlyCreatedElement; }
            set { _strNewlyCreatedElement = value; }
        }
        public frmNewElement(simlink simlink)
        {
            InitializeComponent();

            _simlink = simlink;

            //lkpTable.Properties.DataSource = _simlink.GetLinkTableCodeLookUp();
            //lkpTable.Properties.DisplayMember = "LinkTableCodeName";
            //lkpTable.Properties.ValueMember = "LinkTableCodeID";
            
        }

        private bool IsValidated()
        {
            if (txtName.Text == "")
            {
                Commons.ShowMessage("Please enter a valid name!");
                return false;
            }
            if (_simlink.CheckOptionElementNameExists(txtName.Text, false))
            {
                CH2M.Commons.ShowMessage("Name already exists! Please enter a new name.");
                return false;
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                //DataRowView rowView = lkpTable.GetSelectedDataRow() as DataRowView;
                //DataRow row = rowView.Row;
                //int intTableId = int.Parse(row["LinkTableCodeID"].ToString());

                _strNewlyCreatedElement = txtName.Text;
                _simlink.CreateNewEelement(_simlink._nActiveProjID, txtName.Text, txtType.Text);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}