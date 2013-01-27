using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExamineSystem
{
    public partial class login : System.Web.UI.Page
    {

        protected string WarningText
        {
            get
            {
                string type = (this.Request.QueryString["text"] as string ?? string.Empty).Trim();
                string text = "请您先输入您的用户编号和对应密码，然后登陆系统!";
                switch (type)
                {
                    case "1":
                        text = "用户编号或者对应密码错误，请您重新登陆！";
                        break;
                    case "2":
                        text = "对不起，您没有登陆权限或您没有登录系统，请先登陆！";
                        break;
                    case "3":
                        text = "您的验证码输入有误，请重新登陆！";
                        break;
                    case "4":
                        text = "您的数据库连接出现问题，请检查您的连接字串！";
                        break;
                }
                return text;
            }
        }
    }
}