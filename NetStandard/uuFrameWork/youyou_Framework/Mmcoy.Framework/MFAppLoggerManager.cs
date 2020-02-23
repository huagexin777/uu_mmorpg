using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    public sealed class MFAppLoggerManager
    {
        // Fields
        private static readonly WrapperMap s_wrapperMap;

        // Methods
        static MFAppLoggerManager()
        {
            s_wrapperMap = new WrapperMap(new WrapperCreationHandler(MFAppLoggerManager.WrapperCreationHandler));
        }

        private MFAppLoggerManager()
        {

        }

        public static MFIAppLog Exists(string name)
        {
            return Exists(Assembly.GetCallingAssembly(), name);
        }
        public static MFIAppLog Exists(Assembly assembly, string name)
        {
            return WrapLogger(LoggerManager.Exists(assembly, name));
        }
        public static MFIAppLog Exists(string domain, string name)
        {
            return WrapLogger(LoggerManager.Exists(domain, name));
        }
        public static MFIAppLog[] GetCurrentLoggers()
        {
            return GetCurrentLoggers(Assembly.GetCallingAssembly());
        }
        public static MFIAppLog[] GetCurrentLoggers(Assembly assembly)
        {
            return WrapLoggers(LoggerManager.GetCurrentLoggers(assembly));
        }
        public static MFIAppLog[] GetCurrentLoggers(string domain)
        {
            return WrapLoggers(LoggerManager.GetCurrentLoggers(domain));
        }
        public static MFIAppLog GetLogger(string name)
        {
            return GetLogger(Assembly.GetCallingAssembly(), name);
        }
        public static MFIAppLog GetLogger(Type type)
        {
            return GetLogger(Assembly.GetCallingAssembly(), type.FullName);
        }
        public static MFIAppLog GetLogger(Assembly assembly, string name)
        {
            return WrapLogger(LoggerManager.GetLogger(assembly, name));
        }
        public static MFIAppLog GetLogger(Assembly assembly, Type type)
        {
            return WrapLogger(LoggerManager.GetLogger(assembly, type));
        }
        public static MFIAppLog GetLogger(string domain, string name)
        {
            return WrapLogger(LoggerManager.GetLogger(domain, name));
        }
        public static MFIAppLog GetLogger(string domain, Type type)
        {
            return WrapLogger(LoggerManager.GetLogger(domain, type));
        }
        private static MFIAppLog WrapLogger(ILogger logger)
        {
            return (MFIAppLog)s_wrapperMap.GetWrapper(logger);
        }
        private static MFIAppLog[] WrapLoggers(ILogger[] loggers)
        {
            MFIAppLog[] logArray = new MFIAppLog[loggers.Length];
            for (int i = 0; i < loggers.Length; i++)
            {
                logArray[i] = WrapLogger(loggers[i]);
            }
            return logArray;
        }
        private static ILoggerWrapper WrapperCreationHandler(ILogger logger)
        {
            return new MFAppLogImpl(logger);
        }
    }
}