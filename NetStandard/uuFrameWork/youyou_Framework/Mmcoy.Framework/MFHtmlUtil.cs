using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Newtonsoft.Json.Linq;

namespace Mmcoy.Framework
{
    public static class MFHtmlUtil
    {
        #region 分页
        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="html"></param>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="totalPage">总页数</param>
        /// <param name="action">动作名称</param>
        /// <param name="controller">控制器名称</param>
        /// <returns></returns>
        public static MvcHtmlString ToPagerHtml(this HtmlHelper html, int pageIndex, int pageSize, int totalPage, string action, string controller, object paramObj = null)
        {
            var mmcoyPager = new MFPagerSplit();
            mmcoyPager.TotalPage = totalPage;
            mmcoyPager.PageSize = pageSize;
            mmcoyPager.PageIndex = pageIndex;
            mmcoyPager.Action = action;
            mmcoyPager.Controller = controller;
            mmcoyPager.PageIndexName = "page";
            mmcoyPager.PageSizeName = "pageSize";
            mmcoyPager.ParamObj = paramObj;
            return new MvcHtmlString(mmcoyPager.PagerSplitString);
        }

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="html"></param>
        /// <param name="mmcoyPager">分页对象</param>
        /// <returns></returns>
        public static MvcHtmlString ToPagerHtml(this HtmlHelper html, MFPagerSplit mmcoyPager)
        {
            if (!mmcoyPager.IsNullOrEmpty())
                return new MvcHtmlString(mmcoyPager.PagerSplitString);
            else
                throw new ArgumentNullException("分页对象为空！");
        }

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="html"></param>
        /// <param name="mmcoyPager">分页对象</param>
        /// <param name="routeData">路由数据</param>
        /// <returns></returns>
        public static MvcHtmlString ToPagerHtml(this HtmlHelper html, MFPagerSplit mmcoyPager, RouteData routeData)
        {
            if (!mmcoyPager.IsNullOrEmpty())
            {
                mmcoyPager.Area = routeData.DataTokens["area"].ObjectToString();
                return new MvcHtmlString(mmcoyPager.PagerSplitString);
            }
            else
            {
                throw new ArgumentNullException("分页对象为空！");
            }
        }

        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="html"></param>
        /// <param name="routeData">当前路由数据</param>
        /// <param name="index">页码</param>
        /// <param name="size">每页显示记录数</param>
        /// <param name="totalPage">总页数</param>
        /// <param name="paramObj">Url上的附加数据</param>
        /// <returns></returns>
        public static MvcHtmlString ToPagerHtml(this HtmlHelper html, RouteData routeData, int index, int size, int totalPage, object paramObj = null)
        {
            var pager = new MFPagerSplit(routeData, totalPage, pageIndex: index, pageSize: size, paramObj: paramObj);
            pager.Area = routeData.DataTokens["area"].ObjectToString();
            pager.Action = routeData.Values["action"].ObjectToString();
            pager.Controller = routeData.Values["controller"].ObjectToString();
            return html.ToPagerHtml(pager);
        }

        /// <summary>
        /// 获取JObject的属性值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetJsonValue(this JObject json, string propertyName)
        {
            return json.Value<string>(propertyName);
        }
        #endregion

        #region 生成验证码

        private const string IdPrefix = "validateCode";
        private const int Length = 4;

        /// <summary>
        /// 生成验证码控件
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString ValidateCode(this HtmlHelper helper)
        {
            return ValidateCode(helper, IdPrefix);
        }

        /// <summary>
        /// 生成验证码控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">控件Id前缀</param>
        /// <returns></returns>
        public static MvcHtmlString ValidateCode(this HtmlHelper helper, string id)
        {
            return ValidateCode(helper, id, Length);
        }

        /// <summary>
        /// 生成验证码控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">控件Id前缀</param>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        public static MvcHtmlString ValidateCode(this HtmlHelper helper, string id, int length)
        {
            return ValidateCode(helper, id, length, null);
        }

        /// <summary>
        /// 生成验证码控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">控件Id前缀</param>
        /// <param name="htmlAttributes">Html属性</param>
        /// <returns></returns>
        public static MvcHtmlString ValidateCode(this HtmlHelper helper, string id, object htmlAttributes)
        {
            return ValidateCode(helper, id, Length, htmlAttributes);
        }

        /// <summary>
        /// 生成验证码控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="length">验证码长度</param>
        /// <param name="htmlAttributes">Html属性</param>
        /// <returns></returns>
        public static MvcHtmlString ValidateCode(this HtmlHelper helper, int length, object htmlAttributes)
        {
            return ValidateCode(helper, IdPrefix, length, htmlAttributes);
        }

        /// <summary>
        /// 生成验证码控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="length">验证码长度</param>
        /// <returns></returns>
        public static MvcHtmlString ValidateCode(this HtmlHelper helper, int length)
        {
            return ValidateCode(helper, length, null);
        }

        /// <summary>
        /// 生成验证码控件
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id">控件Id前缀</param>
        /// <param name="length">验证码长度</param>
        /// <param name="htmlAttributes">Html属性</param>
        /// <returns></returns>
        public static MvcHtmlString ValidateCode(this HtmlHelper helper, string id, int length, object htmlAttributes)
        {
            string finalId = id + "_imgValidateCode";
            var tagBuild = new TagBuilder("img");
            tagBuild.GenerateId(finalId);
            var defaultController = ((Route)RouteTable.Routes["Default"]).Defaults["controller"] + "/";
            var controller = HttpContext.Current.Request.Url.Segments.Length == 1
                                 ? defaultController
                                 : HttpContext.Current.Request.Url.Segments[1];
            tagBuild.MergeAttribute("src", string.Format("/{0}GetValidateCode?length={1}", controller, length));
            tagBuild.MergeAttribute("alt", "看不清？点我试试看！");
            tagBuild.MergeAttribute("style", "cursor:pointer;");
            tagBuild.MergeAttributes(AnonymousObjectToHtmlAttributes(htmlAttributes));
            var sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("");
            sb.AppendFormat("document.getElementById(\"{0}\").onclick= function () {{", finalId);
            //sb.Append("$(this).attr(\"style\", \"cursor:pointer;\");");
            sb.Append("var url = this.src;");
            sb.Append("url += \"&\" + Math.random();");
            sb.Append("this.src= url;");
            sb.Append("};");
            sb.Append("</script>");
            return MvcHtmlString.Create(tagBuild + sb.ToString());
        }

        public static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
        {
            var result = new RouteValueDictionary();

            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    result.Add(property.Name.Replace('_', '-'), property.GetValue(htmlAttributes));
                }
            }

            return result;
        }

        #endregion

        #region 自定义Html输出
        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, string name, string text, bool? isChecked)
        {
            string strHtml = string.Format("<input type=\"checkbox\" id=\"{0}\" name=\"{0}\" " + ((isChecked.HasValue && isChecked.Value == true) ? "checked=\"checked\"" : "") + " /><label id=\"lbl{0}\" for=\"{0}\">{1}</label>", name, text);
            return new MvcHtmlString(strHtml);
        }

        /// <summary>
        /// 课件类型枚举Html
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">控件名称</param>
        /// <param name="selectedValue">选中的值</param>
        /// <returns></returns>
        public static MvcHtmlString RadiobuttonFoxEnumCourseWareType(this HtmlHelper htmlHelper, string name, int selectedValue)
        {
            string strHtml = string.Empty;
            strHtml += "<input type=\"radio\" onClick=\"clickForType(0);\" id=\"" + string.Format("radio_{0}_{1}", name, 0) + "\" name=\"" + name + "\" value=\"0\" " + ((selectedValue == 0) ? "checked=\"checked\"" : "") + " /><label id=\"lbl_" + string.Format("radio_{0}_{1}", name, 0) + "\" for=\"" + string.Format("radio_{0}_{1}", name, 0) + "\">视频</label>";
            strHtml += "<input type=\"radio\" onClick=\"clickForType(1);\" id=\"" + string.Format("radio_{0}_{1}", name, 1) + "\" name=\"" + name + "\" value=\"1\" " + ((selectedValue == 1) ? "checked=\"checked\"" : "") + " /><label id=\"lbl_" + string.Format("radio_{0}_{1}", name, 1) + "\" for=\"" + string.Format("radio_{0}_{1}", name, 1) + "\">音频</label>";
            strHtml += "<input type=\"radio\" onClick=\"clickForType(2);\" id=\"" + string.Format("radio_{0}_{1}", name, 2) + "\" name=\"" + name + "\" value=\"2\" " + ((selectedValue == 2) ? "checked=\"checked\"" : "") + " /><label id=\"lbl_" + string.Format("radio_{0}_{1}", name, 2) + "\" for=\"" + string.Format("radio_{0}_{1}", name, 2) + "\">图文</label>";
            //strHtml += "<input type=\"radio\" onClick=\"clickForType(3);\" id=\"" + string.Format("radio_{0}_{1}", name, 3) + "\" name=\"" + name + "\" value=\"3\" " + ((selectedValue == 3) ? "checked=\"checked\"" : "") + " /><label id=\"lbl_" + string.Format("radio_{0}_{1}", name, 3) + "\" for=\"" + string.Format("radio_{0}_{1}", name, 3) + "\">幻灯片</label>";
            return new MvcHtmlString(strHtml);
        }
        #endregion
    }
}