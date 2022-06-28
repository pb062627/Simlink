using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SimLink_CLI
{
    public class Validation
    {
        [DllImport("Advapi32.dll", EntryPoint = "GetUserName", ExactSpelling = false, SetLastError = true)]
        static extern bool GetUserName([MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, [MarshalAs(UnmanagedType.LPArray)] Int32[] nSize);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool LookupAccountName(
            string lpSystemName,
            string lpAccountName,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Sid,
            ref uint cbSid,
            StringBuilder DomainName,
            ref uint DomainNameLength,
            out SID_NAME_USE peUse);

        enum SID_NAME_USE
        {
            SidTypeUser = 1,
            SidTypeGroup,
            SidTypeDomain,
            SidTypeAlias,
            SidTypeWellKnownGroup,
            SidTypeDeletedAccount,
            SidTypeInvalid,
            SidTypeUnknown,
            SidTypeComputer
        }

        //bm 4/18/2012 - from Pro2D     MET from common utilities
        public static bool ValidateDomainName(string sDomainNameRequested)
        {
            bool bCheck;
            int nErr;
            long lDomainLength;
            string sUserID;
            string sDomainName;
            SID_NAME_USE snuUse;

            byte[] bSid = new byte[256];
            byte[] bUserName = new byte[256];
            uint cbSid = 255;
            StringBuilder sbDomainName = new StringBuilder();
            uint nDomainNameLength = (uint)sbDomainName.Capacity;
            Int32[] nLengthUserName = new Int32[1];

            bCheck = false;
            try
            {
                nLengthUserName[0] = 256;
                GetUserName(bUserName, nLengthUserName);
                //nErr = Marshal.GetLastWin32Error();
                sUserID = System.Text.Encoding.ASCII.GetString(bUserName).Substring(0, nLengthUserName[0] - 1);

                LookupAccountName(null, sUserID, bSid, ref cbSid, sbDomainName, ref nDomainNameLength, out snuUse);
                //nErr = Marshal.GetLastWin32Error();
                lDomainLength = nDomainNameLength;
                nDomainNameLength = (uint)lDomainLength;

                LookupAccountName(null, sUserID, bSid, ref cbSid, sbDomainName, ref nDomainNameLength, out snuUse);
                //nErr = Marshal.GetLastWin32Error();
                sDomainName = sbDomainName.ToString();

                if (sDomainName == sDomainNameRequested) bCheck = true;
            }
            catch (Exception ex)
            {
            }

            return bCheck;
        }

    }
}
