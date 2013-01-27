using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core;
using DataFrameworkLibrary.Core.Attribute;
using EntityModel.Command;

namespace EntityModel
{

    [Transaction]
    [CommandBuilder(typeof(TempExamineCommandBuilder))]
    public class TempExamineCollection : EntityCollection<TempExamineEntity>
    {
        public string SchemaName
        {
            get;
            set;
        }

        public void CreateSchemaHost()
        {
            this.DBSession.Action(this, "CREATE_SCHEMA_HOST");
        }
    }
}
