using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamineSystem.action.inner;
using EntityModel;
using System.Text;
using ExamineSystem.utility;
using ExamineSystem.action.utility;

namespace ExamineSystem.action
{
    public class PageHistoryAdv : AAction
    {

        public PageHistoryAdv()
            : base(true)
        {
            this.OperationUserLevels = new UserLevelType[] { UserLevelType.Admin, UserLevelType.Student, UserLevelType.Teacher };
        }

        protected override ITask GetActionTask(string type)
        {
            switch (type)
            {
                case "search":
                    return new SearchHistoryTask();
                case "reviece":
                    return new RevieceHistoryTask();
                case "historylist":
                    return new LoadHistoryListTask();
                case "delhistory":
                    return new DeleteHistoryTask();
            }
            return null;

        }

        private class SearchHistoryTask : ATask
        {
            public SearchHistoryTask()
                : base(false)
            {
                // do nothing
            }

            protected override ActionResult DoTask(string data)
            {
                string sign = Request.QueryString["sign"];
                string condition = PageHistoryTaskUtility.ParseExpVal(sign, data);

                HistoryCollection collection = new HistoryCollection();
                collection.PageSize = 10;
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

        private class RevieceHistoryTask : ATask
        {
            protected override ActionResult DoTask(string data)
            {
                string pageNoStr = Request.QueryString["pagenum"];
                int pageNo = 0;
                if (!int.TryParse(pageNoStr, out pageNo))
                    pageNo = 0;

                string condition = PageHistoryTaskUtility.ParseExpVal("0", "");
                HistoryCollection historyCollection = new HistoryCollection();
                if (pageNo == 0)
                {
                    historyCollection.PageSize = 0;
                    historyCollection.IsReturnDataTable = true;
                    historyCollection.FillByCondition(condition);
                }
                else
                {
                    historyCollection.PageSize = 10;
                    historyCollection.FillByCondition(condition);
                    if (pageNo > historyCollection.PageCount)
                        pageNo = historyCollection.PageCount;
                    historyCollection.AbsolutePage = pageNo;
                    historyCollection.IsReturnDataTable = true;
                    historyCollection.FillByCondition(condition);
                }
                ActionResult result = new ActionResult();
                StringBuilder response = new StringBuilder();
                response.Append(ActionTaskUtility.ReturnClientDataArray(historyCollection.GetFillDataTable()));
                response.Append(string.Format("TmpStr={0};", historyCollection.PageCount));
                result.IsSuccess = true;
                result.ResponseData = response.ToString();
                return result;
            }
        }

        private class LoadHistoryListTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                string pageNoStr = Request.QueryString["pagenum"];
                int pageNo = 0;
                if (!int.TryParse(pageNoStr, out pageNo))
                    pageNo = 0;

                string condition = PageHistoryTaskUtility.CurrentExpVal();
                HistoryCollection historyCollection = new HistoryCollection();
                if (pageNo == 0)
                {
                    historyCollection.PageSize = 0;
                    historyCollection.IsReturnDataTable = true;
                    historyCollection.FillByCondition(condition);
                }
                else
                {
                    historyCollection.PageSize = 10;
                    historyCollection.FillByCondition(condition);
                    if (pageNo > historyCollection.PageCount)
                        pageNo = historyCollection.PageCount;
                    historyCollection.AbsolutePage = pageNo;
                    historyCollection.IsReturnDataTable = true;
                    historyCollection.FillByCondition(condition);
                }
                ActionResult result = new ActionResult();
                StringBuilder response = new StringBuilder();
                response.Append(ActionTaskUtility.ReturnClientDataArray(historyCollection.GetFillDataTable()));
                response.Append(string.Format("TmpStr={0};", historyCollection.PageCount));
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

                collection.PageSize = 10;
                string condition = PageHistoryTaskUtility.CurrentExpVal();
                collection.FillByCondition(condition);
                if (pageNo > collection.PageCount)
                    pageNo = collection.PageCount;
                collection.AbsolutePage = pageNo;
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

        private static class PageHistoryTaskUtility
        {
            public static string CurrentExpVal()
            {
                string exp = null;
                string sessionExp = (SessionManager.UserExp ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(sessionExp))
                {
                    exp = string.Format("history_user={0}", (SessionManager.User == null) ? "0" : SessionManager.User.UserId.ToString());
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
                        condition.Append("");
                        break;
                    case "1":
                        param = Escape.JsUnEscape(param);
                        condition.Append(string.Format("history_ip = '{0}'", param));
                        break;
                    case "2":
                        param = Escape.JsUnEscape(param);
                        condition.Append(string.Format("history_score = {0}", param));
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
                                            conTmp = conTmp + "history_score>" + word;
                                            break;
                                        case "1":
                                            conTmp = conTmp + "history_score<" + word;
                                            break;
                                        case "2":
                                            conTmp = conTmp + "history_score=" + word;
                                            break;
                                        case "3":
                                            conTmp = conTmp + "history_score<>" + word;
                                            break;
                                    }
                                    break;
                                case "1":
                                    word = Escape.JsUnEscape(keyArray[2]);
                                    conTmp = conTmp + "history_ip = '" + word + "'";
                                    break;
                                case "2":
                                    word = Escape.JsUnEscape(keyArray[2]);
                                    switch (Escape.JsUnEscape(keyArray[1]))
                                    {
                                        case "0":
                                            conTmp = conTmp + "history_stime>'" + word + "' AND convert(varchar(10),history_stime,101) not like '%" + word + "%'";
                                            break;
                                        case "1":
                                            conTmp = conTmp + "history_stime<'" + word + "'";
                                            break;
                                        case "2":
                                            conTmp = conTmp + "convert(varchar(10),history_stime,101) like '%" + word + "%'";
                                            break;
                                        case "3":
                                            conTmp = conTmp + "convert(varchar(10),history_stime,101) not like '%" + word + "%'";
                                            break;
                                    }
                                    break;
                            }
                            conTmp = conTmp + " ) ";
                            condition.Append(conTmp);
                        }
                        break;
                }
                string con = condition.ToString().Trim();
                string limit = string.Format("history_user={0}", (SessionManager.User == null) ? "0" : SessionManager.User.UserId.ToString());
                if (!string.IsNullOrEmpty(con))
                {
                    con = string.Format("({0}) AND ", con);
                }
                string result = string.Format("{0} {1}", con, limit);
                SessionManager.UserExp = result;
                return result;
            }
        }
    }

}