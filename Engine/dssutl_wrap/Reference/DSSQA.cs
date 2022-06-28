/*
 * 
 * 
 * HEC-DSS Adaptor for data transfer tool. QA class
 * 
 * Created by: Leng Dieb 30/Apr/2010
 * 
 * Copyright        2010 by Halcrow Group Ltd
 *                  All rights reserved.
 * 
 * This code is not to be distributed without the written 
 * permission of Halcrow Group Ltd
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DTTCommons;

namespace Adaptor4HecDSS
{
    public class DSSQA
    {
        #region Constructors
        /// <summary>
        /// HEC-DSS Q.A
        /// </summary>
        public DSSQA()
        {
        }
        #endregion Constructors

        #region Q.A against XML Common File
        /// <summary>
        /// Process Q.A against the dss file file
        /// </summary>
        /// <param name="strDTTXMLFile"></param>
        /// </summary>
        /// <returns></returns>
        public bool ProcessDSSQA(string strDTTXMLFile)
        {
            try // try Q.A
            {
                bool blnPassed = true; // set to pass by default
                XDocument xdoc = XDocument.Load(strDTTXMLFile);     // load XML document 
                // get history into tag
                XElement xHistory = new XElement("history"); // history
                var history = from h in xdoc.Root.Descendants("history")
                              select h; // history
                foreach (var his in history)
                {
                    xHistory = (XElement)his;
                }
                string strApplication = xdoc.Root.Descendants("applications").Descendants("source").Elements("name").First().Value;
                string strScenarioID = xdoc.Root.Descendants("applications").Descendants("destination").Elements("scenarioid").First().Value;
                string strDSSFile = xdoc.Root.Descendants("applications").Descendants("destination").Elements("outputfile").First().Value;
                
                // Q.A Element to be added back when it failed the Q.A test
                XElement qa = new XElement("event");
                var dtt = from c in xdoc.Root.Descendants("timeseries_commands").Descendants("output")
                          select c;
                
                frmProgress frm = new frmProgress("Please wait while processing Q.A...");
                frm.ctrProg.Value = 0;
                frm.ctrProg.Maximum = dtt.Count();
                frm.Show();
                System.Windows.Forms.Application.DoEvents(); // refreshing

                foreach (var outs in dtt)
                {
                    DTTCommandTSInputOuput output = new DTTCommandTSInputOuput();
                    string strCheckSumFromProject = outs.Element("timeseries").Element("checksum").Value;
                    output.SiteName = outs.Element("timeseries").Element("destsitename").Value; // PART B
                    output.Parameter = outs.Element("timeseries").Element("destparameter").Value; // Part C
                    output.SaveTo = outs.Element("timeseries").Element("saveto").Value;
                    output.Remarks = outs.Element("timeseries").Element("destremarks").Value;
                    output.Frequency = outs.Element("timeseries").Element("destfrequency").Value; // Part E
                    output.FileType = outs.Element("timeseries").Element("destfiletype").Value;
                    output.Unit = outs.Element("timeseries").Element("destunit").Value;
                    output.StartDate = DateTime.Parse(outs.Element("timeseries").Element("startdate").Value);
                    output.EndDate = DateTime.Parse(outs.Element("timeseries").Element("enddate").Value);
                    output.PartA = outs.Element("timeseries").Element("destparta").Value; // part A
                    output.PartF = outs.Element("timeseries").Element("destpartf").Value; // part F

                    output.Flag = "/" + output.PartA + "/" + output.SiteName + "/" + output.Parameter
                                  + "/" + output.StartDate.ToString("ddMMMyyyy")
                                  + "/" + output.Frequency + "/" + output.PartF + "/"; 
                    string strSiteName = output.SiteName; // sitename for error log

                    // prepare DSS 2 CSV via DSSUTL macro with Q.A edition
                    string strCheckSum = ExtractDSS2XMLCommonFormat(strDSSFile, output);
                    
                    // extract time-series and get check-sum for verification
                    //string strCheckSum = ExtractKBDetailTS(strTSID, strScenarioID, strApplication, strParameter, datStart, datEnd);
                    if (strCheckSumFromProject != strCheckSum) // failed
                    {
                        blnPassed = false; // not pass at least one time-series failed to match checksum value
                        qa.SetAttributeValue("status", "FAILED"); // failed
                        qa.Add(outs);    // add failure output to history status 
                    }
                    frm.ctrProg.Value++;
                }
                // Q.A date and username
                qa.SetAttributeValue("date", DateTime.Now); // Q.A date
                qa.SetAttributeValue("by", Environment.GetEnvironmentVariable("USERNAME")); // Q.A date
                if (blnPassed) // Pass the test case
                {
                    qa.SetAttributeValue("status", "PASSED"); // passed Q.A Test
                }
                xHistory.Add(qa); // add QA event into history
                xdoc.Save(strDTTXMLFile); // save back to project file
                frm.Close(); // close dialog
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in processing HEC-DSS Q.A " + ex.Source + ": " + ex.Message);
            }
        }
        #endregion Q.A against XML Common File

        #region Extract Macro to XML File
        /// <summary>
        /// prepare DSS to csv file macro file
        /// </summary>
        /// <param name="strDSSFile">DSSFile name</param>
        /// <param name="input"></param>
        private string ExtractDSS2XMLCommonFormat(string strDSSFile, DTTCommons.DTTCommandTSInputOuput input)
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
                macroFile.WriteLine("EF [T:1][DATE:DDMMMYYYY] [T:11][TIME:HH:MM;] [A:20.3]"); // write content of the file
                macroFile.WriteLine("TIME " + Commons.FormatDATE2HEC(input.StartDate) + " " + Commons.FormatDATE2HEC(input.EndDate));
                macroFile.WriteLine("EXP " + Commons.ToShortPathName(strCSVFile));
            }
            // write refresh
            bool blnSuccess = Commons.ShellDSSUTLExe(strDSSFile, strMacroFile); // executing the refresh catalogue file
            if (blnSuccess)
            {
                // now need to convert to CSV to XML common format
                return TransformCSV2XML(strCSVFile, input);
            }
            else
            {
                Commons.ShowMessage("Failed to extract time-series file '" + input.Flag + "'");
                return "";
            }
        }
        /// <summary>
        /// Tranform the CSV file to XML file
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string TransformCSV2XML(string strCSVFile, DTTCommandTSInputOuput output)
        {
            try
            {
                XElement xTS = new XElement("timeseries");
                using (StreamReader readCSV = new StreamReader(strCSVFile)) // read CSV file
                {
                    while (readCSV.Peek() != -1) // loop through
                    {
                        string[] astrLine = readCSV.ReadLine().Split(';');
                        float fltValue = float.Parse(astrLine[1].Trim()); // floating value

                        XElement xRowValue = new XElement("rowvalue");
                        xRowValue.SetAttributeValue("date", Commons.FormatHEC2Date(astrLine[0])); // date time
                        xRowValue.SetAttributeValue("value", fltValue.ToString("0.000")); // value
                        xTS.Add(xRowValue);
                    }
                }
                xTS.Save(output.SaveTo); // save to XML file
                
                // check-sum
                return CommonUtils.CalculateChecksum(output.SaveTo);
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Failed tranforming data from CSV file from CSV to XML for '" + output.Flag + "'\r\n\r\n" + ex.Source + ": " + ex.Message);
                return "";
            }
        }
        #endregion Extract Macro to XML File
    }
}
