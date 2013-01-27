using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamineSystem.view;
using EntityModel;
using ExamineSystem.utility;
using System.Text;

namespace ExamineSystem
{
    public partial class pageexamine : BasePage
    {
        private int currentHistoryId = 0;

        public pageexamine()
        {
            this.IsValidateLogin = true;
            this.OperationUserLevels = new UserLevelType[] { UserLevelType.Admin, UserLevelType.Student, UserLevelType.Teacher };
        }

        private int selectQuestionCount
        {
            get
            {
                return SettingConfigUtility.ExaminationQuestion;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (SessionManager.User != null)
            {
                if (!SessionManager.User.DoTest)
                {
                    Response.ClearContent();
                    Response.Write(@"
                        <html><head></head><body bgcolor='#cad7f7'>
                            <script>alert('对不起！您没有网络答卷的权限，可能您已经答过试卷');history.go(-1);</script>
                        </body></html>
                    ");
                    Response.End();
                    return;
                }
                UserEntity currentUser = SessionManager.User;
                currentUser.DoTest = false;
                currentUser.Save();

                HistoryEntity history = new HistoryEntity();
                history.HistoryStartTime = DateTime.Now;
                history.HistoryIp = SessionManager.ClientIp;
                history.HistoryUserId = currentUser.UserId;
                history.Save();
                currentHistoryId = history.HistoryId;

                //TempExamineCollection tempExamine = new TempExamineCollection();
                //tempExamine.SchemaName = string.Format("es_{0}_tmp", currentUser.UserId);
                //tempExamine.CreateSchemaHost();
            }
        }

        protected int CurrentHistoryId
        {
            get
            {
                return this.currentHistoryId;
            }
        }

        protected string CurrentClientIP
        {
            get
            {
                string clientIP = SessionManager.ClientIp ?? string.Empty;
                return clientIP;
            }
        }

        protected int ExaminationTime
        {
            get
            {
                return SettingConfigUtility.ExaminationTime;
            }
        }

        private int[] RoundIdRange(int total, int count)
        {
            List<int> idRange = new List<int>();
            int poolRange = (count < total) ? count : total;
            while (idRange.Count < poolRange)
            {
                Random random = new Random(DateTime.Now.Millisecond);
                int randomId = random.Next(0, total);
                if (idRange.Count == 0)
                    idRange.Add(randomId);
                else
                {
                    if (idRange.Contains(randomId))
                        continue;
                    else
                        idRange.Add(randomId);
                }
            }
            return idRange.ToArray();
        }

        protected string RandomListenQuestion
        {
            get
            {
                QuestionCollection collection = new QuestionCollection();
                collection.FillByLinkType(QuestionLinkTypeEnum.ListenSelect);

                List<QuestionLinkModel> links = new List<QuestionLinkModel>();
                foreach (QuestionEntity entity in collection)
                {
                    QuestionLinkModel linkModel = links.Find(item => item.LinkId == entity.QuestionLinkId);
                    if (linkModel == null)
                    {
                        linkModel = new QuestionLinkModel();
                        linkModel.LinkId = entity.QuestionLinkId;
                        linkModel.LinkType = entity.QuestionLinkType;
                        linkModel.LinkContent = entity.QuestionLinkContent;
                        linkModel.LinkAttached = entity.QuestionLinkAttachedInfo;
                        linkModel.QuestionChilds = new List<QuestionModel>();
                        links.Add(linkModel);
                    }
                    if (entity.QuestionId > 0)
                    {
                        QuestionModel quesModel = new QuestionModel();
                        quesModel.QuestionId = entity.QuestionId;
                        quesModel.QuestionType = entity.QuestionType;
                        quesModel.QuestionContent = entity.QuestionContent;
                        quesModel.QuestionAnswer = entity.QuestionAnswer;
                        quesModel.QuestionItems = entity.QuestionItem;
                        linkModel.QuestionChilds.Add(quesModel);
                    }
                }
                StringBuilder html = new StringBuilder();
                if (links.Count == 0)
                {
                    html.Append("此类型题无相关内容");
                }
                else
                {
                    int[] radomIds = this.RoundIdRange(links.Count, this.selectQuestionCount);
                    for (int i = 0; i < radomIds.Length; i++)
                    {
                        int radomId = radomIds[i];
                        if (radomId >= links.Count)
                            radomId = links.Count - 1;
                        html.Append(links[radomId].RenderQuestionLinkHTML(i));
                        html.Append("<hr>");
                    }
                }
                return html.ToString();
            }
        }

        protected string RandomReadQuestion
        {
            get
            {
                QuestionCollection collection = new QuestionCollection();
                collection.FillByLinkType(QuestionLinkTypeEnum.ReadSelect);

                List<QuestionLinkModel> links = new List<QuestionLinkModel>();
                foreach (QuestionEntity entity in collection)
                {
                    QuestionLinkModel linkModel = links.Find(item => item.LinkId == entity.QuestionLinkId);
                    if (linkModel == null)
                    {
                        linkModel = new QuestionLinkModel();
                        linkModel.LinkId = entity.QuestionLinkId;
                        linkModel.LinkType = entity.QuestionLinkType;
                        linkModel.LinkContent = entity.QuestionLinkContent;
                        linkModel.LinkAttached = entity.QuestionLinkAttachedInfo;
                        linkModel.QuestionChilds = new List<QuestionModel>();
                        links.Add(linkModel);
                    }
                    if (entity.QuestionId > 0)
                    {
                        QuestionModel quesModel = new QuestionModel();
                        quesModel.QuestionId = entity.QuestionId;
                        quesModel.QuestionType = entity.QuestionType;
                        quesModel.QuestionContent = entity.QuestionContent;
                        quesModel.QuestionAnswer = entity.QuestionAnswer;
                        quesModel.QuestionItems = entity.QuestionItem;
                        linkModel.QuestionChilds.Add(quesModel);
                    }
                }
                StringBuilder html = new StringBuilder();
                if (links.Count == 0)
                {
                    html.Append("此类型题无相关内容");
                }
                else
                {
                    int[] radomIds = this.RoundIdRange(links.Count, this.selectQuestionCount);
                    for (int i = 0; i < radomIds.Length; i++)
                    {
                        int radomId = radomIds[i];
                        if (radomId >= links.Count)
                            radomId = links.Count - 1;
                        html.Append(links[radomId].RenderQuestionLinkHTML(i));
                        html.Append("<hr>");
                    }
                }
                return html.ToString();
            }
        }

        protected string RandomSingleQuestion
        {
            get
            {
                QuestionCollection collection = new QuestionCollection();
                collection.FillByQuestionType(QuestionTypeEnum.SingleSelect);

                List<QuestionModel> questions = new List<QuestionModel>();
                foreach (QuestionEntity entity in collection)
                {
                    if (entity.QuestionId > 0)
                    {
                        QuestionModel quesModel = new QuestionModel();
                        quesModel.QuestionId = entity.QuestionId;
                        quesModel.QuestionType = entity.QuestionType;
                        quesModel.QuestionContent = entity.QuestionContent;
                        quesModel.QuestionAnswer = entity.QuestionAnswer;
                        quesModel.QuestionItems = entity.QuestionItem;
                        questions.Add(quesModel);
                    }
                }
                StringBuilder html = new StringBuilder();
                if (questions.Count == 0)
                {
                    html.Append("此类型题无相关内容");
                }
                else
                {
                    int[] radomIds = this.RoundIdRange(questions.Count, this.selectQuestionCount);
                    for (int i = 0; i < radomIds.Length; i++)
                    {
                        int radomId = radomIds[i];
                        if (radomId >= questions.Count)
                            radomId = questions.Count - 1;
                        html.Append(questions[radomId].RenderQuestionHTML(i));
                        html.Append("<hr>");
                    }
                }
                return html.ToString();
            }
        }

        protected string RandomMultiQuestion
        {
            get
            {
                QuestionCollection collection = new QuestionCollection();
                collection.FillByQuestionType(QuestionTypeEnum.MultiSelect);

                List<QuestionModel> questions = new List<QuestionModel>();
                foreach (QuestionEntity entity in collection)
                {
                    if (entity.QuestionId > 0)
                    {
                        QuestionModel quesModel = new QuestionModel();
                        quesModel.QuestionId = entity.QuestionId;
                        quesModel.QuestionType = entity.QuestionType;
                        quesModel.QuestionContent = entity.QuestionContent;
                        quesModel.QuestionAnswer = entity.QuestionAnswer;
                        quesModel.QuestionItems = entity.QuestionItem;
                        questions.Add(quesModel);
                    }
                }
                StringBuilder html = new StringBuilder();
                if (questions.Count == 0)
                {
                    html.Append("此类型题无相关内容");
                }
                else
                {
                    int[] radomIds = this.RoundIdRange(questions.Count, this.selectQuestionCount);
                    for (int i = 0; i < radomIds.Length; i++)
                    {
                        int radomId = radomIds[i];
                        if (radomId >= questions.Count)
                            radomId = questions.Count - 1;
                        html.Append(questions[radomId].RenderQuestionHTML(i));
                        html.Append("<hr>");
                    }
                }
                return html.ToString();
            }
        }

        protected string RandomJudgeQuestion
        {
            get
            {
                QuestionCollection collection = new QuestionCollection();
                collection.FillByQuestionType(QuestionTypeEnum.JudgeSelect);

                List<QuestionModel> questions = new List<QuestionModel>();
                foreach (QuestionEntity entity in collection)
                {
                    if (entity.QuestionId > 0)
                    {
                        QuestionModel quesModel = new QuestionModel();
                        quesModel.QuestionId = entity.QuestionId;
                        quesModel.QuestionType = entity.QuestionType;
                        quesModel.QuestionContent = entity.QuestionContent;
                        quesModel.QuestionAnswer = entity.QuestionAnswer;
                        quesModel.QuestionItems = entity.QuestionItem;
                        questions.Add(quesModel);
                    }
                }
                StringBuilder html = new StringBuilder();
                if (questions.Count == 0)
                {
                    html.Append("此类型题无相关内容");
                }
                else
                {
                    int[] radomIds = this.RoundIdRange(questions.Count, this.selectQuestionCount);
                    for (int i = 0; i < radomIds.Length; i++)
                    {
                        int radomId = radomIds[i];
                        if (radomId >= questions.Count)
                            radomId = questions.Count - 1;
                        html.Append(questions[radomId].RenderQuestionHTML(i));
                        html.Append("<hr>");
                    }
                }
                return html.ToString();
            }
        }

        private class QuestionModel
        {
            public int QuestionId
            {
                get;
                set;
            }

            public QuestionTypeEnum QuestionType
            {
                get;
                set;
            }

            public string QuestionContent
            {
                get;
                set;
            }

            public string QuestionAnswer
            {
                get;
                set;
            }

            public string QuestionItems
            {
                get;
                set;
            }

            public string RenderQuestionHTML(int index)
            {
                StringBuilder html = new StringBuilder();
                string[] items = null;
                html.AppendFormat("<table border='0' id='Ques_{0}'>", this.QuestionId);
                switch (this.QuestionType)
                {
                    case QuestionTypeEnum.SingleSelect:
                        html.AppendFormat("<tr><td colspan='2'>({0}). {1}</td></tr>", index, Escape.JsUnEscape(this.QuestionContent));
                        items = StringUtility.Split(this.QuestionItems, ",");
                        for (int i = 0; i < items.Length; i++)
                        {
                            string item = items[i];
                            html.AppendFormat("<tr><td><input type='radio' value='{0}' onclick='radioSelect();'> {1}) </td>", i, StringUtility.ConvertAlphabet(i));
                            html.AppendFormat("<td>{0}</td></tr>", Escape.JsUnEscape(item));
                        }
                        break;
                    case QuestionTypeEnum.MultiSelect:
                        html.AppendFormat("<tr><td colspan='2'>({0}). {1}</td></tr>", index, Escape.JsUnEscape(this.QuestionContent));
                        items = StringUtility.Split(this.QuestionItems, ",");
                        for (int i = 0; i < items.Length; i++)
                        {
                            string item = items[i];
                            html.AppendFormat("<tr><td><input type='checkbox' value='{0}'> {1}) </td>", i, StringUtility.ConvertAlphabet(i));
                            html.AppendFormat("<td>{0}</td></tr>", Escape.JsUnEscape(item));
                        }
                        break;
                    case QuestionTypeEnum.JudgeSelect:
                        html.AppendFormat("<tr><td colspan='2'>({0}). {1}</td></tr>", index, Escape.JsUnEscape(this.QuestionContent));
                        html.Append("<tr><td><input type='radio' value='1' onclick='radioSelect();'> 对</td>");
                        html.Append("<td><input type='radio' value='0' onclick='radioSelect();'> 错</td></tr>");
                        break;
                }
                html.Append("</table>");
                return html.ToString();
            }
        }

        private class QuestionLinkModel
        {
            public int LinkId
            {
                get;
                set;
            }

            public string LinkContent
            {
                get;
                set;
            }

            public QuestionLinkTypeEnum LinkType
            {
                get;
                set;
            }

            public string LinkAttached
            {
                get;
                set;
            }

            public List<QuestionModel> QuestionChilds
            {
                get;
                set;
            }

            public string RenderQuestionLinkHTML(int index)
            {
                StringBuilder html = new StringBuilder();
                switch (this.LinkType)
                {
                    case QuestionLinkTypeEnum.ListenSelect:
                        html.AppendFormat("<table border='0'><tr><td>{0}. 单击按钮听取内容：<img src='images/sound.gif' alt='听力材料' onClick='playMusic(\"{1}\",\"{2}\",this);'> 试听次数({3})<span style='visibility:hidden;'>&nbsp;[正在播放]</span></td></tr>",
                                index, this.LinkContent, this.LinkAttached, this.LinkAttached);
                        for (int i = 0; i < this.QuestionChilds.Count; i++)
                        {
                            QuestionModel question = this.QuestionChilds[i];
                            html.Append("<tr><td>");
                            html.Append(question.RenderQuestionHTML(i));
                            html.Append("</td></tr>");
                        }
                        html.Append("</table>");
                        break;
                    case QuestionLinkTypeEnum.ReadSelect:
                        html.AppendFormat("<table border='0'><tr><td>{0}. 阅读材料：<br>{1}</td></tr>", index, Escape.JsUnEscape(this.LinkContent));
                        for (int i = 0; i < this.QuestionChilds.Count; i++)
                        {
                            QuestionModel question = this.QuestionChilds[i];
                            html.Append("<tr><td>");
                            html.Append(question.RenderQuestionHTML(i));
                            html.Append("</td></tr>");
                        }
                        html.Append("</table>");
                        break;
                }
                return html.ToString();
            }
        }

    }
}