using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using EntityModel;

namespace ExamineSystem.utility
{
    public static class SessionManager
    {
        public static UserEntity User
        {
            get { return (Session["USER"] as UserEntity); }
            set
            {
                bindTimeout();
                Session["USER"] = value;
            }
        }

        public static string ClientIp
        {
            get { return ((Session["CLIENT_IP"] as string) ?? string.Empty); }
            set
            {
                bindTimeout();
                Session["CLIENT_IP"] = value;
            }
        }

        public static string UserExp
        {
            get { return ((Session["USER_EXP"] as string) ?? string.Empty); }
            set
            {
                bindTimeout();
                Session["USER_EXP"] = value;
            }
        }

        public static string ValidateCode
        {
            get { return ((Session["VALIDATE_CODE"] as string) ?? string.Empty); }
            set
            {
                bindTimeout();
                Session["VALIDATE_CODE"] = value;
            }
        }

        public static void RemoveUser()
        {
            Session.Remove("USER");
        }

        public static void RemoveClientIp()
        {
            Session.Remove("CLIENT_IP");
        }

        public static void RemoveUserExp()
        {
            Session.Remove("USER_EXP");
        }

        public static void RemoveValidateCode()
        {
            Session.Remove("VALIDATE_CODE");
        }

        public static void RemoveAll()
        {
            Session.RemoveAll();
        }

        private static void bindTimeout()
        {
            Session.Timeout = SettingConfigUtility.SessionTimeout;
        }

        private static HttpSessionState Session
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }
    }
}
