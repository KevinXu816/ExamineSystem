using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExamineSystem.action.inner;
using EntityModel;
using ExamineSystem.utility;
using System.Text;

namespace ExamineSystem.action
{
    public class PageExamine : AAction
    {
        public PageExamine()
            : base(true)
        {
            this.OperationUserLevels = new UserLevelType[] { UserLevelType.Admin, UserLevelType.Student, UserLevelType.Teacher };
        }

        protected override ITask GetActionTask(string type)
        {
            switch (type)
            {
                case "submit":
                    return new SubmitExamineTask();
            }
            return null;
        }

        private class SubmitExamineTask : ATask
        {
            public SubmitExamineTask()
                : base(false)
            {
                // do nothing
            }

            protected override ActionResult DoTask(string data)
            {
                string[] param = StringUtility.Split(data, "%27");
                int historyId = Convert.ToInt32(Request.QueryString["history"]);
                int score = 0;
                int singscore = 0;
                int multiscore = 0;
                int julgscore = 0;

                DefaultEntity defaultEntity = new DefaultEntity();
                defaultEntity.DefaultType = DefaultTypeEnum.SingleSelect;
                defaultEntity.Fill();
                singscore = defaultEntity.DefaultScore;

                defaultEntity.DefaultType = DefaultTypeEnum.MultiSelect;
                defaultEntity.Fill();
                multiscore = defaultEntity.DefaultScore;

                defaultEntity.DefaultType = DefaultTypeEnum.JudgeSelect;
                defaultEntity.Fill();
                julgscore = defaultEntity.DefaultScore;

                foreach (string val in param)
                {
                    string txt = (val ?? string.Empty).Trim();
                    if (string.IsNullOrEmpty(txt))
                        continue;
                    string[] idandanswer = StringUtility.Split(txt, "%22");
                    QuestionEntity entity = new QuestionEntity();
                    entity.QuestionId = Convert.ToInt32(Escape.JsUnEscape(idandanswer[0]));
                    entity.Fill();
                    int itemScore = 0;
                    if (entity.QuestionAnswer.Equals(Escape.JsUnEscape(idandanswer[1])))
                        itemScore = entity.QuestionScore;
                    if (itemScore == 0)
                    {
                        switch (entity.QuestionType)
                        {
                            case QuestionTypeEnum.SingleSelect:
                                itemScore = singscore;
                                break;
                            case QuestionTypeEnum.JudgeSelect:
                                itemScore = julgscore;
                                break;
                            case QuestionTypeEnum.MultiSelect:
                                itemScore = multiscore;
                                break;
                        }
                    }
                    score = score + itemScore;
                }
                HistoryEntity history = new HistoryEntity();
                history.HistoryId = historyId;
                history.Fill();
                history.HistoryEndTime = DateTime.Now;
                history.HistoryScore = score;
                history.Save();

                ActionResult result = new ActionResult();
                result.IsSuccess = true;
                StringBuilder response = new StringBuilder();
                response.Append(string.Format("TmpStr='{0}';", score));
                result.ResponseData = response.ToString();
                return result;
            }
        }
    }
}