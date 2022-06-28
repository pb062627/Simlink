using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using CH2M;
using CH2M.SimLink.MainUI;

namespace SIM_API_LINKS
{
    public partial class UCOptionList : UserControl
    {
        private string _strSQLUsed = "";
        private DataSet _dsResultData = new DataSet();
        private simlink _simLink;
        private int _intProjectID;
        public delegate void ButtonClickedEventHandler(object sender, EventArgs e);
        public event ButtonClickedEventHandler OnCloseDialog;

        public UCOptionList()
        {
            InitializeComponent();
        }
        public frmSimLinkMain SimlinkMainDialog
        {
            get;
            set;
        }
        public void LoadOptionListData(simlink simLink, int intProjectID)
        {
            _simLink = simLink;
            _intProjectID = intProjectID;
            _dsResultData = _simLink.GetOptionListDetails(_intProjectID.ToString());

            cboOption.Properties.DataSource = simLink.GetOptionLookUp(intProjectID.ToString());
            cboOption.Properties.DisplayMember = "OptionLabel";
            cboOption.Properties.ValueMember = "OptionID";
        }
        /// <summary>
        /// Validate row before inserting/update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdDVView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            foreach (GridColumn col in view.Columns)
            {
                object objValue = view.GetRowCellValue(e.RowHandle, col);
                if (objValue == DBNull.Value)
                {
                    view.SetColumnError(col, "Please enter/select a valid value. It cannot be empty!");
                    e.Valid = false;
                }
            }
        }
        private void grdDVView_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            //Suppress displaying the error message box
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        /// <summary>
        /// Delete the selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intTotalRow = grdOptionView.GetSelectedRows().Count();
            if (intTotalRow>0)
            {
                if (Commons.ShowMessage("Do you wish to delete " + intTotalRow.ToString() + " selected row(s)?", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach(int intRowIndex in grdOptionView.GetSelectedRows())
                    {
                        DataRowView row = grdOptionView.GetRow(intRowIndex) as DataRowView;
                        _simLink.DeleteOptionList(row.Row); // delete it
                    }

                    //reload option list data again
                    LoadOptionListData(_simLink, _intProjectID);
                    cboOption_Properties_EditValueChanged(new object(), new EventArgs());
                }
            }
            else
            {
                Commons.ShowMessage("Please select at least one row to delete!");
            }
        }
        private void grdOptionView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataTable dt =(DataTable)grdOptionListGrid.DataSource;
            DataTable dtUpdate = dt.GetChanges(DataRowState.Modified);

            if (dtUpdate != null)
            {
                _simLink.InsertOrUpdateOptionListTable(dtUpdate, false);  // update data table
            }

            DataTable dtAdded = dt.GetChanges(DataRowState.Added);
            if (dtAdded != null)
            {
                DataRow rowOption = ((DataRowView)cboOption.GetSelectedDataRow()).Row;
                int intOptionID = int.Parse(rowOption["OptionID"].ToString());
                dtAdded.Rows[0]["OptionID"] = intOptionID;
                _simLink.InsertOrUpdateOptionListTable(dtAdded, true);  // update data table
            }
            if (dtAdded != null || dtUpdate != null)
            {
                // reload option again
                _dsResultData = _simLink.GetOptionListDetails(_intProjectID.ToString());

                // reload option again
                cboOption_Properties_EditValueChanged(sender, new EventArgs());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (OnCloseDialog != null)
                OnCloseDialog(sender, e);
        }

        private void cboOption_Properties_EditValueChanged(object sender, EventArgs e)
        {
            DataRow rowOption = ((DataRowView)cboOption.GetSelectedDataRow()).Row;
            DataRow[] allrows = _dsResultData.Tables[0].Select("OptionID=" + rowOption["OptionID"].ToString());
            if (allrows.Count() == 0)
            {
                grdOptionListGrid.DataSource = Commons.CopyEmptyTable(_dsResultData.Tables[0]); ;
            }
            else
            {
                grdOptionListGrid.DataSource = allrows.CopyToDataTable();
            }
            grdOptionView.RefreshData();

        }
        /// <summary>
        /// Repopulate the option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewOption_Click(object sender, EventArgs e)
        {
            frmNewOption frm = new frmNewOption(_simLink);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                cboOption.Properties.DataSource = _simLink.GetOptionLookUp(_intProjectID.ToString());
                cboOption.Properties.DisplayMember = "OptionLabel";
                cboOption.Properties.ValueMember = "OptionID";

                cboOption.Text = frm.NewlyCreatedOption;
            }
        }
    }
}
