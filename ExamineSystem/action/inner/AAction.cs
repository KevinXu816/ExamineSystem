using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Web.Caching;
using ExamineSystem.utility;
using EntityModel;
using System.Data;
using System.Text;

namespace ExamineSystem.action.inner
{
    public abstract class AAction : IAction
    {
        private HttpContext _context = null;
        private bool _isOther = false;
        private UserLevelType[] _operationUserLevels = null;
        private bool _isLogin = false;

        protected AAction(bool isLogin)
        {
            this._isLogin = isLogin;
        }

        protected AAction()
            : this(false)
        {
            // do nothing
        }

        protected abstract ITask GetActionTask(string type);

        protected UserLevelType[] OperationUserLevels
        {
            set
            {
                this._operationUserLevels = value;
            }
        }

        #region implement IAction interface
        HttpContext IAction.CurrentContext
        {
            set { this._context = value; }
        }
        bool IAction.IsOtherRequest 
        {
            set { this._isOther = value; }
        }
        bool IAction.IsLogin
        {
            get { return this._isLogin; }
        }
        ActionResult IAction.DoAction(string actionType, string actionData)
        {
            if (this._isLogin)
            {
                UserEntity entity = SessionManager.User;
                if (entity == null)
                {
                    ActionResult result = new ActionResult();
                    result.IsSuccess = false;
                    result.ResponseData = "location.href='login.aspx?text=2';";
                    return result;
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
                        ActionResult result = new ActionResult();
                        result.IsSuccess = false;
                        result.TipMessage = "对不起!<br>您没有此功能的操作权限";
                        return result;
                    }
                }
            }
            actionData = actionData ?? string.Empty;
            ITask task = this.GetActionTask(actionType);
            if (task == null)
            {
                ActionResult result = new ActionResult();
                result.IsSuccess = false;
                result.TipMessage = "指令发送有误请重新操作";
                return result;
            }
            if (task.IsEscape)
                actionData = Escape.JsUnEscape(actionData);
            task.CurrentContext = this._context;
            task.IsOtherRequest = this._isOther;
            return task.DoTask(actionData);
        }
        #endregion

    }
}