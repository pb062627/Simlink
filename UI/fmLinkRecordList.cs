using CH2M;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIM_API_LINKS
{
    public partial class fmLinkRecordList : Form
    {
        private simlink _simLink = null;
        private int _intPerformaceId = -1;
        DataTable _dtLinkTableCodeDetail = null;
        UCPerformance _ucPerformance = null;
        public fmLinkRecordList(simlink simLink, UCPerformance ucPerformance, string strLinkTableCodeId, int intPerformanceId, int intFuctionTypeId)
        {
            try
            {
                InitializeComponent();
                _ucPerformance = ucPerformance;

                _simLink = simLink;
                _intPerformaceId = intPerformanceId;

                _dtLinkTableCodeDetail = _simLink.GetLinkedRecordDetails(_intPerformaceId).Tables[0];
                //grdLinkedRecordGrid.DataSource = _dtLinkTableCodeDetail;
                //grdOptionViewgrdLinkedRecordView.RefreshData();

                //AddLinkLabelLookUp("2"); // add link label look up (default to 2

                DataTable dt = simLink.GetLinkTableCodeLookUp(intFuctionTypeId);
                cboLinkType.Properties.DataSource = dt;
                cboLinkType.Properties.DisplayMember = "LinkTableCodeName";
                cboLinkType.Properties.ValueMember = "LinkTableCodeID";
                cboLinkType.EditValue = Int32.Parse(strLinkTableCodeId);
            }
            catch(Exception ex)
            {
                Commons.ShowMessage("Error in displaying linked data " + ex.Source + ": " + ex.Message);
            }
        }

        private void grdOptionViewgrdLinkedRecordView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
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
        /// <summary>
        /// Update on row update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdOptionViewgrdLinkedRecordView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataTable dt = (DataTable)grdLinkedRecordGrid.DataSource;
            DataTable dtUpdate = dt.GetChanges(DataRowState.Modified);

            /* will need to modify here */
            if (dtUpdate != null)
            {
                DataRow rowLinkType = ((DataRowView)cboLinkType.GetSelectedDataRow()).Row;
                int intLinkTypeID = int.Parse(rowLinkType["LinkTableCodeId"].ToString());
                int intLinkTableCodeId = int.Parse(dtUpdate.Rows[0]["ID"].ToString());
                int intRecordID = int.Parse(dtUpdate.Rows[0]["RecordID"].ToString());
                int intApplyThreshold = (dtUpdate.Rows[0]["ApplyThreshold"].ToString() != "-2" ? _simLink.GetBoolValue4SQLOrOleDb(dtUpdate.Rows[0]["RecordID"].ToString()) : -2);
                int intIsOverApplyThreshold = (dtUpdate.Rows[0]["IsOver_Threshold"].ToString() != "-2" ? _simLink.GetBoolValue4SQLOrOleDb(dtUpdate.Rows[0]["RecordID"].ToString()) : -2);
                double dblThreshold = double.Parse(dtUpdate.Rows[0]["Threshold"].ToString());

                _simLink.PerformanceUpdateLinkedRecord(intLinkTableCodeId, intLinkTypeID, intRecordID, intApplyThreshold, dblThreshold, intIsOverApplyThreshold);  // update data table
            }

            DataTable dtAdded = dt.GetChanges(DataRowState.Added);
            if (dtAdded != null)
            {
                DataRow rowLinkType = ((DataRowView)cboLinkType.GetSelectedDataRow()).Row;
                int intLinkTypeID = int.Parse(rowLinkType["LinkTableCodeId"].ToString());
                int intLinkRecordId = int.Parse(dtAdded.Rows[0]["ID"].ToString());
                if (dtAdded.Rows[0]["ApplyThreshold"] == null) dtAdded.Rows[0]["ApplyThreshold"] = -2;
                if (dtAdded.Rows[0]["IsOver_Threshold"] == null) dtAdded.Rows[0]["IsOver_Threshold"] = -2;
                if (dtAdded.Rows[0]["Threshold"] == null) dtAdded.Rows[0]["Threshold"] = -1.234;
                int intApplyThreshold = (dtAdded.Rows[0]["ApplyThreshold"].ToString() != "-2" ? _simLink.GetBoolValue4SQLOrOleDb(dtAdded.Rows[0]["RecordID"].ToString()) : -2);
                int intIsOverApplyThreshold = (dtAdded.Rows[0]["IsOver_Threshold"].ToString() != "-2" ? _simLink.GetBoolValue4SQLOrOleDb(dtAdded.Rows[0]["RecordID"].ToString()) : -2);
                double dblThreshold = double.Parse(dtAdded.Rows[0]["Threshold"].ToString());

                // add new record
                _simLink.PerformanceCreateLinkedRecord(_intPerformaceId, intLinkTypeID, intLinkRecordId, intApplyThreshold, dblThreshold, intIsOverApplyThreshold);

                // link table property
                cboLinkTable_Properties_EditValueChanged(sender, new EventArgs());
            }
        }
        private void cboLinkTable_Properties_EditValueChanged(object sender, EventArgs e)
        {
            var obj = cboLinkType.GetSelectedDataRow();
            DataRow[] allrows = null;
            string strLinkTypeId = "";
            if (obj == null)
            {
                strLinkTypeId = cboLinkType.EditValue.ToString();
            }
            else
            {
                DataRow rowLinkType = ((DataRowView)cboLinkType.GetSelectedDataRow()).Row;
                strLinkTypeId = rowLinkType["LinkTableCodeID"].ToString();
            }
            AddLinkLabelLookUp(strLinkTypeId); // refresh label look up
            // get the table code link detail
            _dtLinkTableCodeDetail = _simLink.GetLinkedRecordDetails(_intPerformaceId).Tables[0];
            allrows = _dtLinkTableCodeDetail.Select("LinkType=" + strLinkTypeId);
            grdLinkedRecordGrid.DataSource = null;

            if (allrows.Count() == 0)
            {
                grdLinkedRecordGrid.DataSource = CH2M.Commons.CopyEmptyTable(_dtLinkTableCodeDetail); ;
            }
            else
            {
                grdLinkedRecordGrid.DataSource = allrows.CopyToDataTable();
            }
            grdLinkedRecordView.RefreshData();
            //AddLinkLabelLookUp();

        }
        /// <summary>
        /// Add label look up
        /// </summary>
        /// <param name="strLinkTypeId"></param>
        private void AddLinkLabelLookUp(string strLinkTypeId)
        {
            #region Var Type FK
            
            RepositoryItemLookUpEdit myLookup = new RepositoryItemLookUpEdit();
            myLookup.DataSource = null; // reset
            grdLinkedRecordView.Columns["ID"].ColumnEdit = null; // reset

            myLookup.DataSource = _simLink.GetLinkedRecordsLookUp(int.Parse(strLinkTypeId), _simLink.GetReferenceEvalID());
            myLookup.DisplayMember = "LinkedRecordLabel";
            myLookup.ValueMember = "ID";

            myLookup.PopupFormWidth = 550;
            grdLinkedRecordView.Columns["ID"].ColumnEdit = myLookup;
            #endregion Var Type FK

            #region Threshold setup
            RepositoryItemLookUpEdit lkpApplyThreshold = new RepositoryItemLookUpEdit();
            lkpApplyThreshold.DataSource = null; // reset
            grdLinkedRecordView.Columns["ApplyThreshold"].ColumnEdit = null; // reset
            lkpApplyThreshold.EditValueChanged += lkpApplyThreshold_EditValueChanged;
            lkpApplyThreshold.DataSource = _simLink.GetPerformanceResultXRefDefault(true);
            lkpApplyThreshold.DisplayMember = "ThresholdValue";
            lkpApplyThreshold.ValueMember = "ID";
            grdLinkedRecordView.Columns["ApplyThreshold"].ColumnEdit = lkpApplyThreshold;


            RepositoryItemLookUpEdit lkpIsOverApplyThreshold = new RepositoryItemLookUpEdit();
            lkpIsOverApplyThreshold.DataSource = null; // reset
            lkpIsOverApplyThreshold.EditValueChanged += lkpIsOverApplyThreshold_EditValueChanged;
            grdLinkedRecordView.Columns["IsOver_Threshold"].ColumnEdit = null; // reset

            lkpIsOverApplyThreshold.DataSource = _simLink.GetPerformanceResultXRefDefault(false);
            lkpIsOverApplyThreshold.DisplayMember = "ThresholdValue";
            lkpIsOverApplyThreshold.ValueMember = "ID";
            grdLinkedRecordView.Columns["IsOver_Threshold"].ColumnEdit = lkpIsOverApplyThreshold;
            #endregion Threshold setup
        }

        void lkpApplyThreshold_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit dropdown = (DevExpress.XtraEditors.LookUpEdit)sender;
            if (int.Parse(dropdown.EditValue.ToString()) != -2)
            {
                grdLinkedRecordView.Columns["Threshold"].Caption = "Apply Threshold Value";
                grdLinkedRecordView.BestFitColumns();
            }
        }

        void lkpIsOverApplyThreshold_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit dropdown = (DevExpress.XtraEditors.LookUpEdit)sender;
            if (int.Parse(dropdown.EditValue.ToString()) != -2)
            {
                grdLinkedRecordView.Columns["Threshold"].Caption = "IsOver Threshold Value";
                grdLinkedRecordView.BestFitColumns();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intTotalRow = grdLinkedRecordView.GetSelectedRows().Count();
            if (intTotalRow > 0)
            {
                if (Commons.ShowMessage("Do you wish to delete " + intTotalRow.ToString() + " selected row(s)?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (int intRowIndex in grdLinkedRecordView.GetSelectedRows())
                    {
                        DataRowView row = grdLinkedRecordView.GetRow(intRowIndex) as DataRowView;
                        string strRecordID = row["RecordID"].ToString();
                        _simLink.DeleteRecordLink(strRecordID); // delete it
                    }

                    //reload option list data again
                    cboLinkTable_Properties_EditValueChanged(new object(), new EventArgs());
                }
            }
            else
            {
                Commons.ShowMessage("Please select at least one row to delete!");
            }
        }

        private void fmLinkRecordList_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataRow rowLinkType = ((DataRowView)cboLinkType.GetSelectedDataRow()).Row;
            string strLinkTypeId = rowLinkType["LinkTableCodeID"].ToString();

            // update the performance row
            _simLink.PerformanceUpdateLinkTableCode(int.Parse(strLinkTypeId), _intPerformaceId);

            // property change & update
            _ucPerformance.LoadPerformance(_simLink);
        }

        private DevExpress.XtraGrid.EditForm.Helpers.EditFormBindableControlsCollection _allBindableControls;
        ShowingPopupEditFormEventArgs _editorForm;
        private DevExpress.XtraEditors.LookUpEdit _colApplyThreshold;
        private DevExpress.XtraEditors.LookUpEdit _colIsOverThreshold;

        private void grdLinkedRecordView_ShowingPopupEditForm(object sender, ShowingPopupEditFormEventArgs e)
        {
            _editorForm = e;
            _allBindableControls = e.BindableControls;

            //EditValue/PF_FunctionType
            foreach (var col in e.BindableControls)
            {
                if (col.Tag.ToString() == "EditValue/ApplyThreshold")
                {
                    _colApplyThreshold = (DevExpress.XtraEditors.LookUpEdit)col;
                    _colApplyThreshold.EditValueChanged += _colApplyThreshold_EditValueChanged;
                }
                if (col.Tag.ToString() == "EditValue/IsOver_Threshold")
                {
                    _colIsOverThreshold = (DevExpress.XtraEditors.LookUpEdit)col;
                    _colIsOverThreshold.EditValueChanged += _colIsOverThreshold_EditValueChanged;
                }
            }

        }

        void _colIsOverThreshold_EditValueChanged(object sender, EventArgs e)
        {
           DevExpress.XtraEditors.LookUpEdit colEditApplyThreshold = (DevExpress.XtraEditors.LookUpEdit)sender;
           if (CommonUtilities.IsInteger(colEditApplyThreshold.EditValue.ToString()) == false)
           {
               grdLinkedRecordView.SetRowCellValue(0, "Threshold", "Apply Threshold Value");
           }
        }

        void _colApplyThreshold_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.LookUpEdit colEditIsOverThreshold = (DevExpress.XtraEditors.LookUpEdit)sender;
            if (CommonUtilities.IsInteger(colEditIsOverThreshold.EditValue.ToString()) == false)
            {
                grdLinkedRecordView.SetRowCellValue(0, "Threshold", "IsOver Threshold Value");
            }
        }
    }
}
