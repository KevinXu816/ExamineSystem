using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Attribute;
using EntityModel.Command;
using DataFrameworkLibrary.Core;

namespace EntityModel
{
    [Serializable]
    [Transaction]
    [CommandBuilder(typeof(DefaultCommandBuilder))]
    public class DefaultEntity : Entity
    {
        public DefaultTypeEnum DefaultType
        {
            get;
            set;
        }

        public int DefaultScore
        {
            get;
            set;
        }
    }

    public enum DefaultTypeEnum
    {
        Nothing = -1,
        SingleSelect = 0,
        MultiSelect = 1,
        JudgeSelect = 2
    }
}
