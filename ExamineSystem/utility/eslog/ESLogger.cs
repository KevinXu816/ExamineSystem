using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace ExamineSystem.utility.eslog
{
    public class ESLogger : ILog
    {
        private IESLog esLogger = ESLogManager.GetLogger("track");
        private Type type = null;
        private readonly static string messageFormat = "{0}   -->   {1}";

        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        public void Debug(object message, Exception exception)
        {
            string msg = string.Format(messageFormat, type.FullName, message);
            esLogger.Debug(msg, exception);
        }

        public void Debug(object message)
        {
            string msg = string.Format(messageFormat, type.FullName, message);
            esLogger.Debug(msg);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            esLogger.DebugFormat(provider, format, args);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            esLogger.DebugFormat(format, arg0, arg1, arg2);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            esLogger.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0)
        {
            esLogger.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, params object[] args)
        {
            esLogger.DebugFormat(format, args);
        }

        public void Error(object message, Exception exception)
        {
            string msg = string.Format(messageFormat, type.FullName, message);
            esLogger.Error(msg, exception);
        }

        public void Error(object message)
        {
            string msg = string.Format(messageFormat, type.FullName, message);
            esLogger.Error(msg);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            esLogger.ErrorFormat(provider, format, args);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            esLogger.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            esLogger.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0)
        {
            esLogger.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            esLogger.ErrorFormat(format, args);
        }

        public void Fatal(object message, Exception exception)
        {
            string msg = string.Format(messageFormat, type.FullName, message);
            esLogger.Fatal(msg, exception);
        }

        public void Fatal(object message)
        {
            string msg = string.Format(messageFormat, type.FullName, message);
            esLogger.Fatal(msg);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            esLogger.FatalFormat(provider, format, args);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            esLogger.FatalFormat(format, arg0, arg1, arg2);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            esLogger.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0)
        {
            esLogger.FatalFormat(format, arg0);
        }

        public void FatalFormat(string format, params object[] args)
        {
            esLogger.FatalFormat(format, args);
        }

        public void Info(object message, Exception exception)
        {
            string msg = string.Format(messageFormat, type.FullName, message);
            esLogger.Info(msg, exception);
        }

        public void Info(object message)
        {
            string msg = string.Format(messageFormat, type.FullName, message);
            esLogger.Info(msg);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            esLogger.InfoFormat(provider, format, args);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            esLogger.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            esLogger.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0)
        {
            esLogger.InfoFormat(format, arg0);
        }

        public void InfoFormat(string format, params object[] args)
        {
            esLogger.InfoFormat(format, args);
        }

        public bool IsDebugEnabled
        {
            get { return esLogger.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return esLogger.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return esLogger.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return esLogger.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return esLogger.IsWarnEnabled; }
        }

        public void Warn(object message, Exception exception)
        {
            string msg = string.Format(messageFormat, type.FullName, message);
            esLogger.Warn(msg, exception);
        }

        public void Warn(object message)
        {
            string msg = string.Format(messageFormat, type.FullName, message);
            esLogger.Warn(msg);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            esLogger.WarnFormat(provider, format, args);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            esLogger.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            esLogger.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0)
        {
            esLogger.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, params object[] args)
        {
            esLogger.WarnFormat(format, args);
        }

        #region ILoggerWrapper Members

        public log4net.Core.ILogger Logger
        {
            get { return esLogger.Logger; }
        }

        #endregion
    }
}