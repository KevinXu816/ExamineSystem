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
    public partial class pageselfinfo : BasePage
    {
        public pageselfinfo()
        {
            this.IsValidateLogin = true;
        }

        protected int CurrentUserId
        {
            get
            {
                UserEntity entity = SessionManager.User;
                if (entity == null)
                    return 0;
                return entity.UserId;
            }
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

        protected string CurrentUserLevelName
        {
            get
            {
                UserEntity entity = SessionManager.User;
                UserLevelType level = UserLevelType.Student;
                if (entity != null)
                    level = entity.UserLevel;
                string levelName = string.Empty;
                switch (level)
                {
                    case UserLevelType.Student:
                        levelName = "普通学员考生";
                        break;
                    case UserLevelType.Teacher:
                        levelName = "试题维护教师";
                        break;
                    case UserLevelType.Admin:
                        levelName = "系统级管理员";
                        break;
                    default:
                        levelName = "普通学员考生";
                        break;
                }
                return levelName;
            }
        }


    }
}