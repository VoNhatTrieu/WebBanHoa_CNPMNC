using System.Collections.Generic;
using System.Globalization;

namespace giadinhthoxinh.Helpers
{
    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return CultureInfo.InvariantCulture.CompareInfo
                .Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
