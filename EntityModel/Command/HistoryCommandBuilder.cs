using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Command;
using System.Data;
using System.Data.SqlClient;

namespace EntityModel.Command
{
    class HistoryCommandBuilder : SqlDBAdvanceCommandBuilder<HistoryEntity>
    {

        public override bool InsertSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            SqlDataReader reader = null;
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                    INSERT INTO es_history(history_stime, history_etime, history_score, history_user, history_ip) values(
                        @START_TIME, @END_TIME, @SCORE, @USER_ID, @IP);
                    SELECT SCOPE_IDENTITY();
                ";
                SqlParameter param = new SqlParameter("@START_TIME", SqlDbType.DateTime);
                param.Value = this.Entity.HistoryStartTime;
                command.Parameters.Add(param);

                param = new SqlParameter("@END_TIME", SqlDbType.DateTime);
                param.Value = this.Entity.HistoryEndTime;
                command.Parameters.Add(param);

                param = new SqlParameter("@SCORE", SqlDbType.SmallInt);
                param.Value = this.Entity.HistoryScore;
                command.Parameters.Add(param);

                param = new SqlParameter("@USER_ID", SqlDbType.Int);
                param.Value = this.Entity.HistoryUserId;
                command.Parameters.Add(param);

                param = new SqlParameter("@IP", SqlDbType.VarChar, 50);
                param.Value = this.Entity.HistoryIp;
                command.Parameters.Add(param);

                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.HistoryId = (int)reader.GetDecimal(0);
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

        public override bool DeleteSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                 DELETE FROM es_history WHERE [history_id] = @ID
             ";
            SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
            param.Value = this.Entity.HistoryId;
            command.Parameters.Add(param);
            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }

        public override bool UpdateSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                 UPDATE es_history SET history_stime = @START_TIME, history_etime = @END_TIME, 
                    history_score = @SCORE, history_user = @USER_ID, history_ip = @IP
                WHERE [history_id] = @ID
             ";

            SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
            param.Value = this.Entity.HistoryId;
            command.Parameters.Add(param);

            param = new SqlParameter("@START_TIME", SqlDbType.DateTime);
            param.Value = this.Entity.HistoryStartTime;
            command.Parameters.Add(param);

            param = new SqlParameter("@END_TIME", SqlDbType.DateTime);
            param.Value = this.Entity.HistoryEndTime;
            command.Parameters.Add(param);

            param = new SqlParameter("@SCORE", SqlDbType.SmallInt);
            param.Value = this.Entity.HistoryScore;
            command.Parameters.Add(param);

            param = new SqlParameter("@USER_ID", SqlDbType.Int);
            param.Value = this.Entity.HistoryUserId;
            command.Parameters.Add(param);

            param = new SqlParameter("@IP", SqlDbType.VarChar, 50);
            param.Value = this.Entity.HistoryIp;
            command.Parameters.Add(param);

            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }

        public override bool SelectSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            switch (commandType)
            {
                default:
                    return this.DefaultFill(command);
            }
        }

        #region Default Fill
        private bool DefaultFill(System.Data.SqlClient.SqlCommand command)
        {
            SqlDataReader reader = null;
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                    SELECT H.history_stime, H.history_etime, H.history_score, H.history_user, H.history_ip 
	                    FROM es_history(NOLOCK) H
                        WHERE H.history_id = @ID
                    ";
                SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
                param.Value = this.Entity.HistoryId;
                command.Parameters.Add(param);
                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.HistoryStartTime = reader.GetDateTime(0);
                    this.Entity.HistoryEndTime = reader.GetDateTime(1);
                    this.Entity.HistoryScore = reader.GetInt16(2);
                    this.Entity.HistoryUserId = reader.GetInt32(3);
                    this.Entity.HistoryIp = reader.GetString(4);
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
