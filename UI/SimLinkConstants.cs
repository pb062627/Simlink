using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_API_LINKS
{
    public class SimLinkConstants
    {
        public const string DOCKING_LAYOUT_FILENAME_ONLY = "simlink_docking_layout.xml";
        public const string RIBBON_LAYOUT_FILENAME_ONLY = "simlink_default_ribbon_layout.xml";
        //public const string DOCKING_LAYOUT_FILENAME_ONLY = "simlink_default_panel_layout.xml";
        public const string SIMLINK_FOLDER = "SimLink";

        public static readonly string HydraUserApplicationFolder = Path.Combine(GlobalConstants.CompanyUserApplicationFolder, SIMLINK_FOLDER);
        public static readonly string USER_SETTINGS_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GlobalConstants.CompanyNameFileFolder + "\\" +  @"SIMLINK\");
        public static readonly string DockLayoutFilename = Path.Combine(HydraUserApplicationFolder, DOCKING_LAYOUT_FILENAME_ONLY);
        public static readonly string DEFAULT_RIBBON_LAYOUT_FILE = Path.Combine(USER_SETTINGS_FOLDER, RIBBON_LAYOUT_FILENAME_ONLY);
        public static readonly string DEFAULT_DOCKING_LAYOUT_FILE = Path.Combine(USER_SETTINGS_FOLDER, DOCKING_LAYOUT_FILENAME_ONLY);

    }
}
