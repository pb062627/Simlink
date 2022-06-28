/*
 * 
 * 
 * Adaptor4HECDSS.exe - This adaptor is for HEC-DSS Vue data file (dss)
 * Convert data from DSS file to a common format
 * 
 * Created by: Leng Dieb 08/Apr/2010
 * 
 * Copyright        2010 by Halcrow
 *                  All rights reserved.
 * 
 * This code is not to be distributed without the written 
 * permission of Halcrow Group Ltd
 * 
 * 
 */

using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Data;
using DTTCommons;

namespace Adaptor4HecDSS
{
    public class DSS2CF
    {
        #region Private Variables
        private DTTXMLConfig _config;
        private string _strInputXMLFile;
        #endregion Private Variables

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="strInputXMLFile"></param>
        public DSS2CF(DTTXMLConfig config, string strInputXMLFile)
        {
            _config = config;
            _strInputXMLFile = strInputXMLFile;
        }
        #endregion Constructor

        #region Retrieving Time-series from DSS for CF
        /// <summary>
        /// Read time-series input command for each time-series and extraction into
        /// command format
        /// </summary>
        public void ExportDSS2CF()
        {
            XDocument xdoc = XDocument.Load(_strInputXMLFile); // read xml document
            
            //string strDSSFile = xdoc.Root.Descendants("applications").Descendants("source").Elements("inputfile").First().Value; // DSS File
            string strAdaptorReturnFile = xdoc.Root.Element("adaptorreturn").Value;
            string strDestApp = xdoc.Root.Descendants("applications").Descendants("destination").Elements("name").First().Value;
            string strDSSFile = xdoc.Root.Descendants("applications").Descendants("source").Elements("inputfile").First().Value;
            var inputs = from i in xdoc.Root.Descendants("event").Descendants("input").Descendants("timeseries")
                         select i;
            if (inputs.Count() > 0)
            {
                frmProgress frm = new frmProgress("Please wait while extracting time-series from source application...", strAdaptorReturnFile);
                frm.ctrProg.Value = 0;
                frm.ctrProg.Maximum = inputs.Count();
                frm.Show();
                System.Windows.Forms.Application.DoEvents(); // refreshing
                foreach (var inp in inputs) // loop through inputs and get attribute into object
                {
                    System.Windows.Forms.Application.DoEvents(); // refreshing
                    if (Commons.ReallyAbortProcess(frm)) break; // abort the run operation
                    DTTCommandTSInputOuput objInput = new DTTCommandTSInputOuput();
                    objInput.ScenarioID = inp.Element("scenarioid").Value;
                    objInput.TimeSeriesID = inp.Element("timeseriesid").Value;
                    objInput.SiteName = inp.Element("sitename").Value;
                    objInput.Application = inp.Element("timeseriesapp").Value;
                    objInput.Parameter = inp.Element("parameter").Value;
                    objInput.Frequency = inp.Element("frequency").Value;
                    DTTFunction fun = new DTTFunction();
                    fun.FunctionName = inp.Element("function").Value;
                    fun.FunctionValue = inp.Element("functionvalue").Value;
                    fun.WarmingPeriod = inp.Element("warming").Value; // warming period
                    objInput.Function = fun; // function
                    objInput.FactorOperator = inp.Element("factoroperator").Value; // factor operator
                    objInput.Factor = float.Parse(inp.Element("factor").Value);
                    objInput.StartDate = DateTime.Parse(inp.Element("startdate").Value);
                    objInput.EndDate = DateTime.Parse(inp.Element("enddate").Value);
                    objInput.Flag = inp.Element("flag").Value;
                    objInput.SaveTo = inp.Element("saveto").Value;

                    // prepare DSS 2 CSV via DSSUTL macro
                    bool blnSuccess = ExtractDSS2XMLviaMACRO(_config, strDSSFile, objInput);

                    frm.ctrProg.Value++; // keep progressing
                }
                frm.Close();
            } // end of testing result list
            //xdoc.Save(strInputXML); // save the command file back
        }
        #endregion Retrieving Time-series from DSS for CF

        #region Load DSC file to get all catalog data
        /// <summary>
        /// Processsing XML input command
        /// </summary>
        public void PorcessGeneralXMLCommand()
        {
            XDocument xdoc = XDocument.Load(_strInputXMLFile); // read xml document
            string strError = "", strFileName = "", strCommand = "";
            // this is for source application model
            var commands = from c in xdoc.Descendants("command")
                           select c;

            string strDSSFile = xdoc.Root.Descendants("applications").Descendants("source").Elements("inputfile").First().Value;
            // loop through each scenario list
            foreach (var cmd in commands)
            {
                strFileName = cmd.Attribute("saveto").Value;
                strCommand = cmd.Attribute("name").Value;
                if (strCommand == CommonUtils.GET_SUMMARY_TIMESERIES_LIST) // get summary time-series into knowledge base
                {
                    string strScenarioID = cmd.Attribute("scenarioid").Value;
                    DateTime datStart = DateTime.Parse(cmd.Attribute("startdate").Value);
                    DateTime datEnd = DateTime.Parse(cmd.Attribute("enddate").Value);
                    float fltDefaultFactor = float.Parse(cmd.Attribute("default_factor").Value);
                    // get time-series list
                    bool blnSucceeded = GetSummaryTimeSeriesList(strDSSFile, strFileName, fltDefaultFactor);
                    if (blnSucceeded == false)
                    {
                        Commons.ShowMessage("Error loading time-series list\r\n" + strError, System.Windows.Forms.MessageBoxIcon.Exclamation);
                        break; // exit the process when error
                    }
                }
            } // end of foreach loop of the command
        }
        /// <summary>
        /// Get summary list from DSC file
        /// </summary>
        /// <param name="strMetaFile">txd Meta File</param>
        /// <param name="strSave2XMLFile">Save to XML file</param>
        /// <returns></returns>
        private bool GetSummaryTimeSeriesList(string strDSSFile, string strSave2XMLFile, float fltDefaultFactor)
        {
            // refres DSS catalogue for the last up-to-date
            Commons.RefreshDSSCatalogue(strDSSFile);
            
            XDocument xdoc = XDocument.Load(_strInputXMLFile);
            string strAdaptorReturnFile = xdoc.Root.Element("adaptorreturn").Value;

            // find the catalog file
            string strDSCFile = strDSSFile.Replace(".dss", ".dsc");
            frmProgress frmProg = new frmProgress("Please wait while extracting time-series summary from DSS...", strAdaptorReturnFile);

            frmProg.Show();
            System.Windows.Forms.Application.DoEvents(); // refreshing

            XElement xRoot = new XElement("timeseries"); // time-series
            using (StreamReader readDSC = new StreamReader(strDSCFile)) // start reading file
            {
                int intCounter = 0;
                string strTemp = ""; string strPartExceptDPart="";
                DateTime datStart = DateTime.MaxValue; DateTime datEndPathName = DateTime.MinValue;
                DateTime datActualStart = DateTime.MaxValue; DateTime datActualEndDSC = DateTime.MinValue; // end date from .dsc file
                DateTime datValue = DateTime.Today; DateTime datLastStart = DateTime.Today; // last start

                DateTime datActualEndDate = DateTime.Today;

                string[] astrPart = new string[5];
                XElement station = new XElement("station");
                // loop through the TXD file
                while (readDSC.Peek() != -1)
                {
                    intCounter++;
                    string strLine = readDSC.ReadLine();
                    if (intCounter >= 11)
                    {
                        if (strLine == null) break;
                        int intStartIndex = strLine.IndexOf("/");
                        int intLastIndex = strLine.LastIndexOf("/");
                        
                        //Commons.ShowMessage(strPathName + " :: " + datStart.ToString() + " - " + datEnd.ToString());
                        if (strTemp == "") // temp
                        {
                            strTemp = strPartExceptDPart; // store the temporary
                        }
                        if (strTemp != strPartExceptDPart) // when it's not matched then save it
                        {
                            astrPart = strTemp.Split('/');
                            // get the proper start date
                            string strStartDatPath = "/" + astrPart[1] + "/" + astrPart[2] + "/" + astrPart[3]
                                + "/" + datActualStart.ToString("ddMMMyyyy")
                                + "/" + astrPart[4] + "/" + astrPart[5] + "/";

                            datActualStart = GetActualStartDate(strDSSFile, strStartDatPath); // get actual start date from command line

                            // try to get UNIT and END DATE
                            string strFullPathName = "/" + astrPart[1] + "/" + astrPart[2] + "/" + astrPart[3]
                                            + "/" + datActualEndDSC.ToString("ddMMMyyyy")
                                            + "/" + astrPart[4] + "/" + astrPart[5] + "/";

                            // get unit
                            string strUnit = GetUnitNEndDate(strDSSFile, strFullPathName, ref datActualEndDate); // get unit via pathname
                            
                            // full pathname
                            strFullPathName = "/" + astrPart[1] + "/" + astrPart[2] + "/" + astrPart[3]
                                                + "/" + datActualStart.ToString("ddMMMyyyy") + "-" + datActualEndDate.ToString("ddMMMyyyy")
                                                + "/" + astrPart[4] + "/" + astrPart[5] + "/";

                            #region Add Station to XML
                            // add data to collection
                            station = new XElement("station");
                            station.SetAttributeValue("scenarioid", "");
                            station.SetAttributeValue("timeseriesid", "");
                            station.SetAttributeValue("parta", astrPart[1]);
                            station.SetAttributeValue("sitename", astrPart[2]);
                            station.SetAttributeValue("parameter", astrPart[3]);
                            station.SetAttributeValue("factor", fltDefaultFactor.ToString());
                            station.SetAttributeValue("partd", datActualStart.ToString());
                            station.SetAttributeValue("frequency", astrPart[4]);
                            station.SetAttributeValue("partf", astrPart[5]);
                            station.SetAttributeValue("unit", strUnit);
                            station.SetAttributeValue("timeseriesapp", Commons.THIS_ADAPTOR_NAME);
                            station.SetAttributeValue("startdate", datActualStart.ToString());
                            station.SetAttributeValue("enddate", datActualEndDate.ToString());
                            station.SetAttributeValue("flag", strFullPathName);
                            xRoot.Add(station); // add time-series to station
                            #endregion Add Station to XML

                            // last date start
                            datLastStart = datActualStart;

                            // reset the date and new path
                            //datStart = DateTime.MaxValue;
                            //datEndPathName = DateTime.MinValue;
                            strTemp = strPartExceptDPart; // store the temporary
                        }
                        
                        // start doing the next one
                        string strPathName = strLine.Substring(intStartIndex, intLastIndex - intStartIndex + 1);
                        
                        // split by part
                        astrPart = strPathName.Split('/');
                        strPartExceptDPart = "/" + astrPart[1] + "/" + astrPart[2] + "/" + astrPart[3]
                                             + "/" + astrPart[5] + "/" + astrPart[6] + "/";
                        try
                        {
                            datValue = DateTime.Parse(astrPart[4]);
                            if (strTemp != strPartExceptDPart && strTemp != "")
                            {
                                datActualStart = datStart; // set flag here to make sure that we get the latest and real start date
                                datActualEndDSC = datEndPathName; // flag the actual end date

                                // reset start and end date
                                datStart = DateTime.MaxValue;
                                datEndPathName = DateTime.MinValue;
                                // update the start and end date again
                                if (datStart > datValue) datStart = datValue;
                                if (datEndPathName < datValue) datEndPathName = datValue;

                            }
                            else
                            {
                                if (datStart > datValue) datStart = datValue;
                                if (datEndPathName < datValue) datEndPathName = datValue;
                            }
                        }
                        catch
                        {
                            // do nothing
                        }
                    }
                } // end of loop
                #region add the last one
                station.Remove(); // remove the last one before adding the very last one

                string strFullPathNameLast = "/" + astrPart[1] + "/" + astrPart[2] + "/" + astrPart[3]
                                    + "/" + datEndPathName.ToString("ddMMMyyyy")
                                    + "/" + astrPart[5] + "/" + astrPart[6] + "/";
                // get unit
                string strUnitLast = GetUnitNEndDate(strDSSFile, strFullPathNameLast, ref datActualEndDate); // get unit via pathname

                // full pathname
                strFullPathNameLast = "/" + astrPart[1] + "/" + astrPart[2] + "/" + astrPart[3]
                                    + "/" + datStart.ToString("ddMMMyyyy") + "-" + datEndPathName.ToString("ddMMMyyyy")
                                    + "/" + astrPart[5] + "/" + astrPart[6] + "/";

                // add data to collection
                XElement stationLast = new XElement("station");
                stationLast.SetAttributeValue("scenarioid", "");
                stationLast.SetAttributeValue("timeseriesid", "");
                stationLast.SetAttributeValue("parta", astrPart[1]);
                stationLast.SetAttributeValue("sitename", astrPart[2]);
                stationLast.SetAttributeValue("parameter", astrPart[3]);
                stationLast.SetAttributeValue("factor", fltDefaultFactor.ToString());
                stationLast.SetAttributeValue("partd", datStart.ToString());
                stationLast.SetAttributeValue("frequency", astrPart[5]);
                stationLast.SetAttributeValue("partf", astrPart[6]);
                stationLast.SetAttributeValue("unit", strUnitLast);
                stationLast.SetAttributeValue("timeseriesapp", Commons.THIS_ADAPTOR_NAME);
                stationLast.SetAttributeValue("startdate", datStart.ToString());
                stationLast.SetAttributeValue("enddate", datActualEndDate.ToString());
                stationLast.SetAttributeValue("flag", strFullPathNameLast);
                xRoot.Add(stationLast); // add time-series to station
                #endregion add the last one
            } // end of using read data file
            xRoot.Save(strSave2XMLFile);
            frmProg.ctrProg.Visible = true;
            frmProg.Close();
            return true;
        }
        /// <summary>
        /// Get Unit for each given pathname
        /// </summary>
        /// <param name="strDSSFile"></param>
        /// <param name="strPathname"></param>
        /// <param name="EndDate">return end date</param>
        /// <returns></returns>
        private string GetUnitNEndDate(string strDSSFile, string strPathname, ref DateTime EndDate)
        {
            string strUnit = "No Unit";
            string strUnitFile = Commons.TEMP_FOLDER + "\\unit.txt"; // get unit for each pathname
            using (StreamWriter writeUnit = new StreamWriter(strUnitFile))
            {
                writeUnit.WriteLine("just created...");
            }
            string strDSSUNITMacro = Commons.TEMP_FOLDER + "\\getunit.txt"; // get unit for each pathname
            using (StreamWriter writeRef = new StreamWriter(strDSSUNITMacro))
            {
                writeRef.WriteLine("WR TO=" + Commons.ToShortPathName(strUnitFile) + " " + strPathname); // refresh catalog command
                writeRef.WriteLine("FI");  // finish command
            }
            // write refresh
            Commons.ShellDSSUTLExe(strDSSFile, strDSSUNITMacro); // executing the refresh catalogue file
            using (StreamReader readUnit = new StreamReader(strUnitFile))
            {
                string strLine = readUnit.ReadLine();
                strLine = readUnit.ReadLine();
                if (strLine != null)
                {
                    string[] astrValue = readUnit.ReadLine().Split(';'); // end date
                    string strDate = astrValue[1].Trim().ToLower().Replace("end:", "").Replace("hours", "").Replace("at", "").Trim();


                    strLine = readUnit.ReadLine(); // unit stays in line 4
                    strUnit = strLine.Substring(6, 13);
                    strDate = Commons.RemoveWhiteSpaces(strDate); // date time

                    astrValue = strDate.Split(' '); // value
                    string strDateTimeValue = astrValue[0] + " " + astrValue[1].Substring(0, 2) + ":" + astrValue[1].Substring(2);
                    // end date
                    EndDate = Commons.FormatHEC2Date(strDateTimeValue);
                }
            }
            return strUnit.Trim();
        }
        /// <summary>
        /// Get actual start date
        /// </summary>
        /// <param name="strDSSFile"></param>
        /// <param name="strPathname"></param>
        /// <param name="EndDate">Return End date</param>
        /// <returns></returns>
        private DateTime GetActualStartDate(string strDSSFile, string strPathname)
        {
            DateTime datStart = DateTime.Today;
            string strStartFile = Commons.TEMP_FOLDER + "\\unit.txt"; // get unit for each pathname
            using (StreamWriter writeUnit = new StreamWriter(strStartFile))
            {
                writeUnit.WriteLine("just created...");
            }
            string strDSSUNITMacro = Commons.TEMP_FOLDER + "\\getstart.txt"; // get unit for each pathname
            using (StreamWriter writeRef = new StreamWriter(strDSSUNITMacro))
            {
                writeRef.WriteLine("WR TO=" + Commons.ToShortPathName(strStartFile) + " " + strPathname); // refresh catalog command
                writeRef.WriteLine("FI");  // finish command
            }
            // write refresh
            Commons.ShellDSSUTLExe(strDSSFile, strDSSUNITMacro); // executing the refresh catalogue file
            using (StreamReader readUnit = new StreamReader(strStartFile))
            {
                string strLine = readUnit.ReadLine();
                strLine = readUnit.ReadLine();
                if (strLine != null)
                {
                    string[] astrValue = readUnit.ReadLine().Split(';'); // end date
                    string strDate = astrValue[0].Trim().ToLower().Replace("start:", "").Replace("hours", "").Replace("at", "").Trim();
                    strDate = Commons.RemoveWhiteSpaces(strDate); // date time

                    astrValue = strDate.Split(' '); // value
                    string strDateTimeValue = astrValue[0] + " " + astrValue[1].Substring(0, 2) + ":" + astrValue[1].Substring(2);
                    // end date
                    datStart = Commons.FormatHEC2Date(strDateTimeValue);
                }
            }
            return datStart;
        }
        #endregion Load TXD Meta-data to XML as Summary Timeseries

        #region extract macro file
        /// <summary>
        /// prepare DSS to csv file macro file
        /// </summary>
        /// <param name="strDSSFile">DSSFile name</param>
        /// <param name="input"></param>
        private bool ExtractDSS2XMLviaMACRO(DTTXMLConfig config, string strDSSFile, DTTCommons.DTTCommandTSInputOuput input)
        {
            string strCSVFile = Commons.TEMP_FOLDER + "\\dss2csv.csv";
            using (StreamWriter csvTemp = new StreamWriter(strCSVFile))
            {
                csvTemp.WriteLine("just created...");
            }
            // write macro file
            string strMacroFile = Commons.TEMP_FOLDER + "\\dssout.txt";
            using (StreamWriter macroFile = new StreamWriter(strMacroFile))
            {
                macroFile.WriteLine("EV A=" + input.Flag);
                macroFile.WriteLine("EF.M -9999");
                macroFile.WriteLine("EF [T:1][DATE:DDMMMYYYY] [T:11][TIME:HH:MM;] [A:20.3]"); // write content of the file
                macroFile.WriteLine("TIME " + Commons.FormatDATE2HEC(input.StartDate) + " " + Commons.FormatDATE2HEC(input.EndDate));
                macroFile.WriteLine("EXP " + Commons.ToShortPathName(strCSVFile));
            }
            // write refresh
            bool blnSuccess = Commons.ShellDSSUTLExe(strDSSFile, strMacroFile); // executing the refresh catalogue file
            if (blnSuccess)
            {
                // now need to convert to CSV to XML common format
                return TransformCSV2XML(config, strCSVFile, input);
            }
            else
            {
                Commons.ShowMessage("Failed to extract time-series file '" + input.Flag + "'");
                return false;
            }
        }
        /// <summary>
        /// Tranform the CSV file to XML file
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool TransformCSV2XML(DTTXMLConfig config, string strCSVFile, DTTCommons.DTTCommandTSInputOuput input)
        {
            try
            {
                XElement xTS = new XElement("timeseries");
                using (StreamReader readCSV = new StreamReader(strCSVFile)) // read CSV file
                {
                    while (readCSV.Peek() != -1) // loop through
                    {
                        string[] astrLine = readCSV.ReadLine().Split(';');
                        float fltValue = (CommonUtils.IsMissingValue(config, astrLine[1].Trim()) ? CommonUtils.MISSING_VALUE : float.Parse(astrLine[1].Trim()));
                        if (fltValue != CommonUtils.MISSING_VALUE) // when it's not a missing value
                        {
                            fltValue = (input.FactorOperator.ToLower() == CommonUtils.FACTOR_OPERATOR_ADD ?
                                fltValue + input.Factor : fltValue * input.Factor);
                        }
                        XElement xRowValue = new XElement("rowvalue");
                        xRowValue.SetAttributeValue("date", Commons.FormatHEC2Date(astrLine[0])); // date time
                        xRowValue.SetAttributeValue("value", fltValue.ToString("0.000")); // value
                        xTS.Add(xRowValue);
                    }
                }
                xTS.Save(input.SaveTo); // save to XML file

                if (input.Function.FunctionName.ToLower() == TIMESERIS_FUNCTION.AVERAGE.ToString().ToLower())
                {
                    CommonUtils.GetFrequencyAverageTS(config, input); // calculate frequency input function
                }
                else if (input.Function.FunctionName.ToLower() == TIMESERIS_FUNCTION.AVG_Freq_Multiplier.ToString().ToLower()
                        || input.Function.FunctionName.ToLower() == TIMESERIS_FUNCTION.MA_Freq_Multiplier.ToString().ToLower())
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(input.SaveTo); // load XML into data table

                    string strError = ""; // error string
                    CommonUtils.GetTimeSeriesAVG_MA_Multiplier(config, ds.Tables[0], "date", "value", input, out strError);
                }
                else
                {
                    // NONE function and this is the same as orginal file in the first save so do nothing
                }

                return true;
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Failed tranforming data from CSV file to XML for '" + input.Flag + "'\r\n\r\n" + ex.Source + ": " + ex.Message);
                return false;
            }
        }
        #endregion extract macro file
    }
}
