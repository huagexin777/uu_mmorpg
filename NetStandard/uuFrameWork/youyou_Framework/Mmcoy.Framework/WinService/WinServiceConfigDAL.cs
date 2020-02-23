using Mmcoy.Framework.AbstractBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mmcoy.Framework.WinService
{
    public class WinServiceConfigDAL : MFAbstractXmlDAL
    {
        private WinServiceConfigDAL(string path)
            : base(path)
        {

        }

        #region 单例
        private static readonly object _object = new object();
        private static WinServiceConfigDAL instance;
        public static WinServiceConfigDAL GetInstance(string path)
        {
            if (instance == null)
            {
                lock (_object)
                {
                    if (instance == null)
                    {
                        instance = new WinServiceConfigDAL(path);
                    }
                }
            }
            return instance;
        }
        #endregion

        #region GetConfigList 获取配置集合
        private IList<WinServiceConfigEntity> _ConfigList;
        /// <summary>
        /// 获取配置集合
        /// </summary>
        /// <returns></returns>
        public IList<WinServiceConfigEntity> GetConfigList()
        {
            if (_ConfigList == null)
            {
                _ConfigList = new List<WinServiceConfigEntity>();

                IList<XElement> lst = this.GetElements(this.RootElement, "WinServiceConfig");
                foreach (XElement element in lst)
                {
                    WinServiceConfigEntity config = new WinServiceConfigEntity();
                    config.Id = element.Attribute("Id").Value.ObjectToString();
                    config.Name = element.Attribute("Name").Value.ObjectToString();
                    config.IsOnce = element.Attribute("IsOnce").Value.ToBoolean();
                    if (element.Attribute("Interval") != null)
                    {
                        config.Interval = element.Attribute("Interval").Value.ToInt();
                    }
                    config.IsAppointTime = element.Attribute("IsAppointTime").Value.ToBoolean();
                    if (element.Attribute("AppointTime") != null)
                    {
                        config.AppointTime = element.Attribute("AppointTime").Value.ObjectToString();
                    }
                    config.Enabled = element.Attribute("Enabled").Value.ToBoolean();
                    config.AssemblyString = element.Attribute("AssemblyString").Value.ObjectToString();
                    config.TypeName = element.Attribute("TypeName").Value.ObjectToString();
                    config.Description = element.Attribute("Description").Value.ObjectToString();

                    _ConfigList.Add(config);
                }
            }
            return _ConfigList;
        }
        #endregion
    }
}
