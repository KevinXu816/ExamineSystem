using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamineSystem.view;
using EntityModel;

namespace ExamineSystem
{
    public partial class pageupfile : BasePage
    {
        public pageupfile()
        {
            this.IsValidateLogin = true;
            this.OperationUserLevels = new UserLevelType[] { UserLevelType.Admin, UserLevelType.Teacher };
        }
    }
}