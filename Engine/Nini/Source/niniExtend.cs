using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nini.Config;

namespace Nini
{
    public static class NiniExtend
    {
        public static void AddCommandLineSwtiches(string[] args, ArgvConfigSource source)
        {
            foreach (var item in args)
            {
                if (item.StartsWith("-"))
                {
                    source.AddSwitch("Base", item.Substring(1));
                }
            }
        }
    }
}
