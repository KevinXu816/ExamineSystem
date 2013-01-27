using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Attribute;
using EntityModel.Command;
using System.Data;
using DataFrameworkLibrary.Core;

namespace EntityModel
{
    [Transaction]
    [CommandBuilder(typeof(HistoryCollectionCommandBuilder))]
    public class HistoryCollection : EntityCollection<HistoryEntity>
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


        public void FillByUserId(int userId)
        {
            this.DBSession.Action(this, "FILL_BY_USER_ID", userId);
        }

        public void FillByCondition(string condition)
        {
            this.DBSession.Action(this, "FILL_BY_CONDITION", condition);
        }

        public void DeleteByUserIds(int[] userIds)
        {
            this.DBSession.Action(this, "DELETE_BY_HISTORY_USER_IDS", userIds);
        }

        public void DeleteByHistoryIds(int[] historyIds)
        {
            this.DBSession.Action(this, "DELETE_BY_HISTORY_IDS", historyIds);
        }

    }
}
