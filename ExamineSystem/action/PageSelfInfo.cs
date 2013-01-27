using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using ExamineSystem.utility;
using EntityModel;
using ExamineSystem.exception;
using ExamineSystem.action.inner;

namespace ExamineSystem.action
{

    public class PageSelfInfo : AAction
    {

        public PageSelfInfo()
            : base(true)
        {
            // do nothing
        }

        protected override ITask GetActionTask(string type)
        {
            switch (type)
            {
                case "edit":
                    return new EditSelfTask();
            }
            return null;
        }

        private class EditSelfTask : ATask
        {
            public EditSelfTask()
                : base(false)
            {
                // do nothing
            }

            protected override ActionResult DoTask(string data)
            {
                ActionResult result = new ActionResult();
                result.IsSuccess = false;
                string[] param = StringUtility.Split(data, "%27");
                string username = Escape.JsUnEscape(param[0]);
                string password = EncryptMD5.MD5to16Code(Escape.JsUnEscape(param[1]));
                string oldpassword = EncryptMD5.MD5to16Code(Escape.JsUnEscape(param[2]));
                int id = int.Parse(Request.QueryString["id"]);

                UserEntity entity = SessionManager.User;
                if (entity.UserId != id)
                    throw new ActionParseException("您准备编辑的用户不是您自己的当前用户");
                if (!entity.Password.ToLower().Equals(oldpassword.ToLower()))
                    throw new ActionParseException("您输入的原始密码有误");
                if (!string.IsNullOrEmpty(username))
                    entity.UserName = username;
                if (!string.IsNullOrEmpty(password))
                    entity.Password = password;
                entity.Save();
                result.IsSuccess = true;
                return result;
            }
        }
    }
}