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
    public partial class UCElementList : UserControl
    {
        private string _strSQLUsed = "";
        private DataSet _dsResultData = new DataSet();
        private simlink _simLink = null;
        private int _intProjectID;
        public delegate void ButtonClickedEventHandler(object sender, EventArgs e);
        public event ButtonClickedEventHandler OnCloseDialog;

        public frmSimLinkMain SimlinkMainDialog
        {
            get;
            set;
        }

        public UCElementList()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Load detail element list
        /// </summary>
        /// <param name="simLink"></param>
        /// <param name="intProjectID"></param>
        public void LoadElementListData(simlink simLink, int intProjectID)
        {
            _simLink = simLink;
            _intProjectID = intProjectID;
            _dsResultData = simLink.GetElementListDetails();

            cboElementLabel.Properties.DataSource = simLink.GetElementLookUp(intProjectID.ToString());
            cboElementLabel.Properties.DisplayMember = "ElementListLabel";
            cboElementLabel.Properties.ValueMember = "ElementListID";

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intTotalRow = grdElementView.GetSelectedRows().Count();
            if (intTotalRow > 0)
            {
                if (Commons.ShowMessage("Do you wish to delete " + intTotalRow.ToString() + " selected row(s)?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (int intRowIndex in grdElementView.GetSelectedRows())
                    {
                        DataRowView row = grdElementView.GetRow(intRowIndex) as DataRowView;
                        _simLink.DeleteElementList(row.Row); // delete it
                    }
                    LoadElementListData(_simLink, _simLink._nActiveProjID);
                    //reload option list data again
                    cboElementLabel_EditValueChanged(new object(), new EventArgs());
                }
            }
            else
            {
                Commons.ShowMessage("Please select at least one row to delete!");
            }
        }

        private void lblLabel_Click(object sender, EventArgs e)
        {

        }

        private void grdElementView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataTable dt = (DataTable)grdElementListGrid.DataSource;
            DataTable dtUpdate = dt.GetChanges(DataRowState.Modified);

            if (dtUpdate != null)
            {
                _simLink.InsertOrUpdateElementListTable(dtUpdate, false);  // update data table
            }

            DataTable dtAdded = dt.GetChanges(DataRowState.Added);
            if (dtAdded != null)
            {
                DataRow rowOption = ((DataRowView)cboElementLabel.GetSelectedDataRow()).Row;
                int intElementID = int.Parse(rowOption["ElementListID"].ToString());
                dtAdded.Rows[0]["ElementListID_FK"] = intElementID;
                _simLink.InsertOrUpdateElementListTable(dtAdded, true);  // update data table
            }
            if (dtAdded != null || dtUpdate != null)
            {
                // reload option again
                _dsResultData = _simLink.GetElementListDetails();

                // reload option again
                cboElementLabel_EditValueChanged(sender, new EventArgs());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (OnCloseDialog != null)
                OnCloseDialog(sender, e);
        }

        private void cboElementLabel_EditValueChanged(object sender, EventArgs e)
        {
            DataRow rowElement = ((DataRowView)cboElementLabel.GetSelectedDataRow()).Row;
            txtType.Text = rowElement["Type"].ToString();
            DataRow[] allrows = _dsResultData.Tables[0].Select("ElementListID_FK=" + rowElement["ElementListID"].ToString());
            if (allrows.Count() == 0)
            {
                grdElementListGrid.DataSource = Commons.CopyEmptyTable(_dsResultData.Tables[0]);
            }
            else
            {
                grdElementListGrid.DataSource = allrows.CopyToDataTable();
            }
            grdElementListGrid.RefreshDataSource();

        }

        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            frmNewElement frm = new frmNewElement(_simLink);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                cboElementLabel.Properties.DataSource = _simLink.GetElementLookUp(_intProjectID.ToString());
                cboElementLabel.Properties.DisplayMember = "ElementListLabel";
                cboElementLabel.Properties.ValueMember = "ElementListID";

                cboElementLabel.Text = frm.NewlyCreatedElement;
            }
        }
    }
}
