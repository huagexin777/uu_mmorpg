using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mmcoy.Framework.AbstractBase
{
    public abstract class MFAbstractXmlDAL
    {
        #region 私有变量定义
        MFIAppLog log = MFAppLoggerManager.GetLogger(typeof(MFAbstractXmlDAL));
        #endregion

        #region 构造函数 MFAbstractXmlDAL
        /// <summary>
        /// 构造函数
        /// </summary>
        public MFAbstractXmlDAL()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xmlPath">配置文件路径</param>
        public MFAbstractXmlDAL(string xmlPath)
        {
            this.XmlPath = xmlPath;
        }
        #endregion

        #region XmlPath Xml文件路径
        /// <summary>
        /// Xml文件路径
        /// </summary>
        public string XmlPath
        {
            get;
            set;
        }
        #endregion

        #region Document XDocument文档
        private XDocument _XDocument;
        /// <summary>
        /// XDocument文档
        /// </summary>
        /// <returns></returns>
        public XDocument Document
        {
            get
            {
                try
                {
                    if (_XDocument == null)
                    {
                        _XDocument = (XDocument)MFCacheUtil.GetCacheDependencyFile(this.XmlPath, this.XmlPath);
                        if (_XDocument == null)
                        {
                            _XDocument = XDocument.Load(this.XmlPath);
                            MFCacheUtil.SetCacheDependencyFile(this.XmlPath, _XDocument, this.XmlPath);
                        }
                    }
                    return _XDocument;
                }
                catch (Exception ex)
                {
                    log.Error("加载Xml文档失败", ex);
                    return null;
                }
            }
            set
            {
                _XDocument = value;
            }
        }
        #endregion

        #region RootElement 跟节点
        /// <summary>
        /// 跟节点
        /// </summary>
        /// <returns></returns>
        public XElement RootElement
        {
            get
            {
                try
                {
                    if (this.Document == null)
                    {
                        return null;
                    }
                    return this.Document.Root;
                }
                catch(Exception ex)
                {
                    log.Error("加载根节点失败", ex);
                    return null;
                }
            }
        }
        #endregion

        #region GetElement 获取单一节点
        /// <summary>
        /// 获取单一节点
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="xName"></param>
        /// <returns></returns>
        public XElement GetElement(XElement xElement, string xName)
        {
            try
            {
                return xElement.Element(xName);
            }
            catch (Exception ex)
            {
                log.Error("获取单一节点失败", ex);
                return null;
            }
        }
        #endregion

        #region GetElement 获取单一节点
        /// <summary>
        /// 获取单一节点
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="xName"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public XElement GetElement(XElement xElement, string xName, string attribute, string value)
        {
            try
            {
                IList<XElement> xElements = this.GetElements(xElement, xName, attribute, value);
                if(xElements == null || xElements.Count == 0)
                {
                    return null;
                }
                return xElements[0];
            }
            catch (Exception ex)
            {
                log.Error("获取单一节点失败", ex);
                return null;
            }
        }
        #endregion

        #region GetElements 获取节点集合
        /// <summary>
        /// 获取节点集合
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="xName"></param>
        /// <returns></returns>
        public IList<XElement> GetElements(XElement xElement, string xName)
        {
            try
            {
                return xElement.Elements(xName).ToList();
            }
            catch(Exception ex)
            {
                log.Error("获取节点集合失败", ex);
                return null;
            }
        }
        #endregion

        #region GetElements 获取节点集合
        /// <summary>
        /// 获取节点集合
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="xName"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IList<XElement> GetElements(XElement xElement, string xName, string attribute, string value)
        {
            try
            {
                return xElement.Elements(xName).Where(element => element.Attribute(attribute).Value == value).ToList();
            }
            catch(Exception ex)
            {
                log.Error("获取节点集合失败", ex);
                return null;
            }
        }
        #endregion

        #region Save 保存Xml文档
        /// <summary>
        /// 保存Xml文档
        /// </summary>
        public void Save()
        {
            try
            {
                if (this.Document != null)
                {
                    this.Document.Save(this.XmlPath);
                }
            }
            catch (Exception ex)
            {
                log.Error("保存xml文档失败", ex);
            }
        }
        /// <summary>
        /// 保存Xml文档
        /// </summary>
        /// <param name="doc"></param>
        public void Save(XDocument doc)
        {
            try
            {
                doc.Save(this.XmlPath);
            }
            catch (Exception ex)
            {
                log.Error("保存xml文档失败", ex);
            }
        }
        #endregion
    }
}