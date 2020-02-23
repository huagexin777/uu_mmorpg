using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    public sealed class MFIOUtil
    {
        #region 私有变量定义
        private static MFIAppLog log = MFAppLoggerManager.GetLogger(typeof(MFIOUtil));
        #endregion

        #region IsExists 获取文件是否存在
        /// <summary>
        /// 获取文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsExists(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region GetFolderName 获取文件夹名称
        /// <summary>
        /// 获取文件夹名称
        /// </summary>
        /// <param name="parentPath"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string GetFolderName(string parentPath, string folderName)
        {
            if (Directory.Exists(parentPath + "\\" + folderName))
            {
                if (folderName.IndexOf("(") == -1)
                {
                    folderName += "(1)";
                }
                else
                {
                    int pos1 = folderName.LastIndexOf("(");
                    int pos2 = folderName.LastIndexOf(")");

                    int index = (folderName.Substring(pos1 + 1, pos2 - (pos1 + 1)).ToInt());
                    index++;
                    folderName = folderName.Substring(0, pos1) + "(" + index + ")";
                }
                return GetFolderName(parentPath, folderName);
            }
            else
            {
                return folderName;
            }
        }
        #endregion

        #region GetDirectoryInfoList 获取路径下的全部文件夹
        /// <summary>
        /// 获取路径下的全部文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IList<DirectoryInfo> GetDirectoryInfoList(string path)
        {
            IList<DirectoryInfo> directoryInfoList = null;
            string[] arrDirectoryName = Directory.GetDirectories(path);
            if (arrDirectoryName.Length != 0)
            {
                directoryInfoList = new List<DirectoryInfo>();
                foreach (string directoryName in arrDirectoryName)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
                    directoryInfoList.Add(directoryInfo);
                }
            }
            return directoryInfoList;
        }
        #endregion

        #region CreateDirectory 创建文件夹
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        #endregion

        #region GetFileExt 获取文件扩展名
        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExt(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "";
            }
            if (fileName.IndexOf(".") < 0)
            {
                return "";
            }
            return fileName.Substring(fileName.LastIndexOf(".") + 1, (fileName.Length - fileName.LastIndexOf(".")) - 1);
        }
        #endregion

        #region GetBuffer 获取文件流数组
        /// <summary>
        /// 获取文件流数组
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetBuffer(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            byte[] buffer = new byte[fs.Length];
            try
            {
                fs.Read(buffer, 0, buffer.Length);
                fs.Seek(0, SeekOrigin.Begin);
            }
            catch{
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return buffer;
        }
        #endregion
    }
}