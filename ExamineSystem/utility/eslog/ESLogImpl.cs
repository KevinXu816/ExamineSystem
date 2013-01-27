using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Core;
using log4net.Util;
using System.Globalization;

namespace ExamineSystem.utility.eslog
{
    internal class ESLogImpl : LogImpl, IESLog
    {
        
        private readonly static Type ThisDeclaringType = typeof(ESLogImpl);

        public ESLogImpl(ILogger logger)
            : base(logger)
        {
            // do nothing
        }

        //Debug -----------------------------------------//
        public override void DebugFormat(string format, params object[] args)
        {
            Exception ex = this.filterException(args);
            this.Debug(new SystemStringFormat(CultureInfo.InvariantCulture, format, args), ex);
        }

        public override void DebugFormat(string format, object arg0)
        {
            Exception ex = this.filterException(arg0);
            this.Debug(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0 }), ex);
        }

        public override void DebugFormat(string format, object arg0, object arg1)
        {
            Exception ex = this.filterException(arg0, arg1);
            this.Debug(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0, arg1 }), ex);
        }

        public override void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Exception ex = this.filterException(arg0, arg1, arg2);
            this.Debug(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0, arg1, arg2 }), ex);
        }

        public override void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Exception ex = this.filterException(args);
            this.Debug(new SystemStringFormat(provider, format, args), ex);
        }

        public override void Debug(object message)
        {
            this.Debug(message, "");
        }

        public override void Debug(object message, Exception exception)
        {
            this.Debug(message, exception, "");
        }

        public void Debug(object message, string from) 
        {
            this.Debug(message, null, from);
        }

        public void Debug(object message, Exception exception, string from) 
        {
            string sessionInfo;
            ExpandContext.getContextInfo(out sessionInfo);
            this.Debug(message, exception, sessionInfo, from);
        }

        private void Debug(object message, Exception exception, 
            string session, string errorFrom)
        {
            if (this.IsDebugEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository,
                    Logger.Name, Level.Debug, message, exception);
                loggingEvent.Properties["Session"] = session;
                loggingEvent.Properties["ErrorFrom"] = errorFrom;
                Logger.Log(loggingEvent);
            }
        }

        //Info -----------------------------------------//
        public override void InfoFormat(string format, params object[] args)
        {
            Exception ex = this.filterException(args);
            this.Info(new SystemStringFormat(CultureInfo.InvariantCulture, format, args), ex);
        }

        public override void InfoFormat(string format, object arg0)
        {
            Exception ex = this.filterException(arg0);
            this.Info(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0 }), ex);
        }

        public override void InfoFormat(string format, object arg0, object arg1)
        {
            Exception ex = this.filterException(arg0, arg1);
            this.Info(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0, arg1 }), ex);
        }

        public override void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Exception ex = this.filterException(arg0, arg1, arg2);
            this.Info(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0, arg1, arg2 }), ex);
        }

        public override void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Exception ex = this.filterException(args);
            this.Info(new SystemStringFormat(provider, format, args), ex);
        }

        public override void Info(object message)
        {
            this.Info(message, "");
        }

        public void Info(object message, string from)
        {
            this.Info(message, null, from);
        }

        public override void Info(object message, Exception exception)
        {
            this.Info(message, exception, "");
        }

        public void Info(object message, Exception exception, string from)
        {
            string sessionInfo;
            ExpandContext.getContextInfo(out sessionInfo);
            this.Info(message, exception, sessionInfo, from);
        }

        private void Info(object message, Exception exception,
            string session, string errorFrom)
        {
            if (this.IsInfoEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository,
                    Logger.Name, Level.Info, message, exception);
                loggingEvent.Properties["Session"] = session;
                loggingEvent.Properties["ErrorFrom"] = errorFrom;
                Logger.Log(loggingEvent);
            }
        }


        //Warn -----------------------------------------//
        public override void WarnFormat(string format, params object[] args)
        {
            Exception ex = this.filterException(args);
            this.Warn(new SystemStringFormat(CultureInfo.InvariantCulture, format, args), ex);
        }

        public override void WarnFormat(string format, object arg0)
        {
            Exception ex = this.filterException(arg0);
            this.Warn(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0 }), ex);
        }

        public override void WarnFormat(string format, object arg0, object arg1)
        {
            Exception ex = this.filterException(arg0, arg1);
            this.Warn(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0, arg1 }), ex);
        }

        public override void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Exception ex = this.filterException(arg0, arg1, arg2);
            this.Warn(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0, arg1, arg2 }), ex);
        }

        public override void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Exception ex = this.filterException(args);
            this.Warn(new SystemStringFormat(provider, format, args), ex);
        }

        public override void Warn(object message)
        {
            this.Warn(message, "");
        }

        public void Warn(object message, string from)
        {
            this.Warn(message, null, from);
        }

        public override void Warn(object message, Exception exception)
        {
            this.Warn(message, exception, "");
        }

        public void Warn(object message, Exception exception, string from)
        {
            string sessionInfo;
            ExpandContext.getContextInfo(out sessionInfo);
            this.Warn(message, exception, sessionInfo, from);
        }

        private void Warn(object message, Exception exception,
            string session, string errorFrom)
        {
            if (this.IsWarnEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository,
                    Logger.Name, Level.Warn, message, exception);
                loggingEvent.Properties["Session"] = session;
                loggingEvent.Properties["ErrorFrom"] = errorFrom;
                Logger.Log(loggingEvent);
            }
        }


        //Error -----------------------------------------//
        public override void ErrorFormat(string format, params object[] args)
        {
            Exception ex = this.filterException(args);
            this.Error(new SystemStringFormat(CultureInfo.InvariantCulture, format, args), ex);
        }

        public override void ErrorFormat(string format, object arg0)
        {
            Exception ex = this.filterException(arg0);
            this.Error(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0 }), ex);
        }

        public override void ErrorFormat(string format, object arg0, object arg1)
        {
            Exception ex = this.filterException(arg0, arg1);
            this.Error(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0, arg1 }), ex);
        }

        public override void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Exception ex = this.filterException(arg0, arg1, arg2);
            this.Error(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0, arg1, arg2 }), ex);
        }

        public override void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Exception ex = this.filterException(args);
            this.Error(new SystemStringFormat(provider, format, args), ex);
        }

        public override void Error(object message)
        {
            this.Error(message, "");
        }

        public void Error(object message, string from)
        {
            string sessionInfo;
            ExpandContext.getContextInfo(out sessionInfo);
            this.Error(message, null, sessionInfo, from);
        }

        public void Error(Exception exception) 
        {
            string message = string.Empty;
            this.Error(message, exception);
        }

        public override void Error(object message, Exception exception)
        {
            if (exception == null)
            {
                string sessionInfo;
                ExpandContext.getContextInfo(out sessionInfo);
                this.Error(message, exception, sessionInfo, "");
            }
            else
            {
                string sessionInfo, errorFrom;
                ExpandContext.getContextInfo(ref message, ref exception,
                        out sessionInfo, out errorFrom);
                this.Error(message, exception, sessionInfo, errorFrom);
            }
        }

        private void Error(object message, Exception exception, string session,
            string errorFrom)
        {
            if (this.IsErrorEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository,
                    Logger.Name, Level.Error, message, exception);
                loggingEvent.Properties["Session"] = session;
                loggingEvent.Properties["ErrorFrom"] = errorFrom;
                Logger.Log(loggingEvent);
            }
        }


        //Fatal -----------------------------------------//
        public override void FatalFormat(string format, params object[] args)
        {
            Exception ex = this.filterException(args);
            this.Fatal(new SystemStringFormat(CultureInfo.InvariantCulture, format, args), ex);
        }

        public override void FatalFormat(string format, object arg0)
        {
            Exception ex = this.filterException(arg0);
            this.Fatal(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0 }), ex);
        }

        public override void FatalFormat(string format, object arg0, object arg1)
        {
            Exception ex = this.filterException(arg0, arg1);
            this.Fatal(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0, arg1 }), ex);
        }

        public override void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Exception ex = this.filterException(arg0, arg1, arg2);
            this.Fatal(new SystemStringFormat(CultureInfo.InvariantCulture, format, new object[] { arg0, arg1, arg2 }), ex);
        }

        public override void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Exception ex = this.filterException(args);
            this.Fatal(new SystemStringFormat(provider, format, args), ex);
        }

        public override void Fatal(object message)
        {
            this.Fatal(message, "");
        }

        public void Fatal(object message, string from)
        {
            string sessionInfo;
            ExpandContext.getContextInfo(out sessionInfo);
            this.Fatal(message, null, sessionInfo, from);
        }

        public void Fatal(Exception exception)
        {
            string message = string.Empty;
            this.Fatal(message, exception);
        }

        public override void Fatal(object message, Exception exception)
        {
            if (exception == null)
            {
                string sessionInfo;
                ExpandContext.getContextInfo(out sessionInfo);
                this.Fatal(message, exception, sessionInfo, "");
            }
            else 
            {
                string sessionInfo, errorFrom;
                ExpandContext.getContextInfo(ref message, ref exception,
                        out sessionInfo, out errorFrom);
                this.Fatal(message, exception, sessionInfo, errorFrom);
            }
        }

        private void Fatal(object message, Exception exception, string session,
            string errorFrom)
        {
            if (this.IsFatalEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(ThisDeclaringType, Logger.Repository,
                    Logger.Name, Level.Fatal, message, exception);
                loggingEvent.Properties["Session"] = session;
                loggingEvent.Properties["ErrorFrom"] = errorFrom;
                Logger.Log(loggingEvent);
            }
        }


        private Exception filterException(params object[] objects)
        {
            Exception ex = null;
            if (objects != null)
            {
                foreach (object obj in objects)
                {
                    ex = obj as Exception;
                    if (ex != null) break;
                }
            }
            return ex;
        }

       
    }
}