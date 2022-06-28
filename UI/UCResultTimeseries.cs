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
    public partial class UCResultTimseries : UserControl
    {
        private DataSet _dsResultData = new DataSet();
        private simlink _simLink = null;
        private string _strElementLabel = "";
        private string[] _astrFieldAlias = new string[] { "Result_Label", "FunctionID_FK", "SQN", "VarResultType_FK", "Result_Description", "ElementID_FK", "Element_Label", "TS_StartDate", "TS_StartHour", "TS_StartMin", "TS_Interval", "TS_Interval_Unit", "BeginPeriodNo", "FunctionArgs", "IsAux", "IsSecondary", "RefTS_ID_FK" };
        private string[] _astrFieldCaption = new string[] { "Result_Label", "FunctionID_FK (INT)", "SQN (INT)", "VarResultType_FK (INT)", "Result_Description", "ElementID_FK (INT)", "Element_Label", "TS_StartDate", "TS_StartHour", "TS_StartMin", "TS_Interval (INT)", "TS_Interval_Unit (INT)", "BeginPeriodNo (INT)", "FunctionArgs", "IsAux (Boolean)", "IsSecondary (Boolean)", "RefTS_ID_FK (INT)" };

        public UCResultTimseries()
        {
            InitializeComponent();

            this.grdResultTSView.OptionsBehavior.EditingMode = GridEditingMode.EditForm;
            this.grdResultTSView.ShowingPopupEditForm += new DevExpress.XtraGrid.Views.Grid.ShowingPopupEditFormEventHandler(this.grdResultTSView_ShowingPopupEditForm);

        }

        public void LoadResultData(simlink simLink)
        {
            _simLink = simLink;
            _dsResultData = simLink.GetResultTimeseries(simLink.GetReferenceEvalID());

            //// load to grid
            grdResultTSGrid.DataSource = _dsResultData.Tables[0];
            grdResultTSView.RefreshData();

            bool blnIsEditable = simLink.IsEGEditable();
            grdResultTSView.OptionsBehavior.Editable = blnIsEditable;


            AddLookup(simLink);

            cboViewOption.SelectedIndex = 0;

            // reflect the column and value changes
            cboViewOption_SelectedIndexChanged(new object(), new EventArgs());

        }
        /// <summary>
        /// clear grid
        /// </summary>
        public void ClearGrid()
        {
            grdResultTSGrid.DataSource = null;
        }
        /// <summary>
        /// Add look up
        /// </summary>
        /// <param name="simLink"></param>
        private void AddLookup(simlink simLink)
        {
            #region Var Type FK
            RepositoryItemLookUpEdit myLookup = new RepositoryItemLookUpEdit();

            myLookup.DataSource = simLink.GetResultTimeseriesVarTypeLookup();
            myLookup.DisplayMember = "FieldName";
            myLookup.ValueMember = "ResultsFieldID";

            myLookup.PopupFormWidth = 550;
            grdResultTSView.Columns["VarResultType_FK"].ColumnEdit = myLookup;
            #endregion Var Type FK

            #region Lookup functon type
            RepositoryItemLookUpEdit functionTypeLookup = new RepositoryItemLookUpEdit();

            functionTypeLookup.DataSource = simLink.GetFunctionTableLookup(_simLink._nActiveProjID.ToString());
            functionTypeLookup.DisplayMember = "Label";
            functionTypeLookup.ValueMember = "FunctionID";

            functionTypeLookup.PopupFormWidth = 180;
            grdResultTSView.Columns["FunctionID_FK"].ColumnEdit = functionTypeLookup;
            #endregion Lookup functon type

            #region Element
            RepositoryItemLookUpEdit myElementLookup = new RepositoryItemLookUpEdit();

            myElementLookup.DataSource = simLink.GetElementLookUpWithNewDefine();
            myElementLookup.DisplayMember = "ElementListLabel";
            myElementLookup.ValueMember = "ElementListID";

            myElementLookup.PopupFormWidth = 250;
            grdResultTSView.Columns["ElementID_FK"].ColumnEdit = myElementLookup;
            #endregion Element

            #region Result TS RefTS_ID_FK
            RepositoryItemLookUpEdit myTSLookUp = new RepositoryItemLookUpEdit();

            myTSLookUp.DataSource = simLink.GetResultTSLookUp(_simLink.GetReferenceEvalID().ToString());
            myTSLookUp.DisplayMember = "Result_Label";
            myTSLookUp.ValueMember = "ResultTS_ID";

            myTSLookUp.PopupFormWidth = 250;
            grdResultTSView.Columns["RefTS_ID_FK"].ColumnEdit = myTSLookUp;
            #endregion RefTS_ID_FK
        }
        /// <summary>
        /// Validate row before inserting/update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdDVView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (_allBindableControls == null)
                {
                    return;
                }
                foreach (var con in _allBindableControls)
                {
                    if (con.Tag.ToString() == "EditValue/VarResultType_FK")
                    {
                        DevExpress.XtraEditors.LookUpEdit varResult = (DevExpress.XtraEditors.LookUpEdit)con;
                        if (varResult.EditValue == DBNull.Value)
                        {
                            varResult.ErrorText = "Please select the Result Data Type!";
                            e.Valid = false;
                        }
                    }
                    else if (con.Tag.ToString() == "EditValue/Result_Description")
                    {
                        DevExpress.XtraEditors.TextEdit resultDescription = (DevExpress.XtraEditors.TextEdit)con;
                        if (resultDescription.EditValue == DBNull.Value)
                        {
                            resultDescription.ErrorText = "Please enter the Result Description!";
                            e.Valid = false;
                        }
                    }
                    else if (con.Tag.ToString() == "EditValue/Result_Label")
                    {
                        DevExpress.XtraEditors.TextEdit resultLabel = (DevExpress.XtraEditors.TextEdit)con;
                        if (resultLabel.EditValue == DBNull.Value)
                        {
                            resultLabel.ErrorText = "Please enter the Result Label!";
                            e.Valid = false;
                        }
                    }
                    else if (con.Tag.ToString() == "EditValue/BeginPeriodNo")
                    {
                        DevExpress.XtraEditors.TextEdit txtBeginPeriodNo = (DevExpress.XtraEditors.TextEdit)con;
                        if (txtBeginPeriodNo.EditValue == DBNull.Value)
                        {
                            txtBeginPeriodNo.ErrorText = "Please enter Start At Period!";
                            e.Valid = false;
                        }
                    }
                }
                if (_colEditElementLabel.Text == "")
                {
                    _colEditElementLabel.ErrorText = "Please enter function Element Model Ref.!";
                    e.Valid = false;

                }
                if (_colEditFunctionID.EditValue.ToString() != "-1")
                {
                    if (_functionArg.Text == "")
                    {
                        _functionArg.ErrorText = "Please enter function argument!";
                        e.Valid = false;
                    }
                }
                else
                {
                    _functionArg.Text = "";
                    e.Valid = true;
                }
                if (_chkIsSecondary.Checked) // secondary
                {
                    if (_colEditRefTS_ID_FK.EditValue == DBNull.Value)
                    {
                        _colEditRefTS_ID_FK.ErrorText = "Please select Reference TS!";
                        e.Valid = false;

                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }
        private void grdDVView_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            //Suppress displaying the error message box
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        /// <summary>
        /// Update row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdResultTSView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataSet dsUpdate = _dsResultData.GetChanges(DataRowState.Modified);
            _simLink.InsertOrUpdateSummaryTimeseriesTable(dsUpdate, false, _simLink.GetReferenceEvalID());  // update data table

            DataSet dsInsert = _dsResultData.GetChanges(DataRowState.Added);
            _simLink.InsertOrUpdateSummaryTimeseriesTable(dsInsert, true, _simLink.GetReferenceEvalID());  // insert data table

            if (dsUpdate != null || dsInsert != null)
            {
                int intSelectedIndex = cboViewOption.SelectedIndex;
                // reload the dataset object again
                LoadResultData(_simLink);

                // reset the index
                cboViewOption.SelectedIndex = intSelectedIndex; // set the selected index
                if (_editorForm != null) _editorForm.EditForm.Hide();
                _editorForm = null;
                if (_allBindableControls != null) _allBindableControls = null; // clear it up
            }
        }

        private void btnCSVTemplate_Click(object sender, EventArgs e)
        {
            string strHeader = String.Join(",", _astrFieldCaption);
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Result summary template file (*.csv)|*.csv";
            save.Title = "Save CSV template file";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFile = save.FileName;
                StreamWriter csv = new StreamWriter(strFile);
                csv.WriteLine(Commons.WriteTemplateCSV(grdResultTSView));
                csv.Flush();
                csv.Close();
            }
        }

        private void btnImportCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Result summary CSV File (*.csv)|*.csv";
            open.Title = "Please select Result summary CSV to import";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    List<string> astrFieldList = new List<string>();
                    List<string> astrDataType = new List<string>();
                    Commons.GetFieldDataTypeFromGrid(grdResultTSView, out astrFieldList, out astrDataType);
                    string strErrorMesage = "";
                    string strFilename = open.FileName;
                    bool blnIsSucceed = _simLink.ImportCSVDataFile2Grid("tblResultTS", astrFieldList.ToArray(), astrDataType.ToArray(),
                        strFilename, "EvaluationGroup_FK", _simLink.GetReferenceEvalID().ToString(), out strErrorMesage);
                    //bool blnIsSucceed = _simLink.ImportResultTimeseriesFromCSV(_astrFieldAlias, strFilename, _simLink.GetReferenceEvalID().ToString());
                    if (blnIsSucceed)
                    {
                        CH2M.Commons.ShowMessage("Sucessfully imported the file '" + strFilename + "'", MessageBoxIcon.Information);
                    }
                    else
                    {
                        CH2M.Commons.ShowMessage(strErrorMesage, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    CH2M.Commons.ShowMessage("Error in importing the CSV file.\r\n" + ex.Message);
                }
                int intSelIndex = cboViewOption.SelectedIndex;
                // reload the grid again
                LoadResultData(_simLink);

                cboViewOption.SelectedIndex = intSelIndex;
                cboViewOption_SelectedIndexChanged(sender, e);
            }
        }

        private void cboViewOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool blnViewDetail = (cboViewOption.SelectedIndex == 1);
                colTS_StartDate.Visible = false;//blnViewDetail;
                colTS_StartHour.Visible = false; // blnViewDetail;
                colTS_StartMin.Visible = false;// blnViewDetail;
                colTS_Interval.Visible = false; // blnViewDetail;
                colTS_Interval_Unit.Visible = false;// blnViewDetail;
                colBeginPeriodNo.Visible = blnViewDetail;
                colFunctionID_FK.Visible = blnViewDetail;
                colFunctionArgs.Visible = blnViewDetail;
                //colIsAux.Visible = blnViewDetail;
                colIsSecondary.Visible = blnViewDetail;
                colRefTS_ID_FK.Visible = blnViewDetail;
                grdResultTSView.OptionsBehavior.Editable = blnViewDetail; // only view detail is editable
                if (blnViewDetail == false)
                {
                    lblEditableInfo.Text = "Grid is NOT editable in summary view";
                }
                else
                {
                    lblEditableInfo.Text = "Grid is editable in detailed view";
                }
                btnCSVTemplate.Enabled = blnViewDetail;
                btnImportCSV.Enabled = blnViewDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }

        private DevExpress.XtraEditors.CheckEdit _chkIsSecondary;
        private DevExpress.XtraEditors.TextEdit _functionArg;
        private DevExpress.XtraEditors.LookUpEdit _colEditFunctionID;
        private DevExpress.XtraEditors.LookUpEdit _colEditRefTS_ID_FK;
        private DevExpress.XtraEditors.LookUpEdit _colEditElementID_FK;
        private DevExpress.XtraEditors.TextEdit _colEditElementLabel;
        private DevExpress.XtraGrid.EditForm.Helpers.EditFormBindableControlsCollection _allBindableControls;
        ShowingPopupEditFormEventArgs _editorForm;
        private void grdResultTSView_ShowingPopupEditForm(object sender, ShowingPopupEditFormEventArgs e)
        {
            _editorForm = e;
            _allBindableControls = e.BindableControls;
            if (cboViewOption.SelectedIndex == 0)
            {
                Commons.ShowMessage("You can only add new or edit value on the detailed view mode only!");
                return;
            }

            //EditValue/PF_FunctionType
            foreach (var col in e.BindableControls)
            {
                if (col.GetType().ToString() == "DevExpress.XtraEditors.LookUpEdit" || col.GetType().ToString() == "DevExpress.XtraEditors.TextEdit")
                {
                    col.GotFocus += col_GotFocus;
                }
                if (col.Tag.ToString() == "EditValue/FunctionID_FK")
                {
                    _colEditFunctionID = (DevExpress.XtraEditors.LookUpEdit)col;
                    _colEditFunctionID.EditValueChanged += _editableFunctionID_EditValueChanged;
                }
                else if (col.Tag.ToString() == "EditValue/FunctionArgs")
                {
                    _functionArg = (DevExpress.XtraEditors.TextEdit)col;
                }
                else if (col.Tag.ToString() == "EditValue/IsSecondary")
                {
                    _chkIsSecondary = (DevExpress.XtraEditors.CheckEdit)col;
                    _chkIsSecondary.CheckedChanged += _chkIsSecondary_CheckedChanged;
                }
                else if (col.Tag.ToString() == "EditValue/RefTS_ID_FK")
                {
                    _colEditRefTS_ID_FK = (DevExpress.XtraEditors.LookUpEdit)col;
                }
                else if (col.Tag.ToString() == "EditValue/ElementID_FK")
                {
                    _colEditElementID_FK = (DevExpress.XtraEditors.LookUpEdit)col;
                    _colEditElementID_FK.EditValueChanged += _colEditElementID_FK_EditValueChanged;
                }
                else if (col.Tag.ToString() == "EditValue/Element_Label")
                {
                    _colEditElementLabel = (DevExpress.XtraEditors.TextEdit)col;
                }
            }
            if (_chkIsSecondary.Checked == false)
            {
                if (_colEditRefTS_ID_FK != null)
                {
                    _colEditRefTS_ID_FK.EditValue = "-1";
                    _colEditRefTS_ID_FK.Enabled = false;
                }
            }
        }

        void _colEditElementID_FK_EditValueChanged(object sender, EventArgs e)
        {
            _colEditElementLabel.Enabled = true;
            /*
            try
            {
                DevExpress.XtraEditors.LookUpEdit colEditElementID = (DevExpress.XtraEditors.LookUpEdit)sender;
                if (colEditElementID.EditValue.ToString() == "-1")
                {
                    if (_colEditElementLabel != null) _colEditElementLabel.Enabled = true;
                }
                else // give the same value when it's selected
                {
                    if (_colEditElementLabel != null)
                    {
                        _colEditElementLabel.Enabled = false;
                        _colEditElementLabel.Text = colEditElementID.Text;
                        _strElementLabel = colEditElementID.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
             */
        }

        void _chkIsSecondary_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.CheckEdit colSecondary = (DevExpress.XtraEditors.CheckEdit)sender;
                _colEditRefTS_ID_FK.Enabled = colSecondary.Checked; // check the secondary
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }
        void col_GotFocus(object sender, EventArgs e)
        {
            SetDisableControlToNoSet();
        }
        private void SetDisableControlToNoSet()
        {
            foreach (var con in _allBindableControls)
            {
                if (con.GetType().ToString() == "DevExpress.XtraEditors.LookUpEdit")
                {
                    if (con.Enabled == false)
                    {
                        DevExpress.XtraEditors.LookUpEdit ctr = (DevExpress.XtraEditors.LookUpEdit)con;
                        ctr.EditValue = -1;
                    }
                }
                else if (con.GetType().ToString() == "DevExpress.XtraEditors.TextEdit")
                {
                    if (con.Enabled == false)
                    {
                        Console.WriteLine(con.Tag.ToString());
                        DevExpress.XtraEditors.TextEdit ctr = (DevExpress.XtraEditors.TextEdit)con;
                        if (con.Tag.ToString() == "EditValue/Element_Label")
                        {
                            ctr.Text = _strElementLabel;
                        }
                        else
                        {
                            ctr.Text = "";
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Disable function arg when function id = -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _editableFunctionID_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.LookUpEdit colEditFunctionID = (DevExpress.XtraEditors.LookUpEdit)sender;
                bool blnEnableFunction = !(colEditFunctionID.EditValue.ToString() == "-1");
                if (_functionArg != null) _functionArg.Enabled = blnEnableFunction;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }
        private void grdResultTSView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                grdResultTSView.SetRowCellValue(e.RowHandle, "BeginPeriodNo", "-1");
                grdResultTSView.SetRowCellValue(e.RowHandle, "EvaluationGroup_FK", _simLink.GetReferenceEvalID().ToString());
                grdResultTSView.SetRowCellValue(e.RowHandle, "IsSecondary", false);
                grdResultTSView.SetRowCellValue(e.RowHandle, "ElementIndex", "-1");
                grdResultTSView.SetRowCellValue(e.RowHandle, "FunctionArgs", "");
                _colEditRefTS_ID_FK.Enabled = _chkIsSecondary.Checked; // check the secondary

                _colEditElementLabel.Enabled = (_colEditElementID_FK.EditValue.ToString() == "-1");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intTotalSelectedRows = grdResultTSView.GetSelectedRows().Count();
            if (intTotalSelectedRows > 0)
            {
                if (Commons.ShowMessage("Do you wish to delete the " + intTotalSelectedRows.ToString() + " selected row.\r\nNote: Deleting decision variables may invalidate any dependent results. The user is responsible for ensuring downstream data integrity.",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (int intRowIndex in grdResultTSView.GetSelectedRows())
                    {
                        DataRowView row = (DataRowView)grdResultTSView.GetRow(intRowIndex);
                        DataRow toDelRow = row.Row;

                        // delete the row & associated data
                        _simLink.DeleteTimeseries(toDelRow);
                    }

                    // reload whole object again after deleting
                    LoadResultData(_simLink);

                    // refresh the grid
                    cboViewOption_SelectedIndexChanged(sender, e);
                }
            }
        }
    }
}
