using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Command;
using System.Data.SqlClient;
using System.Data;

namespace EntityModel.Command
{
    class UserCommandBuilder : SqlDBAdvanceCommandBuilder<UserEntity>
    {
        public override bool InsertSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            SqlDataReader reader = null;
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                    INSERT INTO es_user(user_num,user_name,user_password,user_func,user_login,user_do) values(
                        @NUMBER, @NAME, @PASSWORD, @LEVEL, @IS_LOGIN, @IS_TESTED);
                    SELECT SCOPE_IDENTITY();
                ";
                SqlParameter param = new SqlParameter("@NAME", SqlDbType.VarChar);
                param.Value = this.Entity.UserName;
                command.Parameters.Add(param);

                param = new SqlParameter("@IS_TESTED", SqlDbType.Bit);
                param.Value = this.Entity.DoTest;
                command.Parameters.Add(param);

                param = new SqlParameter("@LEVEL", SqlDbType.SmallInt);
                param.Value = this.Entity.UserLevel;
                command.Parameters.Add(param);

                param = new SqlParameter("@IS_LOGIN", SqlDbType.Bit);
                param.Value = this.Entity.IsLogin;
                command.Parameters.Add(param);

                param = new SqlParameter("@NUMBER", SqlDbType.VarChar);
                param.Value = this.Entity.UserNo;
                command.Parameters.Add(param);

                param = new SqlParameter("@PASSWORD", SqlDbType.VarChar);
                param.Value = this.Entity.Password;
                command.Parameters.Add(param);

                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.UserId = (int)reader.GetDecimal(0);
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
                DELETE FROM es_user WHERE [user_id] = @ID
             ";
            SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
            param.Value = this.Entity.UserId;
            command.Parameters.Add(param);
            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }

        public override bool UpdateSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                UPDATE es_user SET [user_name] = @NAME, user_do = @IS_TESTED, 
                    user_func = @LEVEL, user_login = @IS_LOGIN, user_num = @NUMBER,
                    user_password = @PASSWORD
                WHERE [user_id] = @ID
             ";

            SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
            param.Value = this.Entity.UserId;
            command.Parameters.Add(param);

            param = new SqlParameter("@NAME", SqlDbType.VarChar);
            param.Value = this.Entity.UserName;
            command.Parameters.Add(param);

            param = new SqlParameter("@IS_TESTED", SqlDbType.Bit);
            param.Value = this.Entity.DoTest;
            command.Parameters.Add(param);

            param = new SqlParameter("@LEVEL", SqlDbType.SmallInt);
            param.Value = this.Entity.UserLevel;
            command.Parameters.Add(param);

            param = new SqlParameter("@IS_LOGIN", SqlDbType.Bit);
            param.Value = this.Entity.IsLogin;
            command.Parameters.Add(param);

            param = new SqlParameter("@NUMBER", SqlDbType.VarChar);
            param.Value = this.Entity.UserNo;
            command.Parameters.Add(param);

            param = new SqlParameter("@PASSWORD", SqlDbType.VarChar);
            param.Value = this.Entity.Password;
            command.Parameters.Add(param);

            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }

        public override bool CustomSqlCommand(SqlCommand command, string commandType, object commandParam)
        {
            switch (commandType)
            {
                case "ACTION_FILL_IDENTITY_USER_ID":
                    return this.FillIdentityUserId(command);
            }
            return false;
        }

        #region define Fill Identity UserId
        private bool FillIdentityUserId(System.Data.SqlClient.SqlCommand command)
        {
            SqlDataReader reader = null;
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                    SELECT (COUNT(U.user_id)+1) as maxId 
                        FROM es_user (NOLOCK) U
                        WHERE U.user_func = 1
                    ";
                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.UserNo = string.Format("{0:00000}", reader.GetInt32(0));
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

        public override bool SelectSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            switch (commandType)
            {
                case "FILL_BY_USERNO":
                    return this.FillByUserNo(command);
                case "FILL_BY_USERNO_AND_PASSWORD":
                    return this.FillByUserNoAndPassword(command);
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
                    SELECT U.[user_name], U.user_num, U.user_password, U.user_do, U.user_func, U.user_login 
                        FROM es_user(NOLOCK) U
                        WHERE U.[user_id] = @ID
                    ";
                SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
                param.Value = this.Entity.UserId;
                command.Parameters.Add(param);
                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.UserName = reader.GetString(0);
                    this.Entity.UserNo = reader.GetString(1);
                    this.Entity.Password = reader.GetString(2);
                    this.Entity.DoTest = reader.GetBoolean(3);
                    this.Entity.UserLevel = (UserLevelType)reader.GetInt16(4);
                    this.Entity.IsLogin = reader.GetBoolean(5);
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

        #region UserNO And Password Fill
        private bool FillByUserNoAndPassword(System.Data.SqlClient.SqlCommand command)
        {
            SqlDataReader reader = null;
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                    SELECT U.[user_id], U.[user_name], U.user_do, U.user_func, U.user_login 
                        FROM es_user(NOLOCK) U
                        WHERE U.user_num = @NUMBER AND U.user_password = @PASSWORD
                    ";
                SqlParameter param = new SqlParameter("@NUMBER", SqlDbType.VarChar);
                param.Value = this.Entity.UserNo;
                command.Parameters.Add(param);
                param = new SqlParameter("@PASSWORD", SqlDbType.VarChar);
                param.Value = this.Entity.Password;
                command.Parameters.Add(param);
                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.UserId = reader.GetInt32(0);
                    this.Entity.UserName = reader.GetString(1);
                    this.Entity.DoTest = reader.GetBoolean(2);
                    this.Entity.UserLevel = (UserLevelType)reader.GetInt16(3);
                    this.Entity.IsLogin = reader.GetBoolean(4);
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

        #region UserNo Fill
        private bool FillByUserNo(System.Data.SqlClient.SqlCommand command)
        {
            SqlDataReader reader = null;
            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                    SELECT U.[user_id], U.[user_name], U.user_password, U.user_do, U.user_func, U.user_login 
                        FROM es_user(NOLOCK) U
                        WHERE U.user_num = @NUMBER
                    ";
                SqlParameter param = new SqlParameter("@NUMBER", SqlDbType.VarChar);
                param.Value = this.Entity.UserNo;
                command.Parameters.Add(param);
                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.UserId = reader.GetInt32(0);
                    this.Entity.UserName = reader.GetString(1);
                    this.Entity.Password = reader.GetString(2);
                    this.Entity.DoTest = reader.GetBoolean(3);
                    this.Entity.UserLevel = (UserLevelType)reader.GetInt16(4);
                    this.Entity.IsLogin = reader.GetBoolean(5);
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
