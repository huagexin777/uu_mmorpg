using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    /// <summary>
    /// 全局唯一标识
    /// </summary>
    [Serializable]
    public class MFUniqueKey
    {
        #region private fields
        private string uniqueId = null;
        private static MFUniqueKey empty = null;
        private static string preveTime = null;
        private static int priveneNumber;
        private static object lock_obj = new object();
        #endregion

        #region consturctors
        static MFUniqueKey()
        {
            MFUniqueKey.empty = new MFUniqueKey(string.Empty);
        }
        public MFUniqueKey(string id)
        {
            uniqueId = id;
        }
        #endregion

        #region public properties
        public static MFUniqueKey Empty
        {
            get
            {
                return MFUniqueKey.empty;
            }
        }

        public string ID
        {
            get
            {
                return this.uniqueId;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// 获得序号
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private static string GetSequence(string tag)
        {
            //object oLock = new object();
            lock (lock_obj)
            {
                string timeNow = null;
                if (tag == null)
                {
                    timeNow = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                else
                {
                    while (tag.Length < 4)
                    {
                        tag = tag.Insert(0, "0");
                    }

                    timeNow = tag + System.DateTime.Now.ToString("MMddHHmmss");
                }
                if (preveTime == timeNow)
                {
                    priveneNumber++;
                }
                else
                {
                    priveneNumber = 1;
                }
                string sequence = priveneNumber.ToString();

                while (sequence.Length < 5)
                {
                    sequence = sequence.Insert(0, "0");
                }
                //代表上次访问时间
                preveTime = timeNow;

                string id = timeNow + sequence;
                return id;
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// 创建全局唯一标识。
        /// </summary>
        /// <returns>唯一标识</returns>
        public static MFUniqueKey CreateNew()
        {
            string id = GetSequence(null);
            return new MFUniqueKey(id); ;
        }
        public static MFUniqueKey CreateNew(string tag)
        {
            string id = GetSequence(tag);
            return new MFUniqueKey(id); ;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is MFUniqueKey)
            {
                MFUniqueKey key = (MFUniqueKey)obj;
                return (this.uniqueId == key.ID);
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return uniqueId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}