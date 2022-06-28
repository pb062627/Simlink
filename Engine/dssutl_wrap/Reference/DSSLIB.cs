using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace Adaptor4HecDSS
{
    public class DSSLIB
    {
        [DllImport("hlib42.dll", CharSet = CharSet.Auto)]
        public static extern void ZRRTS _zrrts_@60
        //GetShortPathName(string LongPath, StringBuilder ShortPath, int BufferSize);

        Declare Sub ZRRTS Lib "hlib42.dll" Alias "_zrrts_@60" _
    (lngIFLTAB As Long, ByVal strCPATH As String, _
    ByVal strCDate As String, ByVal strCTime As String, _
    lngNVals As Long, sngValues As Single, ByVal strCUnits As String, _
    ByVal strCType As String, lngIOFSET As Long, lngISTAT As Long, _
    ByVal lngL_CPath As Long, ByVal lngL_CDate As Long, _
    ByVal lngL_CTime As Long, ByVal lngL_CUnits As Long, _
    ByVal lngL_CType As Long)       'Working
    }
}
