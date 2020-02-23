using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Mmcoy.Framework.Service;

namespace Mmcoy.Framework.AbstractBase
{
    //[MFAppErrorFilter(Order = 100)]
    public abstract class MFAbstractController<T> : Controller
    {
        #region 模型对象
        /// <summary>
        /// 模型对象
        /// </summary>
        public T CacheModel { get; set; }
        #endregion
    }
}