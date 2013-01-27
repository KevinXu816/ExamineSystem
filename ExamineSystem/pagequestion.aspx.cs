using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExamineSystem.view;
using EntityModel;
using System.Text;
using ExamineSystem.utility;

namespace ExamineSystem
{
    public partial class pagequestion : BasePage
    {
        public pagequestion()
        {
            this.IsValidateLogin = true;
            this.OperationUserLevels = new UserLevelType[] { UserLevelType.Teacher, UserLevelType.Admin };
        }



        private QuestionCollection collection = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            collection = new QuestionCollection();
            collection.PageSize = 8;
            collection.AbsolutePage = 1;
            collection.Fill();
            this.repQuestionList.DataSource = QuestionViewModelList;
            this.repQuestionList.DataBind();
        }

        protected int QuestionCollectionPageCount
        {
            get
            {
                if (collection == null)
                    return 0;
                else
                    return collection.PageCount;
            }
        }

        protected string SystemSettingParamsString
        {
            get
            {
                StringBuilder param = new StringBuilder();
                param.AppendFormat("{0}'{1}'{2}'{3}",
                       SettingConfigUtility.UploadFileMaxSize, SettingConfigUtility.SessionTimeout,
                       SettingConfigUtility.ExaminationTime, SettingConfigUtility.ExaminationQuestion);
                return param.ToString();
            }
        }

        protected bool IsControlSettingParam
        {
            get
            {
                UserEntity entity = SessionManager.User;
                if (entity == null)
                    return false;
                return (entity.UserLevel == UserLevelType.Admin);
            }
        }

        private List<QuestionViewModel> QuestionViewModelList
        {
            get
            {
                if (collection == null)
                    return null;
                else
                {
                    List<QuestionViewModel> list = new List<QuestionViewModel>();
                    foreach (QuestionEntity entity in collection)
                    {
                        if (entity == null)
                            continue;
                        QuestionViewModel model = new QuestionViewModel(entity);
                        list.Add(model);
                    }
                    return list;
                }
            }
        }


        private class QuestionViewModel
        {

            public QuestionViewModel(QuestionEntity entity)
            {
                if (entity.QuestionLinkId == 0)
                {
                    this.QuestionId = entity.QuestionId.ToString();
                    this.QuestionShortDescription = entity.QuestionShortDescription;
                    switch (entity.QuestionType)
                    {
                        case QuestionTypeEnum.SingleSelect:
                            this.QuestionType = "单项选择题";
                            this.QuestionClientFunction = "SingleSelect";
                            break;
                        case QuestionTypeEnum.MultiSelect:
                            this.QuestionType = "多项选择题";
                            this.QuestionClientFunction = "MultiSelect";
                            break;
                        case QuestionTypeEnum.JudgeSelect:
                            this.QuestionType = "对错判断题";
                            this.QuestionClientFunction = "JudgeSelect";
                            break;
                    }
                    this.QuestionStatus = string.Format("{0}'{1}'{2}'{3}'{4}'{5}",
                            entity.QuestionShortDescription, entity.QuestionScore, entity.QuestionDifficulty,
                            entity.QuestionContent, entity.QuestionItem, entity.QuestionAnswer);
                }
                else
                {
                    this.QuestionId = "_" + entity.QuestionLinkId;
                    this.QuestionShortDescription = entity.QuestionLinkShortDescription;
                    switch (entity.QuestionLinkType)
                    {
                        case QuestionLinkTypeEnum.ReadSelect:
                            this.QuestionType = "阅读理解题";
                            this.QuestionClientFunction = "ReadSelect";
                            break;
                        case QuestionLinkTypeEnum.ListenSelect:
                            this.QuestionType = "听力分析题";
                            this.QuestionClientFunction = "ListenSelect";
                            break;
                    }
                    this.QuestionStatus = string.Format("{0}''{1}'{2}'{3}'",
                            entity.QuestionLinkShortDescription, entity.QuestionLinkDifficulty,
                            entity.QuestionLinkContent, entity.QuestionLinkAttachedInfo);
                }
            }

            public string QuestionId
            {
                get;
                private set;
            }

            public string QuestionType
            {
                get;
                private set;
            }

            public string QuestionShortDescription
            {
                get;
                private set;
            }

            public string QuestionStatus
            {
                get;
                private set;
            }

            public string QuestionClientFunction
            {
                get;
                private set;
            }
        }


    }
}