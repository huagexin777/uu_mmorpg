using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    public static class MFStringUtil
    {
        #region IsNullOrEmpty 验证值是否为null

        /// <summary>
        /// 判断对象是否为Null、DBNull、Empty或空白字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object value)
        {
            bool retVal = false;
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || (value.GetType().Equals(DBNull.Value.GetType())))
            {
                retVal = true;
            }
            return retVal;
        }

        #endregion

        #region ObjectToString object转换Str 自动Trim
        /// <summary>
        ///  ObjectToString object转换Str 自动Trim
        /// </summary>
        /// <param name="canNullStr"></param>
        /// <returns></returns>
        public static string ObjectToString(this object canNullStr)
        {
            return canNullStr.ObjectToString("");
        }

        /// <summary>
        /// ObjectToString object转换Str 自动Trim
        /// </summary>
        /// <param name="canNullStr"></param>
        /// <param name="defaultStr"></param>
        /// <returns></returns>
        public static string ObjectToString(this object canNullStr, string defaultStr)
        {
            try
            {
                if ((canNullStr == null) || (canNullStr is DBNull))
                {
                    if (defaultStr != null)
                    {
                        return defaultStr;
                    }
                    return string.Empty;
                }
                return Convert.ToString(canNullStr).Trim();
            }
            catch
            {
                return defaultStr;
            }
        }
        #endregion

        #region ToString

        public static string ToCnString(this bool? value)
        {
            if (value.HasValue && value.Value)
            {
                return "是";
            }
            else
            {
                return "否";
            }
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCnString(this bool value)
        {
            if (value)
            {
                return "是";
            }
            else
            {
                return "否";
            }
        }
        #endregion

        #region ToInt
        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>retValue</returns>
        public static int ToInt(this object value)
        {
            return value.ToInt(0);
        }

        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="defaultValue">defaultValue</param>
        /// <returns>retValue</returns>
        public static int ToInt(this object value, int defaultValue)
        {
            if (!value.IsNullOrEmpty())
            {
                if (value.ToString().IsNumberSign())
                {
                    return Convert.ToInt32(value);
                }
            }
            return defaultValue;
        }
        #endregion

        #region ToLong
        /// <summary>
        /// ToLong
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>retValue</returns>
        public static long ToLong(this object value)
        {
            return value.ToLong(0);
        }

        /// <summary>
        /// ToInt
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="defaultValue">defaultValue</param>
        /// <returns>retValue</returns>
        public static long ToLong(this object value, int defaultValue)
        {
            if (!value.IsNullOrEmpty())
            {
                if (value.ToString().IsNumberSign())
                {
                    return Convert.ToInt64(value);
                }
            }
            return defaultValue;
        }
        #endregion

        #region ToDecimal
        public static decimal ToDecimal(this decimal? value)
        {
            if (value.HasValue)
            {
                return value.Value;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// ToDecimal
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>retValue</returns>
        public static decimal ToDecimal(this object value)
        {
            return value.ToDecimal(0);
        }

        /// <summary>
        /// ToDecimal
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="defaultValue">defaultValue</param>
        /// <returns>retValue</returns>
        public static decimal ToDecimal(this object value, decimal defaultValue)
        {
            if (!value.IsNullOrEmpty())
            {
                if (value.ToString().IsDecimal())
                {
                    return Convert.ToDecimal(value);
                }
            }
            return defaultValue;
        }
        #endregion

        public static string ToRemoveZero(this object value)
        {
            string temp = string.Empty;
            string[] strs = string.Format("{0:F2}", value.ToDecimal()).Split('.');
            if (strs.Length > 1)
            {
                string str = strs[1].TrimEnd('0');
                if (str.IsNullOrEmpty())
                {
                    temp = strs[0];
                }
                else
                {
                    temp = strs[0] + "." + str;
                }
            }
            else
            {
                temp = value.ObjectToString();
            }
            return temp;
        }

        #region ToFloat
        /// <summary>
        /// ToFloat
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>retValue</returns>
        public static float ToFloat(this object value)
        {
            return value.ToFloat(0);
        }

        /// <summary>
        /// ToFloat
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="defaultValue">defaultValue</param>
        /// <returns>retValue</returns>
        public static float ToFloat(this object value, float defaultValue)
        {
            if (!value.IsNullOrEmpty())
            {
                if (value.ToString().IsDecimal())
                {
                    return Convert.ToSingle(value);
                }
            }
            return defaultValue;
        }
        #endregion

        #region ToBoolean
        /// <summary>
        /// ToBoolean
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>retValue</returns>
        public static bool ToBoolean(this object value)
        {
            return value.ToBoolean(false);
        }

        /// <summary>
        /// ToBoolean
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="defaultValue">defaultValue</param>
        /// <returns>retValue</returns>
        public static bool ToBoolean(this object value, bool defaultValue)
        {
            if (value.IsNullOrEmpty())
            {
                return defaultValue;
            }
            try
            {
                if (("1".Equals(value) || "true".Equals(value) || "y".Equals(value) || "t".Equals(value)))
                {
                    return true;
                }
                if (("0".Equals(value) || "false".Equals(value)) || ("n".Equals(value) || "f".Equals(value)))
                {
                    return false;
                }
                return Convert.ToBoolean(value);
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

        #region FilterJavaScript 过滤脚本
        /// <summary>
        /// 过滤脚本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FilterJavaScript(this string value)
        {
            return value.FilterJavaScript(string.Empty);
        }

        /// <summary>
        /// 过滤脚本
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defalutValue"></param>
        /// <returns></returns>
        public static string FilterJavaScript(this string value, string defalutValue)
        {
            if (value.IsNullOrEmpty())
            {
                return defalutValue;
            }
            else
            {
                return Sanitizer.GetSafeHtml(value);
            }
        }
        #endregion

        #region RemoveEnd 移除末尾指定字符
        /// <summary>
        /// 如果当前字符串以给定字符串结尾，则移除结尾的给定字符串。(移除前会先Trim())
        /// </summary>
        /// <param name="target"></param>
        /// <param name="val"></param>
        /// <returns>返回处理过的字符串</returns>
        public static string RemoveEnd(this string target, string val)
        {
            target = target.Trim();
            if (target.EndsWith(val))
                target = target.Remove(target.Length - val.Length);
            return target;
        }
        #endregion

        #region RemoveStart 移除开头指定字符
        /// <summary>
        /// 如果当前字符串以给定字符串开头，则移除开头的给定字符串。(移除前会先Trim())
        /// </summary>
        /// <param name="target"></param>
        /// <param name="val"></param>
        /// <returns>返回处理过的字符串</returns>
        public static string RemoveStart(this string target, string val)
        {
            target = target.Trim();
            if (target.StartsWith(val))
                target = target.Substring(val.Length);
            return target;
        }
        #endregion

        #region Replace 循环替换参数中字符
        /// <summary>
        /// 将当前字符串中能被oldValues集合的字符串匹配到到的字符用newValue值替换
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oldValues"></param>
        /// <param name="newValue"></param>
        /// <returns>返回处理过的字符串</returns>
        public static string Replace(this string target, ICollection<string> oldValues, string newValue)
        {
            oldValues.ForEach(oldValue => target = target.Replace(oldValue, newValue));
            return target;
        }
        #endregion

        #region WrapAt 字符串截断
        /// <summary>
        /// 字符串截断(超出部分用...表示)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string WrapAt(this string target, int index)
        {
            const int DotCount = 3;

            return (target.Length <= index) ? target : string.Concat(target.Substring(0, index - DotCount), new string('.', DotCount));
        }
        #endregion

        #region IsIndexOf 判断当前字符串中是否存在集合中的任意字符串
        /// <summary>
        /// 判断当前字符串中是否存在集合中的任意字符串
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oldValues"></param>
        /// <returns></returns>
        public static bool IsIndexOf(this string target, ICollection<string> oldValues)
        {
            foreach (var val in oldValues)
            {
                if (target.IndexOf(val) != -1)
                    return true;
            }
            return false;
        }
        #endregion

        #region ToEnum 将string转换成指定的枚举类型
        /// <summary>
        /// 将string转换成指定的枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string target, T defaultValue)
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }
        #endregion

        #region FormatWith 相当于string.Format方法
        /// <summary>
        /// 相当于string.Format方法
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args">参数数组</param>
        /// <returns></returns>
        public static string FormatWith(this string target, params object[] args)
        {
            return string.Format(target, args);
        }
        #endregion

        #region CutString

        /// <summary>
        /// 去Html
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(this string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// Html分段 将\n 变成<br/>
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string SubSection(this string content)
        {
            return content.Replace("\n", "<br/>");
        }

        /// <summary>
        /// 去Html标记后，截取指定长度的A标签字符串
        /// </summary>
        /// <param name="stringToSub"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ASubString(this string stringToSub, int length)
        {
            if (string.IsNullOrEmpty(stringToSub))
            {
                return null;
            }
            if (length <= 0)
            {
                return stringToSub;
            }
            stringToSub = RemoveHtml(stringToSub);
            string tag = null;
            string end = null;


            if (stringToSub.ToLower().IndexOf("<a") > -1)
            {
                int wordStart = stringToSub.IndexOf(">") + 1;
                int wordEnd = stringToSub.IndexOf("</");

                tag = stringToSub.Substring(0, wordStart);
                end = stringToSub.Substring(wordEnd);
                stringToSub = stringToSub.Substring(wordStart, wordEnd - wordStart);
            }
            Regex regex = new Regex("[一-龥]+", RegexOptions.Compiled);
            char[] stringChar = stringToSub.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            for (int i = 0; i < stringChar.Length; i++)
            {
                if (stringChar[i].Equals('…') || stringChar[i].Equals('—'))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else if (regex.IsMatch(stringChar[i].ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength++;
                }
                if (nLength > length)
                {
                    sb.Append("…");
                    break;
                }
            }
            return tag + sb.ToString() + end;
        }

        /// <summary>
        /// CutString
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="len">len</param>
        /// <returns>retValue</returns>
        public static string CutString(this string value, int len)
        {

            if (string.IsNullOrEmpty(value)) { return string.Empty; }
            string str = string.Empty;
            if (len == -1)
            {
                return value;
            }
            value = value.FilterHtml();
            value = value.Replace("&nbsp;", "");
            if (System.Text.Encoding.Default.GetByteCount(value) <= len)
            {
                return value;
            }
            int num = 0;
            len -= 2;
            for (int i = 0; i < value.Length; i++)
            {
                str = str + value[i];
                if ((value[i] < '\0') || (value[i] > '\x00ff'))
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                if (num >= len)
                {
                    break;
                }
            }
            return (str + "...");
        }

        /// <summary>
        /// 不区分大小写的匹配
        /// </summary>
        /// <param name="value"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool NewEquals(this string value, object obj)
        {
            if (value.IsNullOrEmpty() || obj.IsNullOrEmpty())
                return false;
            return value.Equals(obj.ToString(), StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region ToDBStr 转化为数据库字符(防止Sql注入)
        /// <summary>
        /// 转化为数据库字符(防止Sql注入)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDBStr(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            bool hasKeywords = false;

            if (value.Length > 0)
            {
                Regex reg = new Regex("insert|update|delete|drop");
                string sLowerStr = value.ToLower();
                Match m = reg.Match(sLowerStr);
                hasKeywords = m.Success;
            }
            if (hasKeywords)
            {
                return string.Empty;
            }
            return value;
        }
        #endregion

        #region IsNullOrEmpty 自动Trim()
        /// <summary>
        /// IsNullOrEmpty 自动Trim()
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            if (value == null)
            {
                return true;
            }
            return string.IsNullOrEmpty(value.Trim());
        }
        #endregion

        #region 是否存在指定字符串

        /// <summary>
        /// 判断是否能匹配到指定字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="subString"></param>
        /// <returns></returns>
        public static bool IsContains(this string str, string subString)
        {
            if (str.IndexOf(subString) == -1)
                return false;
            else
                return true;
        }

        #endregion

        #region 判断字符串中是否包含英文,包含都转化为大写
        /// <summary>
        /// 判断字符串中是否包含英文,包含都转化为大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ChangeContainEn(this string str)
        {
            if (str.IsNullOrEmpty())
            {
                return "";
            }
            else
            {
                if (Regex.IsMatch(str, "[A-Za-z]"))
                {
                    return str.ToUpper();
                }
                else
                {
                    return str;
                }
            }
        }
        #endregion

        #region 当剪切内容不包含标红时，重新剪切，使返回内容包含标红
        /// <summary>
        /// 当剪切内容不包含标红时，从新剪切，使返回内容包含标红。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cutnum"></param>
        /// <returns></returns>
        public static string ReturnRedContent(this string str, int cutnum)
        {
            if (str.IndexOf("<font color=\"red\">") > 0)
            {
                if (str.CutString(cutnum).LastIndexOf("<font color=\"red\">") < 0)
                {
                    int start = str.IndexOf("<font color=\"red\">") - 50;
                    int end = str.IndexOf("</font>") + 50;
                    if (start < 50)
                    {
                        start = 0;
                    }
                    //else
                    //{
                    //    start = str.IndexOf("，", start)+1;
                    //}
                    if (end > str.Length)
                    {
                        end = str.Length;
                    }
                    //else
                    //{
                    //    start = str.LastIndexOf("，", end) - 1;
                    //}
                    return str.Substring(start, end - start);
                }
                else
                {
                    return str.CutString(cutnum);
                }
            }
            else
            {
                return str.CutString(cutnum);
            }
        }
        #endregion

        #region ToList 将以指定隔开的文本 转化成IList(string)

        /// <summary>
        /// 将以","隔开的文本 转化成IList(string)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IList<string> ToList(this string value)
        {
            return value.ToList(',');
        }

        /// <summary>
        /// 将以指定符号隔开的文本 转化成IList(string)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IList<string> ToList(this string value, char opr)
        {
            IList<string> lst = new List<string>();

            if (!value.IsNullOrEmpty())
            {
                string[] arr = value.ToString().TrimStart(opr).TrimEnd(opr).Split(opr);

                foreach (string s in arr)
                {
                    if (!s.IsNullOrEmpty())
                    {
                        if (!lst.Contains(s))
                        {
                            lst.Add(s);
                        }
                    }
                }
            }
            return lst;
        }
        #endregion

        #region ListToString 将IList(string)转化为以指定符号隔开的字符串

        public static string ListToString(this IList<int> lst)
        {
            string str = string.Empty;
            foreach (int i in lst)
            {
                str += string.Format("{0},", i);
            }
            return str.TrimEnd(',');
        }

        /// <summary>
        /// 获取不重复的,隔开的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetNoRepeatString(this string str)
        {
            IList<int> lst = new List<int>();
            string[] arr = str.Split(',');
            foreach (string s in arr)
            {
                if (!lst.Contains(s.ToInt()))
                {
                    lst.Add(s.ToInt());
                }
            }
            return lst.ListToString();
        }

        /// <summary>
        /// 将IList(string)转化为以“,”隔开的字符
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static string ListToString(this IList<string> lst)
        {
            return lst.ListToString(",");
        }

        /// <summary>
        /// 将IList(string)转化为以指定符号隔开的字符
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static string ListToString(this IEnumerable<string> lst, string opr)
        {
            string str = string.Empty;

            if (lst == null || lst.Count() == 0)
            {
                return str;
            }

            foreach (string s in lst)
            {
                if (!s.IsNullOrEmpty())
                {
                    str += string.Format("{0}{1}", s, opr);
                }
            }

            return str.TrimEnd(opr.ToCharArray());
        }
        #endregion

        #region ToHexByte 将16进制的字符串转换成二进制数组
        /// <summary>
        /// 将16进制的字符串转换成二进制数组
        /// </summary>
        /// <param name="hexString">16进制字符串</param>
        /// <returns>二进制数组</returns>
        public static byte[] ToHexByte(this string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        #endregion

        public static string GetEnumDescription(this Enum e)
        {
            System.Reflection.FieldInfo[] ms = e.GetType().GetFields();
            Type t = e.GetType();
            foreach (System.Reflection.FieldInfo f in ms)
            {
                if (f.Name != e.ToString()) continue;
                foreach (Attribute attr in f.GetCustomAttributes(true))
                {
                    System.ComponentModel.DescriptionAttribute dscript = attr as System.ComponentModel.DescriptionAttribute;
                    if (dscript != null)
                        return dscript.Description;
                }
            }
            return e.ToString();
        }

        public static string GetLetter(int i)
        {
            if (i < 0 || i > 25) { return string.Empty; }
            string[] ArrLetter = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            return ArrLetter[i];
        }

        public static int GetLength(this string str)
        {
            if (str.Length == 0) return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0; byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }
    }
}