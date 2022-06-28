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
using System.IO;
using CH2M;

namespace SIM_API_LINKS
{
    public partial class UCResultSummary : UserControl
    {
        private string _strSQLUsed = "";
        private DataSet _dsResultData = new DataSet();
        private simlink _simLink = null;
        private string[] _astrFieldAlias = new string[] { "Result_Label", "VarResultType_FK", "Result_Description", "ElementID_FK", "Element_Label", "Is List Var", "Import Result Detail"};
        private string[] _astrFieldCaption = new string[] { "Label", "Type", "Description", "Element ID (INT) = -1", "Element Label" };

        public UCResultSummary()
        {
            InitializeComponent();
        }

        public void ClearGrid()
        {
            grdResultGrid.DataSource = null;
        }
        public void LoadResultData(simlink simLink)
        {
            _simLink = simLink;
            _dsResultData = simLink.GetResultSummary(simLink.GetReferenceEvalID());

            //// load to grid
            grdResultGrid.DataSource = _dsResultData.Tables[0];
            grdResultView.RefreshData();

            bool blnIsEditable = simLink.IsEGEditable();
            grdResultView.OptionsBehavior.Editable = blnIsEditable;
            btnImportCSV.Enabled = blnIsEditable;
            AddLookup(simLink);
        }
        /// <summary>
        /// Add look up
        /// </summary>
        /// <param name="simLink"></param>
        private void AddLookup(simlink simLink)
        {
            #region Var Type FK
            RepositoryItemLookUpEdit myLookup = new RepositoryItemLookUpEdit();

            myLookup.DataSource = simLink.GetResultVarTypeLookup();
            myLookup.DisplayMember = "FieldName";
            myLookup.ValueMember = "ResultsFieldID";


            myLookup.PopupFormWidth = 550;
            grdResultView.Columns["VarResultType_FK"].ColumnEdit = myLookup;
            #endregion Var Type FK

            #region Element
            //RepositoryItemLookUpEdit myElementLookup = new RepositoryItemLookUpEdit();

            //myElementLookup.DataSource = simLink.GetElementLookUp();
            //myElementLookup.DisplayMember = "ElementListLabel";
            //myElementLookup.ValueMember = "ElementListID";

            //myElementLookup.PopupFormWidth = 250;
            //grdResultView.Columns["ElementID"].ColumnEdit = myElementLookup;
            #endregion Element
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
        /// Import csv file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Result CSV File (*.csv)|*.csv";
            open.Title = "Please select Result CSV to import";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string strFilename = open.FileName;
                    List<string> astrFieldList = new List<string>();
                    List<string> astrDataType = new List<string>();
                    Commons.GetFieldDataTypeFromGrid(grdResultView, out astrFieldList, out astrDataType);

                    string strErrorMessage = "";
                    //bool blnSucceed = _simLink.ImportResultFromCSV(_astrFieldAlias, strFilename, _simLink.GetReferenceEvalID().ToString());
                    bool blnSucceed = _simLink.ImportCSVDataFile2Grid("tblResultVar", astrFieldList.ToArray(), astrDataType.ToArray(),
                            strFilename, "EvaluationGroup_FK", _simLink.GetReferenceEvalID().ToString(), out strErrorMessage);
                    if (blnSucceed)
                    {
                        CH2M.Commons.ShowMessage("Sucessfully imported the file '" + strFilename + "'", MessageBoxIcon.Information);
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
                LoadResultData(_simLink);
            }
        }
        /// <summary>
        /// CSV result template file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCSV_Click(object sender, EventArgs e)
        {
            string strHeader = string.Join(",", _astrFieldCaption);
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Result template file (*.csv)|*.csv";
            save.Title = "Save CSV template file";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFile = save.FileName;
                StreamWriter csv = new StreamWriter(strFile);
                csv.WriteLine(CH2M.Commons.WriteTemplateCSV(grdResultView));
                csv.Flush();
                csv.Close();
            }
        }
        /// <summary>
        /// Result update row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdResultView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            int intEvalId = _simLink.GetReferenceEvalID();
            DataSet dsUpdate = _dsResultData.GetChanges(DataRowState.Modified);
            _simLink.InsertOrUpdateSummaryResultTable(dsUpdate, false, intEvalId);  // update data table

            DataSet dsInsert = _dsResultData.GetChanges(DataRowState.Added);
            _simLink.InsertOrUpdateSummaryResultTable(dsInsert, true, intEvalId);  // insert data table

            if (dsUpdate != null || dsInsert != null)
            {
                // reload the dataset object again
                LoadResultData(_simLink);
            }
        }
        /// <summary>
        /// Initialise new row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdResultView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                grdResultView.SetRowCellValue(e.RowHandle, "IsListVar", false);
                grdResultView.SetRowCellValue(e.RowHandle, "ImportResultDetail", false);
                //grdResultView.SetRowCellValue(e.RowHandle, "ElementID", "-1");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intTotalSelectedRows = grdResultView.GetSelectedRows().Count();
            if (intTotalSelectedRows > 0)
            {
                if (Commons.ShowMessage("Do you wish to delete the " + intTotalSelectedRows.ToString() + " selected row.\r\nNote: Deleting decision variables may invalidate any dependent results. The user is responsible for ensuring downstream data integrity.",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (int intRowIndex in grdResultView.GetSelectedRows())
                    {
                        DataRowView row = (DataRowView)grdResultView.GetRow(intRowIndex);
                        DataRow toDelRow = row.Row;

                        // delete the row & associated data
                        _simLink.DeleteResultSummary(toDelRow);
                    }

                    // reload whole object again after deleting
                    LoadResultData(_simLink);
                }
            }
        }
    }
}
