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
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using System.IO;
using CH2M;

namespace SIM_API_LINKS
{
    public partial class UCPerformance : DevExpress.XtraEditors.XtraUserControl
    {
        ////private string[] _astrFieldAlias = new string[] { "Performance_Label", "PF_Type", "LinkTableCode", 
        ////    "Description", "PF_FunctionType", "FunctionID_FK", "FunctionArgs", "DV_ID_FK", "OptionID_FK", "Function on Linked Data"};
        ////private string[] _astrFieldCaption = new string[] { "Label", "Type (INT 1- Cost 2-Performance)", "Data Type(1-Model Val 2-Result 3-Timeseries 4-DV Option 5-Event)", 
        ////    "Description", "FunctionType (INT 1- Function 2- Lookup)", "FunctionID (-1 default or FunctionID)", "FunctionArgs(text -1 if not used)", "DV_FK (-1 if default)", "OptionID (DV Val only)", "Function on Linked Data (INT)"};
        private simlink _simlink;
        private DataSet _dsPerformance = new DataSet();
        public UCPerformance()
        {
            try
            {
                InitializeComponent();
                this.grdPerformanceView.OptionsBehavior.EditingMode = GridEditingMode.EditForm;
                this.grdPerformanceView.ShowingPopupEditForm += new DevExpress.XtraGrid.Views.Grid.ShowingPopupEditFormEventHandler(this.grdPerformanceView_ShowingPopupEditForm);
            }
            catch (Exception ex)
            {

            }
        }
        public void ClearGrid()
        {
            grdPerformanceGrid.DataSource = null;
        }
        /// <summary>
        /// Load performance data to grid
        /// </summary>
        /// <param name="simLink"></param>
        /// <param name="intEvalID"></param>
        public void LoadPerformance(simlink simLink)
        {
            _simlink = simLink;

            // load to grid
            _dsPerformance = _simlink.LoadPerformace(_simlink.GetReferenceEvalID());
            grdPerformanceGrid.DataSource = _dsPerformance.Tables[0];

            // add look up table to the grid
            AddLookup(simLink);

            grdPerformanceView.OptionsBehavior.Editable = simLink.IsEGEditable();

            SetRowHyperLink(0); // set the first row to be default

            //view selection option
            cboViewOption.SelectedIndex = 0;

            // reflect the column and value changes
            cboViewOption_SelectedIndexChanged(new object(), new EventArgs());

        }

        /// <summary>
        /// Add look up table
        /// </summary>
        /// <param name="simLink"></param>
        private void AddLookup(simlink simLink)
        {
            #region Var PF Type
            RepositoryItemLookUpEdit myPFLookup = new RepositoryItemLookUpEdit();

            myPFLookup.DataSource = simLink.GetPFTypeLookUp();
            myPFLookup.DisplayMember = "PF_TypeName";
            myPFLookup.ValueMember = "PF_TypeID";

            myPFLookup.PopupFormWidth = 180;
            grdPerformanceView.Columns["PF_Type"].ColumnEdit = myPFLookup;
            #endregion Var PF Type

            #region Category
            RepositoryItemLookUpEdit myLookup = new RepositoryItemLookUpEdit();

            myLookup.DataSource = simLink.GetCategoryLookUp();
            myLookup.DisplayMember = "Label";
            myLookup.ValueMember = "CategoryID";

            myLookup.PopupFormWidth = 250;
            grdPerformanceView.Columns["CategoryID_FK"].ColumnEdit = myLookup;
            #endregion Category

            #region Lookup link table code
            RepositoryItemLookUpEdit likTableCodeLookup = new RepositoryItemLookUpEdit();

            likTableCodeLookup.DataSource = _simlink.GetLinkTableCodeLookUp();
            likTableCodeLookup.DisplayMember = "LinkTableCodeName";
            likTableCodeLookup.ValueMember = "LinkTableCodeID";
            likTableCodeLookup.PopupFormWidth = 180;
            grdPerformanceView.Columns["LinkTableCode"].ColumnEdit = likTableCodeLookup;
            #endregion Lookup link table code

            #region Lookup functon type
            RepositoryItemLookUpEdit functionTypeLookup = new RepositoryItemLookUpEdit();

            functionTypeLookup.DataSource = simLink.GetFunctionTypeLookUp();
            functionTypeLookup.DisplayMember = "FunctionTypeName";
            functionTypeLookup.ValueMember = "FunctionTypeID";

            functionTypeLookup.PopupFormWidth = 180;
            grdPerformanceView.Columns["PF_FunctionType"].ColumnEdit = functionTypeLookup;
            #endregion Lookup functon type

            #region Lookup function table
            RepositoryItemLookUpEdit funTableLookup = new RepositoryItemLookUpEdit();

            funTableLookup.DataSource = simLink.GetFunctionTableLookup(_simlink._nActiveProjID.ToString());
            funTableLookup.DisplayMember = "Label";
            funTableLookup.ValueMember = "FunctionID";

            funTableLookup.PopupFormWidth = 250;
            grdPerformanceView.Columns["FunctionID_FK"].ColumnEdit = funTableLookup;
            #endregion Lookup function table

            #region Option Lookup ID
            RepositoryItemLookUpEdit optionLookup = new RepositoryItemLookUpEdit();

            optionLookup.DataSource = simLink.GetOptionLookUp4Performance(_simlink._nActiveProjID.ToString());
            optionLookup.DisplayMember = "OptionLabel";
            optionLookup.ValueMember = "OptionID";

            optionLookup.PopupFormWidth = 180;
            grdPerformanceView.Columns["OptionID_FK"].ColumnEdit = optionLookup;
            #endregion Option Lookup ID

            #region Lookup DV
            RepositoryItemLookUpEdit dvLookup = new RepositoryItemLookUpEdit();

            dvLookup.DataSource = simLink.GetDVLookUp4Performance(_simlink._nActiveEvalID);
            dvLookup.DisplayMember = "DV_Label";
            dvLookup.ValueMember = "DVD_ID";

            dvLookup.PopupFormWidth = 180;
            grdPerformanceView.Columns["DV_ID_FK"].ColumnEdit = dvLookup;
            #endregion Lookup DV
            //ResultFunctionKey

            #region ResultFunctionKey
            RepositoryItemLookUpEdit resultFunctionKey = new RepositoryItemLookUpEdit();

            resultFunctionKey.DataSource = simLink.GetPerf_FunctionOnLinkedData();
            resultFunctionKey.DisplayMember = "Subcategory";
            resultFunctionKey.ValueMember = "VALINT";

            resultFunctionKey.PopupFormWidth = 180;
            grdPerformanceView.Columns["ResultFunctionKey"].ColumnEdit = resultFunctionKey;
            #endregion ResultFunctionKey
        }
        /// <summary>
        /// Row updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPerformanceView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            if (_allBindableControls == null) return;
            try
            {
                int intCurrentViewOption = cboViewOption.SelectedIndex;
                DataSet dsUpdate = _dsPerformance.GetChanges(DataRowState.Modified);
                _simlink.InsertOrUpdatePerformanceTable(dsUpdate, _simlink._nActiveEvalID, false);  // update data table

                DataSet dsInsert = _dsPerformance.GetChanges(DataRowState.Added);
                _simlink.InsertOrUpdatePerformanceTable(dsInsert, _simlink._nActiveEvalID, true);  // insert data table

                if (dsUpdate != null || dsInsert != null)
                {
                    // refresh the grid
                    LoadPerformance(_simlink); // load performance
                    if (_editorForm != null) _editorForm.EditForm.Hide();
                    _editorForm = null;
                    if (_allBindableControls != null) _allBindableControls = null; // clear it up

                    // make it the same as previous view option
                    cboViewOption.SelectedIndex = intCurrentViewOption;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
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
        /// view option selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboViewOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] astrLimitedDetail = new string[]{"Performance_Label","PF_Type","CategoryID_FK","LinkTableCode","PF_FunctionType","ApplyThreshold"}; 
                bool blnViewDetail = (cboViewOption.SelectedIndex == 1);
                if (blnViewDetail == false)
                {
                    foreach (GridColumn col in grdPerformanceView.Columns)
                    {
                        if (astrLimitedDetail.Contains(col.FieldName))
                        {
                            col.Visible = true;
                        }
                        else
                        {
                            col.Visible = false;
                        }
                    }
                }
                else // view all
                {
                    foreach (GridColumn col in grdPerformanceView.Columns)
                    {
                        col.Visible = true;
                    }
                }
                grdPerformanceView.OptionsBehavior.Editable = blnViewDetail; // only view detail is editable
                if (blnViewDetail == false)
                {
                    lblEditableInfo.Text = "Grid is NOT editable in summary view";
                }
                else
                {
                    lblEditableInfo.Text = "Grid is editable in detailed view";
                }
                btnCSV.Enabled = blnViewDetail;
                btnImportCSV.Enabled = blnViewDetail;
            }            
            catch(Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }
        private void btnCSV_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Performance template file (*.csv)|*.csv";
            save.Title = "Save CSV template file";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFile = save.FileName;
                StreamWriter csv = new StreamWriter(strFile);
                csv.WriteLine(Commons.WriteTemplateCSV(grdPerformanceView));
                csv.Flush();
                csv.Close();
            }
        }
        /// <summary>
        /// Import the CSV files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Performance CSV File (*.csv)|*.csv";
            open.Title = "Please select performance CSV to import";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    List<string> astrFieldList = new List<string>();
                    List<string> astrDataType = new List<string>();
                    Commons.GetFieldDataTypeFromGrid(grdPerformanceView, out astrFieldList, out astrDataType);

                    string strFilename = open.FileName; string strErrorMessage = "";
                    bool blnIsSucceed =  _simlink.ImportCSVDataFile2Grid("tblPerformance", astrFieldList.ToArray(), astrDataType.ToArray(),
                        strFilename, "EvalID_FK", _simlink.GetReferenceEvalID().ToString(), out strErrorMessage);
                    //bool blnIsSucceed = _simlink.ImportPerformanceFromCSV(_astrFieldAlias, strFilename, _simlink.GetReferenceEvalID().ToString());
                    if (blnIsSucceed)
                    {
                        CH2M.Commons.ShowMessage("Sucessfully imported the file '" + strFilename + "'", MessageBoxIcon.Information);
                        int intSelIndex = cboViewOption.SelectedIndex;
                        // reload the grid again
                        LoadPerformance(_simlink);
                        cboViewOption.SelectedIndex = intSelIndex;
                        cboViewOption_SelectedIndexChanged(sender, e); // reflect teh selection
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

            }
        }

        /// <summary>
        /// performance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPerformanceView_RowClick(object sender, RowClickEventArgs e)
        {
            SetRowHyperLink(e.RowHandle);
        }
        /// <summary>
        /// Set row hyperlink handle
        /// </summary>
        /// <param name="intRowHandle"></param>
        private void SetRowHyperLink(int intRowHandle)
        {
            try
            {
                linkNewTableCode.Text = "Add new linked data for '" + grdPerformanceView.GetRowCellDisplayText(intRowHandle, "Performance_Label") + "'";
                DataRowView row = grdPerformanceView.GetRow(intRowHandle) as DataRowView;
                linkNewTableCode.Tag = row.Row["LinkTableCode"].ToString() + "|" + row.Row["PF_FunctionType"].ToString() + "|" + row.Row["PerformanceID"].ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }        
        }
        /// <summary>
        /// Open up new dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkNewTableCode_Click(object sender, EventArgs e)
        {
            if (linkNewTableCode.Tag != null)
            {
                int intSelectedIndex = cboViewOption.SelectedIndex;
                string strLinkTableCode = linkNewTableCode.Tag.ToString().Split('|')[0];
                string strFunctionTypeId = linkNewTableCode.Tag.ToString().Split('|')[1];
                string strLinkTypeId = linkNewTableCode.Tag.ToString().Split('|')[2];
                fmLinkRecordList fm = new fmLinkRecordList(_simlink, this, strLinkTableCode, int.Parse(strLinkTypeId), int.Parse(strFunctionTypeId));
                fm.ShowDialog();
                if (intSelectedIndex >= 0) cboViewOption.SelectedIndex = intSelectedIndex; // set the index the same
            }
        }

        private DevExpress.XtraEditors.LookUpEdit _editableFunctionID;
        private DevExpress.XtraEditors.LookUpEdit _editableOptionID;
        private DevExpress.XtraEditors.LookUpEdit _editableDVKeyID;
        private DevExpress.XtraEditors.TextEdit _functionArg;
        private DevExpress.XtraEditors.LookUpEdit _colEditFunctionType;
        private DevExpress.XtraEditors.LookUpEdit _colLinkedTableCode;
        private DevExpress.XtraEditors.LookUpEdit _functionResultKey;
        private DevExpress.XtraEditors.CheckEdit _chkApplyThreshold;
        private DevExpress.XtraEditors.TextEdit _editThreshold;
        private DevExpress.XtraEditors.CheckEdit _chkIsOverThreshold;
        private DevExpress.XtraEditors.CheckEdit _chkComponentApplyThreshold;
        private DevExpress.XtraEditors.CheckEdit _chkComponentIsOverThreshold;
        private DevExpress.XtraEditors.TextEdit _txtComponentThresholdValue;



        private DevExpress.XtraGrid.EditForm.Helpers.EditFormBindableControlsCollection _allBindableControls;
        ShowingPopupEditFormEventArgs _editorForm;
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
                        DevExpress.XtraEditors.TextEdit ctr = (DevExpress.XtraEditors.TextEdit)con;
                        ctr.Text = "";
                    }
                }
            }
        }
        private void grdPerformanceView_ShowingPopupEditForm(object sender, ShowingPopupEditFormEventArgs e)
        {
            try
            {
                _editorForm = e;
                if (cboViewOption.SelectedIndex == 0)
                {
                    Commons.ShowMessage("You can only add new or edit value on the detailed view mode only!");
                    return;
                }
                foreach(var col in e.EditForm.Controls)
                {
                    Console.WriteLine(col.ToString());
                }
                _allBindableControls = e.BindableControls;
                //EditValue/PF_FunctionType
                foreach (var col in e.BindableControls)
                {
                    
                    //Console.WriteLine(col.Tag.ToString());
                    if (col.GetType().ToString() == "DevExpress.XtraEditors.LookUpEdit" || col.GetType().ToString() == "DevExpress.XtraEditors.TextEdit")
                    {
                        col.GotFocus += col_GotFocus;
                    }
                    //Console.WriteLine(col.Tag.ToString() + ":  " + col.GetType().ToString());
                    if (col.Tag.ToString() == "EditValue/PF_FunctionType")
                    {
                        _colEditFunctionType = (DevExpress.XtraEditors.LookUpEdit)col;
                        _colEditFunctionType.EditValueChanged += colEditFunctionType_EditValueChanged;
                    }
                    else if (col.Tag.ToString() == "EditValue/ResultFunctionKey")
                    {
                        _functionResultKey = (DevExpress.XtraEditors.LookUpEdit)col;
                    }
                    else if (col.Tag.ToString() == "EditValue/ApplyThreshold")
                    {
                        _chkApplyThreshold = (DevExpress.XtraEditors.CheckEdit)col;
                        _chkApplyThreshold.CheckedChanged += _chkApplyThreshold_CheckedChanged;
                    }
                    else if (col.Tag.ToString() == "EditValue/Threshold")
                    {
                        _editThreshold = (DevExpress.XtraEditors.TextEdit)col;
                    }
                    else if (col.Tag.ToString() == "EditValue/IsOver_Threshold")
                    {
                        _chkIsOverThreshold = (DevExpress.XtraEditors.CheckEdit)col;
                    }
                    else if (col.Tag.ToString() == "EditValue/FunctionID_FK")
                    {
                        _editableFunctionID = (DevExpress.XtraEditors.LookUpEdit)col;
                        _editableFunctionID.EditValueChanged += _editableFunctionID_EditValueChanged;
                        _editableFunctionID.EnabledChanged += _editableFunctionID_EnabledChanged;
                    }
                    else if (col.Tag.ToString() == "EditValue/FunctionArgs")
                    {
                        _functionArg = (DevExpress.XtraEditors.TextEdit)col; // function arg
                        _functionArg.EnabledChanged += _functionArg_EnabledChanged;
                    }
                    else if (col.Tag.ToString() == "EditValue/LinkTableCode")
                    {
                        _colLinkedTableCode = (DevExpress.XtraEditors.LookUpEdit)col;
                        _colLinkedTableCode.EditValueChanged += colEditLinkCode_EditValueChanged;
                    }
                    else if (col.Tag.ToString() == "EditValue/DV_ID_FK")
                    {
                        _editableDVKeyID = (DevExpress.XtraEditors.LookUpEdit)col;
                    }
                    else if (col.Tag.ToString() == "EditValue/OptionID_FK")
                    {
                        _editableOptionID = (DevExpress.XtraEditors.LookUpEdit)col;
                    }
                    else if (col.Tag.ToString() == "EditValue/ComponentApplyThreshold")
                    {
                        _chkComponentApplyThreshold = (DevExpress.XtraEditors.CheckEdit)col;
                        _chkComponentApplyThreshold.CheckedChanged += _chkComponentApplyThreshold_CheckedChanged;
                    }
                    else if (col.Tag.ToString() == "EditValue/ComponentIsOver_Threshold")
                    {
                        _chkComponentIsOverThreshold = (DevExpress.XtraEditors.CheckEdit)col;
                    }
                    else if (col.Tag.ToString() == "EditValue/ComponentThreshold")
                    {
                        _txtComponentThresholdValue = (DevExpress.XtraEditors.TextEdit)col; // function arg
                    }
                }

                // apply threshold
                _chkApplyThreshold_CheckedChanged(new object(), new EventArgs()); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }
        /// <summary>
        /// Disable when the Apply Component Threshold is true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _chkComponentApplyThreshold_CheckedChanged(object sender, EventArgs e)
        {
            _chkComponentIsOverThreshold.Enabled = (_chkComponentApplyThreshold.Checked == true);
            _txtComponentThresholdValue.Enabled = (_chkComponentApplyThreshold.Checked == true);
        }
        /// <summary>
        /// Apply threshold when the check value is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _chkApplyThreshold_CheckedChanged(object sender, EventArgs e)
        {
            _editThreshold.Enabled = _chkApplyThreshold.Checked;
            _chkIsOverThreshold.Enabled = _chkApplyThreshold.Checked;
        }

        void col_GotFocus(object sender, EventArgs e)
        {
            SetDisableControlToNoSet();
        }

        void _editableFunctionID_EnabledChanged(object sender, EventArgs e)
        {
            if (_editableFunctionID.Enabled == false)
            {
                _editableFunctionID.EditValue = -1;
                _functionArg.Enabled = false;
            }
        }

        void _functionArg_EnabledChanged(object sender, EventArgs e)
        {
            if (_functionArg.Enabled == false)
            {
                _functionArg.EditValue = "";
            }
        }
        /// <summary>
        /// Validating row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPerformanceView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (_allBindableControls == null)
                {
                    return;
                }

                foreach (var con in _allBindableControls)
                {
                    if (con.GetType().ToString() == "DevExpress.XtraEditors.LookUpEdit")
                    {
                        DevExpress.XtraEditors.LookUpEdit lookup = (DevExpress.XtraEditors.LookUpEdit)con;
                        if (lookup.Enabled)
                        {
                            if (lookup.EditValue == DBNull.Value)
                            {
                                lookup.ErrorText = "Please select a value!";
                                e.Valid = false;
                            }
                            else // there are values
                            {
                                if (lookup.Tag.ToString() == "EditValue/PF_FunctionType")
                                {
                                    if (lookup.EditValue.ToString() == "1")
                                    {
                                        if (_functionArg.Text == "")
                                        {
                                            _functionArg.ErrorText = "Please enter the function argument!";
                                            e.Valid = false;
                                        }
                                    }
                                    if (lookup.EditValue.ToString() == "2")
                                    {
                                        if (_colLinkedTableCode.EditValue.ToString() != "4") // when the table codes !=4 then it's required
                                        {
                                            if (_functionResultKey.EditValue.ToString() == "-1")
                                            {
                                                _functionResultKey.ErrorText = "Only Sum, Min, Max and Count is allowed!";
                                                e.Valid = false;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            lookup.EditValue = -1; // set to default -1
                        }
                    }
                    else if (con.GetType().ToString() == "DevExpress.XtraEditors.TextEdit")
                    {
                        DevExpress.XtraEditors.TextEdit txtVal = (DevExpress.XtraEditors.TextEdit)con;
                        if (txtVal.Enabled)
                        {
                            if (txtVal.Text == "" && (txtVal.Tag.ToString() == "EditValue/SQN" || txtVal.Tag.ToString() == "EditValue/Performance_Label"))
                            {
                                txtVal.ErrorText = "Please select a value!";
                                e.Valid = false;
                            }
                        }
                    }
                }
                if (_editThreshold.Enabled)
                {
                    if (_editThreshold.Text =="")
                    {
                        _editThreshold.ErrorText = "Please enter a valid threshold value!";
                        e.Valid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }

        void colEditLinkCode_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.LookUpEdit colEditLinkTableCode = (DevExpress.XtraEditors.LookUpEdit)sender;
                bool blnEnableFunction = !(colEditLinkTableCode.EditValue.ToString() != "4"); // editable 
                if (_editableDVKeyID != null)
                {
                    _editableDVKeyID.Enabled = blnEnableFunction;
                    if (blnEnableFunction == false) _editableDVKeyID.ItemIndex = 0; // set default to not set
                }
                if (_editableOptionID != null)
                {
                    _editableOptionID.Enabled = blnEnableFunction;
                    if (blnEnableFunction == false) _editableOptionID.ItemIndex = 0; // set default to not set
                }
                if (_functionResultKey != null)
                {
                    if (colEditLinkTableCode.EditValue.ToString() == "4" || _colEditFunctionType.EditValue.ToString() =="1" )
                    {
                        _functionResultKey.EditValue = -1; // set to -1
                        _functionResultKey.Enabled = false;
                    }
                    else
                    {
                        _functionResultKey.Enabled = true;
                    }
                }
 
                //if (_colEditFunctionType.EditValue.ToString() == "1")
                //{
                //    if (colEditLinkTableCode.EditValue.ToString() == "-1")
                //    {
                //        Commons.ShowMessage("Link table code cannot be 'Not Set' when the function type is 'Function'");
                //        colEditLinkTableCode.EditValue = 2; // set default to 1
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }

        void colEditFunctionType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.LookUpEdit colEditFunctionType = (DevExpress.XtraEditors.LookUpEdit)sender;
                bool blnEnableFunction = !(colEditFunctionType.EditValue.ToString() == "2");
                if (_editableFunctionID != null)
                {
                    _editableFunctionID.Enabled = blnEnableFunction;
                    if (blnEnableFunction == false)
                    {
                        _editableFunctionID.EditValue = -1; // set default to not set
                        _functionArg.Text = "";
                    }
                    _functionArg.Enabled = blnEnableFunction;
                }
                if (_colLinkedTableCode != null)
                {
                    if (colEditFunctionType.EditValue.ToString() == "2")
                    {
                        _colLinkedTableCode.Enabled = true;
                    }
                    else
                    {
                        _colLinkedTableCode.EditValue = -1; // set to -1
                        _colLinkedTableCode.Enabled = false;
                    }
                }

                if (_functionResultKey != null)
                {
                    if (_colLinkedTableCode.EditValue.ToString() == "4" || colEditFunctionType.EditValue.ToString() == "1")
                    {
                        _functionResultKey.EditValue = -1; // set to -1
                        _functionResultKey.Enabled = false;
                    }
                    else
                    {
                        _functionResultKey.Enabled = true;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }
        /// <summary>
        /// set initial value on the editing dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdPerformanceView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            try
            {
                grdPerformanceView.SetRowCellValue(e.RowHandle, "IsObjective", false);
                grdPerformanceView.SetRowCellValue(e.RowHandle, "IsOver_Threshold", false);
                grdPerformanceView.SetRowCellValue(e.RowHandle, "ApplyThreshold", false);
                grdPerformanceView.SetRowCellValue(e.RowHandle, "ComponentApplyThreshold", false);
                grdPerformanceView.SetRowCellValue(e.RowHandle, "ComponentIsOver_Threshold", false);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Source + ": " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intTotalSelectedRows = grdPerformanceView.GetSelectedRows().Count();
            if (intTotalSelectedRows > 0)
            {
                if (Commons.ShowMessage("Do you wish to delete the " + intTotalSelectedRows.ToString() + " selected row.\r\nNote: Deleting decision variables may invalidate any dependent results. The user is responsible for ensuring downstream data integrity.",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (int intRowIndex in grdPerformanceView.GetSelectedRows())
                    {
                        DataRowView row = (DataRowView)grdPerformanceView.GetRow(intRowIndex);
                        DataRow toDelRow = row.Row;

                        // delete the row & associated data
                        _simlink.DeletePerformace(toDelRow);
                    }
                    int intSel = cboViewOption.SelectedIndex;
                    // reload whole object again after deleting
                    LoadPerformance(_simlink);

                    cboViewOption.SelectedIndex = intSel;
                    // refresh the grid
                    cboViewOption_SelectedIndexChanged(sender, e);
                }
            }
        }

    }
}
