using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver
{
    public class FlagInfo
    {
        string FormatString;
        public List<string> FlagNames = new List<string>();
        public Dictionary<string, int> FlagBits = new Dictionary<string, int>();

        public FlagInfo(string formatString)
        {
            FormatString = formatString;
        }

        public string Format(int flags)
        {
            return flags.ToString(FormatString);
        }

        public FlagInfo AddFlag(string name, int bits)
        {
            FlagNames.Add(name);
            FlagBits.Add(name, bits);
            return this;
        }
    }
}
