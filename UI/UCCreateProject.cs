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
using SIM_API_LINKS;

namespace CH2M
{
    public partial class UCCreateProject : DevExpress.XtraEditors.XtraUserControl
    {
        private simlink _simlink;

        public simlink MySimlink
        {
            get { return _simlink; }
            set { _simlink = value; }
        }

        public UCCreateProject()
        {
            InitializeComponent();

        }
        /// <summary>
        /// Initiate control
        /// </summary>
        /// <param name="simlink"></param>
        public void InitiateControl(simlink simlink)
        {
            cboModelType.DataSource = simlink.LoadUIDictionary(SIM_API_LINKS.UIDictionary.ModelType);
            cboModelType.ValueMember = "ValInt";
            cboModelType.DisplayMember = "Val";
            
            // load project to grid
            ctrUCEdit.LoadProject(simlink);

            _simlink = simlink;
        }
        /// <summary>
        /// Validate input
        /// </summary>
        /// <returns></returns>
        private bool ValidateInput()
        {
            if (cboModelType.SelectedIndex<0)
            {
                Commons.ShowMessage("Please select a model type!", MessageBoxIcon.Exclamation);
                return false;
            }
            if (txtProjectName.Text =="")
            {
                Commons.ShowMessage("Please enter a project name!", MessageBoxIcon.Exclamation);
                return false;
            }
            if (!System.IO.File.Exists(txtFileLocation.Text))
            {
                Commons.ShowMessage("Please select a valid model file!", MessageBoxIcon.Exclamation);
                return false;
            }
            if (txtNote.Text == "")
            {
                Commons.ShowMessage("Please enter a projec note!", MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        #region Event Form Handler
        /// <summary>
        /// Save project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput()) // validate input
            {
                int intModelTypeID = (int)cboModelType.SelectedValue;
                _simlink.CreateProject(intModelTypeID, txtProjectName.Text, txtFileLocation.Text, txtNote.Text);
                
                Application.DoEvents(); // wait till the project is finished then activate query

                ctrUCEdit.LoadProject(_simlink);
            }
        }
        /// <summary>
        /// Open file button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFileLocation_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Please select a model file";
            if (open.ShowDialog() == DialogResult.OK)
            {
                txtFileLocation.Text = open.FileName;
            }
        }
        #endregion Event Form Handler

    }
}
