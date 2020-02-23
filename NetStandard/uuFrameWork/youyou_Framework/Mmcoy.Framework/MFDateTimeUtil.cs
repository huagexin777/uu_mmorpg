using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    public static class MFDateTimeUtil
    {
        #region ToDateTime

        private static readonly DateTime MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 000);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        /// <summary>
        /// 验证日期对象是否为有效值(大于1900年小于9999年)
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsValid(this DateTime target)
        {
            return !target.IsNullOrEmpty() && (target >= MinDate) && (target <= MaxDate);
        }

        /// <summary>
        /// ToDateTime 将字符串转换成日期对象(字符串为空转换成1900-1-1)
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>retValue</returns>
        public static DateTime ToDateTime(this string value)
        {
            return value.ToDateTime(MinDate);
        }

        /// <summary>
        /// ToDateTime 将字符串转换成日期对象(字符串为空转换成默认值)
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="defaultValue">defaultValue</param>
        /// <returns>retValue</returns>
        public static DateTime ToDateTime(this string value, DateTime defaultValue)
        {
            if (!value.IsNullOrEmpty())
            {
                return DateTime.Parse(value.ToString());
            }
            return defaultValue;
        }
        #endregion

        #region ToDateTimeString
        public static string ToFormatString(this DateTime? value)
        {
            return value.ToFormatString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static string ToFormatString(this DateTime? value, string format)
        {
            return value.HasValue ? value.Value.ToFormatString(format) : "";
        }

        /// <summary>
        /// ToFormatString 格式化日期对象
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="format">format</param>
        /// <returns>retValue</returns>
        public static string ToFormatString(this DateTime value, string format)
        {
            return value.IsValid() ? value.ToString(format) : "";
        }

        /// <summary>
        /// ToFormatString 格式化日期对象(使用yyyy-MM-dd HH:mm:ss)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToFormatString(this DateTime value)
        {
            DateTime dateTime = value.IsValid() ? value : MinDate;
            return dateTime.ToFormatString("yyyy-MM-dd HH:mm:ss.fff");
        }
        #endregion

        #region ToMSSqlDateTime
        /// <summary>
        /// ToMSSqlDateTime 如果对象为空或为最小日期,则返回1900-1-1
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>retValue</returns>
        public static DateTime ToMSSqlDateTime(this DateTime value)
        {
            return value.IsValid() ? value : MinDate;
        }

        /// <summary>
        /// 获取不重复的时间(数据库的唯一索引时间列用此方法)
        /// </summary>
        private static object lockTimeObj = new object();
        private static DateTime _old_DateTime = DateTime.Now;
        public static DateTime GetMSSqlDateTime(this DateTime value)
        {
            return GetMSSqlDateTime();
        }

        /// <summary>
        /// 获取不重复的时间(数据库的唯一索引时间列用此方法)
        /// </summary>
        public static DateTime GetMSSqlDateTime()
        {
            lock (lockTimeObj)
            {
                var currDateTime = DateTime.Now;
                if (_old_DateTime == currDateTime)
                    currDateTime = currDateTime.AddMilliseconds(10);
                _old_DateTime = currDateTime;
                return _old_DateTime;
            }
        }
        #endregion

        #region ToListDataTime 转换为列表中的显示时间

        /// <summary>
        /// 转换为列表中的显示时间
        /// </summary>
        /// <param name="objDateTime"></param>
        /// <returns></returns>
        public static string ToListDataTime(this DateTime objDateTime)
        {
            DateTime dt = objDateTime.ToMSSqlDateTime();

            if (dt < DateTime.Now)
            {
                TimeSpan ts1 = new TimeSpan(dt.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();

                string dateDiff = null;
                if (ts.Days < 1)
                {
                    if (ts.Hours > 0)
                    {
                        dateDiff += ts.Hours.ToString() + "小时";
                    }
                    else
                        if (ts.Minutes != 0)
                        {
                            dateDiff += ts.Minutes.ToString() + "分钟";
                        }
                        else
                            if (dateDiff == null)
                            {
                                dateDiff = "1分钟";
                            }
                }
                else if (ts.Days < 31)
                {
                    dateDiff = "{0}天".FormatWith(ts.Days);
                }
                else
                {
                    return dt.ToString("MM-dd HH:mm");
                }
                dateDiff += "前";
                return dateDiff;
            }
            else
            {
                return dt.ToString("MM-dd HH:mm");
            }
        }
        #endregion
    }
}