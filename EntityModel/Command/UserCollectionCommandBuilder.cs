using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Command;
using System.Data.SqlClient;
using System.Data;

namespace EntityModel.Command
{
    class UserCollectionCommandBuilder : SqlDBAdvanceCommandBuilder<UserCollection>
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
                case "FILL_BY_USER_LEVEL":
                    result = this.FillByUserLevel(command, (UserLevelType)commandParam);
                    break;
                case "FILL_BY_CONDITION":
                    result = this.FillByCondition(command, (string)commandParam);
                    break;
                case "DELETE_BY_USER_IDS":
                    result = this.DeleteByUserIds(command, (int[])commandParam);
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
                        SELECT U.[user_id], U.user_num, U.[user_name], U.user_password, U.user_func, U.user_login, U.user_do
	                        FROM es_user(NOLOCK) U
                            ORDER BY U.[user_id] DESC
                        ";
                }
                else
                {
                    command.CommandText = @"
                        SELECT T.[user_id], T.user_num, T.[user_name], T.user_password, T.user_func, T.user_login, T.user_do
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY U.[user_id] DESC) AS ROW_NUM,
		                                U.[user_id], U.user_num, U.[user_name], U.user_password, U.user_func, U.user_login, U.user_do
	                                FROM es_user(NOLOCK) U
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
                        UserEntity entity = new UserEntity();
                        entity.UserId = Convert.ToInt32(row[0]);
                        entity.UserNo = row[1].ToString();
                        entity.UserName = row[2].ToString();
                        entity.Password = row[3].ToString();
                        entity.UserLevel = (UserLevelType)Convert.ToInt16(row[4]);
                        entity.IsLogin = Convert.ToBoolean(row[5]);
                        entity.DoTest = Convert.ToBoolean(row[6]);
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UserEntity entity = new UserEntity();
                        entity.UserId = reader.GetInt32(0);
                        entity.UserNo = reader.GetString(1);
                        entity.UserName = reader.GetString(2);
                        entity.Password = reader.GetString(3);
                        entity.UserLevel = (UserLevelType)reader.GetInt16(4);
                        entity.IsLogin = reader.GetBoolean(5);
                        entity.DoTest = reader.GetBoolean(6);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = "SELECT COUNT(U.[user_id]) AS TOTAL FROM es_user(NOLOCK) U";
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

        #region UserLevel Fill
        private bool FillByUserLevel(System.Data.SqlClient.SqlCommand command, UserLevelType userLevel)
        {
            SqlDataReader reader = null;
            try
            {
                SqlParameter param = null;
                command.CommandType = CommandType.Text;
                if (this.Entity.PageSize < 1)
                {
                    command.CommandText = @"
                        SELECT U.[user_id], U.user_num, U.[user_name], U.user_password, U.user_func, U.user_login, U.user_do
	                        FROM es_user(NOLOCK) U
                            WHERE U.user_func = @LEVEL
                            ORDER BY U.[user_id] DESC
                        ";
                }
                else
                {
                    command.CommandText = @"
                        SELECT T.[user_id], T.user_num, T.[user_name], T.user_password, T.user_func, T.user_login, T.user_do
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY U.[user_id] DESC) AS ROW_NUM,
		                                U.[user_id], U.user_num, U.[user_name], U.user_password, U.user_func, U.user_login, U.user_do
	                                FROM es_user(NOLOCK) U
                                    WHERE U.user_func = @LEVEL
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
                param = new SqlParameter("@LEVEL", SqlDbType.SmallInt);
                param.Value = userLevel;
                command.Parameters.Add(param);
                this.Entity.Clear();
                if (this.Entity.IsReturnDataTable)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable fillDataTable = new DataTable();
                    adapter.Fill(fillDataTable);
                    foreach (DataRow row in fillDataTable.Rows)
                    {
                        UserEntity entity = new UserEntity();
                        entity.UserId = Convert.ToInt32(row[0]);
                        entity.UserNo = row[1].ToString();
                        entity.UserName = row[2].ToString();
                        entity.Password = row[3].ToString();
                        entity.UserLevel = (UserLevelType)Convert.ToInt16(row[4]);
                        entity.IsLogin = Convert.ToBoolean(row[5]);
                        entity.DoTest = Convert.ToBoolean(row[6]);
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UserEntity entity = new UserEntity();
                        entity.UserId = reader.GetInt32(0);
                        entity.UserNo = reader.GetString(1);
                        entity.UserName = reader.GetString(2);
                        entity.Password = reader.GetString(3);
                        entity.UserLevel = (UserLevelType)reader.GetInt16(4);
                        entity.IsLogin = reader.GetBoolean(5);
                        entity.DoTest = reader.GetBoolean(6);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = "SELECT COUNT(U.[user_id]) AS TOTAL FROM es_user(NOLOCK) U WHERE U.user_func = @LEVEL";
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
                        SELECT [user_id], user_num, [user_name], user_password, user_func, user_login, user_do
	                        FROM es_user(NOLOCK)
                            WHERE {0}
                            ORDER BY [user_id] DESC
                        ", condition);
                }
                else
                {
                    command.CommandText = string.Format(@"
                        SELECT T.[user_id], T.user_num, T.[user_name], T.user_password, T.user_func, T.user_login, T.user_do
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY [user_id] DESC) AS ROW_NUM,
		                                [user_id], user_num, [user_name], user_password, user_func, user_login, user_do
	                                FROM es_user(NOLOCK)
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
                        UserEntity entity = new UserEntity();
                        entity.UserId = Convert.ToInt32(row[0]);
                        entity.UserNo = row[1].ToString();
                        entity.UserName = row[2].ToString();
                        entity.Password = row[3].ToString();
                        entity.UserLevel = (UserLevelType)Convert.ToInt16(row[4]);
                        entity.IsLogin = Convert.ToBoolean(row[5]);
                        entity.DoTest = Convert.ToBoolean(row[6]);
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UserEntity entity = new UserEntity();
                        entity.UserId = reader.GetInt32(0);
                        entity.UserNo = reader.GetString(1);
                        entity.UserName = reader.GetString(2);
                        entity.Password = reader.GetString(3);
                        entity.UserLevel = (UserLevelType)reader.GetInt16(4);
                        entity.IsLogin = reader.GetBoolean(5);
                        entity.DoTest = reader.GetBoolean(6);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = string.Format("SELECT COUNT([user_id]) AS TOTAL FROM es_user(NOLOCK) WHERE {0}", condition);
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

        #region UserIds Deleted
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
            command.CommandText = string.Format("DELETE FROM es_user WHERE user_id In ({0})", delIds.ToString());
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
