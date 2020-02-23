using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    public static class MFValidateUtil
    {
        #region 私有变量定义
        private static readonly Regex RegNumber = new Regex("^[0-9]+$");
        private static readonly Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static readonly Regex RegDecimal = new Regex(@"^[0-9]+(\.[0-9]+)?$");
        private static readonly Regex RegDecimalSign = new Regex(@"^[+-]?[0-9]+(\.[0-9]+)?$");
        private static readonly Regex RegEmail = new Regex(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$");
        private static readonly Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        private static readonly Regex RegDate = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        private static readonly Regex RegDateTime = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
        private static readonly Regex RegPrice = new Regex(@"^(?!0\d)(?!\.)[0-9]+(\.[0-9]{1,3})?$");
        private static readonly Regex RegStorage = new Regex(@"^(?:[1-9][0-9]*(?:\.[0-9]+)?|0(?:\.[0-9]+)?)$");
        #endregion

        #region 数字字符串检查

        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static bool IsNumber(this string value)
        {
            Match m = RegNumber.Match(value);
            return m.Success;
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static bool IsNumberSign(this string value)
        {
            Match m = RegNumberSign.Match(value);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="value">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(this string value)
        {
            Match m = RegDecimal.Match(value);
            return m.Success;
        }

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static bool IsHasCHZN(this string value)
        {
            Match m = RegCHZN.Match(value);
            return m.Success;
        }

        #endregion

        #region 邮件地址
        /// <summary>
        /// 是否邮件地址
        /// </summary>
        /// <param name="value">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(this string value)
        {
            Match m = RegEmail.Match(value);
            return m.Success;
        }

        #endregion

        #region 日期
        /// <summary>
        /// 检测是否日期类型数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string value)
        {
            bool IsDate;

            Match mDate = RegDate.Match(value);
            IsDate = mDate.Success;

            if (IsDate)
            {
                return true;
            }
            Match m = RegDateTime.Match(value);
            return m.Success;
        }
        #endregion 日期

        #region 图片

        public static bool IsImage(this string fileName)
        {
            string s = "|.jpg|.jpeg|.gif|.bmp|.png|.pjpeg|";
            string fileSort = Path.GetExtension(fileName).ToLower();
            if (s.IndexOf(fileSort) > 0) { return true; }
            else { return false; }
        }

        #endregion
    }
}