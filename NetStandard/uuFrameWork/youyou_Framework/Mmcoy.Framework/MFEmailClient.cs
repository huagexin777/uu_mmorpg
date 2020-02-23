using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    public class MFEmailClient
    {
        #region 构造函数
        /// <summary>
        ///构造函数
        /// </summary>
        public MFEmailClient()
        {
        }
        /// <summary>
        /// 带参数构造函数
        /// </summary>
        /// <param name="mailServer"></param>
        /// <param name="mailAccount"></param>
        /// <param name="mailAccountPassword"></param>
        /// <param name="mailAccountTitle"></param>
        public MFEmailClient(string mailServer, string mailAccount, string mailAccountPassword, string mailAccountTitle)
        {
            MailServer = mailServer;
            MailAccount = mailAccount;
            MailAccountPassword = mailAccountPassword;
            MailAccountTitle = mailAccountTitle;
        }
        #endregion

        #region 属性
        /// <summary>
        /// mail服务器地址
        /// </summary>
        public string MailServer
        {
            get;
            set;
        }
        /// <summary>
        /// 发送或接受邮件使用的邮件账号
        /// </summary>
        public string MailAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 发送或接受邮件使用的邮件账号密码
        /// </summary>
        public string MailAccountPassword
        {
            get;
            set;
        }
        /// <summary>
        /// 发送邮件‘发送人’友好名称
        /// </summary>
        public string MailAccountTitle
        {
            get;
            set;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="mailToAddress">可以是用‘,’号隔开的多个邮件地址</param>
        /// <param name="isHtml"></param>
        /// <returns></returns>
        public bool Send(string title, string content, string mailToAddress, bool isHtml)
        {
            return Send(title, content, mailToAddress, isHtml, Encoding.GetEncoding("utf-8"));
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="mailToAddress">可以是用‘,’号隔开的多个邮件地址</param>
        /// <param name="isHtml"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public bool Send(string title, string content, string mailToAddress, bool isHtml, Encoding encoding)
        {
            try
            {
                //分隔符替换
                mailToAddress = mailToAddress.Replace(";", ",");
                mailToAddress = mailToAddress.Replace("；", ",");
                mailToAddress = mailToAddress.Replace("，", ",");

                System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage();
                mailMsg.From = new MailAddress(MailAccount, MailAccountTitle);
                mailMsg.To.Add(mailToAddress);
                mailMsg.Subject = title;
                mailMsg.Priority = MailPriority.Normal;
                mailMsg.IsBodyHtml = isHtml;
                mailMsg.Body = content;
                mailMsg.BodyEncoding = encoding;

                SmtpClient smtpClient = new SmtpClient(MailServer);
                smtpClient.Timeout = 30000;
                smtpClient.Credentials = new NetworkCredential(MailAccount, MailAccountPassword);
                smtpClient.Send(mailMsg);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}