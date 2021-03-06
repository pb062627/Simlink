using System;
using System.Runtime.InteropServices;

namespace EPANET_working2
{
    public class EpanetLibrary
    {
        #region Epanet Constants
        public const string EPANETDLL = "C:\\Users\\cmoore4\\Documents\\Visual Studio 2010\\Projects\\EPANET_working2\\EPANET_working2\\bin\\Debug\\epanet2.dll";
        public const int EN_ELEVATION = 0;
        public const int EN_BASEDEMAND = 1;
        public const int EN_PATTERN = 2;
        public const int EN_EMITTER = 3;
        public const int EN_INITQUAL = 4;
        public const int EN_SOURCEQUAL = 5;
        public const int EN_SOURCEPAT = 6;
        public const int EN_SOURCETYPE = 7;
        public const int EN_TANKLEVEL = 8;
        public const int EN_DEMAND = 9;
        public const int EN_HEAD = 10;
        public const int EN_PRESSURE = 11;
        public const int EN_QUALITY = 12;
        public const int EN_SOURCEMASS = 13;
        public const int EN_INITVOLUME = 14;
        public const int EN_MIXMODEL = 15;
        public const int EN_MIXZONEVOL = 16;
        public const int EN_TANKDIAM = 17;
        public const int EN_MINVOLUME = 18;
        public const int EN_VOLCURVE = 19;
        public const int EN_MINLEVEL = 20;
        public const int EN_MAXLEVEL = 21;
        public const int EN_MIXFRACTION = 22;
        public const int EN_TANK_KBULK = 23;
        public const int EN_DIAMETER = 0;
        public const int EN_LENGTH = 1;
        public const int EN_ROUGHNESS = 2;
        public const int EN_MINORLOSS = 3;
        public const int EN_INITSTATUS = 4;
        public const int EN_INITSETTING = 5;
        public const int EN_KBULK = 6;
        public const int EN_KWALL = 7;
        public const int EN_FLOW = 8;
        public const int EN_VELOCITY = 9;
        public const int EN_HEADLOSS = 10;
        public const int EN_STATUS = 11;
        public const int EN_SETTING = 12;
        public const int EN_ENERGY = 13;
        public const int EN_DURATION = 0;
        public const int EN_HYDSTEP = 1;
        public const int EN_QUALSTEP = 2;
        public const int EN_PATTERNSTEP = 3;
        public const int EN_PATTERNSTART = 4;
        public const int EN_REPORTSTEP = 5;
        public const int EN_REPORTSTART = 6;
        public const int EN_RULESTEP = 7;
        public const int EN_STATISTIC = 8;
        public const int EN_PERIODS = 9;
        public const int EN_NODECOUNT = 0;
        public const int EN_TANKCOUNT = 1;
        public const int EN_LINKCOUNT = 2;
        public const int EN_PATCOUNT = 3;
        public const int EN_CURVECOUNT = 4;
        public const int EN_CONTROLCOUNT = 5;
        public const int EN_JUNCTION = 0;
        public const int EN_RESERVOIR = 1;
        public const int EN_TANK = 2;
        public const int EN_CVPIPE = 0;
        public const int EN_PIPE = 1;
        public const int EN_PUMP = 2;
        public const int EN_PRV = 3;
        public const int EN_PSV = 4;
        public const int EN_PBV = 5;
        public const int EN_FCV = 6;
        public const int EN_TCV = 7;
        public const int EN_GPV = 8;
        public const int EN_NONE = 0;
        public const int EN_CHEM = 1;
        public const int EN_AGE = 2;
        public const int EN_TRACE = 3;
        public const int EN_CONCEN = 0;
        public const int EN_MASS = 1;
        public const int EN_SETPOINT = 2;
        public const int EN_FLOWPACED = 3;
        public const int EN_CFS = 0;
        public const int EN_GPM = 1;
        public const int EN_MGD = 2;
        public const int EN_IMGD = 3;
        public const int EN_AFD = 4;
        public const int EN_LPS = 5;
        public const int EN_LPM = 6;
        public const int EN_MLD = 7;
        public const int EN_CMH = 8;
        public const int EN_CMD = 9;
        public const int EN_TRIALS = 0;
        public const int EN_ACCURACY = 1;
        public const int EN_TOLERANCE = 2;
        public const int EN_EMITEXPON = 3;
        public const int EN_DEMANDMULT = 4;
        public const int EN_LOWLEVEL = 0;
        public const int EN_HILEVEL = 1;
        public const int EN_TIMER = 2;
        public const int EN_TIMEOFDAY = 3;
        public const int EN_AVERAGE = 1;
        public const int EN_MINIMUM = 2;
        public const int EN_MAXIMUM = 3;
        public const int EN_RANGE = 4;
        public const int EN_MIX1 = 0;
        public const int EN_MIX2 = 1;
        public const int EN_FIFO = 2;
        public const int EN_LIFO = 3;
        public const int EN_NOSAVE = 0;
        public const int EN_SAVE = 1;
        public const int EN_INITFLOW = 10;
        #endregion

        #region Epanet Imports
        public delegate void UserSuppliedFunction(string param0);
        /// <summary>
        /// Runs a complete EPANET simulation.
        /// </summary>
        /// <param name="f1">name of the input file</param>
        /// <param name="f2">name of an output report file</param>
        /// <param name="f3">name of an output output file </param>
        /// <param name="vfunc">pointer to a user-supplied function which accepts a character string as its argument</param>
        /// <returns>Returns an error code.</returns>
        [DllImport(EPANETDLL, EntryPoint = "ENepanet")]
        public static extern int ENepanet(string f1, string f2, string f3, UserSuppliedFunction vfunc);

        /// <summary>
        /// Opens the Toolkit to analyze a particular distribution system.
        /// </summary>
        /// <param name="param0">name of the input file</param>
        /// <param name="param1">name of an output report file</param>
        /// <param name="param2">name of an output output file</param>
        /// <returns>Returns an error code.</returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENopen")]
        public static extern int ENopen(string param0, string param1, string param2);

        /// <summary>
        /// Writes all current network input data to a file using the format of an EPANET input file.
        /// </summary>
        /// <param name="filename">name of the file where data is saved.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsaveinpfile")]
        public static extern int ENsaveinpfile(string filename);

        /// <summary>
        /// Closes down the Toolkit system (including all files being processed).
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENclose")]
        public static extern int ENclose();

        /// <summary>
        /// Runs a complete hydraulic simulation with results
        /// for all time periods written to the binary Hydraulics file.
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsolveH")]
        public static extern int ENsolveH();

        /// <summary>
        /// Transfers results of a hydraulic simulation from the binary Hydraulics file to the binary Output file,
        /// where results are only reported at uniform reporting intervals.
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsaveH")]
        public static extern int ENsaveH();

        /// <summary>
        /// Opens the hydraulics analysis system.
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENopenH")]
        public static extern int ENopenH();

        /// <summary>
        /// Initializes storage tank levels, link status and settings,
        /// and the simulation clock time prior to running a hydraulic analysis.
        /// </summary>
        /// <param name="saveflag">0-1 flag indicating if hydraulic results will be saved to the hydraulics file.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = " ENinitH")]
        public static extern int ENinitH(int saveflag);

        /// <summary>
        /// Runs a single period hydraulic analysis, retrieving the current simulation clock time t.
        /// </summary>
        /// <param name="t">current simulation clock time in seconds.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENrunH")]
        public static extern int ENrunH(ref long t);

        /// <summary>
        /// Determines the length of time until the next hydraulic event occurs in an extended period simulation.
        /// </summary>
        /// <param name="tstep">time (in seconds) until next hydraulic event occurs or
        /// 0 if at the end of the simulation period.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENnextH")]
        public static extern int ENnextH(ref long tstep);

        /// <summary>
        ///  Closes the hydraulic analysis system, freeing all allocated memory.
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENcloseH")]
        public static extern int ENcloseH();

        /// <summary>
        /// Saves the current contents of the binary hydraulics file to a file.
        /// </summary>
        /// <param name="fname">name of the file where the hydraulics results should be saved.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsavehydfile")]
        public static extern int ENsavehydfile(string fname);

        /// <summary>
        /// Uses the contents of the specified file as the current binary hydraulics file.
        /// </summary>
        /// <param name="fname">name of the file containing hydraulic analysis results for the current network.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENusehydfile")]
        public static extern int ENusehydfile(string fname);

        /// <summary>
        /// Runs a complete water quality simulation with results at uniform reporting
        /// intervals written to EPANET's binary Output file.
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsolveQ")]
        public static extern int ENsolveQ();

        /// <summary>
        /// Opens the water quality analysis system.
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENopenQ")]
        public static extern int ENopenQ();

        /// <summary>
        /// Initializes water quality and the simulation clock time prior to running a water quality analysis.
        /// </summary>
        /// <param name="saveflag">0-1 flag indicating if analysis results
        /// should be saved to EPANET's binary output file at uniform reporting periods.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = " ENinitQ")]
        public static extern int ENinitQ(int saveflag);

        /// <summary>
        /// Makes available the hydraulic and water quality results that occur
        /// at the start of the next time period of a water quality analysis,
        /// where the start of the period is returned in t.
        /// </summary>
        /// <param name="t">current simulation clock time in seconds.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENrunQ")]
        public static extern int ENrunQ(ref long t);

        /// <summary>
        /// Advances the water quality simulation to the start of the next hydraulic time period.
        /// </summary>
        /// <param name="tstep">time (in seconds) until next hydraulic event occurs or
        /// 0 if at the end of the simulation period.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENnextQ")]
        public static extern int ENnextQ(ref long tstep);

        /// <summary>
        /// Advances the water quality simulation one water quality time step.
        /// The time remaining in the overall simulation is returned in tleft.
        /// </summary>
        /// <param name="tleft">seconds remaining in the overall simulation duration.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENstepQ")]
        public static extern int ENstepQ(ref long tleft);

        /// <summary>
        /// Closes the water quality analysis system, freeing all allocated memory.
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENcloseQ")]
        public static extern int ENcloseQ();

        /// <summary>
        /// Writes a line of text to the EPANET report file.
        /// </summary>
        /// <param name="line">text to be written to file.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENwriteline")]
        public static extern int ENwriteline(string line);

        /// <summary>
        /// Writes a formatted text report on simulation results to the Report file.
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENreport")]
        public static extern int ENreport();

        /// <summary>
        /// Clears any report formatting commands that either appeared in the
        /// [REPORT] section of the EPANET Input file or were issued with the ENsetreport function.
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENresetreport")]
        public static extern int ENresetreport();

        /// <summary>
        /// Issues a report formatting command.
        /// Formatting commands are the same as used in the
        /// [REPORT] section of the EPANET Input file.
        /// </summary>
        /// <param name="command">text of a report formatting command.</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsetreport")]
        public static extern int ENsetreport(string command);

        /// <summary>
        /// Retrieves the parameters of a simple control statement.
        /// The index of the control is specified in cindex and
        /// the remaining arguments return the control's parameters.
        /// </summary>
        /// <param name="cindex">control statement index</param>
        /// <param name="ctype">control type code</param>
        /// <param name="lindex">index of link being controlled</param>
        /// <param name="setting">value of the control setting</param>
        /// <param name="nindex">index of controlling node</param>
        /// <param name="level">value of controlling water level or
        /// pressure for level controls or of time of control action (in seconds)
        /// for time-based controls</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetcontrol")]
        public static extern int ENgetcontrol(int cindex, ref int ctype, ref int lindex,
            ref float setting, ref int nindex, ref float level);

        /// <summary>
        /// Retrieves the number of network components of a specified type.
        /// </summary>
        /// <param name="countcode">component code</param>
        /// <param name="count">number of countcode components in the network</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetcount")]
        public static extern int ENgetcount(int countcode, ref int count);

        /// <summary>
        /// Retrieves the value of a particular analysis option.
        /// </summary>
        /// <param name="optioncode">an option code</param>
        /// <param name="value">an option value</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetoption")]
        public static extern int ENgetoption(int optioncode, ref float value);

        /// <summary>
        ///  Retrieves the value of a specific analysis time parameter.
        /// </summary>
        /// <param name="paramcode">time parameter code</param>
        /// <param name="timevalue">value of time parameter in seconds</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgettimeparam")]
        public static extern int ENgettimeparam(int paramcode, ref int timevalue);

        /// <summary>
        /// Retrieves a code number indicating the units used to express all flow rates.
        /// </summary>
        /// <param name="unitscode">value of a flow units code number</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetflowunits")]
        public static extern int ENgetflowunits(ref int unitscode);

        /// <summary>
        /// Retrieves the index of a particular time pattern.
        /// </summary>
        /// <param name="id">pattern ID label</param>
        /// <param name="index">pattern index</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetpatternindex")]
        public static extern int ENgetpatternindex(string id, ref int index);

        /// <summary>
        /// Retrieves the ID label of a particular time pattern.
        /// </summary>
        /// <param name="index">pattern index</param>
        /// <param name="id">ID label of pattern</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetpatternid")]
        public static extern int ENgetpatternid(int index, string id);

        /// <summary>
        /// Retrieves the number of time periods in a specific time pattern.
        /// </summary>
        /// <param name="index">pattern index</param>
        /// <param name="len">number of time periods in the pattern</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetpatternlen")]
        public static extern int ENgetpatternlen(int index, ref int len);

        /// <summary>
        /// Retrieves the multiplier factor for a specific time period in a time pattern.
        /// </summary>
        /// <param name="index">time pattern index</param>
        /// <param name="period">period within time pattern</param>
        /// <param name="value">multiplier factor for the period</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetpatternvalue")]
        public static extern int ENgetpatternvalue(int index, int period, ref float value);

        /// <summary>
        /// Retrieves the type of water quality analysis called for.
        /// </summary>
        /// <param name="qualcode">water quality analysis code</param>
        /// <param name="tracenode">index of node traced in a source tracing analysis</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetqualtype")]
        public static extern int ENgetqualtype(ref int qualcode, ref int tracenode);

        /// <summary>
        /// Retrieves the text of the message associated with a particular error or warning code.
        /// </summary>
        /// <param name="errcode">error or warning code</param>
        /// <param name="errmsg">text of the error or warning message for errcode</param>
        /// <param name="nchar">maximum number of characters that errmsg can hold</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgeterror")]
        public static extern int ENgeterror(int errcode, string errmsg, int nchar);

        /// <summary>
        ///  Retrieves the index of a node with a specified ID.
        /// </summary>
        /// <param name="id">node ID label</param>
        /// <param name="index">node index</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetnodeindex")]
        public static extern int ENgetnodeindex(string id, ref int index);

        /// <summary>
        /// Retrieves the ID label of a node with a specified index
        /// </summary>
        /// <param name="index">node index</param>
        /// <param name="id">ID label of node</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetnodeid")]
        public static extern int ENgetnodeid(int index, string id);

        /// <summary>
        /// Retrieves the node-type code for a specific node.
        /// </summary>
        /// <param name="index">node index</param>
        /// <param name="typecode">node-type code</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetnodetype")]
        public static extern int ENgetnodetype(int index, ref int typecode);

        /// <summary>
        /// Retrieves the value of a specific link parameter.
        /// </summary>
        /// <param name="index">node index</param>
        /// <param name="paramcode">parameter code</param>
        /// <param name="value">parameter value</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetnodevalue")]
        public static extern int ENgetnodevalue(int index, int paramcode, ref float value);

        /// <summary>
        /// Retrieves the index of a link with a specified ID.
        /// </summary>
        /// <param name="id">link ID label</param>
        /// <param name="index">link index</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetlinkindex")]
        public static extern int ENgetlinkindex(string id, ref int index);

        /// <summary>
        /// Retrieves the ID label of a link with a specified index.
        /// </summary>
        /// <param name="index">link index</param>
        /// <param name="id">ID label of link</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetlinkid")]
        public static extern int ENgetlinkid(int index, string id);

        /// <summary>
        ///  Retrieves the link-type code for a specific link.
        /// </summary>
        /// <param name="index">link index</param>
        /// <param name="typecode">link-type code</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetlinktype")]
        public static extern int ENgetlinktype(int index, ref int typecode);

        /// <summary>
        /// Retrieves the indexes of the end nodes of a specified link.
        /// </summary>
        /// <param name="index">link index</param>
        /// <param name="fromnode">index of node at start of link</param>
        /// <param name="tonode">index of node at end of link</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetlinknodes")]
        public static extern int ENgetlinknodes(int index, ref int fromnode, ref int tonode);

        /// <summary>
        /// Retrieves the value of a specific link parameter.
        /// </summary>
        /// <param name="index">link index</param>
        /// <param name="paramcode">parameter code</param>
        /// <param name="value">parameter value</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetlinkvalue")]
        public static extern int ENgetlinkvalue(int index, int paramcode, ref float value);

        /// <summary>
        /// Retrieves version.
        /// </summary>
        /// <param name="version">Version</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENgetversion")]
        public static extern int ENgetversion(ref int version);

        /// <summary>
        /// Sets the parameters of a simple control statement.
        /// </summary>
        /// <param name="cindex">control statement index</param>
        /// <param name="ctype">control type code</param>
        /// <param name="lindex">index of link being controlled</param>
        /// <param name="setting">value of the control setting</param>
        /// <param name="nindex">index of controlling node</param>
        /// <param name="level">value of controlling water level or pressure
        /// for level controls or of time of control action (in seconds)
        /// for time-based controls</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsetcontrol")]
        public static extern int ENsetcontrol(int cindex, int ctype, int lindex,
            float setting, int nindex, float level);

        /// <summary>
        ///  Sets the value of a parameter for a specific node.
        /// </summary>
        /// <param name="index">node index</param>
        /// <param name="paramcode">parameter code</param>
        /// <param name="value">parameter value</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsetnodevalue")]
        public static extern int ENsetnodevalue(int index, int paramcode, float value);

        /// <summary>
        ///  Sets the value of a parameter for a specific link.
        /// </summary>
        /// <param name="index">link index</param>
        /// <param name="paramcode">parameter code</param>
        /// <param name="value">parameter value</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsetlinkvalue")]
        public static extern int ENsetlinkvalue(int index, int paramcode, float value);

        /// <summary>
        /// Adds a new time pattern to the network.
        /// </summary>
        /// <param name="id">ID label of pattern</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENaddpattern")]
        public static extern int ENaddpattern(string id);

        /// <summary>
        ///  Sets all of the multiplier factors for a specific time pattern.
        /// </summary>
        /// <param name="index">time pattern index</param>
        /// <param name="factors">multiplier factors for the entire pattern</param>
        /// <param name="nfactors">number of factors in the pattern</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsetpattern")]
        public static extern int ENsetpattern(int index, float[] factors, int nfactors);

        /// <summary>
        ///  Sets the multiplier factor for a specific period within a time pattern.
        /// </summary>
        /// <param name="index">time pattern index</param>
        /// <param name="period">period within time pattern</param>
        /// <param name="value">multiplier factor for the period</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsetpatternvalue")]
        public static extern int ENsetpatternvalue(int index, int period, float value);

        /// <summary>
        /// Sets the value of a time parameter.
        /// </summary>
        /// <param name="paramcode">time parameter code</param>
        /// <param name="timevalue">value of time parameter in seconds</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsettimeparam")]
        public static extern int ENsettimeparam(int paramcode, long timevalue);

        /// <summary>
        /// Sets the value of a particular analysis option.
        /// </summary>
        /// <param name="optioncode">an option code</param>
        /// <param name="value">an option value</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsetoption")]
        public static extern int ENsetoption(int optioncode, float value);

        /// <summary>
        ///  Sets the level of hydraulic status reporting.
        /// </summary>
        /// <param name="statuslevel">level of status reporting</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsetstatusreport")]
        public static extern int ENsetstatusreport(int statuslevel);

        /// <summary>
        /// Sets the type of water quality analysis called for.
        /// </summary>
        /// <param name="qualcode">water quality analysis code</param>
        /// <param name="chemname">name of the chemical being analyzed</param>
        /// <param name="chemunits">units that the chemical is measured in</param>
        /// <param name="tracenode">ID of node traced in a source tracing analysis</param>
        /// <returns></returns>
        [DllImportAttribute(EPANETDLL, EntryPoint = "ENsetqualtype")]
        public static extern int ENsetqualtype(int qualcode, string chemname, string chemunits, string tracenode);

        #endregion
    }
}