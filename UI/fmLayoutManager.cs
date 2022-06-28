using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using SIM_API_LINKS.Properties;
using SIM_API_LINKS;


namespace CH2M.SimLink.MainUI
{
    public partial class fmLayoutManager : DevExpress.XtraEditors.XtraForm
    {
        private frmSimLinkMain _MainForm;

        /// <summary>
        /// fmLayoutManager constructor  
        /// </summary>
        /// <param name="MainForm">SimLink form</param>
        public fmLayoutManager(frmSimLinkMain MainForm)
        {
            if (MainForm != null)
            _MainForm = MainForm;

            InitializeComponent();
        }

        private void fmLayoutManager_Load(object sender, EventArgs e)
        {
             RefreshLayoutsList();
        }

        /// <summary>
        /// Loads the list of Layouts from the User Config, only adding it after checking that the layout file exists.
        /// Then highlights the current layout if found, otherwise the 'Default' is highlighted. 
        /// </summary>
        private void RefreshLayoutsList()
        {
            lbLayouts.BeginUpdate();

            lbLayouts.Items.Clear();
            bool defaultFound = false;

            // add the list of stored layouts to the Layout selector combo;
            foreach (string str in Settings.Default.LayoutList)
            {
                // if the layout file exists then add it to the Layout list
                string layoutfile = Path.Combine(SimLinkConstants.USER_SETTINGS_FOLDER, str + "_panel_layout.xml");
                if (File.Exists(layoutfile))
                {
                    lbLayouts.Items.Add(str);

                    if (str.Equals("Default"))
                        defaultFound = true;
                }
            }
            // make sure there is always a "Default" option in the layout list
            if (defaultFound == false)
            {
                lbLayouts.Items.Insert(0, "Default");
                _MainForm.SaveLayout("Default");
            }

            // select the current layout in the list
            int index = lbLayouts.FindString(Settings.Default.CurrentLayout);
            lbLayouts.SetSelected(index, true);

            lbLayouts.EndUpdate();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Saves the list of layouts to the user config.
        /// </summary>
        private void SaveLayoutListToUserConfig()
        {
            // save the layout list to the user settings
            Settings.Default.LayoutList.Clear();

            foreach (string str in lbLayouts.Items)
            {
                // if the file exists then save it to the LayoutList in settings
                string layoutfile = Path.Combine(SimLinkConstants.USER_SETTINGS_FOLDER, str + "_panel_layout.xml");
                if (File.Exists(layoutfile))
                    if (Settings.Default.LayoutList.Contains(str) == false)
                        Settings.Default.LayoutList.Add(str);
            }

            // Save settings
            Settings.Default.Save();
        }

        /// <summary>
        /// deletes the currently selected layout from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteLayout_Click(object sender, EventArgs e)
        {
            var layout = lbLayouts.Items[lbLayouts.SelectedIndex];
            if (layout.Equals("Default"))
            {
                MessageBox.Show("The Default Layout cannot be deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show(String.Format("Are you sure you want to delete layout: {0}?", layout.ToString()), "Delete Layout", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    lbLayouts.Items.RemoveAt(lbLayouts.SelectedIndex);
                    _MainForm.DeleteLayout(layout.ToString());
                }
            }
        }

        private void fmLayoutManager_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void fmLayoutManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveLayoutListToUserConfig();
        }

        private void lbLayouts_DoubleClick(object sender, EventArgs e)
        {
            var layout = lbLayouts.Items[lbLayouts.SelectedIndex];
            _MainForm.RestoreLayout(layout.ToString());

        }

        private void btnSaveLayout_Click(object sender, EventArgs e)
        {
            var layout = lbLayouts.Items[lbLayouts.SelectedIndex];
            _MainForm.SaveLayout(layout.ToString());
        }

        private void lbLayouts_SelectedIndexChanged(object sender, EventArgs e)
        {
 
        }

        /// <summary>
        /// Show a prompt dialog to capture input from the user. 
        /// </summary>
        /// <param name="text">Prompt to ask the user for input</param>
        /// <param name="caption">Caption of the prompt dialog</param>
        /// <param name="defaultText">The default text for the user</param>
        /// <returns>The input text entered by the user</returns>
        /// <example>
        ///     <code>
        ///     string name = ShowPrompt("Please enter your name...", "Enter Name", "Default Name");
        ///     </code>
        /// </example>
        public static string ShowPrompt(string text, string caption, string defaultText)
        {
            DevExpress.XtraEditors.XtraForm prompt = new DevExpress.XtraEditors.XtraForm();
            prompt.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            prompt.Width = 300;
            prompt.Height = 120;
            prompt.Text = caption;
            prompt.StartPosition = FormStartPosition.CenterScreen;
            Label textLabel = new Label() { Left = 20, Top = 10, Width = 250, Text = text };
            TextBox textBox = new TextBox() { Left = 20, Top = 30, Width = 250 };
            Button confirmation = new Button() { Text = "Ok", Left = 170, Width = 100, Top = 55 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            if (defaultText.Trim() != string.Empty)
                textBox.Text = defaultText;
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog();
            return textBox.Text;
        }

        /// <summary>
        /// Prompts the use to add a new layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewLayout_Click(object sender, EventArgs e)
        {
            string newLayout = ShowPrompt("Please enter the name of new layout...", "New Layout", Settings.Default.CurrentLayout);
            if (newLayout.Trim() != string.Empty)
            {
                _MainForm.SaveLayout(newLayout.Trim());

                RefreshLayoutsList();
            }
        }

        private void lbLayouts_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }
    }
}