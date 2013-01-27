using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Attribute;
using DataFrameworkLibrary.Core;
using EntityModel.Command;

namespace EntityModel
{
    [Serializable]
    [Transaction]
    [CommandBuilder(typeof(HistoryCommandBuilder))]
    public class HistoryEntity : Entity
    {
        public int HistoryId
        {
            get;
            set;
        }

        private DateTime _historyStartTime = new DateTime(1900, 1, 1);
        public DateTime HistoryStartTime
        {
            get { return this._historyStartTime; }
            set
            {
                if (DateTime.MinValue == value)
                    this._historyStartTime = new DateTime(1900, 1, 1);
                else
                    this._historyStartTime = value;
            }
        }

        private DateTime _historyEndTime = new DateTime(1900, 1, 1);
        public DateTime HistoryEndTime
        {
            get { return this._historyEndTime; }
            set
            {
                if (DateTime.MinValue == value)
                    this._historyEndTime = new DateTime(1900, 1, 1);
                else
                    this._historyEndTime = value;
            }
        }

        public int HistoryScore
        {
            get;
            set;
        }

        public int HistoryUserId
        {
            get;
            set;
        }

        public string HistoryIp
        {
            get;
            set;
        }
    }
}
