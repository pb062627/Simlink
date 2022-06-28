using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CH2M
{
    public class Commons
    {

        /// <summary>
        /// Get short path name
        /// </summary>
        /// <param name="strPathName"></param>
        /// <returns></returns>
        public static string GetShortPathName(string strPathName)
        {
            StringBuilder shortPath = new StringBuilder(255);
            GetShortPathName(strPathName, shortPath, shortPath.Capacity);
            return shortPath.ToString();
        }
        /// <summary>
        /// Convert logn filename to short path name
        /// </summary>
        /// <param name="path"></param>
        /// <param name="shortPath"></param>
        /// <param name="shortPathLength"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(
                 [MarshalAs(UnmanagedType.LPTStr)]
                   string path,
                 [MarshalAs(UnmanagedType.LPTStr)]
                   StringBuilder shortPath,
                 int shortPathLength
                 );
        public static void ShowMessage(string strMessage)
        {
            XtraMessageBox.Show(strMessage, "SimLink", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        public static DialogResult ShowMessage(string strMessage, MessageBoxButtons button, MessageBoxIcon msgIcon)
        {
            return XtraMessageBox.Show(strMessage, "SimLink", button, msgIcon);
        }
        public static void ShowMessage(string strMessage, MessageBoxIcon msgIcon)
        {
            XtraMessageBox.Show(strMessage, "SimLink", MessageBoxButtons.OK, msgIcon);
        }
        /// <summary>
        /// Copy empty table
        /// </summary>
        /// <param name="dtIn"></param>
        /// <returns></returns>
        public static DataTable CopyEmptyTable(DataTable dtIn)
        {
            DataTable dt = new DataTable();
            foreach (DataColumn col in dtIn.Columns)
            {
                dt.Columns.Add(col.ColumnName, col.DataType);
            }
            return dt;

        }

        public static void GetFieldDataTypeFromGrid(DevExpress.XtraGrid.Views.Grid.GridView grid, 
            out List<string> astrHeader, out List<string> astrDataType)
        {
            astrHeader = new List<string>();
            astrDataType = new List<string>();

            foreach (DevExpress.XtraGrid.Columns.GridColumn item in grid.Columns)
            {
                if (item.Visible)
                {
                    astrHeader.Add(item.FieldName);
                    astrDataType.Add(item.ColumnType.ToString());
                }
            }
        }

        /// <summary>
        /// Write template CSV
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static string WriteTemplateCSV(DevExpress.XtraGrid.Views.Grid.GridView grid)
        {
            string[] astrSourceString = new string[] { "Reference TS Label" };
            string[] astrDestString = new string[] { "Reference TS Label (FK)" };
            string strHeader = ""; 
            //string strValue = ""; 
            //string strValue2 = "";
            foreach (DevExpress.XtraGrid.Columns.GridColumn item in grid.Columns)
            {
                if (item.Visible)
                {
                    strHeader += item.Caption + ",";
                }
                //strValue += grid.GetRowCellValue(1, item).ToString() + ",";
                //strValue2 += grid.GetRowCellValue(2, item).ToString() + ",";
            }
            if (strHeader.Length > 0) strHeader= strHeader.Substring(0, strHeader.Length - 1); // write template
            for (int intIndex = 0; intIndex <= astrSourceString.GetUpperBound(0); intIndex++ )
            {
                strHeader = strHeader.Replace(astrSourceString[intIndex], astrDestString[intIndex]);
            }
            //if (strValue.Length > 0) strValue= strValue.Substring(0, strValue.Length - 1);
            //if (strValue2.Length > 0) strValue2= strValue2.Substring(0, strValue2.Length - 1);

            //return strHeader +"\r\n" + strValue + "\r\n" + strValue2;
            return strHeader;
        }

    }
}
