using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using muMathParser;  

namespace SIM_API_LINKS
{
    public partial class simlink
    {
        //this requires a SimLink reference, so why not just include in this class?
        #region MathParser
        public string _sPARSE_COHORT_CODE = "!COH!";        //use to identify special cohort opertaions.
        #region QuickParse

        public static double QuickParse(string sExpression)
        {
            sExpression = Quickparse_ProcessParentheses(sExpression);
            bool bErr = false;
            double dTheVal = Convert.ToDouble(QuickParse_Clean(sExpression, out bErr));                     // no parentheses
            if (bErr)
            {
                //todo           - log it
            }

            return dTheVal;
        }

        //v1- nested parentheses not supported!!!
        //idea - create an object where you can track info done preivously
        private static string Quickparse_ProcessParentheses(string sExpression)
        {
            bool bERR = false;
            int nPosCounter = 0;
            while (sExpression.IndexOf('(') >= 0)
            {
                int nLoc1 = sExpression.IndexOf('(', nPosCounter);
                nPosCounter = sExpression.IndexOf(')', nPosCounter);
                string sVal = QuickParse_Clean(sExpression.Substring(nLoc1 + 1, nPosCounter - nLoc1 - 1), out bERR);
                if (bERR)
                {
                    //todo: log the issue
                    return "-666.66666";
                }
                else
                {
                    string sReplace = sExpression.Substring(nLoc1, nPosCounter - nLoc1 + 1);
                    sExpression = sExpression.Replace(sReplace, sVal);
                }
            }
            return sExpression;
        }

        private static string SubstituteScientific(string sExpression, out bool bHasScientific)
        {
            bHasScientific = false;
            if (sExpression.IndexOf("E-") > 0)
            {
                sExpression = sExpression.Replace("E-", "XX");
                bHasScientific = true;
            }
            return sExpression;
        }

        private static void SubstituteBackScientific(ref string[] sVals)
        {
            for (int i = 0; i < sVals.Length; i++)
            {
                sVals[i] = sVals[i].Replace("XX", "E-");
            }
        }

        //no parentehss in this expression
        //naive evaluation from left to right
        private static string QuickParse_Clean(string sExpression, out bool bErr)
        {
            char[] sDelimiter = new char[] { '+', '?', '*', '/', '^' };
            try
            {
                bErr = false; bool bHasScientific = false;
                sExpression = SubstituteScientific(sExpression, out bHasScientific);                                //need to trick this silly parser.
                string[] sVals = sExpression.Split(sDelimiter);
                if (bHasScientific)
                {
                    SubstituteBackScientific(ref sVals);
                }


                int nNumberOfTerms = sVals.Length;
                if (nNumberOfTerms == 0)
                {
                    return sExpression;         //no terms: easy
                }

                int nCurrentPos = 0;
                double dTheVal = 0;
                for (int i = 0; i < nNumberOfTerms - 1; i++)
                {
                    nCurrentPos = sExpression.IndexOfAny(sDelimiter, nCurrentPos);
                    string sDelimiter1 = sExpression.Substring(nCurrentPos, 1);
                    nCurrentPos += 1;

                    switch (sDelimiter1)
                    {
                        case "+":
                            dTheVal = Convert.ToDouble(sVals[i]) + Convert.ToDouble(sVals[i + 1]);
                            break;
                        case "?":
                            dTheVal = Convert.ToDouble(sVals[i]) - Convert.ToDouble(sVals[i + 1]);          //use a question mark to avoid dup with negative sign.
                            break;
                        case "*":
                            dTheVal = Convert.ToDouble(sVals[i]) * Convert.ToDouble(sVals[i + 1]);
                            break;
                        case "/":
                            dTheVal = Convert.ToDouble(sVals[i]) / Convert.ToDouble(sVals[i + 1]);
                            break;
                    }
                    sVals[i + 1] = dTheVal.ToString();     // this will become the next first argument
                }
                return sVals[nNumberOfTerms - 1];
            }
            catch (Exception EX)
            {
                bErr = true;
                return "-666";
            }
        }


        #endregion

        //todo: error handling, including pass back exception
        public double Parse_Expression(string sVal)
        {
            muMathParser.Parser m_parser = new muMathParser.Parser(muMathParser.Parser.EBaseType.tpDOUBLE);
            muMathParser.ParserVariable m_ans = new ParserVariable(0);
            //    m_parser = new muMathParser.Parser(muMathParser.Parser.EBaseType.tpDOUBLE);
            m_parser.SetExpr(sVal);
            m_ans.Value = m_parser.Eval();

            return Convert.ToDouble(m_ans.Value);
        }

#region cohort parse
        /// <summary>
        /// Process special function call associated with cohort runs.
        ///             VERSION 1 (ACTIVE) : only support one cohort operation
        ///             VERSION 2: (TODO) : go head and 
        /// </summary>
        /// <param name="sExpression"></param>
        /// <returns></returns>
        public double ParseCohortFunctions(string sExpression, int nScenSequence)
        {
            int nIndex = sExpression.IndexOf(_sPARSE_COHORT_CODE);
            char[] sFuncDelimiter = new char[] { '+', '-', '*', ' ', '/', '>', '<', '=', '(', ')' };    //symbols (incl space) identify end of a function
            while (nIndex >= 0)
            {
                int nEndIndex = sExpression.Substring(nIndex).IndexOfAny(sFuncDelimiter);
                string sCohortFunc = sExpression;
                if(nEndIndex>0)
                    sCohortFunc = sExpression.Substring(nIndex, nEndIndex);
                sCohortFunc = sCohortFunc.Substring(_sPARSE_COHORT_CODE.Length + 1);    // add one for trailing "_"
                // now should have some thing like a function
                string sTheVal = ParseCohort(sCohortFunc, nScenSequence).ToString();
                sExpression = sTheVal;
            }
            if (CommonUtilities.IsDouble(sExpression))
                return Convert.ToDouble(sExpression);
            else
                return CommonUtilities._dBAD_DATA;
        }

        public double ParseCohort(string sCohortFunction, int nScenSequence)
        {
            string[] sParts = sCohortFunction.ToLower().Split('_');         // break up function
            double dReturn = CommonUtilities._dBAD_DATA;
            switch (sParts[0].ToString())
            {
                case "agg":
                    dReturn = ParseCohortAgg(sParts[1], Convert.ToInt32(sParts[2]), nScenSequence);
                    break;
                case "":

                    break;



            }
            return dReturn;
        }

        /// <summary>
        /// iteration 1: assuming that the id passed is a perf id. 
        ///     could be result var, may need to support in the future
        /// </summary>
        /// <param name="sFunc"></param>
        /// <param name="nPerfID"></param>
        /// <returns></returns>
        public double ParseCohortAgg(string sFunc, int nPerfID, int nScenSequence)
        {
            List<double> lstVals = GetValsList(nPerfID, nScenSequence);
            string sReturn = CommonUtilities._nBAD_DATA.ToString();
            double dVal = -1;
            switch (sFunc.ToLower())
            {
                case "max":
                    dVal = lstVals.Max();
                    break;
                case "avg":
                    dVal = lstVals.Max();
                    break;
                case "min":
                    dVal = lstVals.Min();
                    break;
                case "sum":
                    dVal = lstVals.Sum();
                    break;
            }


            return dVal;
        }

        /// <summary>
        /// Return a list populated with the values from scenarios of the same sequence in a cohort
        /// </summary>
        /// <param name="nPerfID"></param>
        /// <param name="nScenSequence"></param>
        /// <returns></returns>
        private List<double> GetValsList(int nPerfID, int nScenSequence)
        {
            List<int> lstScen = _cohortSpec._dictScenXREF[nScenSequence].ToList();  //should be clone
            List<double> lstReturn = new List<double>();
            if(_cohortSpec._bHasSummaryEG)
                lstScen.RemoveAt(lstScen.Count-1);  // drop last record if needed
            foreach (int nScenID in lstScen){
                double dVal = Convert.ToDouble(GetSimLinkDetail(SimLinkDataType_Major.Performance, nPerfID, -1, nScenID, -1));
                lstReturn.Add(dVal);
            }
            return lstReturn;
        }


#endregion

        // 
        //
        //  Wraps around Parse_PrepareExpressionValues and performs check for special case
        //met 8/24" do simple case where just one IF
        //future- nesting?  costly in time.
        public string ParseExpressionWrap(int nScenarioID, int nEvalID, string sExpression, string sArgs = "NONE", int nDVID_FK = -1, int nOptionID = -1)
        {
            string sReturn;
            string sConditional = "#IF";

            if (sExpression.IndexOf(sConditional) >= 0)
            {
                string sLogicalExpression = sExpression.Substring(4, sExpression.IndexOf(",") - 4);
                sReturn = Parse_PrepareExpressionValues(nScenarioID, nEvalID, sLogicalExpression, sArgs, nDVID_FK, nOptionID);
                sReturn = Convert.ToString(Parse_Expression(sReturn));
                bool bLogicalConvert = CommonUtilities.IsTrue_MUParser(sReturn);
                // this is where it will get much more complicated to support recursion
                // not required at this stage
                string sExpressionToExecute = "";    //use separate string for ease of debug
                if (bLogicalConvert)
                {
                    sExpressionToExecute = sExpression.Substring(sExpression.IndexOf(",") + 1, sExpression.LastIndexOf(",") - sExpression.IndexOf(",") - 1);
                }
                else
                {
                    sExpressionToExecute = sExpression.Substring(sExpression.LastIndexOf(",") + 1).Replace(")", string.Empty);
                }
                sReturn = Parse_PrepareExpressionValues(nScenarioID, nEvalID, sExpressionToExecute, sArgs, nDVID_FK, nOptionID);
            }
            else
            {       //no conditional - simply passs along the expression
                sReturn = Parse_PrepareExpressionValues(nScenarioID, nEvalID, sExpression, sArgs, nDVID_FK, nOptionID);
            }
            return sReturn;
        }


        //  the following are the delimiters for identifying variables to look up
        //  _tblModelElementVals_                       //what if there are more than one?
        //  !tblResultVar_Details!
        //  @tblResultTS_EventSummary_Detail@
        //  %tblPerformance_Detail%
        //  &Model Database with 'underscore delimited VarType_FK_ElementID
        //  #- use  this following one of the above characters to indicate: compare to baseline ..... not yet supported
        //  ~ - pull from tblConstants (which is loaded into dictionary on rmg.init
        //  $tblResultTS$       performs operation for each value in the TS


        public string Parse_PrepareExpressionValues(int nScenarioID, int nEvalID, string sExpression, string sArgs="NONE", int nDVID_FK = -1, int nOptionID = -1)
        {

            string sEXP = ""; string sCurrentChunk = "";
            int nCurrentIndex = 0;         //index of the string we are looking at
            int nMatch = 0;
            int nReplaceChars = -1; //amount of characters to replaceF
            bool bIsCompareToBaseline = false;
            bool bExit = false;
            string sReturnVal = "";

            Dictionary<string, string> dictDelimiter = new Dictionary<string, string>();
            dictDelimiter.Add("_", "tblModelElementVals");
            dictDelimiter.Add("!", "tblResultVar_Details");
            dictDelimiter.Add("@", "tblResultTS_EventSummary_Detail");
            dictDelimiter.Add("%", "tblPerformance_Detail");

            char[] sDelimiter = new char[] { '_', '!', '@', '%', '~', '&' };
            char sCurrentDel = '1';

            sExpression = Parse_ProcessArgumentTuple(sExpression, sArgs);         //switch in the args.

            //TODO: improved error handling
            while (!bExit)
            {
                nMatch = sExpression.IndexOfAny(sDelimiter, nCurrentIndex);            //get open var ref
                if (nMatch >= 0)
                {
                    sCurrentDel = sExpression[nMatch];
                    nCurrentIndex = sExpression.IndexOf(sCurrentDel, nMatch + 1);       //get close var ref    (must be same type
                    sEXP = sExpression.Substring(nMatch + 1, nCurrentIndex - nMatch - 1);
                    sReturnVal = Parse_GetVariableReferenceValue(sEXP, sCurrentDel.ToString(), nScenarioID, nDVID_FK, nOptionID);
                    int nAddtlChar = sReturnVal.Length - sEXP.Length;
                    sExpression = sExpression.Substring(0, nMatch) + sReturnVal + sExpression.Substring(nCurrentIndex + 1);
                    nCurrentIndex = nCurrentIndex + nAddtlChar - 1;     //adjust index
                }
                else
                {
                    bExit = true;
                }
            }
            return sExpression;
        }


        // Pass back single value
        public string Parse_EvaluateExpression(int nScenarioID, int nEvalID, string sExpression, string sArgs = "NONE", int nDVID_FK = -1, int nOptionID = -1, bool bCheckForValidDouble = true, bool bSkipPrepare = false)
        {
            if (!bSkipPrepare)
            {
                sExpression = ParseExpressionWrap(nScenarioID, nEvalID, sExpression, sArgs, nDVID_FK, nOptionID);     //todo: check if this duplicating the slREF object!!! cannot pass by ref
            }
            string sReturn = Convert.ToString(Parse_Expression(sExpression));
            if (bCheckForValidDouble)
            {
                double dValOut;
                if (!CommonUtilities.IsDouble(sReturn, out dValOut))
                {
                    //log the issue (once?).
                    sReturn = dValOut.ToString();
                }
            }
            return sReturn;
        }



        //creates and stores a TS that 
        //assumption: resultTS,

        //not yet implemented for baseline... 
        public double[,] ParseTimeSeriesExpression(int nScenarioID, int nEvalID, string sExpression, ref hdf5_wrap hdfBaseline, string sArgs = "NONE", bool bUseQuickParse = false)
        {

            sExpression = Parse_ProcessArgumentTuple(sExpression, sArgs);         //switch in the args.
            int nTS_Count = Convert.ToInt32(sExpression.Length - sExpression.Replace("$", "").Length) / 2;               //number of indices
            bool bIsBaseline = false;
            double[][,] dValsJagged = new double[nTS_Count][,];     // local TS only; different from _dResultTS
            string[] sItem = new string[nTS_Count];
            // step 1, get the TS in the order they appear in the expression.
            int nMatch = 0; int nCurrentIndex = 0; string sGroupName = ""; int nTS_Records;

            for (int i = 0; i < nTS_Count; i++)
            {
                nMatch = sExpression.IndexOf("$", nCurrentIndex);          //get open var ref
                nCurrentIndex = sExpression.IndexOf("$", nMatch + 1);
                sItem[i] = sExpression.Substring(nMatch + 1, nCurrentIndex - nMatch - 1);
                nCurrentIndex = nCurrentIndex + 1;              // sItem[i].Length - 1;
                bIsBaseline = (sItem[i].Substring(0, 1) == "#");                                                  //check for baseine indicator
                sGroupName = CommonUtilities.GetSimLink_TS_GroupName(SimLinkDataType_Major.ResultTS, sItem[i].Replace("$", "").Replace("#", ""));
                //now get the array
                //todo sim2.2: get from memory if possible (and not baseline)
                double[,] dResultVals;
                if (bIsBaseline)
                {
                    dResultVals = hdfBaseline.hdfGetDataSeries(sGroupName, "1");        //look up in baseline TS holder
                }
                else
                {
                    // met 1/25/17: changed to just get from memory. should be in memory. commented old code.
                    dResultVals = GetTS_FromMemory(sGroupName);
                }
                dValsJagged[i] = dResultVals;
            }
            int nMinTSRecords;  //
            ParseTimeSeriesCheckForMissing(ref dValsJagged, out nTS_Records, nTS_Count, out nMinTSRecords);          //nTS_Records: this is the # of records in the TS

            //parse the expression ONCE, and then we will fill in the TS info.
            sExpression = Parse_PrepareExpressionValues(nScenarioID, nEvalID, sExpression, sArgs);
            string sExpressionOrig = sExpression;
            double[,] dReturn = new double[nTS_Records, 1];              //make this this size of Max records- as we SHOULD have data for that size
            for (int i = 0; i < nMinTSRecords; i++)
            {                         //only index up to MIN records to avoid error.      WAS : nTS_Records
                sExpression = sExpressionOrig;
                for (int j = 0; j < nTS_Count; j++)
                {
                    double s = dValsJagged[j][i, 0];
                    string sReplace = "$" + sItem[j] + "$";                             // was  _sExpression_TS_Delimiter;
                    sExpression = sExpression.Replace(sReplace, s.ToString());       //add the double val
                }
                if (!bUseQuickParse)
                {
                    dReturn[i, 0] = Convert.ToDouble(Parse_EvaluateExpression(nScenarioID, nEvalID, sExpression, "NONE", -1, -1, true, true));         //skip the "prepare vals step, which is lengthy
                }
                else
                {
                    dReturn[i, 0] = QuickParse(sExpression);
                }


                //        Parse_Expression(sExpression);;
            }
            return dReturn;
        }

        //todo: add a check for different sized arrays
        private static void ParseTimeSeriesCheckForMissing(ref double[][,] dValsJagged, out int nRecords, int nTS_SeriesCount, out int nMinRecords)
        {
            nRecords = 0; int nMaxRecords = 0; int nActiveArrayRecords = 0; nMinRecords = 2000000000;
            int[] nRecordCount = new int[nTS_SeriesCount];
            for (int i = 0; i < nTS_SeriesCount; i++)
            {
                nRecordCount[i] = dValsJagged[i].GetLength(0);
                if (nRecordCount[i] > nMaxRecords) { nMaxRecords = nRecordCount[i]; }
                if (nRecordCount[i] < nMinRecords) { nMinRecords = nRecordCount[i]; }
            }
            for (int i = 0; i < nTS_SeriesCount; i++)
            {
                if (nRecordCount[i] == 0)
                {
                    dValsJagged[i] = new double[nMaxRecords, 1];        //create an empty array of zeroes.
                }
            }
            nRecords = nMaxRecords;
        }

        private const char _sTupleExpressionDelimiter = '?';

        //{arg1,arg2},{$ID1$,$ID2$}  args tuple which allows you to substitute into the function. This enables one to re-use functions
        private static string Parse_ProcessArgumentTuple(string sExpression, string sArgs)
        {
            if ((sArgs == "NONE") || (sArgs == "0"))
            {
                return sExpression;
            }
            else
            {
                sArgs = sArgs.Replace("{", "").Replace("}", "");  //not needed- just for easier visual input.            
                string[] sTuples = sArgs.Split(_sTupleExpressionDelimiter);
                string[] sLabels = sTuples[0].Split(',');
                string[] sVals = sTuples[1].Split(',');
                for (int i = 0; i < sLabels.Length; i++)
                {
                    if (sExpression.IndexOf(sLabels[i].Trim()) < 0)
                    {                           //trim the string (spaces not supported)
                        //todo: log that this was not found.
                        //       _log.AddString("tuple not found: " + sLabels[i], Logging._nLogging_Level_3);

                        //  Console.WriteLine("tuple label not found: " + sLabels[i]);
                    }
                    else
                    {
                        sExpression = sExpression.Replace(sLabels[i].Trim(), sVals[i]);
                    }
                }
                return sExpression;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sEXP"></param>
        /// <param name="nReturnFieldID"></param>
        /// <param name="nElementID"></param>
        private void ParseHELPER_GetTwoPartIdentifier(string sEXP, out int nReturnFieldID, out int nElementID)
        {
            sEXP= sEXP.Replace("DV", "");
            string[] sID_ValPair = sEXP.Split('^');
            nReturnFieldID = Convert.ToInt32(sID_ValPair[0]);
            if (sID_ValPair.Count() > 1)
                nElementID = Convert.ToInt32(sID_ValPair[1]);
            else
                nElementID = -1;

        }


        //takes substring, and goes and gets the necessary values
        private string Parse_GetVariableReferenceValue(string sEXP, string sCurrentDel, int nScenarioID, int nDVID_FK = -1, int nOptionID = -1)
        {
            string sql = "";
            string sReturn = CommonUtilities._sBAD_DATA;      //default return value
            sEXP = sEXP.Replace(sCurrentDel, "");        //get rid of the delimiter
            bool bIsBaseline = sEXP[0] == '#';            //check for vs baseline

            if (bIsBaseline)
            {
                sEXP = sEXP.Substring(1);               //remove the identifier
                nScenarioID = _nActiveBaselineScenarioID;
            }


            int nVarType_FK = -1;
            if ((sCurrentDel != "&") && (sCurrentDel != "_"))
            {                                 //not used in this case
                nVarType_FK = Convert.ToInt32(sEXP);
            }
            //TODO - this will get more complicated as more instructions are injected. met 4/13/2013 this is simplest case
            //todo: we rmg should store baseline     if (bIsBaseline ) {nScenarioID = rmg.BaselineScenarioID; //also remove the # after this is done


            int nElementID;

            switch (sCurrentDel)
            {
                case "_":                       //tblModelElementVals
                    // met 12/12/13- now links to Element ID... todo: consider how to aggregate for  large lists
                    // met 2/4/17: Support definition of type _DV2804_...
                        //Not searching on VAR type (should be consistent for a DV)
                        //ElementID...? Could be multiple.  What to do?
                            //step 1, 




                    if (sEXP == "DV")           //special case when we want to use the referenced DV val to get val from another ref list
                    {
                        sReturn = GetOptionVal_Parse(nScenarioID, nDVID_FK, nOptionID);
                    }
                    else if (sEXP == "DVOPTION")
                    {
                        sReturn = GetOptionVal_Parse(nScenarioID, nDVID_FK, nOptionID, true);
                    }
                    else
                    {
                        int nRecordID = -1;         // met 2/4/17: was vartype... b
                        ParseHELPER_GetTwoPartIdentifier(sEXP, out  nRecordID, out  nElementID);
                        sReturn = GetSimLinkDetail(SimLinkDataType_Major.MEV, nRecordID, -1, nScenarioID, nElementID);
                    }

                    //        sql = "select val from tblModElementVals where ((ScenarioID_FK=@Scenario) AND (DV_ID_FK=@VarType) and (TableFieldKey_FK=345))";    //hardcoded bad         //met todo : this is not fully sorted out. 11/7/2013 for Boston
                    break;
                case "!":                       //tblResultVar_Details      //nVarType_FK- really is RecordID
                    sReturn = GetSimLinkDetail(SimLinkDataType_Major.ResultSummary, nVarType_FK, -1, nScenarioID, -1);  //nVarType_FK is really the ResultID in this case..
                    //  sql = "select val from tblResultVar_Details where ((ScenarioID_FK=@Scenario) AND (Result_ID_FK=@VarType))";
                    break;
                case "@":                       //Event   //nVarType_FK- really is RecordID
                    sReturn = GetSimLinkDetail(SimLinkDataType_Major.Event, nVarType_FK, -1, nScenarioID, -1);
                    // sql = "select TotalVal as val from tblResultTS_EventSummary_Detail where ((ScenarioID_FK=@Scenario) AND (ResultTS_ID_FK=@VarType))";
                    break;
                case "%":                       //tblPerformance_Detail     //nVarType_FK- really is RecordID
                    sReturn = GetSimLinkDetail(SimLinkDataType_Major.Performance, nVarType_FK, -1, nScenarioID, -1);
                    // sql = "select val from tblPerformance_Detail where ((ScenarioID_FK=@Scenario) AND (PerformanceID_FK=@VarType))";
                    break;
                case "&":                       //network
                    ParseHELPER_GetTwoPartIdentifier(sEXP, out  nVarType_FK, out  nElementID);
                    sReturn = GetSimLinkDetail(SimLinkDataType_Major.Network, -1, nVarType_FK, _nActiveReferenceEG_BaseScenarioID, nElementID);
                    //   sql = GetModelInventoryFieldValue(sEXP, 1, ref slREF);         //only works for SWMM rightt now- needs improved.
                    //  nScenarioID = 7586;  // rmg.nActiveBaselineScenario;          //you want the baseline scenario for this, not the actual val
                    break;
                case "~":                       //constant
                    sReturn = _dictConstants[nVarType_FK];

                    //   sql = "SELECT val, tblConstants.ConstantID FROM tblConstants WHERE (((tblConstants.ConstantID)=@VarType))";
                    break;
            }

            return sReturn;
        }

        //todo: actually check for well-formed expression and update IsValid field in db to avoid confusion
        //met 4/14/2013 quick placeholder
        public static bool ParserIsValidExpression(string sFunction)
        {
            return (sFunction.Length >= 4);
        }


        #endregion

    }
}
