using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace ExamineSystem.utility.eslog
{
    public interface IESLog : ILog
    {
        void Error(object message, string from);
        void Error(Exception exception);
        void Fatal(object message, string from);
        void Fatal(Exception exception);
        void Debug(object message, string from);
        void Debug(object message, Exception exception, string from);
        void Info(object message, string from);
        void Info(object message, Exception exception, string from);
        void Warn(object message, string from);
        void Warn(object message, Exception exception, string from);
    }
}