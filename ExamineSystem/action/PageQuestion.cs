using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamineSystem.action.inner;
using EntityModel;
using System.Text;
using ExamineSystem.action.utility;
using ExamineSystem.utility;

namespace ExamineSystem.action
{
    public class PageQuestion : AAction
    {
        public PageQuestion()
            : base(true)
        {
            this.OperationUserLevels = new UserLevelType[] { UserLevelType.Teacher, UserLevelType.Admin };
        }


        protected override ITask GetActionTask(string type)
        {
            switch (type)
            {
                case "AddSelect":
                    return new AddSelectTask();
                case "UpdateSelect":
                    return new UpdateSelectTask();
                case "loadlist":
                    return new LoadQuestionListTask();
                case "AddLinkSelect":
                    return new AddLinkSelectTask();
                case "loadLinkSelectlist":
                    return new LoadLinkListTask();
                case "UpdateLinkSelect":
                    return new UpdateLinkSelectTask();
                case "delLinkSelect":
                    return new DeleteLinkSelectTask();
                case "delSelect":
                    return new DeleteSelectTask();
                case "search":
                    return new SearchQuestionTask();
                case "reviece":
                    return new RevieceQuestionTask();
                case "SaveParams":
                    return new SaveSettingParamsTask();
            }
            return null;
        }


        private class AddSelectTask : ATask
        {
            public AddSelectTask()
                : base(false)
            {
                // do nothing
            }

            protected override ActionResult DoTask(string data)
            {
                DefaultTypeEnum defaultType = (DefaultTypeEnum)int.Parse(Request.QueryString["style"]);
                int linkId = int.Parse(Request.QueryString["linkid"]);
                string[] param = StringUtility.Split(data, "%27");
                string shortDesc = Escape.JsUnEscape(param[0]);
                int score = int.Parse(Escape.JsUnEscape(param[1]));
                int diff = int.Parse(Escape.JsUnEscape(param[2]));
                string content = Escape.JsUnEscape(param[3]);
                string item = Escape.JsUnEscape(param[4]);
                string answer = Escape.JsUnEscape(param[5]);
                if (score == 0)
                {
                    DefaultEntity defaultEntity = new DefaultEntity();
                    defaultEntity.DefaultType = defaultType;
                    defaultEntity.Fill();
                    score = defaultEntity.DefaultScore;
                    if (score == 0)
                    {
                        switch (defaultType)
                        {
                            case DefaultTypeEnum.SingleSelect:
                                score = 2;
                                break;
                            case DefaultTypeEnum.MultiSelect:
                                score = 4;
                                break;
                            case DefaultTypeEnum.JudgeSelect:
                                score = 1;
                                break;
                        }
                    }
                }
                QuestionEntity entity = new QuestionEntity();
                entity.QuestionType = QuestionEntity.ConvertDefaultTypeToQuestionType(defaultType);
                entity.QuestionLinkId = linkId;
                entity.QuestionContent = content;
                entity.QuestionDifficulty = diff;
                entity.QuestionAnswer = answer;
                entity.QuestionScore = score;
                entity.QuestionShortDescription = shortDesc;
                entity.QuestionItem = item;
                entity.QuestionLinkType = QuestionLinkTypeEnum.Nothing;
                entity.Save();

                QuestionCollection collection = new QuestionCollection();
                collection.PageSize = 8;
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

        private class UpdateSelectTask : ATask
        {
            public UpdateSelectTask()
                : base(false)
            {
                // do nothing
            }

            protected override ActionResult DoTask(string data)
            {
                int questId = int.Parse(Request.QueryString["quesid"]);
                string[] param = StringUtility.Split(data, "%27");
                string shortDesc = Escape.JsUnEscape(param[0]);
                int score = int.Parse(Escape.JsUnEscape(param[1]));
                int diff = int.Parse(Escape.JsUnEscape(param[2]));
                string content = Escape.JsUnEscape(param[3]);
                string item = Escape.JsUnEscape(param[4]);
                string answer = Escape.JsUnEscape(param[5]);

                QuestionEntity entity = new QuestionEntity();
                entity.QuestionId = questId;
                entity.QuestionLinkId = 0;
                entity.Fill();
                entity.QuestionContent = content;
                entity.QuestionDifficulty = diff;
                entity.QuestionAnswer = answer;
                entity.QuestionScore = score;
                entity.QuestionShortDescription = shortDesc;
                entity.QuestionItem = item;
                entity.Save();
                ActionResult result = new ActionResult();
                result.IsSuccess = true;
                return result;
            }
        }

        private class LoadQuestionListTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                string pageNoStr = Request.QueryString["pagenum"];
                int pageNo = 0;
                if (!int.TryParse(pageNoStr, out pageNo))
                    pageNo = 0;

                string condition = PageQuestionTaskUtility.CurrentExpVal();
                QuestionCollection collection = new QuestionCollection();
                if (pageNo == 0)
                {
                    collection.PageSize = 0;
                    collection.IsReturnDataTable = true;
                    collection.FillByCondition(condition);
                }
                else
                {
                    collection.PageSize = 8;
                    collection.FillByCondition(condition);
                    if (pageNo > collection.PageCount)
                        pageNo = collection.PageCount;
                    collection.AbsolutePage = pageNo;
                    collection.IsReturnDataTable = true;
                    collection.FillByCondition(condition);
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

        private class AddLinkSelectTask : ATask
        {
            public AddLinkSelectTask()
                : base(false)
            {
                // do nothing
            }


            protected override ActionResult DoTask(string data)
            {
                QuestionLinkTypeEnum linkType = (QuestionLinkTypeEnum)int.Parse(Request.QueryString["style"]);
                int linkId = 0;
                string[] blocks = StringUtility.Split(data, "%22");
                for (int i = 0; i < blocks.Length; i++)
                {
                    string block = blocks[i];
                    string[] param = StringUtility.Split(block, "%27");
                    if (i == 0)
                    {
                        string shortDesc = Escape.JsUnEscape(param[0]);
                        int diff = int.Parse(Escape.JsUnEscape(param[2]));
                        string content = Escape.JsUnEscape(param[3]);
                        string attached = Escape.JsUnEscape(param[4]);

                        QuestionEntity linkEntity = new QuestionEntity();
                        linkEntity.QuestionLinkType = linkType;
                        linkEntity.QuestionLinkContent = content;
                        linkEntity.QuestionLinkDifficulty = diff;
                        linkEntity.QuestionLinkShortDescription = shortDesc;
                        linkEntity.QuestionLinkAttachedInfo = attached;
                        linkEntity.Save();
                        linkId = linkEntity.QuestionLinkId;
                    }
                    else
                    {
                        string shortDesc = Escape.JsUnEscape(param[0]);
                        int score = int.Parse(Escape.JsUnEscape(param[1]));
                        int diff = int.Parse(Escape.JsUnEscape(param[2]));
                        string content = Escape.JsUnEscape(param[3]);
                        string item = Escape.JsUnEscape(param[4]);
                        string answer = Escape.JsUnEscape(param[5]);
                        DefaultTypeEnum defaultType = (DefaultTypeEnum)int.Parse(Escape.JsUnEscape(param[6]));
                        if (score == 0)
                        {
                            DefaultEntity defaultEntity = new DefaultEntity();
                            defaultEntity.DefaultType = defaultType;
                            defaultEntity.Fill();
                            score = defaultEntity.DefaultScore;
                            if (score == 0)
                            {
                                switch (defaultType)
                                {
                                    case DefaultTypeEnum.SingleSelect:
                                        score = 2;
                                        break;
                                    case DefaultTypeEnum.MultiSelect:
                                        score = 4;
                                        break;
                                    case DefaultTypeEnum.JudgeSelect:
                                        score = 1;
                                        break;
                                }
                            }
                        }
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionType = QuestionEntity.ConvertDefaultTypeToQuestionType(defaultType);
                        entity.QuestionLinkId = linkId;
                        entity.QuestionContent = content;
                        entity.QuestionDifficulty = diff;
                        entity.QuestionAnswer = answer;
                        entity.QuestionScore = score;
                        entity.QuestionShortDescription = shortDesc;
                        entity.QuestionItem = item;
                        entity.QuestionLinkType = QuestionLinkTypeEnum.Nothing;
                        entity.Save();
                    }
                }
                QuestionCollection collection = new QuestionCollection();
                collection.PageSize = 8;
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

        private class LoadLinkListTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                int pageNo = Convert.ToInt32(Request.QueryString["pagenum"]);
                int linkId = Convert.ToInt32(Request.QueryString["subid"]);

                QuestionCollection collection = new QuestionCollection();
                if (pageNo == 0)
                {
                    collection.PageSize = 0;
                    collection.IsReturnDataTable = true;
                    collection.FillByLinkId(linkId);
                }
                else
                {
                    collection.PageSize = 4;
                    collection.FillByLinkId(linkId);
                    if (pageNo > collection.PageCount)
                        pageNo = collection.PageCount;
                    collection.AbsolutePage = pageNo;
                    collection.IsReturnDataTable = true;
                    collection.FillByLinkId(linkId);
                }
                ActionResult result = new ActionResult();
                StringBuilder response = new StringBuilder();
                response.Append(ActionTaskUtility.ReturnClientDataArray(collection.GetFillDataTable(), this.IsOtherRequest));
                if (this.IsOtherRequest)
                    response.Append(string.Format("STmpStr={0};", collection.PageCount));
                else
                    response.Append(string.Format("TmpStr={0};", collection.PageCount));

                result.IsSuccess = true;
                result.ResponseData = response.ToString();
                return result;
            }
        }

        private class UpdateLinkSelectTask : ATask
        {
            public UpdateLinkSelectTask()
                : base(false)
            {
                // do nothing
            }

            protected override ActionResult DoTask(string data)
            {
                int linkId = Convert.ToInt32(Request.QueryString["linkid"]);
                string[] param = StringUtility.Split(data, "%27");
                string shortDesc = Escape.JsUnEscape(param[0]);
                int diff = int.Parse(Escape.JsUnEscape(param[2]));
                string content = Escape.JsUnEscape(param[3]);
                string attached = Escape.JsUnEscape(param[4]);

                QuestionEntity entity = new QuestionEntity();
                entity.QuestionId = 0;
                entity.QuestionLinkId = linkId;
                entity.Fill();
                entity.QuestionLinkContent = content;
                entity.QuestionLinkDifficulty = diff;
                entity.QuestionLinkShortDescription = shortDesc;
                entity.QuestionLinkAttachedInfo = attached;
                entity.Save();
                ActionResult result = new ActionResult();
                result.IsSuccess = true;
                return result;
            }
        }

        private class DeleteLinkSelectTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                int pageNo = Convert.ToInt32(Request.QueryString["pagenum"]);
                int linkId = Convert.ToInt32(Request.QueryString["subid"]);
                string delStrIds = Request.QueryString["ids"];

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

                QuestionCollection collection = new QuestionCollection();
                collection.DeleteByQuestionIds(delIds.ToArray());

                collection.PageSize = 4;
                collection.FillByLinkId(linkId);
                if (pageNo > collection.PageCount)
                    pageNo = collection.PageCount;
                collection.AbsolutePage = pageNo;
                collection.IsReturnDataTable = true;
                collection.FillByLinkId(linkId);

                ActionResult result = new ActionResult();
                StringBuilder response = new StringBuilder();
                response.Append(ActionTaskUtility.ReturnClientDataArray(collection.GetFillDataTable()));
                response.Append(string.Format("TmpStr={0};", collection.PageCount));
                result.IsSuccess = true;
                result.ResponseData = response.ToString();
                return result;
            }
        }

        private class DeleteSelectTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                int pageNo = Convert.ToInt32(Request.QueryString["PageN"]);
                string linkStrIds = Request.QueryString["linkid"];
                int[] linkIds = StringUtility.SplitStringToIntArray(linkStrIds, ",");
                if (linkIds == null || linkIds.Length == 0)
                    linkIds = new int[] { -1 };
                string quesStrIds = Request.QueryString["quesid"];
                int[] quesIds = StringUtility.SplitStringToIntArray(quesStrIds, ",");
                if (quesIds == null || quesIds.Length == 0)
                    quesIds = new int[] { -1 };

                QuestionCollection collection = new QuestionCollection();
                collection.DeleteByQuestionIds(quesIds);
                collection.DeleteByLinkIds(linkIds);

                string condition = PageQuestionTaskUtility.CurrentExpVal();
                if (pageNo == 0)
                {
                    collection.PageSize = 0;
                    collection.IsReturnDataTable = true;
                    collection.FillByCondition(condition);
                }
                else
                {
                    collection.PageSize = 8;
                    collection.FillByCondition(condition);
                    if (pageNo > collection.PageCount)
                        pageNo = collection.PageCount;
                    collection.AbsolutePage = pageNo;
                    collection.IsReturnDataTable = true;
                    collection.FillByCondition(condition);
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

        private class SearchQuestionTask : ATask
        {

            public SearchQuestionTask()
                : base(false)
            {
                // do nothing
            }

            protected override ActionResult DoTask(string data)
            {
                string sign = Request.QueryString["sign"];
                string condition = PageQuestionTaskUtility.ParseExpVal(sign, data);

                QuestionCollection collection = new QuestionCollection();
                collection.PageSize = 8;
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

        private class RevieceQuestionTask : ATask
        {

            protected override ActionResult DoTask(string data)
            {
                string pageNoStr = Request.QueryString["pagenum"];
                int pageNo = 0;
                if (!int.TryParse(pageNoStr, out pageNo))
                    pageNo = 0;

                string condition = PageQuestionTaskUtility.ParseExpVal("0", "");
                QuestionCollection collection = new QuestionCollection();
                if (pageNo == 0)
                {
                    collection.PageSize = 0;
                    collection.IsReturnDataTable = true;
                    collection.FillByCondition(condition);
                }
                else
                {
                    collection.PageSize = 8;
                    collection.FillByCondition(condition);
                    if (pageNo > collection.PageCount)
                        pageNo = collection.PageCount;
                    collection.AbsolutePage = pageNo;
                    collection.IsReturnDataTable = true;
                    collection.FillByCondition(condition);
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

        private class SaveSettingParamsTask : ATask
        {
            protected override ActionResult DoTask(string data)
            {
                string[] param = StringUtility.Split(data, "'");
                int uploadMaxSize = Convert.ToInt32(param[0]);
                int sessionTime = Convert.ToInt32(param[1]);
                int examineTime = Convert.ToInt32(param[2]);
                int examineQuestion = Convert.ToInt32(param[3]);

                SettingConfigUtility.UploadFileMaxSize = uploadMaxSize;
                SettingConfigUtility.SessionTimeout = sessionTime;
                SettingConfigUtility.ExaminationTime = examineTime;
                SettingConfigUtility.ExaminationQuestion = examineQuestion;

                ActionResult result = new ActionResult();
                result.IsSuccess = true;
                return result;
            }
        }


        private static class PageQuestionTaskUtility
        {

            public static string CurrentExpVal()
            {
                string exp = null;
                string sessionExp = (SessionManager.UserExp ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(sessionExp))
                {
                    exp = @"
                        (
		                    es_question.ques_link = 0 or 
		                    es_question.ques_id in (
			                    select max(es_question.ques_id) 
				                    from es_question(nolock) 
				                    group by es_question.ques_link
			                   ) or 
		                    es_question.ques_id is null
	                    )
                     ";
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
                        condition.Append(string.Format(@"
                            ((es_question.ques_short like '%{0}%' and es_question.ques_link=0) or es_link.link_short like '%{0}%')
                          ", param));
                        break;
                    case "2":
                        param = Escape.JsUnEscape(param);
                        if (param == "3" || param == "4")
                        {
                            int TypeNum = 0;
                            if (param == "3") TypeNum = 0;
                            if (param == "4") TypeNum = 1;
                            condition.Append(string.Format(@"(es_link.link_type={0})", TypeNum));
                        }
                        else
                        {
                            condition.Append(string.Format(@"(es_question.ques_type={0} and es_question.ques_link=0)", param));
                        }
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
                                    conTmp = conTmp + "(es_question.ques_short like '%" + word + "%' and es_question.ques_link=0) or es_link.link_short like '%" + word + "%'";
                                    break;
                                case "1":
                                    word = Escape.JsUnEscape(keyArray[1]);
                                    if (word == "3" || word == "4")
                                    {
                                        int TypeNum = 0;
                                        if (word == "3") TypeNum = 0;
                                        if (word == "4") TypeNum = 1;
                                        conTmp = conTmp + "es_link.link_type=" + TypeNum;
                                    }
                                    else
                                    {
                                        conTmp = conTmp + "es_question.ques_type=" + word + " and es_question.ques_link=0";
                                    }
                                    break;
                                case "2":
                                    word = Escape.JsUnEscape(keyArray[2]);
                                    switch (Escape.JsUnEscape(keyArray[1]))
                                    {
                                        case "0":
                                            conTmp = conTmp + "(es_question.ques_data>'" + word + "' AND convert(varchar(10),es_question.ques_data,101) not like '%" + word + "%' AND es_question.ques_link=0) or (es_link.link_data>'" + word + "' AND convert(varchar(10),es_link.link_data,101) not like '%" + word + "%')";
                                            break;
                                        case "1":
                                            conTmp = conTmp + "(es_question.ques_data<'" + word + "' AND es_question.ques_link=0) or es_link.link_data<'" + word + "'";
                                            break;
                                        case "2":
                                            conTmp = conTmp + "(convert(varchar(10),es_question.ques_data,101) like '%" + word + "%' AND es_question.ques_link=0) or convert(varchar(10),es_link.link_data,101) like '%" + word + "%'";
                                            break;
                                        case "3":
                                            conTmp = conTmp + "(convert(varchar(10),es_question.ques_data,101) not like '%" + word + "%' AND es_question.ques_link=0) or convert(varchar(10),es_link.link_data,101) not like '%" + word + "%'";
                                            break;
                                    }
                                    break;
                                case "3":
                                    word = Escape.JsUnEscape(keyArray[1]);
                                    conTmp = conTmp + "(es_question.ques_diff=" + word + " AND es_question.ques_link=0) or es_link.link_diff=" + word;
                                    break;
                                case "4":
                                    word = Escape.JsUnEscape(keyArray[2]);
                                    switch (Escape.JsUnEscape(keyArray[1]))
                                    {
                                        case "0":
                                            conTmp = conTmp + "es_question.ques_score>" + word + " OR (es_question.ques_score=0 AND es_question.ques_type In (Select top 1 default_type from es_default where default_score>" + word + "))";
                                            break;
                                        case "1":
                                            conTmp = conTmp + "es_question.ques_score<" + word + " OR (es_question.ques_score=0 AND es_question.ques_type In (Select top 1 default_type from es_default where default_score<" + word + "))";
                                            break;
                                        case "2":
                                            conTmp = conTmp + "es_question.ques_score=" + word + " OR (es_question.ques_score=0 AND es_question.ques_type In (Select top 1 default_type from es_default where default_score=" + word + "))";
                                            break;
                                        case "3":
                                            conTmp = conTmp + "es_question.ques_score<>" + word + " OR (es_question.ques_score=0 AND es_question.ques_type In (Select top 1 default_type from es_default where default_score<>" + word + "))";
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
                string limit = @"
                        (
		                    es_question.ques_link = 0 or 
		                    es_question.ques_id in (
			                    select max(es_question.ques_id) 
				                    from es_question(nolock) 
				                    group by es_question.ques_link
			                   ) or 
		                    es_question.ques_id is null
	                    )
                     ";
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