using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Diagnostics;
using System.Reflection;

namespace ExamineSystem.utility.eslog
{
    internal class ExpandContext
    {

        public static void getContextInfo(out string sessionInfo)
        {
            sessionInfo = string.Empty;

            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                StringBuilder sessionStr = new StringBuilder();
                HttpSessionState session = context.Session;
                if (session != null)
                {
                    sessionStr.AppendLine(string.Format("[ClientIP = {0}]", getUserClientIP(context)));
                    sessionStr.AppendLine(string.Format("[ClientIPInfo = {0}]", getUserClientIPInfo(context)));
                    sessionStr.AppendLine(string.Format("SessionID = {0}", session.SessionID));
                    foreach (string key in session.Keys)
                    {
                        string val = Convert.ToString(session[key]);
                        sessionStr.AppendLine(string.Format("{0} = {1}", key, val));
                    }
                }
                sessionInfo = sessionStr.ToString();
            }
        }

        public static void getContextInfo(ref object message, ref Exception exception, 
                out string sessionInfo, out string errorFrom)
        {
            sessionInfo = string.Empty;
            errorFrom = string.Empty;

            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                StringBuilder sessionStr = new StringBuilder();
                HttpSessionState session = context.Session;
                if (session != null)
                {
                    sessionStr.AppendLine(string.Format("[ClientIP = {0}]", getUserClientIP(context)));
                    sessionStr.AppendLine(string.Format("[ClientIPInfo = {0}]", getUserClientIPInfo(context)));
                    sessionStr.AppendLine(string.Format("SessionID = {0}", session.SessionID));
                    foreach (string key in session.Keys)
                    {
                        string val = Convert.ToString(session[key]);
                        sessionStr.AppendLine(string.Format("{0} = {1}", key, val));
                    }
                }
                sessionInfo = sessionStr.ToString();
            }
            string mess = message as string;
            if(mess != null)
            {
                message = mess + "----------" + exception.Message;
            }

            ExceptionWrapper ex = new ExceptionWrapper();
            ex.messageStr = exception.Message;
            ex.stackTraceStr = ExpandContext.EnhancedStackTrace(exception, out errorFrom);
            exception = ex;
        }
        

        private static string EnhancedStackTrace(Exception ex, out string errorFrom)
        {
            StackTrace ExceptionST = (ex == null) ? null : new StackTrace(ex, true);
            StackTrace EnvironmentST = new StackTrace(true);
            return EnhancedStackTrace(EnvironmentST, ExceptionST, ex, out errorFrom);
        }

        private static string EnhancedStackTrace(StackTrace EnvironmentST, StackTrace ExceptionST, Exception OrgException,
                                out string errorFrom)
        {
            errorFrom = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (OrgException != null)
            {
                sb.Append(Environment.NewLine);
                sb.Append("---- Original Exception Stack Trace Begin ----");
                sb.Append(Environment.NewLine);
                sb.Append(OrgException.StackTrace);
                sb.Append(Environment.NewLine);
                sb.Append("---- Original Exception Stack Trace End ----");
                sb.Append(Environment.NewLine);
            }
            if (ExceptionST != null)
            {
                sb.Append(Environment.NewLine);
                sb.Append("---- Exception Stack Trace Begin ----");
                sb.Append(Environment.NewLine);
                string tmpFrom = null;
                for (int i = 0; i < ExceptionST.FrameCount; i++)
                {
                    StackFrame sf = ExceptionST.GetFrame(i);
                    sb.Append(StackFrameToString(sf, ref tmpFrom));
                }
                sb.Append(Environment.NewLine);
                sb.Append("---- Exception Stack Trace End ----");
                sb.Append(Environment.NewLine);
            }
            if (EnvironmentST != null)
            {
                sb.Append(Environment.NewLine);
                sb.Append("---- Environment Stack Trace Begin ----");
                sb.Append(Environment.NewLine);
                for (int i = 0; i < EnvironmentST.FrameCount; i++)
                {
                    StackFrame sf = EnvironmentST.GetFrame(i);
                    sb.Append(StackFrameToString(sf, ref errorFrom));
                }
                sb.Append(Environment.NewLine);
                sb.Append("---- Environment Stack Trace End ----");
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private static string StackFrameToString(StackFrame sf, ref string errorFrom)
        {
            StringBuilder sb = new StringBuilder();
            int intParam;
            MemberInfo mi = sf.GetMethod();
            Type type = mi.DeclaringType;
            string namspace = type.Namespace ?? string.Empty;
            if (namspace.Equals("ExamineSystem.utility.eslog"))
            {
                return sb.ToString();
            }
            if (string.IsNullOrEmpty(errorFrom))
            {
                try
                {
                    if (type.IsSubclassOf(typeof(System.Web.UI.Page)))
                    {
                        errorFrom = type.FullName;
                    }
                    else if (type.IsSubclassOf(typeof(System.Web.UI.Control)))
                    {
                        errorFrom = type.FullName;
                    }
                    else if (type.GetInterface("System.Web.IHttpHandler") != null)
                    {
                        errorFrom = type.FullName;
                    }
                    else if (type.GetInterface("System.Web.IHttpAsyncHandler") != null)
                    {
                        errorFrom = type.FullName;
                    }
                    else if (type.GetInterface("System.Web.IHttpModule") != null)
                    {
                        errorFrom = type.FullName;
                    }
                    else if (type.GetInterface("System.Web.IHttpHandlerFactory") != null)
                    {
                        errorFrom = type.FullName;
                    }
                }
                catch (Exception)
                {
                    errorFrom = string.Empty;
                }
            }
            sb.Append("   ");
            sb.Append(namspace);
            sb.Append(".");
            sb.Append(type.Name);
            sb.Append(".");
            sb.Append(mi.Name);
            // -- build method params
            sb.Append("(");
            intParam = 0;
            foreach (ParameterInfo param in sf.GetMethod().GetParameters())
            {
                if (intParam > 0)
                    sb.Append(" , ");
                sb.Append(param.Name);
                sb.Append(" As ");
                sb.Append(param.ParameterType.Name);
                intParam += 1;
            }
            sb.Append(")");
            sb.Append(Environment.NewLine);
            // -- if source code is available, append location info
            sb.Append("       ");
            if (string.IsNullOrEmpty(sf.GetFileName()))
            {
                sb.Append("(unknown file)");
                //-- native code offset is always available
                sb.Append(": N ");
                sb.Append(String.Format("{0:#00000}", sf.GetNativeOffset()));
            }
            else
            {
                sb.Append(System.IO.Path.GetFileName(sf.GetFileName()));
                sb.Append(": line ");
                sb.Append(String.Format("{0:#0000}", sf.GetFileLineNumber()));
                sb.Append(", col ");
                sb.Append(String.Format("{0:#00}", sf.GetFileColumnNumber()));
                if (sf.GetILOffset() != StackFrame.OFFSET_UNKNOWN)
                {
                    sb.Append(", IL ");
                    sb.Append(String.Format("{0:#0000}", sf.GetILOffset()));
                }
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        private static string getUserClientIP(HttpContext context)
        {
            string userIP;
            if (context.Request.ServerVariables["HTTP_VIA"] == null)
            {
                userIP = context.Request.UserHostAddress;
            }
            else
            {
                userIP = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (string.IsNullOrEmpty(userIP))
            {
                userIP = context.Request.ServerVariables["REMOTE_ADDR"];
            }
            return userIP;
        }

        private static string getUserClientIPInfo(HttpContext context)
        {
            string ipInfo = string.Format(
                "USER_HOST: {0}, HTTP_VIA: {1}, REMOTE_ADDR: {2}",
                (context.Request.UserHostAddress ?? string.Empty),
                (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? string.Empty),
                (context.Request.ServerVariables["REMOTE_ADDR"] ?? string.Empty)
            );
            return ipInfo;
        }

    }
}
