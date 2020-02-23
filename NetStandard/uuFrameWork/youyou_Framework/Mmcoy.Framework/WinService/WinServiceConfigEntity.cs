using Mmcoy.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework.WinService
{
    /// <summary>
    /// WinService配置
    /// </summary>
    [Serializable]
    public class WinServiceConfigEntity
    {
        #region Id 获取或设置编号
        /// <summary>
        /// 获取或设置编号
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        #endregion

        #region Name 获取或设置名称
        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        #endregion

        #region IsOnce 是否只运行一次
        /// <summary>
        /// 是否只运行一次
        /// </summary>
        public bool IsOnce
        {
            get;
            set;
        }
        #endregion

        #region Interval 获取或设置时间间隔（单位：秒）
        /// <summary>
        /// 获取或设置时间间隔（单位：秒）
        /// </summary>
        public int Interval
        {
            get;
            set;
        }
        #endregion

        #region IsAppointTime 获取或设置是否指定时间
        /// <summary>
        /// 获取或设置是否指定时间
        /// </summary>
        public bool IsAppointTime
        {
            get;
            set;
        }
        #endregion

        #region AppointTime 指定时间
        /// <summary>
        /// 指定时间（例：01:00:00）
        /// </summary>
        public string AppointTime
        {
            get;
            set;
        }
        #endregion

        #region Enable 获取或设置是否可用
        /// <summary>
        /// 获取或设置是否可用
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }
        #endregion

        #region AssemblyString 获取或设置程序集
        /// <summary>
        /// 获取或设置程序集
        /// </summary>
        public string AssemblyString
        {
            get;
            set;
        }
        #endregion

        #region TypeName 获取或设置类
        /// <summary>
        /// 获取或设置类
        /// </summary>
        public string TypeName
        {
            get;
            set;
        }
        #endregion

        #region Description 获取或设置描述
        /// <summary>
        /// 获取或设置描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        #endregion

        #region ServiceInstance 获取配置接口
        private IWinService _ServiceInstance;

        /// <summary>
        /// 获取配置接口
        /// </summary>
        public IWinService ServiceInstance
        {
            get
            {
                if (_ServiceInstance == null)
                {
                    try
                    {
                        _ServiceInstance = Assembly.Load(this.AssemblyString).CreateInstance(this.TypeName) as IWinService;
                        _ServiceInstance.Config = this;
                    }
                    catch
                    {

                    }
                }
                return _ServiceInstance;
            }
        }
        #endregion

        #region IsAlreadyDoService 是否已经执行过
        /// <summary>
        /// 是否已经执行过
        /// </summary>
        public bool IsAlreadyDoService
        {
            get;
            set;
        }
        #endregion
    }
}
