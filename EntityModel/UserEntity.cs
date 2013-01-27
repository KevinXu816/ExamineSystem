using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core;
using DataFrameworkLibrary.Core.Attribute;
using EntityModel.Command;

namespace EntityModel
{
    [Serializable]
    [Transaction]
    [CommandBuilder(typeof(UserCommandBuilder))]
    public class UserEntity : Entity
    {
        public int UserId
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string UserNo
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public bool DoTest
        {
            get;
            set;
        }

        public bool IsLogin
        {
            get;
            set;
        }

        public UserLevelType UserLevel
        {
            get;
            set;
        }

        public void FillByUserNo()
        {
            this.DBSession.Fill(this, "FILL_BY_USERNO");
        }

        public void FillByUserNoAndPassword()
        {
            this.DBSession.Fill(this, "FILL_BY_USERNO_AND_PASSWORD");
        }

        public void FillIdentityStudentUserId()
        {
            this.DBSession.Action(this, "ACTION_FILL_IDENTITY_USER_ID");
        }
    }

    public enum UserLevelType
    {
        Student = 1,
        Teacher = 2,
        Admin = 3
    }
}
