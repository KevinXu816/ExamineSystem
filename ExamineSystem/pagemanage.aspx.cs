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
    public partial class pagemanage : BasePage
    {
        public pagemanage()
        {
            this.IsValidateLogin = true;
            this.OperationUserLevels = new UserLevelType[] { UserLevelType.Admin };
        }

        private UserCollection userCollection = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            userCollection = new UserCollection();
            userCollection.PageSize = 6;
            userCollection.AbsolutePage = 1;
            userCollection.Fill();
            this.repUserList.DataSource = userCollection;
            this.repUserList.DataBind();
        }

        protected int UserCollectionPageCount
        {
            get
            {
                if (userCollection == null)
                    return 0;
                else
                    return userCollection.PageCount;
            }
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

        protected string GetUserLevelName(int userLevel)
        {
            UserLevelType level = (UserLevelType)userLevel;
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

        protected string GetSelectedLevel(int userLevel, int selectedLevel)
        {
            if (userLevel == selectedLevel)
                return "selected";
            else
                return string.Empty;
        }

        protected string GetSelectedStatus(bool userStatus, bool selectedStatus)
        {
            if (userStatus == selectedStatus)
                return "selected";
            else
                return string.Empty;
        }

        protected string GetUserShowNumber(string userNumber)
        {
            return userNumber.Replace("-", "");
        }

    }
}