using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foldup
{
    /// <summary>
    /// Performs a case-insensitive comparison of file or folder names
    /// to support the Foldup class' "Contains" checks.
    /// </summary>
    class NameComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y) { return x.ToUpper() == y.ToUpper(); }
        public int GetHashCode(string obj) { return obj.GetHashCode(); }
    }
}
