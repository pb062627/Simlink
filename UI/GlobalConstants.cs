using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_API_LINKS
{
    /// <summary>
    /// Constant/static values valid for all CH2M Software
    /// Changing the values here could affect a lot of CH2M Software. Please make sure you are aware of the ramifications
    /// </summary>
    public static class GlobalConstants
    {
        /// <summary>
        /// This is the company name as used for folders, files etc. as opposed to in the UI
        /// In the UI we might want to translate, but for files/folders we don't
        /// </summary>
        public const string CompanyNameFileFolder = "CH2M Hill";

        public static readonly string CompanyUserApplicationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CompanyNameFileFolder);

        //public static readonly string CompanyCommonApplicationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CompanyNameFileFolder);

        public static readonly string CompanyCommonDocumentsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), CompanyNameFileFolder);
        public static readonly string AppDatFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"SimLink\"); //.Replace(Application.ProductVersion, "ISISMapper");
        public static readonly string QuickAccessBarXML = AppDatFolder + "qatoolbox.xml";


        public static string GetFileVersion(string file)
        {
            string ver = "";
            //MessageBox.Show(file);
            if (File.Exists(file) && file.ToLower().EndsWith(".exe"))
            {

                var info = FileVersionInfo.GetVersionInfo(file);
                ver = info.FileVersion;
            }
            return ver;
        }
    }
}
