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
using System.IO;
using CH2M;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace SIM_API_LINKS
{
    public partial class frmDefineRuns : DevExpress.XtraEditors.XtraForm
    {
        private simlink _simlink = null;
        private string _strProjectId;
        private string _strEVGroupId;
        private DataSet _dsResultData;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="simlink"></param>
        /// <param name="strProjectId"></param>
        /// <param name="strEVGroupId"></param>
        public frmDefineRuns(simlink simlink)
        {
            InitializeComponent();

            _simlink = simlink;
            _strProjectId = simlink._nActiveProjID.ToString();
            _strEVGroupId = simlink._nActiveEvalID.ToString(); // for scenario it needs the Active Eval ID not the RefEvalID
        }
        #endregion Constructor

        #region Private Method
        /// <summary>
        /// Load scenario list
        /// </summary>
        /// <param name="strProjectId"></param>
        /// <param name="strEVGroupId"></param>
        private void LoadScenarioList(string strProjectId, string strEVGroupId)
        {
            _dsResultData = _simlink.LoadScenario(strProjectId, strEVGroupId);
            grdDefineRun.DataSource = _dsResultData.Tables[0];
            grdViewScenario.BestFitColumns();

            bool blnIsEditable = true; // set it to be editable all the time for now // _simlink.IsEGEditable(); 
            grdViewScenario.OptionsBehavior.Editable = blnIsEditable;
            btnImportScenarioFile.Enabled = blnIsEditable;
            btnPasteFromExcel.Enabled = blnIsEditable;
        }
        #endregion

        #region Form Event Handler
        /// <summary>
        /// On load event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDefineRuns_Load(object sender, EventArgs e)
        {
            LoadScenarioList(_strProjectId, _strEVGroupId);
        }
        #endregion Form Event Handler

        private void btnImportScenarioFile_Click(object sender, EventArgs e)
        {
            ImportFromCSVFile();
        }

        /// <summary>
        /// Import the scenario from a csv file
        /// </summary>
        /// <param name="strCSVFile"></param>
        private void ImportFromCSVFile()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Scenario CSV File (*.csv)|*.csv";
            open.Title = "Please select scenario CSV to import";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFilename = open.FileName;
                try
                {
                    _simlink.ImportScenarioFromCSV(strFilename, _strProjectId, _strEVGroupId);

                    LoadScenarioList(_strProjectId, _strEVGroupId);
                }
                catch (Exception ex)
                {
                    Commons.ShowMessage("Invalid csv file format to import '" + ex.Message + "'");
                }
            }
        }
        /// <summary>
        /// Close dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // close dialog
        }
        /// <summary>
        /// Download the template csv file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDownloadSampleCSV_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Scenario CSV template file (*.csv)|*.csv";
            save.Title = "Save CSV template file";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFile = save.FileName;
                StreamWriter csv = new StreamWriter(strFile);
                csv.WriteLine("ScenarioLabel,DNA,HasBeenRun,ScenStart,ScenEnd,DateEvaluated");
                csv.Flush();
                csv.Close();
            }
        }
        /// <summary>
        /// Delete the selected rows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int intTotalSelectedRows = grdViewScenario.GetSelectedRows().Count();
            if (intTotalSelectedRows > 0)
            {
                if (Commons.ShowMessage("Do you wish to delete the " + intTotalSelectedRows.ToString() + " selected scenarios.\r\nNote: Deleting a scenario(s) will delete all associated results data.", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (int intRowIndex in grdViewScenario.GetSelectedRows())
                    {
                        DataRowView row = (DataRowView)grdViewScenario.GetRow(intRowIndex);
                        DataRow toDelRow = row.Row;

                        // delete the row & associated data
                        _simlink.DeleteScenario(int.Parse(_strEVGroupId), toDelRow); 
                    }

                    LoadScenarioList(_strProjectId, _strEVGroupId);
                }
            }
        }

        private void grdViewScenario_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            DataSet dsUpdate = _dsResultData.GetChanges(DataRowState.Modified);
            _simlink.InsertOrUpdateScenarioTable(dsUpdate, false, _strEVGroupId);  // update data table

            DataSet dsInsert = _dsResultData.GetChanges(DataRowState.Added);
            _simlink.InsertOrUpdateScenarioTable(dsInsert, true, _strEVGroupId);  // insert data table

            if (dsUpdate != null || dsInsert != null)
            {
                // reload the dataset object again
                LoadScenarioList(_strProjectId, _strEVGroupId);
            }
        }

        private void grdViewScenario_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            GridView view = sender as GridView;
            foreach (GridColumn col in view.Columns)
            {
                object objValue = view.GetRowCellValue(e.RowHandle, col);
                if (objValue == DBNull.Value && col.FieldName != "ScenarioID")
                {
                    view.SetColumnError(col, "Please enter/select a valid value. It cannot be empty!");
                    e.Valid = false;
                }
            }
        }
        /// <summary>
        /// Invalid row exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewScenario_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            //Suppress displaying the error message box
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        /// <summary>
        /// Row selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewScenario_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (grdViewScenario.GetSelectedRows().Count() >0)
            {
                DataRowView rowView = (DataRowView)grdViewScenario.GetRow(grdViewScenario.FocusedRowHandle);
                DataRow row = rowView.Row;
                bool blnHasBeenRun = bool.Parse(row["HasBeenRun"].ToString());
                btnRunScenario.Enabled = !blnHasBeenRun;
            }
        }

        /// <summary>
        /// Run the first selected scenario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRunScenario_Click(object sender, EventArgs e)
        {
            if (grdViewScenario.GetSelectedRows().Count() > 0)
            {
                string[] astrScenarioId2Run = new string[grdViewScenario.GetSelectedRows().Count()];
                int intIndex = 0;
                foreach (int intRowIndex in grdViewScenario.GetSelectedRows())
                {
                    astrScenarioId2Run[intIndex] = grdViewScenario.GetRowCellValue(intRowIndex, "ScenarioID").ToString();
                    intIndex++;
                }
                lblInfo.Visible = true;
                lblInfo.Text = "Please wait while running scenario(s)...";
                lblInfo.Refresh();
                _simlink.ProcessEvaluationGroup(astrScenarioId2Run); // run only the selected scenario
                LoadScenarioList(_strProjectId, _strEVGroupId);
                lblInfo.Visible = false;
                lblInfo.Refresh();
            }
            else
            {
                Commons.ShowMessage("Please select at least one scenario to run!");
            }
        }

        private void frmDefineRuns_KeyDown(object sender, KeyEventArgs e)
        {
            Commons.ShowMessage("Down");
        }

        private void btnPasteFromExcel_Click(object sender, EventArgs e)
        {
            if (CommonUtilities.ClipboardData.Split('\n').Length > 1)
            {
                string strFile = System.IO.Path.GetTempPath() + @"\scen" + System.Guid.NewGuid().ToString() + ".csv";
                StreamWriter write = new StreamWriter(strFile);
                write.WriteLine("ScenarioLabel,DNA,HasBeenRun,ScenStart,ScenEnd,DateEvaluated"); // write header
                string[] astrData = CommonUtilities.ClipboardData.Split('\n');
                foreach(string strDataLine in astrData)
                {
                    string[] astrEachCol = strDataLine.Split('\t');
                    string strLine = "";
                    foreach(string strVal in astrEachCol)
                    {
                        strLine += "\"" + strVal + "\",";
                    }
                    if (strLine.Length > 1) strLine = strLine.Substring(0, strLine.Length - 1);
                    write.WriteLine(strLine); // line data
                }

                write.Close();

                try
                {
                    _simlink.ImportScenarioFromCSV(strFile, _strProjectId, _strEVGroupId);

                    LoadScenarioList(_strProjectId, _strEVGroupId);
                }
                catch (Exception ex)
                {
                    Commons.ShowMessage("Error pasting data from Excel " + ex.Message);
                }
                try
                {
                    File.Delete(strFile); // delete the files
                }
                catch
                {
                    // do nothing
                }
            }
            else
            {
                Commons.ShowMessage("No clipboard data to paste!");
            }
        }

    }
}