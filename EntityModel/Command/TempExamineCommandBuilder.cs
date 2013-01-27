using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Command;
using System.Data.SqlClient;
using System.Data;

namespace EntityModel.Command
{
    class TempExamineCommandBuilder : SqlDBAdvanceCommandBuilder<TempExamineCollection>
    {

        public override bool InsertSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            throw new NotImplementedException();
        }

        public override bool SelectSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            throw new NotImplementedException();
        }

        public override bool CustomSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType, object commandParam)
        {
            bool result = false;
            switch (commandType)
            {
                case "CREATE_SCHEMA_HOST":
                    result = this.CreateSchemaHost(command);
                    break;
            }
            return result;
        }

        #region Create Schema Host
        private bool CreateSchemaHost(System.Data.SqlClient.SqlCommand command)
        {
            string schemaName = (this.Entity.SchemaName ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(schemaName))
                return false;

            command.CommandType = CommandType.Text;
            command.CommandText = string.Format(@"
                    DROP TABLE {0};
                    CREATE TABLE {0} 
                    (
                        quesid INT NOT NULL,
                        quesanswer VARCHAR(50) NOT NULL,
                        useranswer VARCHAR(50)
                    )
                ", schemaName);
            command.ExecuteNonQuery();
            return true;
        }
        #endregion

    }
}
