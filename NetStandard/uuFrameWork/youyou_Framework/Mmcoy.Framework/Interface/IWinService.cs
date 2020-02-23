using Mmcoy.Framework.WinService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework.Interface
{
    public interface IWinService
    {
        /// <summary>
        /// 是否繁忙
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// 配置
        /// </summary>
        WinServiceConfigEntity Config
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可以执行
        /// </summary>
        /// <returns></returns>
        bool CanDoService();

        /// <summary>
        /// 执行服务
        /// </summary>
        void DoService();
    }
}
