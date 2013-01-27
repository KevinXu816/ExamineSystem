using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Command;
using System.Data.SqlClient;
using System.Data;

namespace EntityModel.Command
{
    class DefaultCommandBuilder : SqlDBAdvanceCommandBuilder<DefaultEntity>
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
            return DefaultFill(command);
        }

        #region Default Fill
        private bool DefaultFill(System.Data.SqlClient.SqlCommand command)
        {
            SqlDataReader reader = null;
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT default_score FROM es_default(NOLOCK) WHERE default_type = @DEFAULT_TYPE";
                SqlParameter param = new SqlParameter("@DEFAULT_TYPE", SqlDbType.SmallInt);
                param.Value = (int)this.Entity.DefaultType;
                command.Parameters.Add(param);
                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.DefaultScore = reader.GetInt16(0);
                }
                return flag;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }
        #endregion
    }
}
