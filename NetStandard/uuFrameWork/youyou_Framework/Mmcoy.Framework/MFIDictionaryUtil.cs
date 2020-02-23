using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    public static class MFIDictionaryUtil
    {
        public static bool IsNullOrEmpty(this IDictionary<string, object> collection)
        {
            return (collection == null) || (collection.Count == 0);
        }
    }
}
