using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mmcoy.Framework
{
    public static class MFEnumerableUtil
    {
        /// <summary>
        /// 相当于Foreach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// 相当于For
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumable"></param>
        /// <param name="action"></param>
        public static void For<T>(this IEnumerable<T> enumable, Action<T, int> action)
        {
            for (int i = 0; i < enumable.Count(); i++)
            {
                action(enumable.ElementAt(i), i);
            }
        }

        /// <summary>
        /// 将对象转换成字典类型
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this FormCollection form)
        {
            var dict = new Dictionary<string, object>();
            foreach (var key in form.AllKeys)
            {
                dict[key] = form[key];
            }
            return dict;
        }

        #region

        /// <summary>
        /// 将对象转换成字典类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this NameValueCollection obj)
        {
            IDictionary<string, object> valueDictionary = new Dictionary<string, object>();
            foreach (var key in obj.AllKeys)
            {
                valueDictionary[key] = obj[key];
            }
            return valueDictionary;
        }

        #endregion
    }
}
