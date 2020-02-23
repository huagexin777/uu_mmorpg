using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework.AbstractBase
{
    public abstract class MFAbstractModel<T>
    {
        #region 模型对象
        /// <summary>
        /// 模型对象
        /// </summary>
        public static T CacheModel { get; set; }
        #endregion
    }
}