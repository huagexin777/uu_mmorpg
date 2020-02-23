using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mmcoy.Framework
{
    /// <summary>
    /// XML扩展方法类
    /// </summary>
    public static class MFXMLUtil
    {
        #region FindElements 获取指定节点列表的元素值
        /// <summary>
        /// 获取指定节点列表的元素值
        /// </summary>
        /// <param name="el">根元素</param>
        /// <param name="els">节点列表 例(ParentEl/El/ChildEl)</param>
        /// <returns>节点列表</returns>
        public static IList<XElement> FindElements(this XElement el, string els)
        {
            var elArray = els.ToList('/').ToArray();
            return FindElements(el, elArray);
        }
        #endregion

        #region FindElements 获取指定节点列表的元素值
        /// <summary>
        /// 获取指定节点列表的元素值
        /// </summary>
        /// <param name="el">根元素</param>
        /// <param name="els">节点列表</param>
        /// <returns>节点列表</returns>
        public static IList<XElement> FindElements(this XElement el, IList<string> els)
        {
            return FindElements(el, els.ToArray());
        }
        #endregion

        #region FindElements 获取指定节点列表的元素值
        /// <summary>
        /// 获取指定节点列表的元素值
        /// </summary>
        /// <param name="el">根元素</param>
        /// <param name="els">节点集合</param>
        /// <returns>节点列表</returns>
        public static IList<XElement> FindElements(this XElement el, string[] els)
        {
            if (el.IsNullOrEmpty())
                throw new Exception("您给定的根元素为空！");
            var node = el;
            XNamespace ns = string.Empty;
            if (el.HasAttributeName("xmlns"))
                ns = el.Attribute("xmlns").Value;
            var elList = new List<XElement>();
            for (int i = 0; i < els.Length; i++)
            {
                if (i != els.Length - 1)
                {
                    var tempEls = node.Element(ns + els[i]);
                    if (!tempEls.IsNullOrEmpty())
                        node = tempEls;
                }
                else
                {
                    elList = node.Elements(ns + els[i]).ToList();
                }
            }
            return elList;
        }
        #endregion

        #region ElToString 集合转换成指定符号分隔的字符串
        /// <summary>
        /// 将XElement集合转换成指定符号分隔的字符串
        /// </summary>
        /// <param name="els">元素列表</param>
        /// <param name="opr">指定分隔符</param>
        /// <returns></returns>
        public static string ElToString(this IList<XElement> els, string opr)
        {
            var list = new List<string>();
            foreach (var el in els)
            {
                list.Add(el.Value);
            }
            return list.ListToString(opr);
        }
        #endregion

        #region HasAttributeName 是否存在该属性名称
        /// <summary>
        /// 是否存在该属性名称
        /// </summary>
        /// <param name="el">指定元素</param>
        /// <param name="attrName">属性名称</param>
        /// <returns>是否存在属性</returns>
        public static bool HasAttributeName(this XElement el, string attrName)
        {
            if (!el.IsNullOrEmpty())
            {
                var node = el.Attribute(attrName);
                if (!node.IsNullOrEmpty() && !node.Value.IsNullOrEmpty())
                    return true;
            }
            return false;
        }
        #endregion
    }
}