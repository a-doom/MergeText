using System;
using System.Collections.Generic;

namespace MergeTextLib
{
    public class StrComparer : IComparer<string>
    {
        public int Compare(string str1, string str2)
        {
            return String.Compare(str1, str2, StringComparison.CurrentCulture);
        }
    }

    public class StrComparerIgnoreSpace : IComparer<string>
    {
        public int Compare(string str1, string str2)
        {
            return String.Compare(str1.Trim(), str2.Trim(), StringComparison.CurrentCulture);
        }
    }
}
