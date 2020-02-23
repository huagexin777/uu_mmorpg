using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Reflection;

using Mmcoy.Framework.Interface;

namespace Mmcoy.Framework
{
    public sealed class MFCacheUtil : ICache
    {
        #region CreateObject 创建对象或从缓存获取(工厂模式 用反射创建实例)
        private static readonly object _object = new object();

        /// <summary>
        /// 创建对象或从缓存获取(工厂模式 用反射创建实例)
        /// </summary>
        public static object CreateObject(string path, string key)
        {

            object objType = MFCacheUtil.GetCache(key);//从缓存读取

            if (objType == null)
            {
                lock (_object)
                {
                    try
                    {
                        objType = Assembly.Load(path).CreateInstance(key);//反射创建
                        MFCacheUtil.SetCache(key, objType);// 写入缓存
                    }
                    catch
                    { }
                }
            }
            return objType;
        }
        #endregion

        #region SetCacheDependencyFile 设置缓存
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="filePath">filePath</param>
        public static void SetCacheDependencyFile(string key, object value, string filePath)
        {
            if (value == null)
            {
                return;
            }
            if (!File.Exists(filePath))
            {
                return;
            }
            HttpRuntime.Cache.Insert(string.Format("MmcoyCache_{0}_{1}", key, filePath), value, new CacheDependency(filePath));
        }

        /// <summary>
        /// 设置缓存 永久缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCache(string key, object value)
        {
            if (value == null)
            {
                return;
            }
            HttpRuntime.Cache["MmcoyCache_{0}".FormatWith(key)] = value;
        }



        /// <summary>
        /// 设置缓存 依赖时间的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second"></param>
        public static void SetCacheDependencyTime(string key, object value, double second)
        {
            if (value == null)
            {
                return;
            }
            HttpRuntime.Cache.Insert("MmcoyCache_" + key, value, null, DateTime.Now.AddSeconds(second), TimeSpan.Zero);
        }
        #endregion

        #region GetCacheDependencyFile 获取缓存
        ///// <summary>
        ///// 获取缓存
        ///// </summary>
        ///// <param name="key">key</param>
        ///// <param name="filePath">filePath</param>
        ///// <returns></returns>
        public static object GetCacheDependencyFile(string key, string filePath)
        {
            return HttpRuntime.Cache.Get(string.Format("MmcoyCache_{0}_{1}", key, filePath));
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static object GetCache(string key)
        {
            return HttpRuntime.Cache.Get("MmcoyCache_{0}".FormatWith(key));
        }
        #endregion

        #region ICache接口方法
        /// <summary>
        /// 缓存的总数
        /// </summary>
        public int Count
        {
            get { return HttpRuntime.Cache.Count; }
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void Clear()
        {
            var caches = HttpRuntime.Cache.GetEnumerator();
            while (caches.MoveNext())
            {
                HttpRuntime.Cache.Remove(caches.Key.ToString());
            }
        }

        /// <summary>
        /// 判断是否存在指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            var caches = HttpRuntime.Cache.GetEnumerator();
            while (caches.MoveNext())
            {
                if (caches.Key.Equals("MmcoyCache_" + key))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 根据指定key获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return (T)HttpRuntime.Cache["MmcoyCache_" + key];
        }

        /// <summary>
        /// 尝试获取缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet<T>(string key, out T value)
        {
            value = Get<T>(key);
            return !value.IsNullOrEmpty();
        }

        /// <summary>
        /// 设置缓存 永久缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string key, T value)
        {
            SetCache(key, value);
        }

        /// <summary>
        /// 设置缓存 依赖时间的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            SetCacheDependencyTime(key, value, (absoluteExpiration - DateTime.Now).TotalSeconds);
        }

        /// <summary>
        /// 设置缓存 依赖时间的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration"></param>
        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            SetCacheDependencyTime(key, value, slidingExpiration.TotalSeconds);
        }

        /// <summary>
        /// 设置缓存 依赖时间的缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        public void Set<T>(string key, T value, int seconds)
        {
            SetCacheDependencyTime(key, value, seconds);
        }

        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove("MmcoyCache_" + key);
        }
        #endregion
    }
}