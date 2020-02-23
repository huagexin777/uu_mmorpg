using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mmcoy.Framework.WinService;

namespace Mmcoy.Framework.AbstractBase
{
    public class MFAbstractWinService
    {
        #region 私有变量定义
        public bool _IsBusy;
        #endregion

        #region CanDoService 是否可以执行服务
        /// <summary>
        /// 是否可以执行服务
        /// </summary>
        /// <returns></returns>
        public bool CanDoService()
        {
            if (!this.Config.IsAlreadyDoService)
            {
                return true;
            }

            this.SecondTimeCount++;
            if (this.SecondTimeCount > this.Config.Interval)
            {
                this.SecondTimeCount = 0;
                return true;
            }
            return false;
        }
        #endregion

        #region SecondTimeCount 计算时间
        /// <summary>
        /// 计算时间
        /// </summary>
        public int SecondTimeCount
        {
            get;
            set;
        }
        #endregion

        #region IsBusy 是否繁忙
        /// <summary>
        /// 是否繁忙
        /// </summary>
        public bool IsBusy
        {
            get { return _IsBusy; }
        }
        #endregion

        #region Config 对应的服务配置
        /// <summary>
        /// 对应的服务配置
        /// </summary>
        public WinServiceConfigEntity Config
        {
            get;
            set;
        }
        #endregion
    }
}