using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH2M.SimLink.MainUI;
using System.Windows.Forms;
using DevExpress.LookAndFeel;

namespace SIM_API_LINKS
{
    class Program
    {
        private static string sSQL_CONN_NH = ("Data Source=HCHHL5JSN1;"            //ALIAS\\SQLEXPRESS;"
        + "User ID=simlink_user;"
        + "Password=admin;"
        + "Initial Catalog = SimLink2_NewHaven;"
        + "Integrated Security=True;");
        [STAThread()]
        static void Main(string[] args)
        {
            //simlink simlink = new simlink();
            //simlink.InitializeModelLinkage(sSQL_CONN_NH, 1, false);
            //simlink.CreateProject(1, "TEST", @"C:\a\test\file.txt", "INFORMATION");
            //simlink.CloseModelLinkage();
            //simlink.LoadProject();
            /*
            simlink simlink = new simlink();
            simlink.InitializeModelLinkage(sSQL_CONN_NH, 1, false);
            simlink.CreateProject(1, "TEST", @"C:\a\test\file.txt", "INFORMATION");
            simlink.CloseModelLinkage();
             */
            try
            {
                Application.SetCompatibleTextRenderingDefault(false);

                DevExpress.Skins.SkinManager.EnableFormSkins();

                UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");
                Application.Run(new frmSimLinkMain());
            }
            catch(Exception ex)
            {
                CH2M.Commons.ShowMessage("Application has detected the error '" + ex.Source + ": " + ex.Message + "'");
            }
        }
    }
}
