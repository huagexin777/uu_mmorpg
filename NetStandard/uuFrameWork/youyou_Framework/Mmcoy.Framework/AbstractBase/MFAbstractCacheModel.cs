using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework.AbstractBase
{
    public abstract class MFAbstractCacheModel
    {
        /// <summary>
        /// 缓存时间（秒）
        /// </summary>
        protected virtual int CacheTime
        {
            get { return 60 * 3; }
        }
    }
}