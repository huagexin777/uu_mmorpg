using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using log4net.Core;

namespace Mmcoy.Framework
{
    public interface MFIAppLog : ILog, ILoggerWrapper
    {
        // Methods
        void Trace(object message);
        void Trace(object message, Exception t);
        void TraceFormat(string format, params object[] args);

        // Properties
        bool IsTraceEnabled { get; }
    }
}