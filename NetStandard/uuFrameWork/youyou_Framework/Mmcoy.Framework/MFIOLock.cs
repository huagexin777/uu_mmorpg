using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    class MFIOLock
    {
        private System.Threading.ReaderWriterLock m_lock;
        public MFIOLock()
        {
            m_lock = new System.Threading.ReaderWriterLock();
        }

        /// <summary>
        /// 获取读者锁
        /// </summary>
        public void AcquireReaderLock()
        {
            m_lock.AcquireReaderLock(-1);
        }

        /// <summary>
        /// 释放读者锁
        /// </summary>
        public void ReleaseReaderLock()
        {
            m_lock.ReleaseReaderLock();
        }

        /// <summary>
        /// 获取写者锁
        /// </summary>
        public void AcquireWriterLock()
        {
            m_lock.AcquireWriterLock(-1);
        }

        /// <summary>
        /// 释放写者锁
        /// </summary>
        public void ReleaseWriterLock()
        {
            m_lock.ReleaseWriterLock();
        }
    }
}