using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Command;
using System.Data.SqlClient;
using System.Data;

namespace EntityModel.Command
{
    class HistoryCollectionCommandBuilder : SqlDBAdvanceCommandBuilder<HistoryCollection>
    {
        public override bool SelectSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            return this.DefaultFill(command);
        }

        public override bool CustomSqlCommand(SqlCommand command, string commandType, object commandParam)
        {
            bool result = false;
            switch (commandType)
            {
                case "FILL_BY_USER_ID":
                    result = this.FillByUserId(command, (int)commandParam);
                    break;
                case "FILL_BY_CONDITION":
                    result = this.FillByCondition(command, (string)commandParam);
                    break;
                case "DELETE_BY_HISTORY_USER_IDS":
                    result = this.DeleteByUserIds(command, (int[])commandParam);
                    break;
                case "DELETE_BY_HISTORY_IDS":
                    result = this.DeleteByHistoryIds(command, (int[])commandParam);
                    break;
            }
            return result;
        }

        #region Default Fill
        private bool DefaultFill(System.Data.SqlClient.SqlCommand command)
        {

            SqlDataReader reader = null;
            try
            {
                SqlParameter param = null;
                command.CommandType = CommandType.Text;
                if (this.Entity.PageSize < 1)
                {
                    command.CommandText = @"
                        SELECT H.history_id, H.history_stime, H.history_etime, H.history_score, H.history_user, H.history_ip
                            FROM es_history(NOLOCK) H
                            ORDER BY H.history_id DESC
                        ";
                }
                else
                {
                    command.CommandText = @"
                        SELECT T.history_id, T.history_stime, T.history_etime, T.history_score, T.history_user, T.history_ip
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY H.history_id DESC) AS ROW_NUM,
		                                H.history_id, H.history_stime, H.history_etime, H.history_score, H.history_user, H.history_ip
	                                FROM es_history(NOLOCK) H
                              ) AS T
                            WHERE (ROW_NUM BETWEEN @START_RECORD AND @END_RECORD)
                       ";
                    int startRecord = this.Entity.PageSize * (this.Entity.AbsolutePage - 1) + 1;
                    int endRecord = this.Entity.PageSize * this.Entity.AbsolutePage;
                    param = new SqlParameter("@START_RECORD", SqlDbType.Int);
                    param.Value = startRecord;
                    command.Parameters.Add(param);
                    param = new SqlParameter("@END_RECORD", SqlDbType.Int);
                    param.Value = endRecord;
                    command.Parameters.Add(param);
                }
                this.Entity.Clear();
                if (this.Entity.IsReturnDataTable)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable fillDataTable = new DataTable();
                    adapter.Fill(fillDataTable);
                    foreach (DataRow row in fillDataTable.Rows)
                    {
                        HistoryEntity entity = new HistoryEntity();
                        entity.HistoryId = Convert.ToInt32(row[0]);
                        entity.HistoryStartTime = Convert.ToDateTime(row[1]);
                        entity.HistoryEndTime = Convert.ToDateTime(row[2]);
                        entity.HistoryScore = Convert.ToInt16(row[3]);
                        entity.HistoryUserId = Convert.ToInt32(row[4]);
                        entity.HistoryIp = row[5].ToString();
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        HistoryEntity entity = new HistoryEntity();
                        entity.HistoryId = reader.GetInt32(0);
                        entity.HistoryStartTime = reader.GetDateTime(1);
                        entity.HistoryEndTime = reader.GetDateTime(2);
                        entity.HistoryScore = reader.GetInt16(3);
                        entity.HistoryUserId = reader.GetInt32(4);
                        entity.HistoryIp = reader.GetString(5);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = "SELECT COUNT(H.[history_id]) AS TOTAL FROM es_history(NOLOCK) H";
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    float total = reader.GetInt32(0);
                    if (this.Entity.PageSize == 0)
                        this.Entity.PageCount = 1;
                    else
                        this.Entity.PageCount = (int)Math.Round((total / this.Entity.PageSize) + 0.5f, 0);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return (this.Entity.Count > 0);
        }
        #endregion

        #region UserId Fill
        private bool FillByUserId(System.Data.SqlClient.SqlCommand command, int userId)
        {
            SqlDataReader reader = null;
            try
            {
                SqlParameter param = null;
                command.CommandType = CommandType.Text;
                if (this.Entity.PageSize < 1)
                {
                    command.CommandText = @"
                        SELECT H.history_id, H.history_stime, H.history_etime, H.history_score, H.history_user, H.history_ip
                            FROM es_history(NOLOCK) H
                            WHERE H.history_user = @USER_ID
                            ORDER BY H.history_id DESC
                        ";
                }
                else
                {
                    command.CommandText = @"
                        SELECT T.history_id, T.history_stime, T.history_etime, T.history_score, T.history_user, T.history_ip
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY H.history_id DESC) AS ROW_NUM,
		                                H.history_id, H.history_stime, H.history_etime, H.history_score, H.history_user, H.history_ip
	                                FROM es_history(NOLOCK) H
                                    WHERE H.history_user = @USER_ID
                              ) AS T
                            WHERE (ROW_NUM BETWEEN @START_RECORD AND @END_RECORD)
                       ";
                    int startRecord = this.Entity.PageSize * (this.Entity.AbsolutePage - 1) + 1;
                    int endRecord = this.Entity.PageSize * this.Entity.AbsolutePage;
                    param = new SqlParameter("@START_RECORD", SqlDbType.Int);
                    param.Value = startRecord;
                    command.Parameters.Add(param);
                    param = new SqlParameter("@END_RECORD", SqlDbType.Int);
                    param.Value = endRecord;
                    command.Parameters.Add(param);
                }
                param = new SqlParameter("@USER_ID", SqlDbType.Int);
                param.Value = userId;
                command.Parameters.Add(param);
                this.Entity.Clear();
                if (this.Entity.IsReturnDataTable)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable fillDataTable = new DataTable();
                    adapter.Fill(fillDataTable);
                    foreach (DataRow row in fillDataTable.Rows)
                    {
                        HistoryEntity entity = new HistoryEntity();
                        entity.HistoryId = Convert.ToInt32(row[0]);
                        entity.HistoryStartTime = Convert.ToDateTime(row[1]);
                        entity.HistoryEndTime = Convert.ToDateTime(row[2]);
                        entity.HistoryScore = Convert.ToInt16(row[3]);
                        entity.HistoryUserId = Convert.ToInt32(row[4]);
                        entity.HistoryIp = row[5].ToString();
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        HistoryEntity entity = new HistoryEntity();
                        entity.HistoryId = reader.GetInt32(0);
                        entity.HistoryStartTime = reader.GetDateTime(1);
                        entity.HistoryEndTime = reader.GetDateTime(2);
                        entity.HistoryScore = reader.GetInt16(3);
                        entity.HistoryUserId = reader.GetInt32(4);
                        entity.HistoryIp = reader.GetString(5);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = "SELECT COUNT(H.[history_id]) AS TOTAL FROM es_history(NOLOCK) H WHERE H.history_user = @USER_ID";
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    float total = reader.GetInt32(0);
                    if (this.Entity.PageSize == 0)
                        this.Entity.PageCount = 1;
                    else
                        this.Entity.PageCount = (int)Math.Round((total / this.Entity.PageSize) + 0.5f, 0);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return (this.Entity.Count > 0);
        }
        #endregion

        #region Condition Fill
        private bool FillByCondition(System.Data.SqlClient.SqlCommand command, string condition)
        {
            condition = (condition ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(condition))
                condition = "1 = 1";
            SqlDataReader reader = null;
            try
            {
                SqlParameter param = null;
                command.CommandType = CommandType.Text;
                if (this.Entity.PageSize < 1)
                {
                    command.CommandText = string.Format(@"
                        SELECT history_id, history_stime, history_etime, history_score, history_user, history_ip
                            FROM es_history(NOLOCK)
                            WHERE {0}
                            ORDER BY history_id DESC
                        ", condition);
                }
                else
                {
                    command.CommandText = string.Format(@"
                        SELECT T.history_id, T.history_stime, T.history_etime, T.history_score, T.history_user, T.history_ip
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY history_id DESC) AS ROW_NUM,
		                                history_id, history_stime, history_etime, history_score, history_user, history_ip
	                                FROM es_history(NOLOCK)
                                    WHERE {0}
                              ) AS T
                            WHERE (ROW_NUM BETWEEN @START_RECORD AND @END_RECORD)
                       ", condition);
                    int startRecord = this.Entity.PageSize * (this.Entity.AbsolutePage - 1) + 1;
                    int endRecord = this.Entity.PageSize * this.Entity.AbsolutePage;
                    param = new SqlParameter("@START_RECORD", SqlDbType.Int);
                    param.Value = startRecord;
                    command.Parameters.Add(param);
                    param = new SqlParameter("@END_RECORD", SqlDbType.Int);
                    param.Value = endRecord;
                    command.Parameters.Add(param);
                }
                this.Entity.Clear();
                if (this.Entity.IsReturnDataTable)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable fillDataTable = new DataTable();
                    adapter.Fill(fillDataTable);
                    foreach (DataRow row in fillDataTable.Rows)
                    {
                        HistoryEntity entity = new HistoryEntity();
                        entity.HistoryId = Convert.ToInt32(row[0]);
                        entity.HistoryStartTime = Convert.ToDateTime(row[1]);
                        entity.HistoryEndTime = Convert.ToDateTime(row[2]);
                        entity.HistoryScore = Convert.ToInt16(row[3]);
                        entity.HistoryUserId = Convert.ToInt32(row[4]);
                        entity.HistoryIp = row[5].ToString();
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        HistoryEntity entity = new HistoryEntity();
                        entity.HistoryId = reader.GetInt32(0);
                        entity.HistoryStartTime = reader.GetDateTime(1);
                        entity.HistoryEndTime = reader.GetDateTime(2);
                        entity.HistoryScore = reader.GetInt16(3);
                        entity.HistoryUserId = reader.GetInt32(4);
                        entity.HistoryIp = reader.GetString(5);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = string.Format("SELECT COUNT([history_id]) AS TOTAL FROM es_history(NOLOCK) WHERE {0}", condition);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    float total = reader.GetInt32(0);
                    if (this.Entity.PageSize == 0)
                        this.Entity.PageCount = 1;
                    else
                        this.Entity.PageCount = (int)Math.Round((total / this.Entity.PageSize) + 0.5f, 0);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return (this.Entity.Count > 0);
        }
        #endregion

        #region History UserIds Deleted
        private bool DeleteByUserIds(System.Data.SqlClient.SqlCommand command, int[] userIds)
        {
            if (userIds == null || userIds.Length == 0)
                return true;
            StringBuilder delIds = null;
            foreach (int userId in userIds)
            {
                if (delIds == null)
                    delIds = new StringBuilder(userId.ToString());
                else
                    delIds.AppendFormat(",{0}", userId);
            }
            if (delIds == null)
                return true;
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format("DELETE FROM es_history WHERE history_user In ({0})", delIds.ToString());
            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }
        #endregion

        #region History Ids Deleted
        private bool DeleteByHistoryIds(System.Data.SqlClient.SqlCommand command, int[] historyIds)
        {
            if (historyIds == null || historyIds.Length == 0)
                return true;
            StringBuilder delIds = null;
            foreach (int historyId in historyIds)
            {
                if (delIds == null)
                    delIds = new StringBuilder(historyId.ToString());
                else
                    delIds.AppendFormat(",{0}", historyId);
            }
            if (delIds == null)
                return true;
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format("DELETE FROM es_history WHERE history_id In ({0})", delIds.ToString());
            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }
        #endregion


        public override bool InsertSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            return false;
        }

        public override bool DeleteSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            return false;
        }

        public override bool UpdateSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            return false;
        }
    }
}
