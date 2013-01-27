using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamineSystem.utility;
using EntityModel;
using ExamineSystem.exception;
using System.Text;
using ExamineSystem.action.inner;

namespace ExamineSystem.action
{

    public class Login : AAction
    {

        protected override ITask GetActionTask(string type)
        {
            return new LoginTask();
        }

        private class LoginTask : ATask
        {
            protected override ActionResult DoTask(string data)
            {
                ActionResult result = new ActionResult();
                result.IsSuccess = false;

                string[] param = data.Split('\'');
                string number = param[0];
                string password = EncryptMD5.MD5to16Code(param[1]);
                string validateCode = param[2];

                if (!SessionManager.ValidateCode.Equals(validateCode))
                    throw new ActionParseException("您的验证码输入有误");

                UserEntity entity = new UserEntity();
                entity.UserNo = number;
                entity.Password = password;
                entity.FillByUserNoAndPassword();
                if (entity.EntityState != DataFrameworkLibrary.Core.EntityState.Inserted)
                    throw new ActionParseException("用户编号或者对应密码错误");

                if (entity.IsLogin == false || entity.UserLevel == UserLevelType.Admin)
                {
                    entity.IsLogin = true;
                    entity.Save();

                    SessionManager.User = entity;
                    SessionManager.UserExp = "";
                    SessionManager.ClientIp = getUserClientIP();

                    StringBuilder response = new StringBuilder();
                    response.Append("Lock = false;");
                    response.Append("top.location.href = 'welcome.aspx';");
                    result.ResponseData = response.ToString();
                    result.IsSuccess = true;
                }
                else
                    throw new ActionParseException("您的帐号已在登陆状态，请不要连续登陆系统！");
                return result;
            }

            private string getUserClientIP()
            {
                string userIP = string.Empty;
                if (Request.ServerVariables["HTTP_VIA"] == null)
                {
                    userIP = Request.UserHostAddress;
                }
                else
                {
                    userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (string.IsNullOrEmpty(userIP))
                {
                    userIP = Request.ServerVariables["REMOTE_ADDR"];
                }
                return userIP;
            }
        }

    }
}