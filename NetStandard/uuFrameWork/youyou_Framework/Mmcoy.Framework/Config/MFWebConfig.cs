using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Configuration;

namespace Mmcoy.Framework.Config
{
    public static class MFWebConfig
    {
        #region ReadSetting 根据key读取WebConfig配置value
        /// <summary>
        /// 根据key读取WebConfig配置value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadSetting(string key)
        {
            return ReadSetting(key, string.Empty);
        }

        /// <summary>
        /// 根据key读取WebConfig配置value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">defaultValue</param>
        /// <returns>retValue</returns>
        public static string ReadSetting(string key, string defaultValue)
        {
            string str = ConfigurationManager.AppSettings[key];
            if (str.IsNullOrEmpty())
            {
                return defaultValue;
            }
            return str;
        }

        /// <summary>
        /// 根据key读取WebConfig配置value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">defaultValue</param>
        /// <returns>retValue</returns>
        public static bool ReadSetting(string key, bool defaultValue)
        {
            return ReadSetting(key, defaultValue.ToString()).ToBoolean(defaultValue);
        }

        /// <summary>
        /// 根据key读取WebConfig配置value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">defaultValue</param>
        /// <returns>retValue</returns>
        public static decimal ReadSetting(string key, decimal defaultValue)
        {
            return ReadSetting(key, defaultValue.ToString()).ToDecimal(defaultValue);
        }

        /// <summary>
        /// 根据key读取WebConfig配置value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">defaultValue</param>
        /// <returns>retValue</returns>
        public static int ReadSetting(string key, int defaultValue)
        {
            return ReadSetting(key, defaultValue.ToString()).ToInt(defaultValue);
        }
        #endregion

        #region ReadConnectionString 根据key读取连接字符串
        /// <summary>
        /// 根据key读取连接字符串
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string ReadConnectionString(string key, string defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            else
            {
                var conn = ConfigurationManager.ConnectionStrings[key];
                if (conn.IsNullOrEmpty())
                    return string.Empty;
                return conn.ConnectionString;
            }
        }

        /// <summary>
        /// 根据key读取连接字符串
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string ReadConnectionString(string key)
        {
            return ReadConnectionString(key, "");
        }

        private static string _ConnectionKey = string.Empty;
        public static string ConnectionKey
        {
            get
            {
                if (string.IsNullOrEmpty(_ConnectionKey))
                    _ConnectionKey = ReadConnectionString("ConnectionString");
                return _ConnectionKey;
            }
        }
        #endregion

        #region CreateOrUpdateSetting 新增或更新根目录的WebConfig文件的AppStrings(仅用于web.config)
        /// <summary>
        /// 新增或更新根目录的WebConfig文件的AppStrings(仅用于web.config)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void CreateOrUpdateSetting(string key, string value)
        {
            var config = WebConfigurationManager.OpenWebConfiguration(null);
            var setting = config.AppSettings.Settings[key];
            if (setting.IsNullOrEmpty())
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;
        }
        #endregion

        #region ServiceKey 服务密钥
        /// <summary>
        /// 服务密钥
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return ReadSetting("ServiceKey", "");
            }
        }
        #endregion
    }
}