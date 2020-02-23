using Mmcoy.Framework.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework.WebAPIAuth
{
    public class MFClientAuthRequestHandler : DelegatingHandler
    {
        public MFClientAuthRequestHandler()
        {
        }

        public MFClientAuthRequestHandler(string clientKeyName)
        {
            this.WebAPIClientKeyName = clientKeyName;
        }

        public string WebAPIClientKeyName { get; set; }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Add("MmcoyWebAPIKey", MFWebConfig.ReadSetting(WebAPIClientKeyName, "WebAPIClientKeyName"));
            return base.SendAsync(request, cancellationToken);
        }
    }
}
