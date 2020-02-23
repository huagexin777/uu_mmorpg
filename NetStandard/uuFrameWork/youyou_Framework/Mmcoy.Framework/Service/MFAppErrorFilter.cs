using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mmcoy.Framework.Service
{
    /// <summary>
    /// Asp.Net Mvc 的通用异常处理
    /// </summary>
    public class MFAppErrorFilter : HandleErrorAttribute, IExceptionFilter
    {
        #region 日志成员变量
        private static MFIAppLog log;//= NAppLoggerManager.GetLogger(typeof(NAppErrorFilter));
        #endregion

        #region 重写方法
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.IsNullOrEmpty())
                throw new ArgumentNullException("filterContext is null");

            var ex = filterContext.Exception as Exception;

            var refUrl = filterContext.HttpContext.Request.UrlReferrer.IsNullOrEmpty()
                             ? ""
                             : filterContext.HttpContext.Request.UrlReferrer.OriginalString;
            log = MFAppLoggerManager.GetLogger(filterContext.Controller.GetType());
            var errorMessage = "错误信息:{0}-Controller:{1}-Action:{2},源Url:{3}".FormatWith(ex.Message,
                                                                                        filterContext.RouteData.Values[
                                                                                            "controller"],
                                                                                        filterContext.RouteData.Values[
                                                                                            "action"], refUrl);
            log.Error(errorMessage, ex);

            filterContext.Controller.ViewData["ErrorMessage"] = ex.Message;

            filterContext.Result = new ViewResult() { ViewName = "Error", ViewData = filterContext.Controller.ViewData };

            filterContext.ExceptionHandled = true;

            filterContext.HttpContext.ClearError();

        }
        #endregion
    }
}