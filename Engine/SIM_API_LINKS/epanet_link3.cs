using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Data.OleDb;
using System.Web;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using EPANET_working2;

namespace SIM_API_Links
{
    public class epanet_link
    {
        public static SIM_API_Links.CommonUtilities cu = new SIM_API_Links.CommonUtilities();
        private bool _bSimCondor = false;                // if true, execute model run as a condor step                      the only condor step that needs to happen within SWMM class is       dSWMM_CondorStep = -1;                //-1: no condor  ; otherwise, where does condor step fit in to process
        //private CIRRUS_HTC_NS.CIRRUS_HTC _htc;
        private const int _nActiveModelID_EPANET= 3;           
        private hdf5_wrap _hdf5_SWMM = new hdf5_wrap();

        public static OleDbConnection _connMod;
        public static OleDbConnection _connRMG;
        public static SIM_API_Links.rmgDB_link _rmgDB_link = new SIM_API_Links.rmgDB_link();


        public void InitEPANET_Class(string sconnMod, string sConnRMG, bool bRunCondor = false)
        {
            _connMod = new OleDbConnection(sconnMod);
            _connRMG = new OleDbConnection(sConnRMG);

            _connMod.Open();
            _connRMG.Open();
            _bSimCondor = bRunCondor;

            _rmgDB_link.InitRMG_Class(sConnRMG, sconnMod, 3);           //met 5/22/2013: need to figure out relationship between rmg and base class.
        }

        public void CloseEPANET_Close()
        {
            _connMod.Close();
            _connRMG.Close();
            _connMod.Dispose();
            _connRMG.Dispose();

           _rmgDB_link.CloseRMG_Class();        //met 5/22/2013: need to figure out relationship between rmg and base class..
        }

        public void epanetProcess_Eval(int nEvalID)                                                                             //met 5/22/2013: test concept; not decided whether rmg or specific linkages are driving...
        {                                                                                                                       //epanet linkage is a test case for linkage driving...
            _rmgDB_link.rmgProcessEval(nEvalID, _nActiveModelID_EPANET, nEvalID,true);
        }

        public void EPANET_ImportNameValuePair(string sNameValPair, ref DataRow dr)                                             // takes a pair of valus, and puts the second into the column named the first of a datarow.
        {
            if (sNameValPair.Length > 3)       //minimum possible entry
            {
                string sFieldName; string sVal;
                sFieldName = sNameValPair.Substring(0, sNameValPair.IndexOf(" "));
                if (sFieldName.Substring(sFieldName.Length - 1, 1) == ":")
                {
                    sFieldName = sFieldName.Substring(0, sFieldName.Length - 1);        //drop the : in Scenario: EPANET
                }


                sVal = sNameValPair.Substring(sNameValPair.IndexOf(" ") + 1, sNameValPair.Length - sNameValPair.IndexOf(" ") - 1).Trim();
                dr[sFieldName] = sVal;
            }
        }

        public void EPANET_ReadINP_ToDB(string FileName, int nScenario, int nProjID, string sConnRMG)
        {
            if (File.Exists(FileName))
            {
                StreamReader file = null;
                string sFirstChar = ""; string sbuf = ""; string sConcat = "";

                sConnRMG = cu.getModelSpecificConnectionString(5);

                using (OleDbConnection conn = new OleDbConnection(sConnRMG))
                {
                    conn.Open();
                    DataSet dsRS = new DataSet();
                    OleDbDataAdapter daRS = cu.SWMM_GetTableByElementType(ref dsRS, "tblEPANET_RunSettings", "Empty", -1, -1, conn);
                    DataRow rowRunSettings = dsRS.Tables[0].NewRow();
                    try
                    {
                        file = new StreamReader(FileName);
                        while (!file.EndOfStream)
                        {
                            if (sFirstChar != "[")
                            {
                                sbuf = file.ReadLine();
                            }
                            //test for special cases which must be handled uniquely
                            if (sbuf == "[TITLE]" || sbuf == "[OPTIONS]" || sbuf == "[REPORT]" || sbuf == "[TAGS]" || sbuf == "[MAP]")
                            {
                                sFirstChar = "X";
                                sConcat = "";
                                switch (sbuf)
                                {
                                    case "[TITLE]":
                                        while (sFirstChar != "[")
                                        {
                                            int jj = 0; //dummy;do better
                                            sbuf = file.ReadLine();
                                            if ((sbuf.Length > 0) && (sbuf.Substring(0,1)=="S"))  
                                            {
                                                EPANET_ImportNameValuePair(sbuf, ref rowRunSettings);
                                                jj = 100;
                                                //sConcat = sConcat + sbuf + "\n"; 
                                            }    //add the title string into a single str
                                            sFirstChar = cu.GetFirstChar(sbuf);
                                        }
                               //         rowRunSettings["Title"] = sConcat;
                                        break;

                                    case "[REPORT]":
                                        //allow code to execute through to Options; same processing
                                        while (sFirstChar != "[")
                                        {
                                            sbuf = file.ReadLine();
                                            sFirstChar = cu.GetFirstChar(sbuf);
                                            if (sbuf.Trim().Length > 2 & sFirstChar != "[")
                                            {
                                                EPANET_ImportNameValuePair(sbuf, ref rowRunSettings);
                                            }
                                        }

                                        break;
                                    case "[OPTIONS]":
                                        while (sFirstChar != "[")
                                        {
                                            sbuf = file.ReadLine();
                                            sFirstChar = cu.GetFirstChar(sbuf);
                                            if (sbuf.Trim().Length > 2 & sFirstChar != "[")
                                            {
                                                EPANET_ImportNameValuePair(sbuf, ref rowRunSettings);
                                            }
                                        }

                                        break;
                                    case "[TAGS]":
                                        break;
                                    case "[MAP]": //parse this @#$@#$ing line to get the coordinates
                                        sbuf = file.ReadLine();
                                        sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" ")).Trim();
                                        rowRunSettings["MAP_X1"] = sbuf.Substring(0, sbuf.IndexOf(" "));
                                        sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" ")).Trim();
                                        rowRunSettings["MAP_Y1"] = sbuf.Substring(0, sbuf.IndexOf(" "));
                                        sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" ")).Trim();
                                        rowRunSettings["MAP_X2"] = sbuf.Substring(0, sbuf.IndexOf(" "));
                                        sbuf = sbuf.Substring(sbuf.IndexOf(" "), sbuf.Length - sbuf.IndexOf(" ")).Trim();
                                        rowRunSettings["MAP_Y2"] = sbuf;
                                        sbuf = file.ReadLine();
                                        EPANET_ImportNameValuePair(sbuf, ref rowRunSettings);
                                        sbuf = file.ReadLine();
                                        break;
                                }
                            }
                            else
                            {
                                if (sbuf.Length > 0)
                                {
                                    DataSet ds = new DataSet();
                                    char[] nogood = { '[', ']', ';', 'Z', '\t', ' ' };
                                    string sTableName = "tblEPANET_" + sbuf.TrimStart(nogood).TrimEnd(nogood);
                                    OleDbDataAdapter da = cu.SWMM_GetTableByElementType(ref ds, sTableName, "Empty", -1, -1, conn);
                                    sFirstChar = "X";
                                    while ((sFirstChar != "[") & !file.EndOfStream)
                                    {
                                        sbuf = file.ReadLine();
                                        sbuf = sbuf.Trim();
                                        sFirstChar = cu.GetFirstChar(sbuf);

                                        if (sbuf.Length <= 1 || sFirstChar == ";" || sFirstChar == "[")
                                        {
                                            //do nothing
                                        }
                                        else
                                        {
                                            //SWMM_ELEMENT_FillDT(ref ds.Tables[0], sbuf);

                                            // read in the SWMM Section Block into the proper table
                                            //TODO: This should be tightened up to use the tlkpFieldDictoinary, not assuming columns are in right order! for now it works.
                                            string sEntry; int i = 0;
                                            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());       //add a new row to the datatable
                                            while (sbuf.Length > 0)
                                            {

                                                i = i + 1;  //increment (skipping the first ID field
                                                if (sbuf.IndexOf(" ") > 0)
                                                {
                                                    int nIndex = sbuf.IndexOf(" ");
                                                    int nIndex2 = sbuf.IndexOf("\t");
                                                    if ((nIndex2 > 0) && (nIndex2 < nIndex))    //if there is a tab space instead of a space, use that.
                                                    {
                                                        nIndex = nIndex2;
                                                    }
                                                    sEntry = sbuf.Substring(0, nIndex);
                                                    sbuf = sbuf.Substring(nIndex + 1, sbuf.Length - nIndex - 1).Trim();
                                                    sbuf = sbuf.TrimStart(nogood);
                                                }
                                                else
                                                {
                                                    sEntry = sbuf;
                                                    sbuf = "";
                                                }
                                                ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1][i] = sEntry;
                                                ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["ModelVersion"] = nScenario;
                                            }//end inner while: decoding the sbuf string
                                        }
                                    }//end while
                                    da.InsertCommand = new OleDbCommandBuilder(da).GetInsertCommand();
                                    da.Update(ds);
                                }
                            }//end else

                        }   //end while    
                        rowRunSettings["ModelVersion"] = nScenario;         //now finalize the creation of the Runsetting record specific to this SWMM file
                        daRS.InsertCommand = new OleDbCommandBuilder(daRS).GetInsertCommand();
                        daRS.Update(dsRS);

                        //create lookup tables for the appropriate columns
                        cu.insertOptionList_MODEL_Lookups(nScenario, 3, nProjID, conn,conn);
                    }
                    finally
                    {
                        if (file != null)
                            file.Close();
                    }

                }

            }
        }

        public void EPANET_Update_INP(string sIncomingINP, int nScenarioID, string sTarget_INP, string sTarget_INP_temp, string sSummaryFile)
        {

            //string[] sTextFile_ALL = File.ReadAllLines(sIncomingINP);

            //string sql = "SELECT ElementName, Val, ScenarioID_FK, TableName, FieldName, SectionNumber, SectionName, FieldINP_Number, ElementID, KeyColumn, TableFieldKey_FK, DV_ID_FK, IsScenarioSpecific, RowNo, Qualifier1, Qualifier1Pos, IsInsertFeature FROM qryRMG001_EPANET_ModelChanges WHERE (((ScenarioID_FK)=@Scenario)) ORDER BY SectionNumber, ElementName, Model_ID;";                     //BYM2012
            //string sql = "SELECT ElementName, Val, ScenarioID_FK, TableName, FieldName, SectionNumber, SectionName, FieldINP_Number, ElementID, KeyColumn, TableFieldKey_FK, DV_ID_FK, IsScenarioSpecific, RowNo, Qualifier1, Qualifier1Pos, IsInsertFeature FROM qryRMG001_EPANET_ModelChanges WHERE (((ScenarioID_FK)=" + nScenarioID + ")) ORDER BY SectionNumber, ElementName, Model_ID;";                     //CM 05/2013
            string sql = "SELECT* FROM qryRMG001_EPANET_ModelChanges WHERE (((ScenarioID_FK)=" + nScenarioID + ")) ORDER By SectionNumber, ElementName, Model_ID;";                     //CM 06/2013  

            OleDbCommand cmd = new OleDbCommand(sql, _connRMG);
            cmd.Parameters.Add("@Scenario", OleDbType.Integer).Value = nScenarioID;

            OleDbDataReader drModVals = cmd.ExecuteReader();

            DataTable oTable = new DataTable();
            DataSet dsRMG_Changes = new DataSet();
            dsRMG_Changes.Tables.Add(oTable);
            dsRMG_Changes.Tables[0].Load(drModVals);

            System.IO.File.Copy(sIncomingINP, sTarget_INP_temp);

            //EpanetLibrary.ENopen(sIncomingINP, sSummaryFile, "");
            //EpanetLibrary.ENsaveinpfile(sTarget_INP_temp);
            //EpanetLibrary.ENclose();

            //string[] sTextFile_ALL = File.ReadAllLines(sTarget_INP_temp);

            foreach (DataRow row in oTable.Rows)
            {
                int _nScenarioID = Convert.ToInt32(row["ScenarioID_FK"].ToString());
                int _nTableFieldKey_FK = Convert.ToInt32(row["TableFieldKey_FK"].ToString());
                string _sID = row["ElementName"].ToString();
                string _sFieldName = row["FieldName"].ToString();
                int _nVal = Convert.ToInt32(row["Val"].ToString());
                int _nVarTypeNum = Convert.ToInt32(row["VariableTypeNum"].ToString());
                bool APIModify = Convert.ToBoolean(row["API_Update"].ToString());
                int _nCLibInt = Convert.ToInt32(row["EPANET_CSharp_LibInt"].ToString());

                Console.WriteLine("Scenario: "+_nScenarioID);
                Console.WriteLine("Element: "+_sID);
                Console.WriteLine("Variable: "+_sFieldName);
                Console.WriteLine("UpVal: "+_nVal);
                Console.WriteLine("API: " + APIModify);
                Console.WriteLine("LibInt: " + _nCLibInt);

                #region API UPDATES

                if (APIModify == true)
                {
                    ////Swithch statement for variable type number: 1 = nodes, 2 = links, 3 = ...

                    EpanetLibrary.ENopen(sTarget_INP_temp, sSummaryFile, "");  // todo: find cleaner way to write with API. currently use temporary file to prevent problem with features not showing up in epanet  cm 6/18/2013
                    int index = 0;
                    float value = 1;
                    switch (_nVarTypeNum)
                    {
                        case (1): // case 1 nodes
                            EpanetLibrary.ENgetnodeindex(_sID, ref index);
                            EpanetLibrary.ENgetnodevalue(index, _nCLibInt, ref value);
                            EpanetLibrary.ENsetnodevalue(index, _nCLibInt, _nVal);
                            break;

                        case (2): // case 2 links
                            EpanetLibrary.ENgetlinkindex(_sID, ref index);
                            EpanetLibrary.ENgetlinkvalue(index, _nCLibInt, ref value);
                            EpanetLibrary.ENsetlinkvalue(index, _nCLibInt, _nVal);
                            break;

                        case (3): // case 3...
                            break;

                        default:
                            break;
                    }

                    long t = 0;

                    Console.WriteLine("VarType: " + _nVarTypeNum);
                    Console.WriteLine("Index: " + index);
                    Console.WriteLine("CurrentVal: " + value);
                    Console.WriteLine();

                    EpanetLibrary.ENsaveinpfile(sTarget_INP);
                    EpanetLibrary.ENclose();

                    EpanetLibrary.ENopen(sTarget_INP, sSummaryFile, "");
                    //EpanetLibrary.ENrunH(ref t);
                    //EpanetLibrary.ENreport();
                    EpanetLibrary.ENsaveinpfile(sTarget_INP_temp);
                    EpanetLibrary.ENclose();


                }
                #endregion

                #region NON-API UPDATES

                else
                {
                    //// adapted from swmm5022_link.cs  06/17/2013 CM
                    
                    string[] sTextFile_ALL = File.ReadAllLines(sTarget_INP_temp);
                    
                    int nListOffset = 0;                   //used to keep track of the insert position. a value >=0 indicates that there was at LEAST one insert
                    //
                    List<string> listTextFile_ALL = new List<string>();
                    for (int i = 0; i <= 1; i++)
                    {         //loop through twice

                        //initialize variables for each loop
                        bool bIsInsert = false; bool bInCurrentSection = false;
                        if (i == 0) { bIsInsert = true; } else { bIsInsert = false; }       //first loop is insert
                        string sCurrentSectionName = "none"; string sCheckSectionName = ""; string sCurrentElementName = "none";
                        int nCurrentWriteLine = 0;
                        int nFileTotalRows; int nCurrentChange = 0;
                        int nTotalChanges = dsRMG_Changes.Tables[0].Rows.Count;
                        int nSectionLine = 0; //BYM2012


                        Debug.Print("Total Changes: " + nTotalChanges);
                        //Console.WriteLine("Total Changes: " + nTotalChanges);

                        if ((!bIsInsert) && (nListOffset > 0))             //update loop, and at lest one insert; need to convert list back
                        {
                            sTextFile_ALL = listTextFile_ALL.ToArray();         //
                        }
                        nFileTotalRows = sTextFile_ALL.Length;
                        while (nCurrentChange < nTotalChanges)
                        {
                            Debug.Print("Begin: " + nCurrentChange);
                            //Console.WriteLine("CurrentChange: " + nCurrentChange);
                            //met 1/2/2013: check that we do ONLY inserts on inserts loop and ONLY updates on update loop
                            if (bIsInsert == Convert.ToBoolean(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["IsInsertFeature"]))
                            {

                                //MET 4/13/2012: 
                                if (Convert.ToInt32(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["TableFieldKey_FK"].ToString()) == -1)        //perform insert (don't go through all the "section name" stuff
                                {
                                    cu.cuFILE_InsertTextChars(ref sTextFile_ALL, Convert.ToInt32(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["Val"].ToString()), ref _connRMG);
                                    nCurrentChange++;
                                    if (nCurrentChange == nTotalChanges) { break; }    // no more changes, so break out of loop
                                    Debug.Print(nScenarioID + ": " + nCurrentChange);      //met 3/1/2012   figure out why the file is sometimes not getting written.

                                }
                                else                  //general case. standard update being performed
                                {
                                    sCurrentSectionName = cu.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");                                            //BYM   
                                    while ((sCurrentSectionName != dsRMG_Changes.Tables[0].Rows[nCurrentChange]["SectionName"].ToString()) && (nCurrentWriteLine < nFileTotalRows))
                                    {
                                        //sCurrentSectionName = MOUSE_CheckSectionName(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName);                                                      //BYM
                                        sCurrentSectionName = cu.cuFile_CheckCurrentFileSection(sTextFile_ALL[nCurrentWriteLine], sCurrentSectionName, "[");                                        //BYM
                                        nCurrentWriteLine++;
                                        nSectionLine = nCurrentWriteLine;
                                        bInCurrentSection = false;
                                    }
                                    if (sCurrentSectionName == dsRMG_Changes.Tables[0].Rows[nCurrentChange]["SectionName"].ToString())
                                    {
                                        if (bIsInsert)      //don't need to find right location; insert now  (new 1/2/2013)
                                        {
                                            if (nListOffset == 0)
                                            {
                                                listTextFile_ALL = sTextFile_ALL.ToList();  //create the list. we only want to do this if needed, because it takes considerable time.
                                            }
                                            if (!bInCurrentSection)         //first time we navigate to new section
                                            {                               //only advance below until we leave section.
                                                HELPER_EPANET_AdvanceToData(ref sTextFile_ALL, ref nCurrentWriteLine);     //advance to data section (some have 2 header, some 3- this is easiest.
                                                bInCurrentSection = true;
                                            }

                                            listTextFile_ALL.Insert(nCurrentWriteLine + nListOffset + 1, dsRMG_Changes.Tables[0].Rows[nCurrentChange]["Val"].ToString());         //met: off by one: nCurrentWriteLine + nListOffset-1,
                                            //   nCurrentWriteLine++;                      met: think this is not needed. offset handles
                                            nCurrentChange++;
                                            nListOffset++;              //this counter keeps track of how many additional inserts there are

                                        }
                                        else
                                        {                   //case : update loopo.
       
                                            bInCurrentSection = true;

                                            if (sTextFile_ALL[nCurrentWriteLine].IndexOf(" ") > 0)                   //check whether we have data row
                                            {

                                                string sIDName = "";

                                                //TODO:!!  need to store the position of the SectionName. This allows
                                                //there is a chance it may work right now, but it should not be considered implemented


                                                //test for whether we are using a "system/scenario specific var (eg option table)
                                                //or (more standard) an element name.
                                                bool bIsScenarioLevelVar = false; string sFindElementNameOrField;
                                                sFindElementNameOrField = EPANET_INP_Helper_GetElementNameOrField(dsRMG_Changes.Tables[0].Rows[nCurrentChange], ref bIsScenarioLevelVar);

                                                sIDName = HELPER_EPANET_AdvanceToCurrent_ID(ref sTextFile_ALL, sFindElementNameOrField, ref nCurrentWriteLine, nSectionLine, sCurrentSectionName); //BYM
                                                Debug.Print("Found CurrentID: " + nCurrentChange);

                                                if (sIDName != "No_ID_Found")
                                                {
                                                    //BYM string[] sbufDATA = sTextFile_ALL[nCurrentWriteLine].Split(',');
                                                    string[] sbufDATA = sTextFile_ALL[nCurrentWriteLine].Trim().Split(' ');             //met 4/18/2013 drop leading zero

                                                    int nLastRow = 1;                                                                      //met 6/18/2012: modified to support multiple row values. Val of 1 is the default.
                                                    int nCurrentRow = 1;

                                                    //met 1/4/2013: modified to check thta the next ID has the right type.
                                                    while ((sIDName == EPANET_INP_Helper_GetElementNameOrField(dsRMG_Changes.Tables[0].Rows[nCurrentChange], ref bIsScenarioLevelVar)) && (bIsInsert == Convert.ToBoolean(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["IsInsertFeature"])))
                                                    {
                                                        bool bCorrectRow = true;

                                                        //LID USAGE- can be multiple different types on a single subcatchment
                                                        if (dsRMG_Changes.Tables[0].Rows[nCurrentChange]["Qualifier1"].ToString() != "-1")
                                                        {

                                                            int nCol_Qual = Convert.ToInt32(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["Qualifier1POS"].ToString());

                                                            if (nCol_Qual > -1)
                                                            {

                                                                bCorrectRow = false;
                                                                while (!bCorrectRow && sbufDATA[0] == sIDName)
                                                                {
                                                                    if (sbufDATA[nCol_Qual] == dsRMG_Changes.Tables[0].Rows[nCurrentChange]["Qualifier1"].ToString())
                                                                    {
                                                                        bCorrectRow = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        nCurrentWriteLine++;
                                                                        sbufDATA = cu.cuRemoveRepeating(sTextFile_ALL[nCurrentWriteLine]).Split(' ');
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                rmgDB_link.cu.cuLogging_AddString("Potential error- DV has Qualifer but does not define column position", cu.nLogging_Level_2);
                                                            }
                                                        }


                                                        if (bCorrectRow)
                                                        {
                                                            //nCurrentRow = Convert.ToInt32(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["RowNo"].ToString());  // Assign correct RowNo's. Values temporarily assigned in tlkpEPANETFieldDictionary
                                                            //Console.WriteLine("CurrentChange: " + nCurrentChange);
                                                            if (nCurrentRow > nLastRow)                                                         // if this is true, we need to get the new row.
                                                            {
                                                                nCurrentWriteLine += nCurrentRow - nLastRow;
                                                                sbufDATA = cu.cuRemoveRepeating(sTextFile_ALL[nCurrentWriteLine]).Split(' ');                      //remove the repeating spaces so the split works (occurs in HELPER_SWMM_AdvanceToCurrent_ID for normal workflow)
                                                            }

                                                            int index2 = Convert.ToInt32(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["FieldINP_Number"].ToString());
                                                            sbufDATA[index2 - 1] = dsRMG_Changes.Tables[0].Rows[nCurrentChange]["Val"].ToString();
                                                            nCurrentChange++;

                                                            if (nCurrentChange == nTotalChanges) { break; }    // no more changes, so break out of loop

                                                            Debug.Print(nScenarioID + ": " + nCurrentChange);      //met 3/1/2012   figure out why the file is sometimes not getting written.
                                                        }


                                                    }
                                                    //BYM sTextFile_ALL[nCurrentWriteLine] = String.Join(",", sbufDATA);
                                                    sTextFile_ALL[nCurrentWriteLine] = String.Join(" ", sbufDATA);
                                                    
                                                }
                                                else
                                                {
                                                    nCurrentChange++;  //nothing found- move on to the next change.  TODO L log this change
                                                }
                                            }
                                            else
                                            {
                                                nCurrentWriteLine++;
                                            }
                                        }
                                    }
                                    else                                       //didn't find the proper section name
                                    {
                                        Debug.Print("MODEL CHANGE NOT TRANSLATED TO INP");
                                        Console.WriteLine("MODEL CHANGE NOT TRANSLATED TO INP");
                                        nCurrentChange++;       //met 5/17/2012 skip over to avoid infinite loop. this most likely happens if tblMEV has multiple elements...
                                    }
                                }       //end else
                            }
                            else
                            {               //end if   if (bIsInsert == Convert.ToBoolean(dsRMG_Changes.Tables[0].Rows[nCurrentChange]["IsInsertFeature"]))
                                nCurrentChange++;       //change not applicable for current loop (insert/update)
                            }
                        }
                    }
                    //standard update changes are made at this point. now we need to back and insert any values.
                    //               HELPER_Mouse_InsertNewElementFeatures(ref sTextFile_ALL, nScenarioID);

                    //string sOUT;
                    //if (sOptionalOutput_TextFile == "nothing")
                    //{
                    //    sOUT = sIncomingINP;
                    //}
                    //else
                    //{
                    //    sOUT = sOptionalOutput_TextFile;

                    //}

                    Debug.Print("Write: " + sTarget_INP);
                    //Console.WriteLine("Write: " + sTarget_INP);
                    Console.WriteLine();
                    File.WriteAllLines(sTarget_INP_temp, sTextFile_ALL);              //overwrite the file initially passed

                }
                #endregion
            }

            EpanetLibrary.ENopen(sTarget_INP_temp, sSummaryFile, "");
            EpanetLibrary.ENsaveinpfile(sTarget_INP);
            EpanetLibrary.ENclose();


        }



        //public void EPANET_ReadRPT_IntoDB(string sRPTfile, int nEvalID, int nScenarioID)
        //{
        //    if (File.Exists(sRPTfile))
        //    {
        //        StreamReader fileINP = null;
        //        try
        //        {
        //            string[] sResults_RPT = File.ReadAllLines(sRPTfile);

        //            string sql = "SELECT Result_ID, Element_Label, Result_Label, VarResultType_FK, FeatureType, FieldName, TableName, SectionNumber, ColumnNo, EvaluationGroupID, ElementID_FK FROM qryRMG_Result001_SWMM_LinkInfo "
        //            + "WHERE (((EvaluationGroupID)=@nEvalID)) ORDER BY SectionNumber, ColumnNo, Element_Label;";

        //            OleDbCommand cmd = new OleDbCommand(sql, connRMG);
        //            cmd.Parameters.Add("@nEvalID", OleDbType.Integer).Value = nEvalID;
        //            OleDbDataReader drModVals = cmd.ExecuteReader();
        //            DataTable oTable = new DataTable();
        //            DataSet dsRMG_Changes = new DataSet();
        //            dsRMG_Changes.Tables.Add(oTable);
        //            dsRMG_Changes.Tables[0].Load(drModVals);

        //            int nResults = dsRMG_Changes.Tables[0].Rows.Count;

        //            string[] sVals = new string[nResults];
        //            int[] nIDs = new int[nResults];                     //MET 1/15/2011
        //            int count2 = 0;
        //            bool bIsCorrectResultSection = false; bool bIsSummaryResult;
        //            int nCurrentFilePosition = 135;      //skip header bullshit. probably should advance through file for this.
        //            string sCurrentTableName; string sCurrentElementName; int nCurrentResultCol;
        //            int nDataVals = 0;
        //            string sLastTableName = "set in code below";
        //            foreach (DataRow dr in dsRMG_Changes.Tables[0].Rows)                                    //now populate the  counter that will tell us how to read the file in...?
        //            {
        //                sCurrentTableName = dr["TableName"].ToString();

        //                if (dr["FeatureType"].ToString() != "System")             //met 4/16/2012: 
        //                {
        //                    sCurrentElementName = dr["Element_Label"].ToString();       //typical case
        //                    bIsSummaryResult = false;
        //                }
        //                else
        //                {
        //                    sCurrentElementName = dr["FieldName"].ToString();           //find the field name for summmary variables. 
        //                    bIsSummaryResult = true;
        //                }
        //                nCurrentResultCol = Convert.ToInt32(dr["ColumnNo"].ToString());              //Column numbers are indexed from 1 (in the tlkp KEY)
        //                if (sCurrentTableName != sLastTableName)
        //                {
        //                    nCurrentFilePosition = HELPER_SWMMResults_GetTablePosition(sResults_RPT, sCurrentTableName, nCurrentFilePosition);
        //                    bIsCorrectResultSection = true;
        //                }

        //                if (HELPER_SWMMResults_FindElementName(sResults_RPT, sCurrentElementName, sCurrentTableName, ref nCurrentFilePosition, bIsSummaryResult))
        //                {      //this function loops to the actual data entry
        //                    sVals[nDataVals] = HELPER_SWMMResults_GetResultVal(sResults_RPT[nCurrentFilePosition], nCurrentResultCol);

        //                    if (string.IsNullOrEmpty(dr["ElementID_FK"].ToString()))        //test for null (bad problem set up- but don't want to die here)
        //                    {
        //                        nIDs[nDataVals] = -111666;         //ElementID_FK was not filled out.
        //                    }
        //                    else
        //                    {
        //                        nIDs[nDataVals] = Convert.ToInt32(dr["ElementID_FK"].ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    sVals[nDataVals] = "-666";                    //value was not found. nCurrentFilePosition is set to where it was at beginning
        //                    nIDs[nDataVals] = -666;
        //                }
        //                nDataVals++;
        //                sLastTableName = sCurrentTableName;
        //            }

        //            // now insert the records into the database
        //            string sql_da = "SELECT ResultDetail_ID, Result_ID_FK, TableFieldKey_FK, ScenarioID_FK, val, ElementName, ElementID FROM tblResultVar_Details where (false);";
        //            DataSet dsInsert = new DataSet();
        //            OleDbDataAdapter da = cu.getDataAdapterfromSQL(sql_da, connRMG);
        //            da.Fill(dsInsert);
        //            int nResultsCol = 0;
        //            int nRowInsertCounter = 0;
        //            foreach (DataRow dr in dsRMG_Changes.Tables[0].Rows)                                    //now populate the  counter that will tell us how to read the file in...?
        //            {
        //                dsInsert.Tables[0].Rows.Add(dsInsert.Tables[0].NewRow());
        //                dsInsert.Tables[0].Rows[nRowInsertCounter]["Result_ID_FK"] = Convert.ToInt32(dr["Result_ID"].ToString());
        //                dsInsert.Tables[0].Rows[nRowInsertCounter]["ScenarioID_FK"] = nScenarioID;
        //                dsInsert.Tables[0].Rows[nRowInsertCounter]["TableFieldKey_FK"] = -1;            //i don't think this is needed.
        //                dsInsert.Tables[0].Rows[nRowInsertCounter]["val"] = sVals[nRowInsertCounter];
        //                dsInsert.Tables[0].Rows[nRowInsertCounter]["ElementName"] = dr["Element_Label"].ToString();
        //                dsInsert.Tables[0].Rows[nRowInsertCounter]["ElementID"] = nIDs[nRowInsertCounter];  //added 1/15/2011 MET
        //                nRowInsertCounter++;
        //            }
        //            OleDbCommand cmdInsert = new OleDbCommandBuilder(da).GetInsertCommand();
        //            da.Update(dsInsert);
        //        }

        //        catch (Exception ex)
        //        {
        //            rmgDB_link.cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), cu.nLogging_Level_1);
        //        }
        //        finally
        //        {
        //            if (fileINP != null)
        //                fileINP.Close();
        //        }

        //    }
        //}
       





        private void HELPER_EPANET_AdvanceToData(ref string[] sTextFile_ALL, ref int nCurrentWriteLine)
        {
            bool bExit = false;
            int nCountList = sTextFile_ALL.Length;
            while (!bExit && (nCurrentWriteLine<nCountList)){
                nCurrentWriteLine++;                            //index whether found or not
                if (sTextFile_ALL[nCurrentWriteLine].IndexOf(";;--") >= 0)
                {
                    bExit = true;
                }
            }
        }

        public string EPANET_INP_Helper_GetElementNameOrField(DataRow dr, ref bool bIsScenarioLevelVar)
        {
            string sFindElementNameOrField;
            if (Convert.ToInt32(dr["IsScenarioSpecific"].ToString()) == 0)             //met 4/16/2012: 
            {
                sFindElementNameOrField = dr["ElementName"].ToString();       //typical case
                bIsScenarioLevelVar = true;
            }
            else
            {
                sFindElementNameOrField = dr["FieldName"].ToString();           //find the field name for summmary variables. 
                bIsScenarioLevelVar = false;
            }
            return sFindElementNameOrField;

        }
            
        public string HELPER_EPANET_AdvanceToCurrent_ID(ref string[] sTEXT_File, string sFindID, ref int nCurrentFilePosition, int nSectionStartIndex, string sSectionName)
        {
            int nStartingPosition = nCurrentFilePosition; string sbuf; int nID_Index = 0;
            string sReturn = "No_ID_Found"; bool bFound = false;
            while ((nCurrentFilePosition < sTEXT_File.Length) && (!bFound))
            {
                if (HELPER_EPANET_IsNewMEX_Section(sTEXT_File[nCurrentFilePosition].ToString())) { break; }  //exit the while if we hit a new section

                if ((sSectionName == "MOUSE_RTC_LOGIC_CONDITIONS") || (sSectionName == "MOUSE_RTC_CONTROLLED_DEVICES"))
                {
                    nID_Index = -1;
                }
                else if ((sSectionName != "MOUSE_Catchments") && (sSectionName != "LogicCondition") && (sSectionName != "ControlledDevice"))
                {
                    nID_Index = 0;        //typical case
                }
                else
                {
                    nID_Index = 1;
                }
                sTEXT_File[nCurrentFilePosition] = cu.cuRemoveRepeating(sTEXT_File[nCurrentFilePosition]);                                  //BYM2012
                sbuf = HELPER_EPANET_GetIDFromDataRow(sTEXT_File[nCurrentFilePosition].ToString(), nID_Index);
                if (sbuf == sFindID)
                {
                    sReturn = sFindID;          //we have found the ID exit loop
                    bFound = true;
                }
                else
                {
                    nCurrentFilePosition++;
                }
            }
            if (!bFound)
            {
                for (int i = nSectionStartIndex; i < nStartingPosition; i++)
                {
                    nCurrentFilePosition = i;
                    sbuf = HELPER_EPANET_GetIDFromDataRow(sTEXT_File[nCurrentFilePosition].ToString(), nID_Index);                //BYM2012
                    if (sbuf == sFindID)
                    {
                        sReturn = sFindID;
                        bFound = true;          //we have found the ID exit loop
                        break;
                    }
                    else
                    {
                        nCurrentFilePosition++;
                    }
                }
            }
            return sReturn;
        }

        public bool HELPER_EPANET_IsNewMEX_Section(string s, bool bExcludeInteriorTables = true)
        {
            if (s.Trim() == "")
            {
                return false;
            }
            else if (s.Trim().Substring(0, 1) == "[")
            {
                if (bExcludeInteriorTables)
                {
                    bool bInteriorTable = false;
                    if (s.IndexOf("LogicCondition") > 0) { bInteriorTable = true; }
                    if (s.IndexOf("ControlFunction") > 0) { bInteriorTable = true; }
                    if (s.IndexOf("ControlledDevice") > 0) { bInteriorTable = true; }
                    if (bInteriorTable)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public string HELPER_EPANET_GetIDFromDataRow(string sbuf, int nID_Column = 0)
        {
            sbuf = sbuf.Trim();         //met 4/18/2013: first  blank was causing problems.

            int nSpaceIndex = -1;
            if (nID_Column == -1)           //this is a special case where no comma exists; just send back cleaned id field.
            {
                return muCleanIDField(sbuf);
            }

            for (int i = 0; i < nID_Column; i++)
            {
                //BYM2012 nCommaIndex = sbuf.IndexOf(",");
                nSpaceIndex = sbuf.IndexOf(" ");
                if (nSpaceIndex > 0)
                {
                    sbuf = sbuf.Substring(nSpaceIndex + 1, sbuf.Length - nSpaceIndex - 1);
                }
                else
                {
                    return "No_ID_Found";
                }
            }
            //BYM nCommaIndex = sbuf.IndexOf(",");
            nSpaceIndex = sbuf.IndexOf(" ");
            if (nSpaceIndex >= 0)
            {
                return muCleanIDField(sbuf.Substring(0, nSpaceIndex));
            }
            else
            {               //no comma found, this is not a typical data row
                return "No_ID_Found";
            }

        }

        private string muCleanIDField(string sID)
        {
            //BYM string sReturn = sID.Substring(sID.IndexOf('=') + 1, sID.Length - sID.IndexOf('=') - 1).Trim();
            string sReturn = sID.Substring(sID.IndexOf(' ') + 1, sID.Length - sID.IndexOf(' ') - 1).Trim();
            return muCleanMEXString(sReturn);
        }

        private string muCleanMEXString(string sbuf)
        {
            //BYM2012 char[] nogood = { '\'' };
            char[] nogood = { ' ' };
            return sbuf.TrimStart(nogood).TrimEnd(nogood);
        }


        #region PROCESS SCENARIO

        
        // 5/22/2013: adapted from SWMM ProcessScenario
        public int EPANET_ProcessScenario(int nProjID, int nEvalID, int nReferenceEvalID, string sINP_File, int nScenarioID = -1, int nScenStartAct = 1, int nScenEndAct = 100, string sDNA = "-1")
        {
            string sPath; string sTargetPath; string sTarget_INP; string sIncomingINP; string sTarget_INP_FileName; string sTarget_INP_temp;
            int nCurrentLoc = nScenStartAct;

            try
            {

                if (nScenarioID != -1)     //we should have a valid ScenarioID at this point.
                {

                    //todo: break out into cu function with parameters for customization
                    sPath = System.IO.Path.GetDirectoryName(sINP_File);
                    sTargetPath = sPath.Substring(0, sPath.LastIndexOf("\\")) + "\\" + nEvalID.ToString() + "\\" + nScenarioID.ToString();
                    sTarget_INP_FileName = System.IO.Path.GetFileNameWithoutExtension(sINP_File) + "_" + nScenarioID.ToString() + System.IO.Path.GetExtension(sINP_File);       //append scenario name (good for gathering up the files into a single folder if needed)
                    sIncomingINP = System.IO.Path.Combine(sTargetPath, System.IO.Path.GetFileName(sINP_File));
                    sTarget_INP = System.IO.Path.Combine(sTargetPath, sTarget_INP_FileName);
                    sTarget_INP_temp = System.IO.Path.Combine(sTargetPath,"_temp" +sTarget_INP_FileName);  
                    string sSummaryFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTarget_INP_FileName) + ".RPT";
                    string sOUT = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sTarget_INP_FileName) + ".OUT";
                    string sINIFile = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sINP_File) + ".ini";
                    bool bContinue = true;              //identifies whether to continue

                    Console.WriteLine(sIncomingINP);
                    Console.WriteLine("ScenarioID: " + nScenarioID);
                    Console.WriteLine(sTarget_INP);
                    Console.WriteLine();

                    if ((nCurrentLoc <= cu.nScenLCModElementExist) && (nScenEndAct >= cu.nScenLCModElementExist) && bContinue)       //met 1/8/2011 add dna unspool to the regular scenario flow; TODO: handle when we don't yet have the scenario
                    {
                        if (sDNA != "-1")
                        {           //not an optimization run, no DNA is passed

                            nScenarioID = _rmgDB_link.rmgDNA_DistribToScenario(sDNA, nEvalID, nReferenceEvalID, nProjID, 3, -1, _connMod, nScenarioID);
  
                            if (nScenarioID == -1) 
                            {
                                bContinue = false;
                            }       // some failure in the DNA distribution

                            else
                            {
                                nCurrentLoc = cu.nScenLCBaselineFileSetup;
                            }
                            rmgDB_link.cu.cuLogging_UpdateFileOutName("logEG_" + nEvalID.ToString() + "_" + nScenarioID.ToString());
                        }
                        else
                        {
                            nCurrentLoc = cu.nScenLCBaselineFileSetup;          //
                        }


                        if (true)
                        {
                            cu.cuCloseOpenDBConnection(ref _connRMG);
                        }
                    }

                    if ((nCurrentLoc <= cu.nScenLCBaselineFileSetup) && (nScenEndAct >= cu.nScenLCBaselineFileSetup))
                    {
                        rmgDB_link.cu.cuLogging_AddString("EPANET File Setup Begin: " + System.DateTime.Now.ToString(), cu.nLogging_Level_2);      //log begin scenario step
                        if (!System.IO.Directory.Exists(sTargetPath)) { System.IO.Directory.CreateDirectory(sTargetPath); }
                        cu.cuCopyDirectoryAndSubfolders(sPath, sTargetPath, true);
                        nCurrentLoc = cu.nScenLCBaselineFileSetup;          //
                    }
                    if ((nCurrentLoc <= cu.nScenLCBaselineModified) && (nScenEndAct >= cu.nScenLCBaselineModified))
                    {
                        rmgDB_link.cu.cuLogging_AddString("EPANET File Update Begin: " + System.DateTime.Now.ToString(), cu.nLogging_Level_2);      //log begin scenario step

                        
                        //CMOORE TODO    
                        EPANET_Update_INP(sIncomingINP, nScenarioID, sTarget_INP, sTarget_INP_temp, sSummaryFile);
                        

                        System.IO.File.Delete(sIncomingINP);
                        System.IO.File.Delete(sTarget_INP_temp);
                        nCurrentLoc = cu.nScenLCBaselineModified;

                        string sTargetINI = sTargetPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(sINIFile) + "_" + nScenarioID.ToString() + ".ini";
                        if (File.Exists(sINIFile) && !File.Exists(sTargetINI))
                        {      //there may not be one in the root file, but update if there is.
                            File.Move(sINIFile, sTargetINI);
                        }
                    }

                    if ((nCurrentLoc <= cu.nScenLCModelExecuted) && (nScenEndAct >= cu.nScenLCModelExecuted))
                    {
                        //create batch file information for running the program
                        string[] s = new string[] {"epanet2d.exe " + System.IO.Path.GetFileName(sTarget_INP) + " " + System.IO.Path.GetFileName(sSummaryFile) + " " + System.IO.Path.GetFileName(sOUT) };
                        string sBat = System.IO.Path.Combine(sTargetPath, "run_EPANET2.bat");

                        if (_bSimCondor) //run the EPANET job as a Condor job.
                        {               //note: 
                        //    //_htc = new CIRRUS_HTC_NS.CIRRUS_HTC();
                        //    Dictionary<string, string> dictHTC_EPANET = new Dictionary<string, string>();
                        //    dictHTC_EPANET.Add("transfer_input_files", System.IO.Path.GetFileName(sTarget_INP));

                        //    if (true)       //option 1: Do NOT pas exe - look for requirement that it is installed.
                        //    {               //todo: parameterize this
                        //        //uncomment as soon as Condor instances updated                     dictHTC_EPANET.Add("requirements", "(EPANET_INSTALLED =?= True)");              //todo: consider how to suppor other versions of EPANET. maybe other versions must have exe passed
                        //        //in standard config                        dictHTC_EPANET.Add("executable", "run_EPANET5.bat");

                        //        File.WriteAllLines(sBat, s);                                 //output the .bat file to run as a EPANET file.
                        //    }
                        //    else
                        //    {
                        //        // this option works if you need to pass the exe
                        //        //in standard config                       dictHTC_EPANET.Add("executable", "EPANET5.exe");
                        //        dictHTC_EPANET.Add("arguments", System.IO.Path.GetFileName(sTarget_INP) + " " + System.IO.Path.GetFileName(sSummaryFile) + " " + System.IO.Path.GetFileName(sOUT));
                        //    }
                        //    //_htc.InitHTC_Vars(dictHTC_EPANET, sTargetPath + "\\", _nActiveModelID_EPANET, true);
                        //    //_htc.HTC_Submit();
                        }
                        else
                        {   //run within SimLink
                            s[0] = "cd %~dp0 \r\n" + s[0];
                            File.WriteAllLines(sBat, s);

                            cu.cuRunBatchFile(sBat);

                        }
                        nCurrentLoc = cu.nScenLCModelExecuted;
                    }






                    if ((nCurrentLoc <= cu.nScenLCModelResultsRead) && (nScenEndAct >= cu.nScenLCModelResultsRead))
                    {
                        rmgDB_link.cu.cuLogging_AddString("EPANET Results Read Begin: " + System.DateTime.Now.ToString(), cu.nLogging_Level_2);      //log begin scenario step
                        
                        //EPANET_ReadRPT_IntoDB(sSummaryFile, nReferenceEvalID, nScenarioID);      //CMOORE TODO   
     
                        nCurrentLoc = cu.nScenLCModelResultsRead;
                    }


          //          if ((nCurrentLoc <= cu.nScenLModelResultsTS_Read) && (nScenEndAct >= cu.nScenLModelResultsTS_Read))
          //          {
          //              //     hdf5_wrap h5f = new hdf5_wrap();
          //              string sTS_H5 = sTargetPath + "\\" + cu.GetSimLinkFileName("EPANET_TS.H5", nScenarioID);
          // //CMOORE TODO                _hdf5_EPANET.hdfCheckOrCreateH5(sTS_H5);
          //              //   h5f.hdfCheckOrCreateH5(sTS_H5);

          ////CMOORE TODO                 EPANET_GetResultOUTData(nReferenceEvalID, nScenarioID, sOUT, sTS_H5);
          //    //CMOORE TODO             _hdf5_EPANET.hdfClose();
          //              nCurrentLoc = cu.nScenLModelResultsTS_Read;
          //          }






                }

                return nCurrentLoc;
            }

            catch (Exception ex)                //log the error
            {
                rmgDB_link.cu.cuLogging_AddString("Exception: " + ex.Message + " : " + System.DateTime.Now.ToString(), cu.nLogging_Level_1);
                return 0;   //TODO: refine based upon code succes met 6/12/2012
            }
        }


        #endregion


    }
}
