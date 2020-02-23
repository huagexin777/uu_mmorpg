using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using log4net.Core;
using log4net.Repository;


namespace Mmcoy.Framework
{
    internal class MFAppLogImpl : LogImpl, MFIAppLog, ILog, ILoggerWrapper
    {
        // Fields
        private Level m_levelTrace;
        private static readonly Level s_defaultLevelTrace;
        private static readonly Type ThisDeclaringType;

        // Methods
        static MFAppLogImpl()
        {
            ThisDeclaringType = typeof(MFAppLogImpl);
            s_defaultLevelTrace = new Level(0x4e20, "TRACE");
        }
        public MFAppLogImpl(ILogger logger)
            : base(logger)
        {
        }

        protected override void ReloadLevels(ILoggerRepository repository)
        {
            base.ReloadLevels(repository);
            this.m_levelTrace = repository.LevelMap.LookupWithDefault(s_defaultLevelTrace);
        }
        public void Trace(object message)
        {
            this.Logger.Log(ThisDeclaringType, this.m_levelTrace, message, null);
        }
        public void Trace(object message, Exception t)
        {
            this.Logger.Log(ThisDeclaringType, this.m_levelTrace, message, t);
        }
        public void TraceFormat(string format, params object[] args)
        {
            this.Logger.Log(ThisDeclaringType, this.m_levelTrace, string.Format(format, args), null);
        }
        public bool IsTraceEnabled
        {
            get
            {
                return this.Logger.IsEnabledFor(this.m_levelTrace);
            }
        }
    }
}
