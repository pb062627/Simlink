using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using CH2M;

namespace SIM_API_LINKS
{
    public partial class UCEventResult : DevExpress.XtraEditors.XtraUserControl
    {
        private string[] _astrFieldAlias = new string[] { "ResultTS_or_Event_ID_FK", "EventFunctionID", "InterEvent_Threshold", "Threshold_Inst", "IsOver_Threshold_Inst", "Threshold_Cumulative", "IsOver_Threshold_Cumulative", "CalcValueInExcessOfThreshold" };
        private string[] _astrFieldCaption = new string[] { "Result ID (FK)", "Calc On Timeseries", "Time between Events (sec)", "Instantaneous Threshold", "Trig if Over Thres (True or False)", "Cumulative Threshold", "Trig If Over Thres (Cumulative) (True or False)", "Calc. Value In Excess of Threshold (True or False)" };
        private simlink _simlink;
        private DataSet _dsEventResult = null;
        public UCEventResult()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load event result
        /// </summary>
        /// <param name="simLink"></param>
        public void LoadEventResult(simlink simLink)
        {
            _simlink = simLink;
            _dsEventResult = simLink.LoadEventResult(simLink.GetReferenceEvalID());
            if (_dsEventResult != null)
            {
                grdEventGrid.DataSource = _dsEventResult.Tables[0];
                grdEventView.BestFitColumns();
            }
            else
            {
                grdEventGrid.DataSource = null;
            }
            AddLookup();
        }
        /// <summary>
        /// Add look up
        /// </summary>
        private void AddLookup()
        {
            RepositoryItemLookUpEdit myPFLookup = new RepositoryItemLookUpEdit();

            myPFLookup.DataSource = _simlink.GetEventFunction();
            myPFLookup.DisplayMember = "EventFunctionName";
            myPFLookup.ValueMember = "EventFunctionID";

            myPFLookup.PopupFormWidth = 180;
            grdEventView.Columns["EventFunctionID"].ColumnEdit = myPFLookup;

            RepositoryItemLookUpEdit myResultLookup = new RepositoryItemLookUpEdit();

            myResultLookup.DataSource = _simlink.GetResultTSLookUp(_simlink.GetReferenceEvalID().ToString());
            myResultLookup.DisplayMember = "Result_Label";
            myResultLookup.ValueMember = "ResultTS_ID";

            myResultLookup.PopupFormWidth = 180;
            grdEventView.Columns["ResultTS_or_Event_ID_FK"].ColumnEdit = myResultLookup;

            // ResultTS_or_Event_ID_FK
        }
        /// <summary>
        /// Clear grid
        /// </summary>
        public void ClearGrid()
        {
            grdEventGrid.DataSource = null;
        }

        private void btnCSV_Click(object sender, EventArgs e)
        {
            string strHeader = string.Join(",", _astrFieldCaption);
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Event result template file (*.csv)|*.csv";
            save.Title = "Save CSV template file";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFile = save.FileName;
                StreamWriter csv = new StreamWriter(strFile);
                csv.WriteLine(string.Join(",", _astrFieldCaption));
                //csv.WriteLine(CH2M.Commons.WriteTemplateCSV(grdEventView));
                csv.Flush();
                csv.Close();
            }

        }

        private void btnImportCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Event result CSV File (*.csv)|*.csv";
            open.Title = "Please select event result CSV to import";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string strFilename = open.FileName;
                    //List<string> astrFieldList = new List<string>();
                    //List<string> astrDataType = new List<string>();
                    //Commons.GetFieldDataTypeFromGrid(grdEventView, out astrFieldList, out astrDataType);

                    string strErrorMessage = "";
                    //bool blnIsSucceed = _simlink.ImportCSVDataFile2Grid("tblResultTS_EventSummary", astrFieldList.ToArray(), astrDataType.ToArray(),
                    //    strFilename, "EvaluationGroupID_FK", _simlink.GetReferenceEvalID().ToString(), out strErrorMessage);
                    bool blnIsSucceed = _simlink.ImporEventResultFromCSV(_astrFieldAlias, strFilename, _simlink.GetReferenceEvalID().ToString());
                    if (blnIsSucceed)
                    {
                        CH2M.Commons.ShowMessage("Sucessfully imported the file '" + strFilename + "'", MessageBoxIcon.Information);
                        LoadEventResult(_simlink);
                    }
                    else
                    {
                        CH2M.Commons.ShowMessage(strErrorMessage, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    CH2M.Commons.ShowMessage("Error in importing the CSV file.\r\n" + ex.Message);
                }
                // reload the grid again
                LoadEventResult(_simlink);
            }
        }
        /// <summary>
        /// Row updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPerformanceView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataSet dsUpdate = _dsEventResult.GetChanges(DataRowState.Modified);
            _simlink.InsertOrUpdateEventTable(dsUpdate, _simlink._nActiveEvalID, false);  // update data table

            DataSet dsInsert = _dsEventResult.GetChanges(DataRowState.Added);
            _simlink.InsertOrUpdateEventTable(dsInsert, _simlink._nActiveEvalID, true);  // insert data table

            if (dsUpdate != null || dsInsert != null)
            {
                // refresh the grid
                LoadEventResult(_simlink); // load performance
            }
        }
        /// <summary>
        /// Invalid exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPerformanceView_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            //Suppress displaying the error message box
            e.ExceptionMode = ExceptionMode.NoAction;
        }
                /// <summary>
        /// Validating row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPerformanceView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            e.Valid = true;
            foreach (GridColumn col in view.Columns)
            {
                if (col.FieldName != "Threshold_Inst" && col.FieldName != "Threshold_Cumulative" && col.FieldName != "EventSummaryID")
                {
                    object objValue = view.GetRowCellValue(e.RowHandle, col);
                    if (objValue == DBNull.Value)
                    {
                        view.SetColumnError(col, "Please enter/select a valid value. It cannot be empty!");
                        e.Valid = false;
                    }
                }
            }
            object objThresholdInst = view.GetRowCellValue(e.RowHandle, "Threshold_Inst");
            object objThresholdCumulative = view.GetRowCellValue(e.RowHandle, "Threshold_Cumulative");
            if (objThresholdInst  == DBNull.Value && objThresholdCumulative == DBNull.Value)
            {
                view.SetColumnError(view.Columns["IsOver_Threshold_Inst"], "Instantaneous Threshold or Cumulative Threshold must not be empty!");
                view.SetColumnError(view.Columns["Threshold_Cumulative"], "Instantaneous Threshold or Cumulative Threshold must not be empty!");
                e.Valid = false;
            }
        }

        private void grdEventView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            grdEventView.SetRowCellValue(e.RowHandle, "IsOver_Threshold_Inst", false);
            grdEventView.SetRowCellValue(e.RowHandle, "IsOver_Threshold_Cumulative", false);
            grdEventView.SetRowCellValue(e.RowHandle, "CalcValueInExcessOfThreshold", false);
            grdEventView.SetRowCellValue(e.RowHandle, "CategoryID_FK", "-1");
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            int intTotalSelectedRows = grdEventView.GetSelectedRows().Count();
            if (intTotalSelectedRows > 0)
            {
                if (Commons.ShowMessage("Do you wish to delete the " + intTotalSelectedRows.ToString() + " selected row.\r\nNote: Deleting decision variables may invalidate any dependent results. The user is responsible for ensuring downstream data integrity.",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (int intRowIndex in grdEventView.GetSelectedRows())
                    {
                        DataRowView row = (DataRowView)grdEventView.GetRow(intRowIndex);
                        DataRow toDelRow = row.Row;

                        // delete the row & associated data
                        _simlink.DeleteEventResult(toDelRow);
                    }
                    LoadEventResult(_simlink);
                }
            }
        }
    }
}
