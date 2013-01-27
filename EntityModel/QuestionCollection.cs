using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityModel.Command;
using DataFrameworkLibrary.Core.Attribute;
using DataFrameworkLibrary.Core;
using System.Data;

namespace EntityModel
{
    [Transaction]
    [CommandBuilder(typeof(QuestionCollectionCommandBuilder))]
    public class QuestionCollection : EntityCollection<QuestionEntity>
    {
        private int _pageSize = 0;
        public int PageSize
        {
            set { this._pageSize = (value < 0) ? 0 : value; }
            get { return this._pageSize; }
        }

        private int _absolutePage = 1;
        public int AbsolutePage
        {
            set { this._absolutePage = (value < 1) ? 1 : value; }
            get { return this._absolutePage; }
        }

        private int _pageCount = 0;
        public int PageCount
        {
            set { this._pageCount = (value < 0) ? 0 : value; }
            get { return this._pageCount; }
        }

        public bool IsReturnDataTable
        {
            set;
            get;
        }

        internal DataTable fillDataTable;
        public DataTable GetFillDataTable()
        {
            return this.fillDataTable;
        }

        public void FillByCondition(string condition)
        {
            this.DBSession.Action(this, "FILL_BY_CONDITION", condition);
        }

        public void FillByLinkId(int linkId)
        {
            this.DBSession.Action(this, "FILL_BY_LINK_ID", linkId);
        }

        public void FillByLinkType(QuestionLinkTypeEnum linkType)
        {
            this.DBSession.Action(this, "FILL_BY_LINK_TYPE", linkType);
        }

        public void FillByQuestionType(QuestionTypeEnum questionType)
        {
            this.DBSession.Action(this, "FILL_BY_QUESTION_TYPE", questionType);
        }

        public void DeleteByQuestionIds(int[] questionIds)
        {
            this.DBSession.Action(this, "DELETE_BY_QUESTION_IDS", questionIds);
        }

        public void DeleteByLinkIds(int[] linkIds)
        {
            this.DBSession.Action(this, "DELETE_BY_LINK_IDS", linkIds);
        }
    }
}
