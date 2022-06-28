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
    public partial class UCEditOpenProject : DevExpress.XtraEditors.XtraUserControl
    {
        private bool _blnOpenProject = false;
        private bool _blnHideOpenButton = false;
        public delegate void ButtonClickedEventHandler(object sender, EventArgs e);
        public event ButtonClickedEventHandler OpenProjectButtonClick;


        public bool HideOpenButton
        {
            get { return _blnHideOpenButton; }
            set { 
                _blnHideOpenButton = value;
                btnOpenEdit.Visible = !_blnHideOpenButton;
            }
        }
        private simlink _simlink;

        public simlink MySimlink
        {
            get { return _simlink; }
            set { _simlink = value; }
        }
        /// <summary>
        /// constructor
        /// </summary>
        public UCEditOpenProject()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Check if it's open project
        /// </summary>
        public bool IsOpenProject
        {
            get
            {
                return _blnOpenProject;
            }
            set
            {
                _blnOpenProject = value;
                btnOpenEdit.Text = (_blnOpenProject ? "Open" : "Edit");
            }
        }
        /// <summary>
        /// Load project to the grid
        /// </summary>
        /// <param name="simlink"></param>
        public void LoadProject(simlink simlink)
        {
            grdProject.DataSource = simlink.LoadProject();
        }
        /// <summary>
        /// Open project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenEdit_Click(object sender, EventArgs e)
        {
            if (OpenProjectButtonClick != null)
                OpenProjectButtonClick(this, e);
        }
    }
}
