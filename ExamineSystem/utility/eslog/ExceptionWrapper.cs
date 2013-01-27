using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamineSystem.utility.eslog
{
    public class ExceptionWrapper : Exception
    {

        internal string stackTraceStr;
        internal string messageStr;

        public override string Message
        {
            get
            {
                return (string.IsNullOrEmpty(messageStr)) ? base.Message : messageStr;
            }
        }

        public override string StackTrace
        {
            get
            {
                return (string.IsNullOrEmpty(stackTraceStr)) ? base.StackTrace : stackTraceStr;
            }
        }
    }
}
