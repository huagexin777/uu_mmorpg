using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mmcoy.Framework
{
    public class MFCookieUtil
    {
        /// <summary>
        /// 写入指定Cookie加密
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetCookie(string name, string value)
        {
            if (value == null) return;
            HttpCookie cookie = new HttpCookie(name, MFEncryptUtil.EncryptDES(value, MFEncryptUtil.KeyDES));
            //cookie.Path = "/";
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static void SetCookie(string name, string value, DateTime Expires)
        {
            if (value == null) return;
            HttpCookie cookie = new HttpCookie(name, MFEncryptUtil.EncryptDES(value, MFEncryptUtil.KeyDES));
            cookie.Expires = Expires;
            //cookie.Path = "/";
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 读取指定Cookie解密
        /// </summary>
        /// <param name="name"></param>
        public static string GetCookie(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(name);
            if (cookie == null) return null;
            return MFEncryptUtil.DecryptDES(cookie.Value, MFEncryptUtil.KeyDES);
        }
        /// <summary>
        /// 写入指定Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetCookieNoEn(string name, string value)
        {
            if (value == null) return;
            HttpCookie cookie = new HttpCookie(name, value);
            cookie.Path = "/";
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 读取指定Cookie
        /// </summary>
        /// <param name="name"></param>
        public static string GetCookieNoDe(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(name);
            if (cookie == null) return null;
            return cookie.Value;
        }
        /// <summary>
        /// 删除指定Cookie
        /// </summary>
        /// <param name="name"></param>
        public static void DeleteCookie(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                cookie.Expires = DateTime.Now.Add(ts);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }
        /// <summary>
        /// 删除Cookie
        /// </summary>
        public static void DeleteCookie()
        {
            HttpContext.Current.Response.Cookies.Clear();
        }
    }
}
