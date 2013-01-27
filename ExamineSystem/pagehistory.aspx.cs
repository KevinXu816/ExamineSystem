using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamineSystem.view;
using ExamineSystem.utility;
using EntityModel;

namespace ExamineSystem
{
    public partial class pagehistory : BasePage
    {

        public pagehistory()
        {
            this.IsValidateLogin = true;
            this.OperationUserLevels = new UserLevelType[] { UserLevelType.Admin, UserLevelType.Student, UserLevelType.Teacher };
        }


        private HistoryCollection collection = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            collection = new HistoryCollection();
            collection.PageSize = 10;
            collection.AbsolutePage = 1;
            collection.FillByUserId(this.CurrentUserId);
            this.repHistoryList.DataSource = collection;
            this.repHistoryList.DataBind();
        }

        protected int HistoryCollectionPageCount
        {
            get
            {
                if (collection == null)
                    return 0;
                else
                    return collection.PageCount;
            }
        }

        protected string CurrentUserNo
        {
            get
            {
                UserEntity entity = SessionManager.User;
                if (entity == null)
                    return "";
                return entity.UserNo.Replace("-", "");
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

    }
}