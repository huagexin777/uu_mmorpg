using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mmcoy.Framework.Helper
{
    public class MFSQLInjectionHelper
    {
        /// <summary>
        /// 获取Get的数据
        /// </summary>
        public static bool ValidUrlGetData()
        {
            bool result = false;

            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                result = ValidData(HttpContext.Current.Request.QueryString[i].ToString());
                if (result)
                {
                    //如果检测存在漏洞
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取Post的数据
        /// </summary>
        public static bool ValidUrlPostData()
        {
            bool result = false;

            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                result = ValidData(HttpContext.Current.Request.Form[i].ToString());
                if (result)
                {
                    //如果检测存在漏洞
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 验证是否存在注入代码
        /// </summary>
        /// <param name="inputData"></param>
        public static bool ValidData(string inputData)
        {
            //里面定义恶意字符集合
            string[] checkWord = { "and", "exec", "insert", "select", "delete", "update", "count", "from", "drop", "asc", "char", "*", "%", ";", ":", "\'", "\"", "chr", "mid", "master", "truncate", "char", "declare", "SiteName", "net user", "xp_cmdshell", "/add", "exec master.dbo.xp_cmdshell", "net localgroup administrators" };
            if (inputData == null || inputData == "")
            {
                return false;
            }
            else
            {
                foreach (string s in checkWord)
                {
                    //验证inputData是否包含恶意集合
                    if (inputData.ToString().ToLower().IndexOf(s) > -1)
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
                return false;
            }

        }
    }
}