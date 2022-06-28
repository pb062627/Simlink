using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using CH2M;
using System.IO;
using DevExpress.XtraGrid.Views.Base;

namespace SIM_API_LINKS
{
    public partial class UCDVGridEditor : XtraUserControl
    {
        private string _strSQLUsed = "";
        private DataSet _dsEG_DecisionVariables = new DataSet();
        private simlink _simLink = null;
        private int _intActiveModelTypeId;
        private bool _blnCancelSelection = false;
        private DataTable _dtOption = null;
        string[] _astrFieldAlias = new string[]{"DV_Label","VarType_FK","DV_Description","DV_Type","Option_FK",
                        "Option_MIN","Option_MAX","GetNewValMethod","FunctionID_FK","FunctionArgs","ElementID_FK","sqn",
                        "SecondaryDV_Key","PrimaryDV_ID_FK","IsSpecialCase"};
        string[] _astrFieldCaption = new string[] { "Name", "Type ID", "Description", "Type", "Option ID", "Min Option", "Max Option", 
                        "New Val Method", "Function ID", "Function Arguments", "Element ID", "Order", "Secondary DV Label", 
                        "Primary DV ID (default=-1)", "Special Case? (0=no 1=yes)" };

        public UCDVGridEditor()
        {
            InitializeComponent();

            _blnCancelSelection = true;
            cboFilter.SelectedIndex = 0; // always display show all
            _blnCancelSelection = false;

            this.grdDVView.OptionsBehavior.EditingMode = GridEditingMode.EditForm;
            this.grdDVView.ShowingPopupEditForm += new DevExpress.XtraGrid.Views.Grid.ShowingPopupEditFormEventHandler(this.grdDVView_ShowingPopupEditForm);
            cboViewOption.SelectedIndex = 1; // detail view
        }

        public void ClearGrid()
        {
            grdDVGrid.DataSource = null;
        }
        /// <summary>
        /// Load DV data
        /// </summary>
        /// <param name="simlink"></param>
        /// <param name="intEvalID"></param>
        /// <param name="intActiveModelTypeID"></param>
        public void LoadDVData(simlink simLink, int intActiveModelTypeID)
        {
            _simLink = simLink;
            _intActiveModelTypeId = intActiveModelTypeID;
            _dsEG_DecisionVariables = simLink.DNA_GetDVInfo(simLink.GetReferenceEvalID(), intActiveModelTypeID, out _strSQLUsed, "");
            CleanUpUnNeededCols(); // clean up the dataset

            //// load to grid
            grdDVGrid.DataSource = _dsEG_DecisionVariables.Tables[0];
            grdDVView.RefreshData();
            //grdDVGrid.MainView.LayoutChanged();

            AddLookup(simLink);

            // check if it's editable
            bool blnIsEditable = simLink.IsEGEditable();
            grdDVView.OptionsBehavior.Editable = blnIsEditable;
            btnImportCSV.Enabled = blnIsEditable;

            cboViewOption_SelectedIndexChanged(new object(), new EventArgs());
        }
        /// <summary>
        /// Clean up not needed cols
        /// </summary>
        private void CleanUpUnNeededCols()
        {
            if (_dsEG_DecisionVariables.Tables[0].Columns.Contains("EvaluationGroupID")) _dsEG_DecisionVariables.Tables[0].Columns.Remove("EvaluationGroupID");
            if (_dsEG_DecisionVariables.Tables[0].Columns.Contains("ElementName")) _dsEG_DecisionVariables.Tables[0].Columns.Remove("ElementName");
            if (_dsEG_DecisionVariables.Tables[0].Columns.Contains("codeArrColl")) _dsEG_DecisionVariables.Tables[0].Columns.Remove("codeArrColl");
            if (_dsEG_DecisionVariables.Tables[0].Columns.Contains("HasConsDV")) _dsEG_DecisionVariables.Tables[0].Columns.Remove("HasConsDV");
            if (_dsEG_DecisionVariables.Tables[0].Columns.Contains("UnitConversion")) _dsEG_DecisionVariables.Tables[0].Columns.Remove("UnitConversion");
            if (_dsEG_DecisionVariables.Tables[0].Columns.Contains("ElementVal")) _dsEG_DecisionVariables.Tables[0].Columns.Remove("ElementVal");
        }
        /// <summary>
        /// Update the dataset back
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="ds"></param>
        public void UpdateDVData(SIM_API_LINKS.DAL.DBContext dbContext, DataSet ds)
        {
            dbContext.InsertOrUpdateDBByDataset(false, ds, _strSQLUsed); // update the dataset back
        }

        #region Var Type Grid Lookup

        /// <summary>
        /// Add var type look up
        /// </summary>
        /// <param name="simLink"></param>
        public void AddLookup(simlink simLink)
        {
            #region Var Type FK
            RepositoryItemLookUpEdit myLookup = new RepositoryItemLookUpEdit();

            myLookup.DataSource = simLink.GetVarTypeLookUp();
            myLookup.DisplayMember = "FieldName";
            myLookup.ValueMember = "VarType_FK";

            myLookup.PopupFormWidth = 550;
            grdDVView.Columns["VarType_FK"].ColumnEdit = myLookup;
            #endregion Var Type FK

            #region Option List
            RepositoryItemLookUpEdit myOptionLookup = new RepositoryItemLookUpEdit();

            _dtOption = simLink.GetOptionLookUp(_simLink._nActiveProjID.ToString());
            myOptionLookup.DataSource = _dtOption;
            //DataTable dtOption =  simLink.GetOptionLookUp(_simLink._nActiveProjID.ToString());
            //myOptionLookup.DataSource = dtOption;
            myOptionLookup.DisplayMember = "OptionLabel";
            myOptionLookup.ValueMember = "OptionID";
            myOptionLookup.PopupFormWidth = 250;
            grdDVView.Columns["OptionID"].ColumnEdit = myOptionLookup;
            #endregion Option List

            #region Element
            RepositoryItemLookUpEdit myElementLookup = new RepositoryItemLookUpEdit();

            myElementLookup.DataSource = simLink.GetElementLookUp(_simLink._nActiveProjID.ToString());
            myElementLookup.DisplayMember = "ElementListLabel";
            myElementLookup.ValueMember = "ElementListID";

            myElementLookup.PopupFormWidth = 250;
            grdDVView.Columns["ElementID"].ColumnEdit = myElementLookup;
            #endregion Element

            #region Operation
            RepositoryItemLookUpEdit myOperationLookup = new RepositoryItemLookUpEdit();
            myOperationLookup.BeginUpdate();
            myOperationLookup.DataSource = simLink.GetOperationLookUp();
            myOperationLookup.DisplayMember = "Operation";
            myOperationLookup.ValueMember = "Operation";

            myOperationLookup.PopupFormWidth = 250;
            grdDVView.Columns["Operation"].ColumnEdit = myOperationLookup;
            myOperationLookup.EndUpdate();
            #endregion  Operation

            #region Get New Val Method
            RepositoryItemLookUpEdit myGetNewValLookup = new RepositoryItemLookUpEdit();

            myGetNewValLookup.DataSource = simLink.GetNewValMethodLookUp();
            myGetNewValLookup.DisplayMember = "NewMethodName";
            myGetNewValLookup.ValueMember = "NewMethodValID"; 

            myGetNewValLookup.PopupFormWidth = 200;
            grdDVView.Columns["GetNewValMethod"].ColumnEdit = myGetNewValLookup;
            #endregion  Get New Val Method

            #region Lookup functon type
            RepositoryItemLookUpEdit functionTypeLookup = new RepositoryItemLookUpEdit();

            functionTypeLookup.DataSource = simLink.GetFunctionTableLookup(_simLink._nActiveProjID.ToString());
            functionTypeLookup.DisplayMember = "Label";
            functionTypeLookup.ValueMember = "FunctionID";

            functionTypeLookup.PopupFormWidth = 180;
            grdDVView.Columns["FunctionID_FK"].ColumnEdit = functionTypeLookup;
            #endregion Lookup functon type

            #region Primary DV Lookup
            RepositoryItemLookUpEdit myPrimary = new RepositoryItemLookUpEdit();

            myPrimary.DataSource = simLink.GetParimaryDVLookUp(_simLink.GetReferenceEvalID());
            myPrimary.DisplayMember = "DV_Label";
            myPrimary.ValueMember = "DVD_ID";

            myPrimary.PopupFormWidth = 250;
            grdDVView.Columns["PrimaryDV_ID_FK"].ColumnEdit = myPrimary;
            #endregion Secondary DV Lookup

            #region Primary DV Lookup
            RepositoryItemLookUpEdit SecondaryDV_Key = new RepositoryItemLookUpEdit();

            SecondaryDV_Key.DataSource = simLink.GetSecondaryDVLookup();
            SecondaryDV_Key.DisplayMember = "Value";
            SecondaryDV_Key.ValueMember = "ID";

            SecondaryDV_Key.PopupFormWidth = 250;
            grdDVView.Columns["SecondaryDV_Key"].ColumnEdit = SecondaryDV_Key;
            #endregion Secondary DV Lookup

            grdDVGrid.Refresh();
        }
        #endregion
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
                    if (con.Tag.ToString() != "EditValue/EvaluationGroup_FK")
                    {
                        Console.WriteLine(con.Name);
                        if (con.Tag.ToString() == "EditValue/DV_Label")
                        {
                            DevExpress.XtraEditors.TextEdit DV_Label = (DevExpress.XtraEditors.TextEdit)con;
                            if (DV_Label.EditValue == DBNull.Value)
                            {
                                DV_Label.ErrorText = "Please enter Label!";
                                e.Valid = false;
                            }
                        }
                        if (con.Tag.ToString() == "EditValue/VarType_FK")
                        {
                            DevExpress.XtraEditors.LookUpEdit VarType_FK = (DevExpress.XtraEditors.LookUpEdit)con;
                            if (VarType_FK.EditValue == DBNull.Value)
                            {
                                VarType_FK.ErrorText = "Please select Type!";
                                e.Valid = false;
                            }
                        }
                        if (con.Tag.ToString() == "EditValue/Option_FK")
                        {
                            DevExpress.XtraEditors.LookUpEdit VarType_FK = (DevExpress.XtraEditors.LookUpEdit)con;
                            if (VarType_FK.EditValue == DBNull.Value)
                            {
                                VarType_FK.ErrorText = "Please selection Option!";
                                e.Valid = false;
                            }
                        }
                        if (con.Tag.ToString() == "EditValue/Option_MIN")
                        {
                            DevExpress.XtraEditors.TextEdit OptionMin = (DevExpress.XtraEditors.TextEdit)con;
                            if (OptionMin.EditValue == DBNull.Value)
                            {
                                OptionMin.ErrorText = "Please enter Option Min!";
                                e.Valid = false;
                            }
                        }
                        if (con.Tag.ToString() == "EditValue/Option_MAX")
                        {
                            DevExpress.XtraEditors.TextEdit OptionMax = (DevExpress.XtraEditors.TextEdit)con;
                            if (OptionMax.EditValue == DBNull.Value)
                            {
                                OptionMax.ErrorText = "Please enter Option Max!";
                                e.Valid = false;
                            }
                        }
                        if (con.Tag.ToString() == "EditValue/ElementID")
                        {
                            DevExpress.XtraEditors.LookUpEdit ElementID = (DevExpress.XtraEditors.LookUpEdit)con;
                            if (ElementID.EditValue == DBNull.Value)
                            {
                                ElementID.ErrorText = "Please selection Elements!";
                                e.Valid = false;
                            }
                        }
                        if (_colEditFunctionID.EditValue.ToString() != "-1")
                        {
                            if (_functionArg.EditValue.ToString() == "")
                            {
                                _functionArg.ErrorText = "Please enter the function argument!";
                                e.Valid = false;
                            }
                        }
                        else
                        {
                            _functionArg.EditValue = "";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in ValidateRow " + ex.Source + ": " + ex.Message);
            }
        }
        private void grdDVView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            grdDVView.SetRowCellValue(e.RowHandle, "PrimaryDV_ID_FK", "-1");
            grdDVView.SetRowCellValue(e.RowHandle, "SecondaryDV_Key", "-1");
            grdDVView.SetRowCellValue(e.RowHandle, "FunctionID_FK", "-1");
            grdDVView.SetRowCellValue(e.RowHandle, "IsListVar", false);
            grdDVView.SetRowCellValue(e.RowHandle, "SkipMinVal", false);
            grdDVView.SetRowCellValue(e.RowHandle, "FunctionArgs", "");
            grdDVView.SetRowCellValue(e.RowHandle, "XModelID_FK", "-1");
            grdDVView.SetRowCellValue(e.RowHandle, "IsSpecialCase", false);
            grdDVView.SetRowCellValue(e.RowHandle, "IsTS", false);
            grdDVView.SetRowCellValue(e.RowHandle, "SQN", "100");
            //grdDVView.SetRowCellValue(e.RowHandle, "ElementName", "not used");
            grdDVView.SetRowCellValue(e.RowHandle, "Operation", "Identity");
            grdDVView.SetRowCellValue(e.RowHandle, "GetNewValMethod", "-1");
        }
        private void grdDVView_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            if (e.ErrorText != "Column 'ElementName' does not allow nulls.")
            {
                //Suppress displaying the error message box
                e.ExceptionMode = ExceptionMode.NoAction;
            }
            else
            {
                e.ExceptionMode = ExceptionMode.Ignore;
            }

        }

        private void btnImportCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "DV CSV File (*.csv)|*.csv";
            open.Title = "Please select DV CSV to import";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFilename = open.FileName;
                List<string> astrFieldList = new List<string>();
                List<string> astrDataType = new List<string>();
                Commons.GetFieldDataTypeFromGrid(grdDVView, out astrFieldList, out astrDataType);

                string strErrorMessage = "";
                bool blnIsSucceed = _simLink.ImportCSVDataFile2Grid("tblDV", astrFieldList.ToArray(), astrDataType.ToArray(),
                    strFilename, "EvalID_FK", _simLink.GetReferenceEvalID().ToString(), out strErrorMessage);

                //bool blnSuccess = _simLink.ImportDVFromCSV(_astrFieldAlias, strFilename, _simLink.GetReferenceEvalID().ToString());
                if (blnIsSucceed)
                {
                    CH2M.Commons.ShowMessage("Sucessfully imported the file '" + strFilename + "'", MessageBoxIcon.Information);

                    int intSel = cboFilter.SelectedIndex;
                    // reload the grid again
                    LoadDVData(_simLink, _intActiveModelTypeId);
                    cboFilter.SelectedIndex = intSel;

                }
                else
                {
                    CH2M.Commons.ShowMessage("Error importing file '" + strFilename + "'\r\n" + strErrorMessage, MessageBoxIcon.Information);
                }
            }
        }

        private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blnCancelSelection == false)
            {
                if (cboFilter.SelectedIndex == 0) // show all
                {
                    _dsEG_DecisionVariables = _simLink.DNA_GetDVInfo(_simLink.GetReferenceEvalID(), _intActiveModelTypeId, out _strSQLUsed, "");
                    CleanUpUnNeededCols(); // clean up the dataset
                    grdDVGrid.DataSource = _dsEG_DecisionVariables.Tables[0];
                    grdDVGrid.Refresh();
                    //grdDVView.Columns["PrimaryDV_ID_FK"].Visible = true;
                }
                else
                {
                    string strFilter = (cboFilter.SelectedIndex == 1 ? "PrimaryDV_ID_FK=-1" : "PrimaryDV_ID_FK<>-1");

                    //grdDVView.Columns["PrimaryDV_ID_FK"].Visible = (cboFilter.SelectedIndex == 1 ? false : true);
                    //grdDVView.Columns["PrimaryDV_ID_FK"].Visible = true;
                    _dsEG_DecisionVariables = _simLink.DNA_GetDVInfo(_simLink.GetReferenceEvalID(), _intActiveModelTypeId, out _strSQLUsed, strFilter);
                    CleanUpUnNeededCols(); // clean up the dataset

                    //// load to grid
                    grdDVGrid.DataSource = _dsEG_DecisionVariables.Tables[0];
                    grdDVView.RefreshData();

                    
                }
                AddLookup(_simLink);
                // set limited and detail view check
                cboViewOption_SelectedIndexChanged(new object(), new EventArgs());
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intTotalSelectedRows = grdDVView.GetSelectedRows().Count();
            if (intTotalSelectedRows > 0)
            {
                if (Commons.ShowMessage("Do you wish to delete the " + intTotalSelectedRows.ToString() + " selected DV.\r\nNote: Deleting decision variables may invalidate any dependent results. The user is responsible for ensuring downstream data integrity.",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (int intRowIndex in grdDVView.GetSelectedRows())
                    {
                        DataRowView row = (DataRowView)grdDVView.GetRow(intRowIndex);
                        DataRow toDelRow = row.Row;

                        // delete the row & associated data
                        _simLink.DeleteDV(toDelRow);
                    }

                    // reload whole object again after deleting
                    LoadDVData(_simLink, _intActiveModelTypeId);

                    // refresh the grid
                    cboFilter_SelectedIndexChanged(sender, e);
                }
            }
        }
        /// <summary>
        /// Save on updating row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdDVView_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataSet dsUpdate = _dsEG_DecisionVariables.GetChanges(DataRowState.Modified);
            _simLink.InsertOrUpdateDVTable(dsUpdate, false, _simLink.GetReferenceEvalID());  // update data table

            DataSet dsInsert = _dsEG_DecisionVariables.GetChanges(DataRowState.Added);
            _simLink.InsertOrUpdateDVTable(dsInsert, true, _simLink.GetReferenceEvalID());  // insert data table
            
            if (dsUpdate != null || dsInsert != null)
            {
                // refresh the grid
                cboFilter_SelectedIndexChanged(sender, e);
                if (_editorForm != null) _editorForm.EditForm.Hide();
                _editorForm = null;
                if (_allBindableControls != null) _allBindableControls = null; // clear it up
                Application.DoEvents();
            }

        }
        /// <summary>
        /// export csv tempate file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCSVTemplate_Click(object sender, EventArgs e)
        {
            string strHeader = String.Join(",", _astrFieldCaption);
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "DV template file (*.csv)|*.csv";
            save.Title = "Save CSV template file";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFile = save.FileName;
                StreamWriter csv = new StreamWriter(strFile);
                csv.WriteLine(Commons.WriteTemplateCSV(grdDVView));
                csv.Flush();
                csv.Close();
            }
        }

        private void grdDVView_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
            {
                e.Valid = false;
            }
        }

        private void grdDVView_EditFormPrepared(object sender, EditFormPreparedEventArgs e)
        {
            ReloadOptionElement(_simLink, (DevExpress.XtraEditors.LookUpEdit)e.BindableControls["OptionID"],
                (DevExpress.XtraEditors.LookUpEdit)e.BindableControls["ElementID"]); //reload it again
        }

        private void ReloadOptionElement(simlink simLink, LookUpEdit editOption, LookUpEdit editElement)
        {
            #region Option List

            _dtOption = simLink.GetOptionLookUp(_simLink._nActiveProjID.ToString());
            editOption.Properties.DataSource = _dtOption;
            editOption.Properties.DisplayMember = "OptionLabel";
            editOption.Properties.ValueMember = "OptionID";
            editOption.Properties.PopupFormWidth = 250;
            //grdDVView.Columns["OptionID"].ColumnEdit = editOption;
            #endregion Option List

            #region Element

            editElement.Properties.DataSource = simLink.GetElementLookUp(_simLink._nActiveProjID.ToString());
            editElement.Properties.DisplayMember = "ElementListLabel";
            editElement.Properties.ValueMember = "ElementListID";

            editElement.Properties.PopupFormWidth = 250;
            //grdDVView.Columns["ElementID"].ColumnEdit = myElementLookup;
            #endregion Element
        }
        private DevExpress.XtraGrid.EditForm.Helpers.EditFormBindableControlsCollection _allBindableControls;
        private ShowingPopupEditFormEventArgs _editorForm;
        private DevExpress.XtraEditors.TextEdit _functionArg;
        private DevExpress.XtraEditors.LookUpEdit _colEditFunctionID;
        private DevExpress.XtraEditors.LookUpEdit _colPrimaryDV;
        private DevExpress.XtraEditors.LookUpEdit _colSecondaryDV;
        private DevExpress.XtraEditors.TextEdit _colElementName;
        /// <summary>
        /// Show the editor form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdDVView_ShowingPopupEditForm(object sender, ShowingPopupEditFormEventArgs e)
        {
            _editorForm = e;
            _allBindableControls = e.BindableControls;
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
                else if (col.Tag.ToString() == "EditValue/PrimaryDV_ID_FK")
                {
                    _colPrimaryDV = (DevExpress.XtraEditors.LookUpEdit)col;
                    _colPrimaryDV.EditValueChanged += _colPrimaryDV_EditValueChanged;
                }
                else if (col.Tag.ToString() == "EditValue/SecondaryDV_Key")
                {
                    _colSecondaryDV = (DevExpress.XtraEditors.LookUpEdit)col;
                }
                //else if (col.Tag.ToString() == "EditValue/ElementName")
                //{
                //    _colElementName = (DevExpress.XtraEditors.TextEdit)col;
                //}

            }
        }

        void _colPrimaryDV_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.LookUpEdit colPrimaryDVKey = (DevExpress.XtraEditors.LookUpEdit)sender;
                bool blnEnableFunction = !(colPrimaryDVKey.EditValue.ToString() == "-1");
                if (_colSecondaryDV != null) _colSecondaryDV.Enabled = blnEnableFunction;
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
            if (_allBindableControls == null) return;
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
                        ctr.EditValue = "";
                    }
                }
            }
        }

        private void cboViewOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool blnIsDetailedView = (cboViewOption.SelectedIndex == 1); // detail view
            colGetNewValMethod.Visible = blnIsDetailedView;
            colFunctionID_FK.Visible = blnIsDetailedView;
            colFunctionArgs.Visible = blnIsDetailedView;
            colIsListVar.Visible = blnIsDetailedView;
            colSkipMinVal.Visible = blnIsDetailedView;
            colPrimaryDV_ID_FK.Visible = blnIsDetailedView;
            colSecondaryDVLabel.Visible = blnIsDetailedView;
            colIsSpecialCase.Visible = blnIsDetailedView;
            colIsTS.Visible = blnIsDetailedView;
            colXModelID_FK.Visible = blnIsDetailedView;

            grdDVView.OptionsBehavior.Editable = blnIsDetailedView; // only view detail is editable
            if (blnIsDetailedView == false)
            {
                lblEditableInfo.Text = "Grid is NOT editable in limited detailed view";
            }
            else
            {
                lblEditableInfo.Text = "Grid is editable in detailed view";
            }
            btnCSVTemplate.Enabled = blnIsDetailedView;
            btnImportCSV.Enabled = blnIsDetailedView;
        }

        private void grpFilter_Enter(object sender, EventArgs e)
        {

        }

        private void grdDVView_HiddenEditor(object sender, EventArgs e)
        {
            Console.WriteLine("Hidding ....");
        }
    }
}
