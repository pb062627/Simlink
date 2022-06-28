using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

namespace ExtendSimWrapper
{

    public enum ExtendExecuteCommandType
    {
        MenuCommand,
        OpenExtendFile,
        GetSimulationPhase,
        GetRunParameter,
        GetBlockNumber,
        GetDBDBIndex,
        GetDBTableIndex,
        GetDBFieldIndex,
        GetDBRecordIndex,
        SaveModelAs,
        SaveModel,
        RunSimulation
    }

    public enum ExtendMenuCommandType
    {
        Close,
        ExitOrQuit,
        RevertModel,
        SaveModel
    }

    public enum ExtendPhaseType
    {
        SimStart = 11,
        SimFinish = 12
    }

    public enum ExtendRunParameter
    {
        EndTime = 1,
        StartTime = 2,
        NumberSims = 3,
        NumberSteps = 4,
        TimeUnits = 8
    }

    public enum ExtendTimeUnits
    {
        Generic = 1,
        Milliseconds = 2,
        Seconds = 3,
        Minutes = 4,
        Hours = 5,
        Days = 6,
        Weeks = 7,
        Months = 8,
        Years = 9
    }


    public static class ExtendSimFunctions
    {
        private const string sGlobalStorageLoc0_General = "Global0";
        private const string sGlobalStorageLoc0_Int = "GlobalInt0";
        private const int _nSaveWaitTime = 2000;

        #region ExtendApplicationFunctions

        public static object EXTEND_OpenExtendInstance(string sExtendModelPath)
        {
            Guid ExtendCLSID = new Guid("E167B362-7044-11d2-99DE-00C0230406DF");
            //Guid ExtendCLSID = new Guid("65A27B59-16DA-4F68-B459-598885729000");
            //HKEY_CLASSES_ROOT\AppID\{ 65A27B59 - 16DA - 4F68 - B459 - 598885729000}

            Type ExtendType = Type.GetTypeFromCLSID(ExtendCLSID, true);
            object objExtend = null;

            objExtend = EXTEND_getActiveExtendAPP(ExtendCLSID);
            ///````testing only!`````
            /*string _RUNEXTENDSIMBAT = "OpenExtendSim.bat";
            string[] s = new string[] { @"/c ExtendSim " + sExtendModelPath};

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = s[0];
            process.StartInfo = startInfo;
            process.Start();

            Thread.Sleep(15000);

            //string sBat = System.IO.Path.Combine(Directory.GetCurrentDirectory(), _RUNEXTENDSIMBAT);

            //create the string for the .bat
            //s[0] = "cd %~dp0 \r\n" + s[0];
            //File.WriteAllLines(sBat, s);
            ///````finish testing only!`````

            //call a batch file to open ExtendSim
            //CommonUtilities.cuRunBatchFile(sBat, true);

            //wait for model to open and instance of extend sim to be obtained
            while (objExtend == null)
            {
                objExtend = EXTEND_getActiveExtendAPP();
            }
            return true;*/

            if (objExtend == null)
                objExtend = Activator.CreateInstance(ExtendType);

            string sExtendCommand;
            sExtendCommand = GetExtendCommand(ExtendExecuteCommandType.OpenExtendFile, new object[] { sExtendModelPath });

            //sExtendCommand = "OpenExtendFile(\"\\\\HCHFPP01\\groups\\WBG\\TRWD\\Model\\Replica\\Replica_CondorTest\\Calibration\\Wilimington_Treatment_Plant_2021_8-6_WR_MODS.mox\");"


            object[] paramExtend = new object[1];
            paramExtend[0] = sExtendCommand;

            object sReturn = objExtend.GetType().InvokeMember("[DispID=1]", BindingFlags.Instance | BindingFlags.SetField | BindingFlags.SetProperty, null, objExtend, paramExtend);
            
            //object sReturn = objExtend.GetType().InvokeMember("Execute", BindingFlags.InvokeMethod | BindingFlags.CreateInstance | BindingFlags.GetProperty, null, objExtend, paramExtend);
            
            return sReturn;
        }

        //PB -testing 
        public static class Marshal2
        {
            internal const String OLEAUT32 = "oleaut32.dll";
            internal const String OLE32 = "ole32.dll";

            [System.Security.SecurityCritical]  // auto-generated_required
            public static Object GetActiveObject(String progID, Guid clsid2)
            {
                Object obj = null;
               //Guid clsid;

                // Call CLSIDFromProgIDEx first then fall back on CLSIDFromProgID if
                // CLSIDFromProgIDEx doesn't exist.
                //try
                //{
                //    CLSIDFromProgIDEx(progID, out clsid);
                //}
                //            catch
                //catch (Exception)
                //{
                //    CLSIDFromProgID(progID, out clsid);
                //}

                GetActiveObject(ref clsid2, IntPtr.Zero, out obj);
                return obj;
            }

            //[DllImport(Microsoft.Win32.Win32Native.OLE32, PreserveSig = false)]
            [DllImport(OLE32, PreserveSig = false)]
            [ResourceExposure(ResourceScope.None)]
            [SuppressUnmanagedCodeSecurity]
            [System.Security.SecurityCritical]  // auto-generated
            private static extern void CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] String progId, out Guid clsid);

            //[DllImport(Microsoft.Win32.Win32Native.OLE32, PreserveSig = false)]
            [DllImport(OLE32, PreserveSig = false)]
            [ResourceExposure(ResourceScope.None)]
            [SuppressUnmanagedCodeSecurity]
            [System.Security.SecurityCritical]  // auto-generated
            private static extern void CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] String progId, out Guid clsid);

            //[DllImport(Microsoft.Win32.Win32Native.OLEAUT32, PreserveSig = false)]
            [DllImport(OLEAUT32, PreserveSig = false)]
            [ResourceExposure(ResourceScope.None)]
            [SuppressUnmanagedCodeSecurity]
            [System.Security.SecurityCritical]  // auto-generated
            private static extern void GetActiveObject(ref Guid rclsid, IntPtr reserved, [MarshalAs(UnmanagedType.Interface)] out Object ppunk);

        }



        public static object EXTEND_getActiveExtendAPP(Guid clsid2)
        {
            try
            {
                object objExtend = new object();

                objExtend = Marshal2.GetActiveObject("Extend.application",clsid2);        //get an instance of extend

                //objExtend = System.Runtime.InteropServices.Marshal.GetActiveObject("Extend.application",);        //get an instance of extend
                return objExtend;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object EXTEND_ReturnExtendInstance(string sExtendPath)
        {
            Guid ExtendCLSID = new Guid("E167B362-7044-11d2-99DE-00C0230406DF");
            Type ExtendType = Type.GetTypeFromCLSID(ExtendCLSID, true);
            object ExtendInstance = Activator.CreateInstance(ExtendType);
            return ExtendInstance;
        }

        #endregion



        #region HelperFunctionArguments

        //SP 24-Jun-2016 get menu command type
        public static int GetMenuCommand(ExtendMenuCommandType eMCT)
        {
            switch (eMCT)
            {
                case ExtendMenuCommandType.ExitOrQuit:
                    return 1;
                case ExtendMenuCommandType.Close:
                    return 4;
                case ExtendMenuCommandType.SaveModel:
                    return 5;
                default:
                    return 1; //run simulation
            }
        }

        //SP 24-Jun-2016 get ExtendSim command
        public static string GetExtendCommand(ExtendExecuteCommandType eCT, object[] oParams, string tmpStorageLocation = sGlobalStorageLoc0_General)
        {
            switch (eCT)
            {
                case ExtendExecuteCommandType.MenuCommand:
                    return string.Format("ExecuteMenuCommand({0});", GetMenuCommand((ExtendMenuCommandType)oParams[0]));
                case ExtendExecuteCommandType.OpenExtendFile:
                    return string.Format("OpenExtendFile(\"{0}\");", oParams[0].ToString());
                case ExtendExecuteCommandType.SaveModelAs:
                    return string.Format("SaveModelAs(\"{0}\");", oParams[0].ToString());
                case ExtendExecuteCommandType.SaveModel:
                    return string.Format("SaveModel();");
                case ExtendExecuteCommandType.RunSimulation:
                    return string.Format("RunSimulation(False);");
                case ExtendExecuteCommandType.GetSimulationPhase:
                    return string.Format("{0} = GetSimulationPhase();", tmpStorageLocation);
                case ExtendExecuteCommandType.GetRunParameter:
                    return string.Format("{0} = GetRunParameter({1});", tmpStorageLocation, (int)oParams[0]);
                case ExtendExecuteCommandType.GetBlockNumber:
                    return string.Format("integer b, NoBlks; string CurrentBlkName, CurrentBlkLbl; NoBlks= NumBlocks(); for(b=0; b<NoBlks;  b++) " +
                        "{{CurrentBlkName=BlockName(b); CurrentBlkLbl=GetBlockLabel(b); if(StringCase(CurrentBlkName, true)==StringCase(\"{0}\", true) " +
                        "and StringCase(CurrentBlkLbl, true)==StringCase(\"{1}\", true)) {{{2}=b; b=NoBlks;}}}}",
                        oParams[0].ToString(), oParams[1].ToString(), tmpStorageLocation);
                case ExtendExecuteCommandType.GetDBDBIndex:
                    return string.Format("{0} = DBDatabaseGetIndex(\"{1}\");", tmpStorageLocation, oParams[0].ToString());
                case ExtendExecuteCommandType.GetDBTableIndex:
                    return string.Format("{0} = DBTableGetIndex({1}, \"{2}\");", tmpStorageLocation, (int)oParams[0], oParams[1].ToString());
                case ExtendExecuteCommandType.GetDBFieldIndex:
                    return string.Format("{0} = DBFieldGetIndex({1}, {2}, \"{3}\");", tmpStorageLocation, (int)oParams[0], (int)oParams[1], oParams[2].ToString());
                case ExtendExecuteCommandType.GetDBRecordIndex:
                    return string.Format("{0} = DBRecordFind({1}, {2}, {3}, 1, True, \"{4}\");", tmpStorageLocation, (int)oParams[0], (int)oParams[1], (int)oParams[2], oParams[3].ToString());
                default:
                    return "";
            }
        }


        //Get ItemString for modifying data in DB
        public static string EXTEND_getItemString_DB(int nDBIndex, int nTableIndex, int nRecordStart, int nFieldStart, int nRecordEnd = 0, int nFieldEnd = 0)
        {
            if (nRecordEnd == 0)
                nRecordEnd = nRecordStart;

            if (nFieldEnd == 0)
                nFieldEnd = nFieldStart;

            return string.Format("DB:#{0}:{1}:{2}:{3}:{4}:{5}", nDBIndex, nTableIndex, nRecordStart, nFieldStart, nRecordEnd, nFieldEnd);
        }


        //Get ItemString for modifying data in Object
        public static string EXTEND_getItemString_Object(string sVariableName, int nBlockNumber, int nRowStart = 0, int nColStart = 0, int nRowEnd = 0, int nColEnd = 0)
        {
            return string.Format("{0}:#{1}:{2}:{3}:{4}:{5}", sVariableName, nBlockNumber, nRowStart, nColStart, nRowEnd, nColEnd);
        }

        #endregion



        #region ExtendSimAsServerAutomationFunctions


        //get the table indexes from the names
        private static void EXTEND_GetDBIndexes(string sDBName, string sDBTable, string sDBReturnFieldName, string sDBRecordFieldName, string sDBRecord, string sDBRecordEnd,
            ref int nDBDBIndex, ref int nDBTableIndex, ref int nDBReturnFieldIndex, ref int nDBRecordFieldIndex, ref int nDBRecordStartIndex, ref int nDBRecordEndIndex)
        {
            //this sets Global0 parameter in ExtendSim to be the returned value
            var nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBDBIndex, new object[] { sDBName });
            string sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
            nDBDBIndex = Convert.ToInt32(EXTEND_Request("System", sItem));

            //this sets Global0 parameter in ExtendSim to be the returned value
            nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBTableIndex, new object[] { nDBDBIndex, sDBTable });
            sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
            nDBTableIndex = Convert.ToInt32(EXTEND_Request("System", sItem));

            //this sets Global0 parameter in ExtendSim to be the returned value
            nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBFieldIndex, new object[] { nDBDBIndex, nDBTableIndex, sDBReturnFieldName });
            sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
            nDBReturnFieldIndex = Convert.ToInt32(EXTEND_Request("System", sItem));

            if (sDBRecordFieldName != "")
            {
                //this sets Global0 parameter in ExtendSim to be the returned value
                nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBFieldIndex, new object[] { nDBDBIndex, nDBTableIndex, sDBRecordFieldName });
                sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
                nDBRecordFieldIndex = Convert.ToInt32(EXTEND_Request("System", sItem));

                //this sets Global0 parameter in ExtendSim to be the returned value
                nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBRecordIndex, new object[] { nDBDBIndex, nDBTableIndex, nDBRecordFieldIndex, sDBRecord });
                sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
                nDBRecordStartIndex = Convert.ToInt32(EXTEND_Request("System", sItem));

                //this sets Global0 parameter in ExtendSim to be the returned value
                nDBRecordEndIndex = nDBRecordStartIndex;
                if (sDBRecordEnd != "")
                {
                    nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetDBRecordIndex, new object[] { nDBDBIndex, nDBTableIndex, nDBRecordFieldIndex, sDBRecordEnd });
                    sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
                    nDBRecordEndIndex = Convert.ToInt32(EXTEND_Request("System", sItem));
                }
            }
            else
            {
                //don't modify the nRecordStartIndex nor the nRecordEndIndex as they should be dictated by time periods in simulation and not specified by user
            }
        }


        public static object EXTEND_GetDBData_FromNames(string sDBName, string sDBTable, string sDBReturnFieldName, string sDBRecordFieldName, string sDBRecord, string sDBRecordEnd = "")
        {
            //if don't want an array, sRecordEnd will be set to the start record # in EXTEND_getItemString_DB

            int nDBDBIndex = -1;
            int nDBTableIndex = -1;
            int nRecordFieldIndex = -1; //not used in the request or poke - just used to find the start and end rows
            int nReturnFieldIndex = -1;
            int nRecordStartIndex = -1;
            int nDBRecordEndIndex = -1;

            EXTEND_GetDBIndexes(sDBName, sDBTable, sDBReturnFieldName, sDBRecordFieldName, sDBRecord, sDBRecordEnd,
                ref nDBDBIndex, ref nDBTableIndex, ref nReturnFieldIndex, ref nRecordFieldIndex, ref nRecordStartIndex, ref nDBRecordEndIndex);

            //return a string to be used with 'request' to obtain the required data from the tables in ExtendSim
            string sDBRequest = EXTEND_getItemString_DB(nDBDBIndex, nDBTableIndex, nRecordStartIndex, nReturnFieldIndex, nDBRecordEndIndex);
            return EXTEND_Request("System", sDBRequest);
        }

        //for time series, just get the start row and end row
        public static string[,] EXTEND_GetDBData_FromNames(string sDBName, string sDBTable, string sDBReturnFieldName, int nStartTimeRow, int nEndTimeRow)
        {
            //if don't want an array, sRecordEnd will be set to the start record # in EXTEND_getItemString_DB
            int nDBDBIndex = -1;
            int nDBTableIndex = -1;
            int nRecordFieldIndex = -1; //not used in the request or poke - just used to find the start and end rows
            int nReturnFieldIndex = -1;
            int nRecordStartIndex = nStartTimeRow;
            int nDBRecordEndIndex = nEndTimeRow;

            EXTEND_GetDBIndexes(sDBName, sDBTable, sDBReturnFieldName, "", "", "",
                ref nDBDBIndex, ref nDBTableIndex, ref nReturnFieldIndex, ref nRecordFieldIndex, ref nRecordStartIndex, ref nDBRecordEndIndex);

            //return a string to be used with 'request' to obtain the required data from the tables in ExtendSim
            string sDBRequest = EXTEND_getItemString_DB(nDBDBIndex, nDBTableIndex, nRecordStartIndex, nReturnFieldIndex, nDBRecordEndIndex);
            return (string[,])EXTEND_Request("System", sDBRequest);
        }


        //set a table value through names
        public static void EXTEND_SetDBData_FromNames(string sDBName, string sDBTable, string sDBReturnFieldName, string sDBRecordFieldName, string sDBRecord, string sNewTableValue)
        {
            //if don't want an array, sRecordEnd will be set to the start record # in EXTEND_getItemString_DB

            int nDBDBIndex = -1;
            int nDBTableIndex = -1;
            int nRecordFieldIndex = -1;
            int nReturnFieldIndex = -1;
            int nRecordStartIndex = -1;
            int nDBRecordEndIndex = -1;

            //for poking, only allow one record to be modified at once - record end will be set to record start
            string sDBRecordEnd = "";

            EXTEND_GetDBIndexes(sDBName, sDBTable, sDBReturnFieldName, sDBRecordFieldName, sDBRecord, sDBRecordEnd,
                ref nDBDBIndex, ref nDBTableIndex, ref nReturnFieldIndex, ref nRecordFieldIndex, ref nRecordStartIndex, ref nDBRecordEndIndex);

            //return a string to be used with 'poke' to obtain the required data from the tables in ExtendSim - simple, keep to single values for now
            string sDBPoke = EXTEND_getItemString_DB(nDBDBIndex, nDBTableIndex, nRecordStartIndex, nReturnFieldIndex);
            EXTEND_PokeVal("System", sNewTableValue, sDBPoke);
        }


        //get Item string for Objects for Request and Pokes from Names
        private static string EXTEND_getItemString_Object_FromNames(string sBlockName, string sBlockLabel, string sBlockProperty)
        {
            int nBlockNumber = EXTEND_GetBlockNumber(sBlockName, sBlockLabel);
            //get the request string
            return EXTEND_getItemString_Object(sBlockProperty, nBlockNumber);
        }

        //Get the property value of a block from names
        public static object EXTEND_GetBlockProperty_FromNames(string sBlockName, string sBlockLabel, string sBlockProperty)
        {
            string sItem = EXTEND_getItemString_Object_FromNames(sBlockName, sBlockLabel, sBlockProperty);
            return EXTEND_Request("System", sItem); //SP 24-Jun-2016 if this is a DB call the result is likely to be an array - process this array
        }


        //Set the property value of a block from names
        public static void EXTEND_SetBlockProperty_FromNames(string sBlockName, string sBlockLabel, string sBlockProperty, string sNewPropertyValue)
        {
            string sItem = EXTEND_getItemString_Object_FromNames(sBlockName, sBlockLabel, sBlockProperty);
            EXTEND_PokeVal("System", sNewPropertyValue, sItem);
        }



        //get the property value of a block
        private static int EXTEND_GetBlockNumber(string sBlockName, string sBlockLabel)
        {
            //this sets Global0 parameter in ExtendSim to be the block number of interest
            var nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetBlockNumber, new object[] { sBlockName, sBlockLabel });
            string sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
            return Convert.ToInt32(EXTEND_Request("System", sItem));
        }

        public static object EXTEND_GetRunTimeParameter(ExtendRunParameter eRTP)
        {
            //this sets Global0 parameter in ExtendSim to be the run parameter of interest
            var nSuccess = EXTEND_Execute(ExtendExecuteCommandType.GetRunParameter, new object[] { eRTP });
            string sItem = EXTEND_getItemString_Object(sGlobalStorageLoc0_General, 0); //request the temp value stored in Global0 from Execute command
            return EXTEND_Request("System", sItem);
        }


        public static object EXTEND_Request(string sTopic, string sItem)
        {
            object objExtend = new object();
            objExtend = System.Runtime.InteropServices.Marshal.GetActiveObject("Extend.application");        //get an instance of extend

            object[] paramExtend = new object[2];
            paramExtend[0] = sTopic;
            paramExtend[1] = sItem;
            //object vOutput = new object();
            object sReturn = objExtend.GetType().InvokeMember("Request", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty, null, objExtend, paramExtend);
            return sReturn;

        }


        public static object EXTEND_Execute(ExtendExecuteCommandType eCT = ExtendExecuteCommandType.MenuCommand, object[] oParams = null, string tmpStorageLocation = sGlobalStorageLoc0_General)
        {
            object objExtend = new object();
            objExtend = System.Runtime.InteropServices.Marshal.GetActiveObject("Extend.application");        //get an instance of extend

            string sExtendCommand;
            sExtendCommand = GetExtendCommand(eCT, oParams, tmpStorageLocation);

            object[] paramExtend = new object[1];
            paramExtend[0] = sExtendCommand;

            object sReturn = objExtend.GetType().InvokeMember("Execute", BindingFlags.Instance | BindingFlags.SetField | BindingFlags.SetProperty, null, objExtend, paramExtend);

            //if save or save as - always put a sleep to ensure model has a chance to save before moving on
            if (eCT == ExtendExecuteCommandType.SaveModel || eCT == ExtendExecuteCommandType.SaveModelAs)
                System.Threading.Thread.Sleep(_nSaveWaitTime);

            return sReturn;
        }


        public static void EXTEND_PokeVal(string sTopic, string sVAL, string sItem)           //      for now just get object within the function, object ExtendApp)
        {
            object objExtend = new object();
            objExtend = System.Runtime.InteropServices.Marshal.GetActiveObject("Extend.application");        //get an instance of extend

            //string sTopic = "System";
            object[] paramExtend = new object[3];
            paramExtend[0] = sTopic;
            paramExtend[1] = sItem;
            paramExtend[2] = sVAL;
            object sReturn = objExtend.GetType().InvokeMember("Poke", BindingFlags.Instance | BindingFlags.SetField | BindingFlags.SetProperty, null, objExtend, paramExtend);
        }


        #endregion

    }
}
