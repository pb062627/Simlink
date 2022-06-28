/*
 * 
 * 
 * Adaptor4HECDSS.exe - This adaptor is for HEC-DSS Vue data file (dss)
 * Convert data from CF -> DSS
 * 
 * Created by: Leng Dieb 19/Apr/2010
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;
using DTTCommons;

namespace Adaptor4HecDSS
{
    public class CF2DSS
    {
        #region Private Variables
        private DTTXMLConfig _config;
        private string _strXMLCatalogueFile;
        #endregion Private Variables

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config"></param>
        public CF2DSS(DTTXMLConfig config)
        {
            _config = config;
        }
        #endregion Constructors

        #region Public Methods
        /// <summary>
        /// Load XML commands and start writing data to DSS file using DSSUTL
        /// </summary>
        /// <param name="strCmdXMLFile">XML Command file</param>
        /// <returns></returns>
        public void ConvertCF2DSS(string strCmdXMLFile)
        {
            XDocument xdoc = XDocument.Load(strCmdXMLFile);
            string strSiteName = "";
            try
            {
                string strAdaptorReturnFile = xdoc.Root.Element("adaptorreturn").Value;
                string strDSSFile = xdoc.Root.Descendants("applications").Descendants("destination").Elements("outputfile").First().Value;

                // create a new dss file when needed
                CreateNewDSSFile(strDSSFile);

                // transform XML catalogue to DSS file
                TransformCatalogue2XML(strDSSFile); // transform catalogue to XML 

                // reading CF to DSS file
                var inout = from c in xdoc.Root.Descendants("timeseries_commands").Descendants("event")
                            select c;
                frmProgress frm = new frmProgress("Please wait while saving common format time-series to DSS format...", strAdaptorReturnFile);
                frm.ctrProg.Value = 0;
                frm.ctrProg.Minimum = 0;
                frm.ctrProg.Maximum = inout.Count() + 1;
                frm.Show();

                DialogResult dlfReturnConfirm = DialogResult.OK;
                System.Windows.Forms.Application.DoEvents(); // refresh
                // loop through inputs
                foreach (var obj in inout)
                {
                    System.Windows.Forms.Application.DoEvents(); // refresh
                    if (Commons.ReallyAbortProcess(frm)) return; // exit on return
                    // reading all inputs before extracting to a new output
                    var inputs = from i in obj.Descendants("input").Descendants("timeseries")
                                 select i;

                    // input from xml file
                    DTTCommandTSInputOuput[] ainput = new DTTCommandTSInputOuput[inputs.Count()];
                    int intIndex = 0;
                    foreach (var inp in inputs)
                    {
                        DTTCommandTSInputOuput input = new DTTCommandTSInputOuput();
                        input.Parameter = inp.Element("parameter").Value;
                        input.Unit = inp.Element("unit").Value; // unit
                        input.SaveTo = inp.Element("saveto").Value;
                        ainput[intIndex] = input;
                        intIndex++; // input file
                    }
                    // reading output file
                    DTTCommandTSInputOuput output = new DTTCommandTSInputOuput(); // command output
                    var outputs = from o in obj.Descendants("output").Descendants("timeseries")
                                  select o;

                    foreach (var outs in outputs)
                    {
                        output.SiteName = outs.Element("destsitename").Value; // PART B
                        output.Parameter = outs.Element("destparameter").Value; // Part C
                        output.SaveTo = outs.Element("saveto").Value;
                        output.Remarks = outs.Element("destremarks").Value;
                        output.Frequency = outs.Element("destfrequency").Value; // Part E
                        output.FileType = outs.Element("destfiletype").Value;
                        output.Unit = outs.Element("destunit").Value;
                        output.StartDate = DateTime.Parse(outs.Element("startdate").Value);
                        output.EndDate = DateTime.Parse(outs.Element("enddate").Value);
                        output.PartA = outs.Element("destparta").Value; // part A
                        output.PartF = outs.Element("destpartf").Value; // part F
                        strSiteName = output.SiteName; // sitename for error log
                    }

                    string[] astrInputFile = new string[ainput.Length]; // input file
                    for (int intLoop = 0; intLoop <= ainput.GetUpperBound(0); intLoop++)
                    {
                        astrInputFile[intLoop] = ainput[intLoop].SaveTo;
                    }
                    
                    // other parameter
                    string strCheckSum = CommonUtils.MergeCF2OneXMLFile(_config, astrInputFile, output, false);

                    // check pathname for duplication
                    bool blnExists = IsPathnameDuplicated(strDSSFile, output, ref dlfReturnConfirm);

                    if ((blnExists == false) ||
                        (blnExists && (dlfReturnConfirm == DialogResult.Yes || dlfReturnConfirm == DialogResult.OK)))
                    {
                        // transform to XML file
                        bool blnSuccess = TransformCFXML2DSS(strDSSFile, output);

                        if (blnSuccess)
                        {
                            // Add check-sum value to XML
                            foreach (var outs in outputs)
                            {
                                outs.Element("checksum").SetValue(strCheckSum); // add check-sum to XML
                            }
                        }
                        else
                        {
                            Commons.ShowMessage("Failed importing time-series for site '" + strSiteName + "'");
                        }
                        xdoc.Save(strCmdXMLFile); // save back data with check-sum value
                    }
                    frm.ctrProg.Value++; // increase the progress bar
                }
            }
            catch (Exception ex)
            {
                string strError = "Error writing data to DSS file for sitename '" + strSiteName + "'\r\n" + ex.Source + ": " + ex.Message;
                throw new Exception(strError);
            }
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Check to figure out if pathname was duplicated or not
        /// </summary>
        /// <param name="strDSSFile">DSS File</param>
        /// <param name="output">Ouput document</param>
        /// <returns></returns>
        private bool IsPathnameDuplicated(string strDSSFile, DTTCommandTSInputOuput output, ref DialogResult dlgConfirm)
        {
            XDocument xdoc = XDocument.Load(_strXMLCatalogueFile);
            IEnumerable<XElement> xParts = from x in xdoc.Root.Elements("station")
                                           where x.Attribute("parta").Value.ToLower() == output.PartA.ToLower()
                                           && x.Attribute("partb").Value.ToLower() == output.SiteName.ToLower()
                                           && x.Attribute("partc").Value.ToLower() == output.Parameter.ToLower()
                                           && x.Attribute("parte").Value.ToLower() == output.Frequency.ToLower()
                                           && x.Attribute("partf").Value.ToLower() == output.PartF.ToLower()
                                           && DateTime.Parse(x.Attribute("startdate").Value) >= output.StartDate
                                           select x;
            if (xParts.Count() > 0) // part
            {
                string strPathname = "/" + output.PartA + "/" + output.SiteName + "/" + output.Parameter
                                     + "/" + output.StartDate.ToString("ddMMMyyyy") + "-" + output.EndDate.ToString("ddMMMyyyy")
                                     + "/" + output.Frequency + "/" + output.PartF + "/";
                if (dlgConfirm != DialogResult.Yes)
                {
                    frmConfirmation frmConfirm = new frmConfirmation("'" + strPathname + "' already exists!\r\n\r\nDo you wish to replace the existing one?");
                    dlgConfirm = frmConfirm.ShowDialog();
                }
                return true; // record exists
            }
            else
            {
                // add to XML catalogue when it does not exist yet
                XElement station = new XElement("station");
                station.SetAttributeValue("parta", output.PartA);
                station.SetAttributeValue("partb", output.SiteName);
                station.SetAttributeValue("partc", output.Frequency);
                station.SetAttributeValue("parte", output.Frequency);
                station.SetAttributeValue("partf", output.PartF);
                station.SetAttributeValue("startdate", output.StartDate);
                station.SetAttributeValue("enddate", output.EndDate);
                xdoc.Root.Add(station);
                xdoc.Save(_strXMLCatalogueFile); // save the file back to XML catalogue
                return false;
            }
        }
        /// <summary>
        /// Transform Catalogue to XML file
        /// </summary>
        /// <param name="strDSSFile"></param>
        /// <returns></returns>
        private bool TransformCatalogue2XML(string strDSSFile)
        {
            try
            {
                _strXMLCatalogueFile = Commons.TEMP_FOLDER + "\\catalogue.xml";

                // refres DSS catalogue for the last up-to-date
                Commons.RefreshDSSCatalogue(strDSSFile);

                // find the catalog file
                string strDSCFile = strDSSFile.Replace(".dss", ".dsc");

                XElement xRoot = new XElement("timeseries"); // time-series
                using (StreamReader readDSC = new StreamReader(strDSCFile)) // start reading file
                {
                    int intCounter = 0;
                    string strTemp = ""; string strPartExceptDPart = "";
                    DateTime datStart = DateTime.Today; DateTime datEnd = DateTime.Today;
                    // loop through the TXD file
                    while (readDSC.Peek() != -1)
                    {
                        intCounter++;
                        string strLine = readDSC.ReadLine();
                        if (intCounter >= 11)
                        {
                            strLine = readDSC.ReadLine();// read data line
                            if (strLine == null) break;
                            int intStartIndex = strLine.IndexOf("/");
                            int intLastIndex = strLine.LastIndexOf("/");
                            string strPathName = strLine.Substring(intStartIndex, intLastIndex - intStartIndex + 1);

                            // split by part
                            string[] astrPart = strPathName.Split('/');
                            strPartExceptDPart = astrPart[1] + "/" + astrPart[2] + "/" + astrPart[3]
                                                 + "/" + astrPart[5] + "/" + astrPart[6];
                            if (strTemp == "") // temp
                            {
                                datStart = DateTime.Parse(astrPart[4]);
                                strTemp = strPartExceptDPart; // store the temporary
                            }
                            if (strTemp != strPartExceptDPart) // when it's not matched then save it
                            {
                                string strFullPathName = "/" + astrPart[1] + "/" + astrPart[2] + "/" + astrPart[3]
                                    + "/" + datStart.ToString("ddMMMyyyy") + "-" + datEnd.ToString("ddMMMyyyy")
                                    + "/" + astrPart[5] + "/" + astrPart[6] + "/";

                                // add data to collection
                                XElement station = new XElement("station");
                                station.SetAttributeValue("parta", astrPart[1]);
                                station.SetAttributeValue("partb", astrPart[2]);
                                station.SetAttributeValue("partc", astrPart[3]);
                                station.SetAttributeValue("parte", astrPart[5]);
                                station.SetAttributeValue("partf", astrPart[6]);
                                station.SetAttributeValue("startdate", datStart);
                                station.SetAttributeValue("enddate", datEnd);
                                station.SetAttributeValue("flag", strFullPathName);
                                xRoot.Add(station); // add time-series to station
                                // reset the date and new path
                                datStart = DateTime.Parse(astrPart[4]);
                                strTemp = strPartExceptDPart; // store the temporary
                            }
                            datEnd = DateTime.Parse(astrPart[4]); // date end
                        }
                    } // end of loop
                } // end of using read data file
                xRoot.Save(_strXMLCatalogueFile);
                return true;
            }
            catch (Exception ex)
            {
                Commons.ShowMessage("Error building catalogue file to XML " + ex.Source + ": " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Check and create a dss file when it does not exist
        /// </summary>
        /// <param name="strDSSFile"></param>
        private void CreateNewDSSFile(string strDSSFile)
        {
            if (File.Exists(strDSSFile) == false) // create a dss file
            {
                // create this just to make sure that ToShortPathName working
                using (StreamWriter write = new StreamWriter(strDSSFile))
                {
                    write.WriteLine("testing...");
                }
                string strDOSDSSPath = Commons.ToShortPathName(strDSSFile);
                // delete the temp
                File.Delete(strDSSFile); // delete file

                string strCreateNewDSSFileInput = Commons.TEMP_FOLDER + "\\newdss.txt";
                using (StreamWriter writeMacro = new StreamWriter(strCreateNewDSSFileInput))
                {
                    writeMacro.WriteLine(strDOSDSSPath);
                }
                Commons.ShellDSSUTLExe(strCreateNewDSSFileInput); // create dss file using DSSUTL
            }
        }
        /// <summary>
        /// Convert the common format to DSS using the DSSUTL
        /// We need to initially transform to CSV file first and then using DSSUTL to write to dss
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private bool TransformCFXML2DSS(string strDSSFile, DTTCommandTSInputOuput output)
        {
            string strCSVFile = Commons.TEMP_FOLDER + "\\indss.csv";
            using (StreamWriter writer = new StreamWriter(strCSVFile))
            {
                writer.WriteLine("A"); // column header
                // start reading data from a common format
                XDocument xdoc = XDocument.Load(output.SaveTo);
                IEnumerable<XElement> rowVals = from c in xdoc.Root.Descendants("rowvalue")
                                                select c;
                // write each row data
                foreach (XElement row in rowVals)
                {
                    writer.WriteLine(row.Attribute("value").Value);
                }
            }
            // now prepare macro file to import the CSV file
            string strMacroIn = Commons.TEMP_FOLDER + "\\inmacro.txt";
            using (StreamWriter writer = new StreamWriter(strMacroIn))
            {
                string strPathname = "/" + output.PartA + "/" + output.SiteName + "/" + output.Parameter 
                                    + "/" + output.StartDate.ToString("ddMMMyyyy") + "/" + output.Frequency + "/" + output.PartF + "/";
                writer.WriteLine("TI " + Commons.FormatDATE2HEC(output.StartDate) + " " + Commons.FormatDATE2HEC(output.EndDate));
                writer.WriteLine("EV A=" + strPathname + " UNITS=" + output.Unit + " TYPE=" + output.FileType);
                writer.WriteLine("EF.M -9999"); // defining a missing value
                writer.WriteLine("EF.H SKIP 1");
                writer.WriteLine("EF [A]");
                writer.WriteLine("IMP " + Commons.ToShortPathName(strCSVFile));
                writer.WriteLine("CL");
                writer.WriteLine("FI");
            }
            return Commons.ShellDSSUTLExe(strDSSFile, strMacroIn); // executing the refresh catalogue file
        }
        #endregion Private Methods
    }
}
