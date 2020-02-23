using Mmcoy.Framework.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Mmcoy.Framework.WebAPIAuth
{
    public class MFServiceAuthHandler : DelegatingHandler
    {
        #region 构造函数
        /// <summary>
        /// WebAPI服务器端安全认证
        /// </summary>
        public MFServiceAuthHandler()
        {
        }

        /// <summary>
        /// WebAPI服务器端安全认证
        /// </summary>
        /// <param name="serviceKeyName">服务器端服务密钥Key名称</param>
        /// <param name="ipKeyName">服务器端IP密钥Key名称</param>
        public MFServiceAuthHandler(string serviceKeyName, string ipKeyName)
        {
            WebAPIKeyName = serviceKeyName;
            WebAPIAuthIPKeyName = ipKeyName;
        }
        #endregion

        #region SendAsync 验证授权
        /// <summary>
        /// 验证授权
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var clientIp = MFSystemUtil.GetIP();

            if (!MmcoyWebAPIAuthIPs.Contains(clientIp))//判断客户端的IP是否合法
            {
                int matchAuthCount = request.Headers.Count(item =>
                {
                    if ("MmcoyWebAPIKey".NewEquals(item.Key))
                    {
                        foreach (var key in item.Value)
                            return MmcoyWebAPIKeys.Contains(key);
                    }
                    return false;
                });
                if (matchAuthCount > 0)
                    return base.SendAsync(request, cancellationToken);
            }
            HttpContext.Current.Response.Write("调用WebAPI失败，你的服务密钥不正确或IP地址没有得到授权！");
            return Task.Factory.StartNew<HttpResponseMessage>(() => { return new HttpResponseMessage(HttpStatusCode.OK); });
        }
        #endregion

        #region 属性

        #region WebAPIAuthIPKeyName 密钥名称
        public string WebAPIAuthIPKeyName
        {
            get;
            set;
        }
        #endregion

        #region 键名称
        public string WebAPIKeyName
        {
            get;
            set;
        }
        #endregion

        #region MmcoyWebAPIAuthIPs 服务器端设置的合法访问IP
        private IList<string> _MmcoyWebAPIAuthIPs = null;

        /// <summary>
        /// 服务器端设置的合法访问IP
        /// </summary>
        public IList<string> MmcoyWebAPIAuthIPs
        {
            get
            {
                if (_MmcoyWebAPIAuthIPs.IsNullOrEmpty())
                    _MmcoyWebAPIAuthIPs = MFWebConfig.ReadSetting(WebAPIAuthIPKeyName, "WebAPIAuthIPs").ToList(',');
                return _MmcoyWebAPIAuthIPs;
            }
        }
        #endregion

        #region MmcoyWebAPIKeys 服务器端设置的合法认证密钥
        private IList<string> _MmcoyWebAPIKey = null;

        /// <summary>
        /// 服务器端设置的合法认证密钥
        /// </summary>
        protected IList<string> MmcoyWebAPIKeys
        {
            get
            {
                if (_MmcoyWebAPIKey.IsNullOrEmpty())
                    _MmcoyWebAPIKey = MFWebConfig.ReadSetting(WebAPIKeyName, "MmcoyWebAPIKey").ToList();
                return _MmcoyWebAPIKey;
            }
        }
        #endregion

        #endregion
    }
}