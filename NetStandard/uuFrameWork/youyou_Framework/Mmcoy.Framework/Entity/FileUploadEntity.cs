using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework.Entity
{
    /// <summary>
    /// 文件上传实体
    /// </summary>
    public class FileUploadEntity
    {
        #region 属性

        #region FileName 文件原始名称
        /// <summary>
        /// 文件原始名称
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        #endregion

        #region FileExtendName 文件扩展名
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtendName
        {
            get
            {
                if (string.IsNullOrEmpty(FileName))
                    return string.Empty;
                var fileNames = FileName.Split('.');
                return fileNames[fileNames.Length - 1];
            }
        }
        #endregion

        #region FileType
        /// <summary>
        /// 上传的文件类型(Video, Audio, Image)
        /// </summary>
        public string FileType
        {
            get;
            set;
        }
        #endregion

        #region NewFileName 新生成的文件名称
        private string _newFileName = string.Empty;
        /// <summary>
        /// 新生成的文件名称
        /// </summary>
        public string NewFileName
        {
            get
            {
                if (string.IsNullOrEmpty(_newFileName))
                {
                    var id = this.UniqueId;
                    _newFileName = string.Format("{0}.{1}", id, FileExtendName);
                }
                return _newFileName;
            }
        }
        #endregion

        #region NewFileNameMin 新生成的文件名称缩略
        private string _newFileNameMin = string.Empty;
        /// <summary>
        /// 新生成的文件名称缩略
        /// </summary>
        public string NewFileNameMin
        {
            get
            {
                if (string.IsNullOrEmpty(_newFileNameMin))
                {
                    var id = this.UniqueId;
                    _newFileNameMin = string.Format("{0}_min.{1}", id, FileExtendName);
                }
                return _newFileNameMin;
            }
        }
        #endregion

        #region MonthPath 年月目录
        /// <summary>
        /// 年月目录
        /// </summary>
        public string MonthPath
        {
            get
            {
                return  DateTime.Now.ToString("yyyyMM");
            }
        }
        #endregion

        #region UniqueId 全局编号
        private string _UniqueId = null;
        /// <summary>
        /// 全局编号
        /// </summary>
        public string UniqueId
        {
            get
            {
                if (_UniqueId == null)
                {
                    _UniqueId = MFUniqueKey.CreateNew().ID;
                }
                return _UniqueId;
            }
        }
        #endregion

        #region Data 文件数据
        /// <summary>
        /// 文件数据
        /// </summary>
        public byte[] Data
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region 常量

        private const string JPEG_CODE = "0xFFD8FF";

        private const string PNG_CODE = "0x89504E470D0A1A0A";

        private const string GIF_CODE = "GIF8";

        #endregion

        #region IsValid 已验证
        /// <summary>
        /// 已验证
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (Data!=null && Data.Length>0)
            {
                if (FileExtendName.Equals("jpg", StringComparison.CurrentCultureIgnoreCase) || FileExtendName.Equals("jpeg", StringComparison.CurrentCultureIgnoreCase) || FileExtendName.Equals("png", StringComparison.CurrentCultureIgnoreCase) || FileExtendName.Equals("gif", StringComparison.CurrentCultureIgnoreCase))
                {
                    using (MemoryStream mStream = new MemoryStream(Data))
                    {
                        string fileclass = mStream.ReadByte().ToString() + mStream.ReadByte().ToString();
                        if (fileclass == "7790" || fileclass == "8269" || fileclass == "64101" || fileclass == "10056" || fileclass == "4742")
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}