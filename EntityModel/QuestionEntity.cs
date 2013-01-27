using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core;
using EntityModel.Command;
using DataFrameworkLibrary.Core.Attribute;

namespace EntityModel
{
    [Serializable]
    [Transaction]
    [CommandBuilder(typeof(QuestionCommandBuilder))]
    public class QuestionEntity : Entity
    {
        public int QuestionId
        {
            get;
            set;
        }

        private QuestionTypeEnum _questionType = QuestionTypeEnum.Nothing;
        public QuestionTypeEnum QuestionType
        {
            get { return this._questionType; }
            set { this._questionType = value; }
        }

        public string QuestionShortDescription
        {
            get;
            set;
        }

        public DateTime QuestionCreateDate
        {
            get;
            set;
        }

        public string QuestionContent
        {
            get;
            set;
        }

        public string QuestionItem
        {
            get;
            set;
        }

        public int QuestionDifficulty
        {
            get;
            set;
        }

        public string QuestionAnswer
        {
            get;
            set;
        }

        public int QuestionScore
        {
            get;
            set;
        }

        public int QuestionLinkId
        {
            get;
            set;
        }

        private QuestionLinkTypeEnum _linkType = QuestionLinkTypeEnum.Nothing;
        public QuestionLinkTypeEnum QuestionLinkType
        {
            get { return this._linkType; }
            set { this._linkType = value; }
        }

        public DateTime QuestionLinkCreateDate
        {
            get;
            set;
        }

        public string QuestionLinkShortDescription
        {
            get;
            set;
        }

        public string QuestionLinkContent
        {
            get;
            set;
        }

        public int QuestionLinkDifficulty
        {
            get;
            set;
        }

        public string QuestionLinkAttachedInfo
        {
            get;
            set;
        }

        public static QuestionTypeEnum ConvertDefaultTypeToQuestionType(DefaultTypeEnum defaultType)
        {
            switch (defaultType)
            {
                case DefaultTypeEnum.JudgeSelect:
                    return QuestionTypeEnum.JudgeSelect;
                case DefaultTypeEnum.MultiSelect:
                    return QuestionTypeEnum.MultiSelect;
                case DefaultTypeEnum.SingleSelect:
                    return QuestionTypeEnum.SingleSelect;
                case DefaultTypeEnum.Nothing:
                    return QuestionTypeEnum.Nothing;
            }
            return QuestionTypeEnum.Nothing;
        }
    }

    public enum QuestionTypeEnum
    {
        Nothing = -1,
        SingleSelect = 0,
        MultiSelect = 1,
        JudgeSelect = 2
    }

    public enum QuestionLinkTypeEnum
    {
        Nothing = -1,
        ReadSelect = 0,
        ListenSelect = 1
    }
}
