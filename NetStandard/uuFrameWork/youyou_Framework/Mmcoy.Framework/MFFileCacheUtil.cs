using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    /// <summary>
    /// 文件缓存
    /// </summary>
    public sealed class MFFileCacheUtil
    {
        private static string _state = "open";
        private static string _path = string.Empty;
        private static Object thisLock = new Object();
        private static MFIOLock io_lock = new MFIOLock();

        /// <summary>
        /// 静态构造
        /// </summary>
        static MFFileCacheUtil()
        {
            try
            {
                if (System.Web.HttpContext.Current != null)
                {
                    _path = System.Web.HttpContext.Current.Server.MapPath("~/filecache/");
                    _state = ConfigurationManager.AppSettings["filecache"];
                }
            }
            catch { }
        }

        /// <summary>
        /// 缓存过期时间(秒)
        /// </summary>
        private static int _timeOut = 3000;
        public static int TimeOut
        {
            get { return _timeOut; }
            set { _timeOut = value; }
        }

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objID">对象的键值</param>
        /// <param name="obj">缓存的对象</param>
        public static void AddObject(string objID, object obj)
        {
            AddObject(objID, obj, TimeOut);
        }

        public static void AddObject(string objID, object obj, int expire)
        {
            if (_state == "close") return;
            if (obj != null)
            {
                DateTime expireTime = DateTime.Now.AddSeconds(expire);
                if (expireTime.CompareTo(DateTime.Now) > 0)
                {
                    objID = objID.Md5();
                    FileJson j = new FileJson();
                    j.ExpireTime = expireTime;
                    j.Obj = obj;

                    string content = MFSerializationUtil.Serialize<FileJson>(j);
                    string directoryPath = GetCacheFile(objID);
                    if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                    if (!File.Exists(directoryPath + objID))
                    {
                        io_lock.AcquireWriterLock();
                        try
                        {
                            File.WriteAllText(directoryPath + objID, content, Encoding.UTF8);
                        }

                        finally
                        {
                            io_lock.ReleaseWriterLock();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="objID"></param>
        public static void RemoveObject(string objID)
        {
            objID = objID.Md5();
            string directoryPath = GetCacheFile(objID);
            if (File.Exists(directoryPath + objID)) File.Delete(directoryPath + objID);
        }

        public static void RemovePatternObject(string objID)
        {

        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void FlushAll()
        {
            DirectoryInfo dir = new DirectoryInfo(_path);
            foreach (FileInfo i in dir.GetFiles())
            {
                i.Delete();
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="objID"></param>
        /// <returns></returns>
        public static object GetObject(string objID)
        {
            if (string.IsNullOrEmpty(objID)) return null;
            if (_state == "close") return null;

            string k = objID.Md5();
            string path = GetCacheFile(k) + k;
            if (File.Exists(path))
            {
                FileInfo i = new FileInfo(path);
                if (i != null)
                {
                    object data = null;
                    io_lock.AcquireReaderLock();
                    try
                    {
                        FileJson j = MFSerializationUtil.Deserialize<FileJson>(File.ReadAllText(i.FullName, Encoding.UTF8));
                        if (j != null)
                        {
                            if (j.ExpireTime.CompareTo(DateTime.Now) < 0)
                            {
                                File.Delete(i.FullName);
                            }

                            else
                            {
                                data = j.Obj;
                            }
                        }
                    }

                    catch
                    { }

                    finally
                    {
                        io_lock.ReleaseReaderLock();
                    }

                    return data;
                }
            }

            return null;
        }

        private static string GetCacheFile(string objID)
        {
            if (objID.Length >= 2)
            {
                string directoryName = objID.Substring(0, 2);
                directoryName = _path + directoryName;
                directoryName += "\\" + objID.Substring(2, 2);
                directoryName += "\\" + objID.Substring(4, 2);
                directoryName += "\\" + objID.Substring(6, 2);
                directoryName += "\\" + objID.Substring(8, 2);
                return directoryName + "\\";
            }

            else
            {
                return _path;
            }
        }
    }

    [Serializable]
    class FileJson
    {
        private DateTime _expireTime;
        public DateTime ExpireTime
        {
            get { return _expireTime; }
            set { _expireTime = value; }
        }

        private string _codeName;
        public string CodeName
        {
            get { return _codeName; }
            set { _codeName = value; }
        }


        private object _obj;
        public object Obj
        {
            get { return _obj; }
            set { _obj = value; }
        }
    }
}