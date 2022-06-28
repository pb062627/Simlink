using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace muMathParser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {

            muMathParser.Parser m_parser = new muMathParser.Parser(muMathParser.Parser.EBaseType.tpDOUBLE);
            muMathParser.ParserVariable m_ans = new ParserVariable(0);
            //    m_parser = new muMathParser.Parser(muMathParser.Parser.EBaseType.tpDOUBLE);
            m_parser.SetExpr("2*4");
            m_ans.Value = m_parser.Eval();
            
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new muWrapper.WndSample());
        }
        //YW remove pre build event (not working):
        //copy "$(SolutionDir)..\muParser_csharp\CppDLL\muParser.dll" "$(SolutionDir)\$(OutDir)"
    }
}