using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;
using ExamineSystem.utility;

namespace ExamineSystem.view
{
	public class BasePage : System.Web.UI.Page
	{
        private UserLevelType[] _operationUserLevels = null;
        protected UserLevelType[] OperationUserLevels
        {
            set
            {
                this._operationUserLevels = value;
            }
        }

        private bool _isValidateLogin = false;
        protected bool IsValidateLogin
        {
            set 
            {
                this._isValidateLogin = value;
            }
        }

        protected override void  OnInit(EventArgs e)
        {
            if (this._isValidateLogin)
            {
                SessionManager.UserExp = "";
                UserEntity entity = SessionManager.User;
                if (entity == null)
                {
                    Response.Redirect("/login.aspx?text=2", true);
                    return;
                }
                if (this._operationUserLevels != null)
                {
                    bool isOperation = false;
                    foreach (UserLevelType userLevel in this._operationUserLevels)
                    {
                        if (entity.UserLevel == userLevel)
                        {
                            isOperation = true;
                            break;
                        }
                    }
                    if (!isOperation)
                    {
                        Response.ClearContent();
                        Response.Write(@"
                            <html>
                                <head></head>
                                <body bgcolor='#cad7f7'>
                                    <script>alert('对不起！您没有此功能的操作权限');history.go(-1);</script>
                                </body>
                            </html>
                         ");
                        Response.End();
                    }
                }
            }
            base.OnInit(e);
        }
	}
}