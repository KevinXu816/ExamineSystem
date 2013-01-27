using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamineSystem.view;
using EntityModel;
using ExamineSystem.utility;

namespace ExamineSystem
{
    public partial class pageleft : BasePage
    {

        public pageleft()
        {
            this.IsValidateLogin = true;
        }

        protected string CurrentUserName
        {
            get
            {
                UserEntity entity = SessionManager.User;
                if (entity == null)
                    return string.Empty;
                return entity.UserName ?? string.Empty;
            }
        }

        protected int CurrentUserLevel
        {
            get
            {
                UserEntity entity = SessionManager.User;
                if (entity == null)
                    return (int)UserLevelType.Student;
                return (int)entity.UserLevel;
            }
        }
    }
}