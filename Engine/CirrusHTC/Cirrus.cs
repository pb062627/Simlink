using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Data.OleDb;
using System.Data;
//using CirrusHTC;



namespace CIRRUS_HTC_NS
{
    public class Cirrus
    {
        public static string sCIR_DIR_Watch;
        public static FileSystemWatcher fswCirrusInputWatcher;              //check when a new file(s) loaded
        public static FileSystemWatcher fswCirrusOutputWatcher;
        public OleDbConnection connCirrus;                              //probably don't need the database in master; it is the job that uses
        public string sCONN_Cirrus;
        private CIRRUS_HTC htc;
        private string sActiveCirrusDir=@"C:\Users\mthroneb\Documents\Optimization\Cirrus\JobCenter\";         //directory the active cirrus engine is preparing jobs for condor in; the htc job gets a unique slot in there.
        private CommonUtilities cu = new CommonUtilities();

       

        public void InitCirrus(bool bIsDebugging, string sActiveDir = "NOTHING")
        {
          //  CirrusReadConfig();                                             //get parameters from .config
  
            connCirrus = new OleDbConnection(sCONN_Cirrus);
            connCirrus.Open();

            if (sActiveDir == "NOTHING") { sActiveCirrusDir = sActiveDir; }         

            if (!bIsDebugging)
            {
                fswCirrusInputWatcher = new FileSystemWatcher();
                fswCirrusInputWatcher.IncludeSubdirectories = true;
                fswCirrusInputWatcher.Path = sCIR_DIR_Watch;
                fswCirrusInputWatcher.Created += new FileSystemEventHandler(fswCirrusInputWatcher_Created);
                fswCirrusInputWatcher.EnableRaisingEvents = true;
            }
        }
        
        public void fswCirrusInputWatcher_CHEATINGCHEATING(string sPath, int nMasterID)
        {
            if (true)                           //user settings will be defined
            {
                if (fswHELPER_IsCirrusTypeFile(sPath))
                {
                    htc = new CIRRUS_HTC_NS.CIRRUS_HTC();
                    htc.InitHTC_Vars(sActiveCirrusDir,sPath,-1, false,1);
                    htc.HTC_Submit();
                }
            }
        }
         public void fswCirrusInputWatcher_Created(object sender, FileSystemEventArgs e)
        {
            int i = 0;
            i += 1;

            if (true)                           //user settings will be defined
            {
                if (fswHELPER_IsCirrusTypeFile(e.FullPath))
                {
                    htc = new CIRRUS_HTC_NS.CIRRUS_HTC();
                    htc.InitHTC_Vars(sActiveCirrusDir, e.FullPath, -1, false, 1);
                    htc.HTC_Submit();
                }
            }
        }


        //return whether the file provide is a cirrus type file.
         private bool fswHELPER_IsCirrusTypeFile(string sPath)
        {
            if (Path.GetExtension(sPath).ToUpper() == ".CIR") { return true; }
            if (sPath.Length > 7)
            {
                if (sPath.Substring(sPath.Length - 8, 8).ToUpper() == "CIR.XLSM")
                {
                    return true;
                }
            }
            return false;
        }

    }

}
