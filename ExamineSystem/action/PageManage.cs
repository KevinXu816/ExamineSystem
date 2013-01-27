using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityModel;
using ExamineSystem.utility;
using ExamineSystem.exception;
using System.Text;
using ExamineSystem.action.inner;
using ExamineSystem.action.utility;
using System.Text.RegularExpressions;

namespace ExamineSystem.action
{

    public class PageManage : AAction
    {

        public PageManage()
            : base(true)
        {
            this.OperationUserLevels = new UserLevelType[] { UserLevelType.Admin };
        }

        protected override ITask GetActionTask(string type)
        {
            switch (type)
            {
                case "add":
                    return new AddUserTask();
                case "edit":
                    return new EditUserTask();
                case "del":
                    return new DeleteUserTask();
                case "loadlist":
                    return new LoadUserListTask();
                case "search":
                    return new SearchUserTask();
                case "reviece":
                    return new RevieceUserTask();
                case "historylist":
                    return new HistoryListTask();
                case "delhistory":
                    return new DeleteHistoryTask();
            }
            return null;
        }

        private class AddUserTask : ATask
        {
            public AddUserTask()
                : base(false)
            {
                // do nothing
            }

            protected override ActionResult DoTask(string data)
            {
                string[] param = StringUtility.Split(data, "%27");
                string usernum = Escape.JsUnEscape(param[0]);
                string username = Escape.JsUnEscape(param[1]);
                string password = EncryptMD5.MD5to16Code(Escape.JsUnEscape(param[2]));
                UserLevelType usertype = (UserLevelType)int.Parse(Escape.JsUnEscape(param[3]));

                UserEntity entity = new UserEntity();
                if (usertype == UserLevelType.Student)
                {
                    entity.FillIdentityStudentUserId();
                }
                else
                {
                    entity.UserNo = usernum;
                    entity.FillByUserNo();
                    if (entity.EntityState == DataFrameworkLibrary.Core.EntityState.Inserted)
                        throw new ActionParseException("系统中已存在相同编号的用户<br>请更换别的编号");
                }
                entity.UserName = username;
                entity.Password = password;
                entity.UserLevel = usertype;
                entity.IsLogin = false;
                entity.DoTest = true;
                entity.Save();

                UserCollection collection = new UserCollection();
                collection.PageSize = 6;
                collection.AbsolutePage = 1;
                collection.IsReturnDataTable = true;
                collection.Fill();
                ActionResult result = new ActionResult();
                result.IsSuccess = true;
                StringBuilder response = new StringBuilder();
                response.Append(ActionTaskUtility.ReturnClientDataArray(collection.GetFillDataTable()));
                response.Append(string.Format("TmpStr={0};", collection.PageCount));
                result.ResponseData = response.ToString();
                return result;
            }
        }

        private class EditUserTask : ATask
        {
            public EditUserTask()
                : base(false)
            {
                // do nothing
            }

            protected override ActionResult DoTask(string data)
            {
                string[] param = StringUtility.Split(data, "%27");
                int userid = int.Parse(Escape.JsUnEscape(param[0]));
                string usernum = Escape.JsUnEscape(param[1]);
                string username = Escape.JsUnEscape(param[2]);
                UserLevelType usertype = (UserLevelType)int.Parse(Escape.JsUnEscape(param[3]));
                bool userlogin = StringUtility.ConvertBool(Escape.JsUnEscape(param[4]));
                bool usertest = StringUtility.ConvertBool(Escape.JsUnEscape(param[5]));
                string password = EncryptMD5.MD5to16Code(Escape.JsUnEscape(param[6]));

                UserEntity entity = new UserEntity();
                if (!string.IsNullOrEmpty(usernum))
                {
                    entity.UserNo = usernum;
                    entity.FillByUserNo();
                    if (entity.EntityState == DataFrameworkLibrary.Core.EntityState.Inserted)
                        throw new ActionParseException("系统不允许定义<br>两个编号相同的用户");
                }

                bool isChange = false;
                if (usertype == UserLevelType.Admin)
                    isChange = true;
                else
                {
                    UserCollection userCollection = new UserCollection();
                    userCollection.FillByUserLevel(UserLevelType.Admin);
                    if (userCollection.Count == 1)
                    {
                        if (userCollection[0].UserId == userid)
                            throw new ActionParseException("系统不允许移出最后一位<br>进行人员管理的用户权限");
                        else
                            isChange = true;
                    }
                    else
                        isChange = true;
                }
                if (isChange)
                {
                    entity = new UserEntity();
                    entity.UserId = userid;
                    entity.Fill();
                    if (entity.EntityState == DataFrameworkLibrary.Core.EntityState.Inserted)
                    {
                        if (!string.IsNullOrEmpty(usernum))
                            entity.UserNo = usernum;
                        if (!string.IsNullOrEmpty(password))
                            entity.Password = password;
                        entity.UserName = username;
                        entity.UserLevel = usertype;
                        entity.IsLogin = userlogin;
                        entity.DoTest = usertest;
                        entity.Save();

                        UserEntity sessionEntity = SessionManager.User;
                        if (sessionEntity != null && sessionEntity.UserId == entity.UserId)
                        {
                            SessionManager.User = entity;
                        }
                    }
                }
                ActionResult result = new ActionResult();
                result.IsSuccess = true;
                return result;
            }
        }

        private class DeleteUserTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                ActionResult result = new ActionResult();
                StringBuilder response = new StringBuilder();

                string delStrIds = Request.QueryString["id"];
                string pageNoStr = Request.QueryString["PageN"];
                int pageNo = 0;
                if (!int.TryParse(pageNoStr, out pageNo))
                    pageNo = 0;
                string[] delStrIdCol = delStrIds.Split(',');
                List<int> delIds = new List<int>();
                foreach (string delStrId in delStrIdCol)
                {
                    string tmpDelIdStr = (delStrId ?? string.Empty).Trim();
                    if (string.IsNullOrEmpty(tmpDelIdStr))
                        continue;
                    else
                    {
                        int tmpDelId = 0;
                        if (int.TryParse(tmpDelIdStr, out tmpDelId))
                            delIds.Add(tmpDelId);
                        else
                            continue;
                    }
                }
                string flag = "";
                int currentId = (SessionManager.User == null) ? 0 : SessionManager.User.UserId;
                bool isFilter = this.filterIds(delIds, currentId);
                if (isFilter)
                {
                    if (string.IsNullOrEmpty(flag))
                        flag = "1";
                    else
                        flag = flag + "1";
                }
                UserCollection userCollection = new UserCollection();
                userCollection.FillByUserLevel(UserLevelType.Admin);
                if (userCollection.Count == 1)
                {
                    currentId = userCollection[0].UserId;
                    isFilter = this.filterIds(delIds, currentId);
                    if (isFilter)
                    {
                        if (string.IsNullOrEmpty(flag))
                            flag = "2";
                        else
                            flag = flag + "2";
                    }
                }
                if (delIds.Count == 0)
                {
                    if (string.IsNullOrEmpty(flag))
                        flag = "3";
                    else
                        flag = flag + "3";
                }
                else
                {
                    int[] ids = delIds.ToArray();
                    userCollection.DeleteByUserIds(ids);
                    HistoryCollection historyCollection = new HistoryCollection();
                    historyCollection.DeleteByUserIds(ids);

                    string condition = PageManageTaskUtility.CurrentExpVal();
                    userCollection.PageSize = 6;
                    userCollection.FillByCondition(condition);
                    if (pageNo > userCollection.PageCount)
                        pageNo = userCollection.PageCount;
                    userCollection.AbsolutePage = pageNo;
                    userCollection.IsReturnDataTable = true;
                    userCollection.FillByCondition(condition);

                    response.Append(ActionTaskUtility.ReturnClientDataArray(userCollection.GetFillDataTable()));
                    flag = flag + "|" + userCollection.PageCount;
                }
                result.IsSuccess = true;
                response.Append(string.Format("TmpStr='{0}';", flag));
                result.ResponseData = response.ToString();
                return result;
            }

            private bool filterIds(List<int> ids, int specId)
            {
                bool result = false;
                if (ids == null || ids.Count == 0)
                    return result;
                for (int i = 0; i < ids.Count; i++)
                {
                    int curId = ids[i];
                    if (curId == specId)
                    {
                        ids.RemoveAt(i);
                        result = true;
                        i--;
                    }
                }
                return result;
            }


        }

        private class LoadUserListTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                string pageNoStr = Request.QueryString["pagenum"];
                int pageNo = 0;
                if (!int.TryParse(pageNoStr, out pageNo))
                    pageNo = 0;

                string condition = PageManageTaskUtility.CurrentExpVal();
                UserCollection userCollection = new UserCollection();
                if (pageNo == 0)
                {
                    userCollection.PageSize = 0;
                    userCollection.IsReturnDataTable = true;
                    userCollection.FillByCondition(condition);
                }
                else
                {
                    userCollection.PageSize = 6;
                    userCollection.FillByCondition(condition);
                    if (pageNo > userCollection.PageCount)
                        pageNo = userCollection.PageCount;
                    userCollection.AbsolutePage = pageNo;
                    userCollection.IsReturnDataTable = true;
                    userCollection.FillByCondition(condition);
                }
                ActionResult result = new ActionResult();
                StringBuilder response = new StringBuilder();
                response.Append(ActionTaskUtility.ReturnClientDataArray(userCollection.GetFillDataTable()));
                response.Append(string.Format("TmpStr={0};", userCollection.PageCount));
                result.IsSuccess = true;
                result.ResponseData = response.ToString();
                return result;
            }
        }

        private class SearchUserTask : ATask
        {
            public SearchUserTask()
                : base(false)
            {
                // do nothing
            }

            protected override ActionResult DoTask(string data)
            {
                string sign = Request.QueryString["sign"];
                string condition = PageManageTaskUtility.ParseExpVal(sign, data);

                UserCollection collection = new UserCollection();
                collection.PageSize = 6;
                collection.AbsolutePage = 1;
                collection.IsReturnDataTable = true;
                collection.FillByCondition(condition);

                ActionResult result = new ActionResult();
                StringBuilder response = new StringBuilder();
                response.Append(ActionTaskUtility.ReturnClientDataArray(collection.GetFillDataTable()));
                response.Append(string.Format("TmpStr={0};", collection.PageCount));
                result.IsSuccess = true;
                result.ResponseData = response.ToString();
                return result;
            }
        }

        private class RevieceUserTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                string pageNoStr = Request.QueryString["pagenum"];
                int pageNo = 0;
                if (!int.TryParse(pageNoStr, out pageNo))
                    pageNo = 0;

                string condition = PageManageTaskUtility.ParseExpVal("0", "");
                UserCollection userCollection = new UserCollection();
                if (pageNo == 0)
                {
                    userCollection.PageSize = 0;
                    userCollection.IsReturnDataTable = true;
                    userCollection.FillByCondition(condition);
                }
                else
                {
                    userCollection.PageSize = 6;
                    userCollection.FillByCondition(condition);
                    if (pageNo > userCollection.PageCount)
                        pageNo = userCollection.PageCount;
                    userCollection.AbsolutePage = pageNo;
                    userCollection.IsReturnDataTable = true;
                    userCollection.FillByCondition(condition);
                }
                ActionResult result = new ActionResult();
                StringBuilder response = new StringBuilder();
                response.Append(ActionTaskUtility.ReturnClientDataArray(userCollection.GetFillDataTable()));
                response.Append(string.Format("TmpStr={0};", userCollection.PageCount));
                result.IsSuccess = true;
                result.ResponseData = response.ToString();
                return result;
            }
        }

        private class HistoryListTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                string pageNoStr = Request.QueryString["pagenum"];
                int pageNo = 0;
                if (!int.TryParse(pageNoStr, out pageNo))
                    pageNo = 0;
                int userId = 0;
                if (!int.TryParse(data, out userId))
                    userId = 0;

                HistoryCollection collection = new HistoryCollection();
                if (pageNo == 0)
                {
                    collection.PageSize = 0;
                    collection.IsReturnDataTable = true;
                    collection.FillByUserId(userId);
                }
                else
                {
                    collection.PageSize = 6;
                    collection.FillByUserId(userId);
                    if (pageNo > collection.PageCount)
                        pageNo = collection.PageCount;
                    collection.AbsolutePage = pageNo;
                    collection.IsReturnDataTable = true;
                    collection.FillByUserId(userId);
                }
                ActionResult result = new ActionResult();
                StringBuilder response = new StringBuilder();
                response.Append(ActionTaskUtility.ReturnClientDataArray(collection.GetFillDataTable()));
                response.Append(string.Format("TmpStr={0};", collection.PageCount));
                result.IsSuccess = true;
                result.ResponseData = response.ToString();
                return result;
            }
        }

        private class DeleteHistoryTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                string delStrIds = Request.QueryString["id"];
                string pageNoStr = Request.QueryString["PageN"];
                int pageNo = 0;
                if (!int.TryParse(pageNoStr, out pageNo))
                    pageNo = 0;
                int userId = 0;
                if (!int.TryParse(data, out userId))
                    userId = 0;
                string[] delStrIdCol = delStrIds.Split(',');
                List<int> delIds = new List<int>();
                foreach (string delStrId in delStrIdCol)
                {
                    string tmpDelIdStr = (delStrId ?? string.Empty).Trim();
                    if (string.IsNullOrEmpty(tmpDelIdStr))
                        continue;
                    else
                    {
                        int tmpDelId = 0;
                        if (int.TryParse(tmpDelIdStr, out tmpDelId))
                            delIds.Add(tmpDelId);
                        else
                            continue;
                    }
                }
                HistoryCollection collection = new HistoryCollection();
                collection.DeleteByHistoryIds(delIds.ToArray());

                collection.PageSize = 6;
                collection.FillByUserId(userId);
                if (pageNo > collection.PageCount)
                    pageNo = collection.PageCount;
                collection.AbsolutePage = pageNo;
                collection.IsReturnDataTable = true;
                collection.FillByUserId(userId);

                ActionResult result = new ActionResult();
                StringBuilder response = new StringBuilder();
                response.Append(ActionTaskUtility.ReturnClientDataArray(collection.GetFillDataTable()));
                response.Append(string.Format("TmpStr={0};", collection.PageCount));
                result.IsSuccess = true;
                result.ResponseData = response.ToString();
                return result;
            }
        }

        private static class PageManageTaskUtility
        {
            public static string CurrentExpVal()
            {
                string exp = null;
                string sessionExp = (SessionManager.UserExp ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(sessionExp))
                {
                    exp = "1 = 1";
                    SessionManager.UserExp = exp;
                }
                else
                {
                    exp = sessionExp;
                }
                return exp;
            }

            public static string ParseExpVal(string type, string param)
            {
                StringBuilder condition = new StringBuilder();
                switch (type)
                {
                    case "0":
                        condition.Append("1 = 1");
                        break;
                    case "1":
                        param = Escape.JsUnEscape(param);
                        condition.Append(string.Format("user_name like '%{0}%'", param));
                        break;
                    case "2":
                        param = Escape.JsUnEscape(param);
                        condition.Append(string.Format("user_num like '%{0}%'", param));
                        break;
                    case "3":
                        string[] blockArray = StringUtility.Split(param, "%22");
                        for (int i = 0; i < blockArray.Length; i++)
                        {
                            string block = blockArray[i];
                            string[] keyArray = StringUtility.Split(block, "%27");
                            string subOpare = string.Empty;
                            if (i > 0)
                            {
                                if (keyArray[3] == "0") subOpare = "OR";
                                if (keyArray[3] == "1") subOpare = "AND";
                            }
                            string conTmp = subOpare + "(";
                            string word = null;
                            switch (Escape.JsUnEscape(keyArray[0]))
                            {
                                case "0":
                                    word = Escape.JsUnEscape(keyArray[2]);
                                    switch (Escape.JsUnEscape(keyArray[1]))
                                    {
                                        case "0":
                                            conTmp = conTmp + "user_num>" + word + " OR user_num<-" + word;
                                            break;
                                        case "1":
                                            conTmp = conTmp + "user_num<" + word + " AND user_num>-" + word;
                                            break;
                                        case "2":
                                            conTmp = conTmp + "user_num=" + word + " OR user_num=-" + word;
                                            break;
                                        case "3":
                                            conTmp = conTmp + "user_num<>" + word + " AND user_num<>-" + word;
                                            break;
                                        case "4":
                                            conTmp = conTmp + "user_num like '%" + word + "%'";
                                            break;
                                    }
                                    break;
                                case "1":
                                    word = Escape.JsUnEscape(keyArray[2]);
                                    conTmp = conTmp + "user_name like '%" + word + "%'";
                                    break;
                                case "2":
                                    conTmp = conTmp + "user_func=" + Escape.JsUnEscape(keyArray[1]);
                                    break;
                                case "3":
                                    conTmp = conTmp + "user_login=" + Escape.JsUnEscape(keyArray[1]);
                                    break;
                                case "4":
                                    conTmp = conTmp + "user_do=" + Escape.JsUnEscape(keyArray[1]);
                                    break;
                                case "5":
                                    switch (Escape.JsUnEscape(keyArray[1]))
                                    {
                                        case "0":
                                            conTmp = conTmp + "user_id In (select history_user from es_history(nolock))";
                                            break;
                                        case "1":
                                            conTmp = conTmp + "user_id Not In (select history_user from es_history(nolock))";
                                            break;
                                    }
                                    break;
                            }
                            conTmp = conTmp + " ) ";
                            condition.Append(conTmp);
                        }
                        break;
                }
                string result = condition.ToString();
                SessionManager.UserExp = result;
                return result;
            }

        }
    }
}