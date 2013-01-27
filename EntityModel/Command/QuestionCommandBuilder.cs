using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Command;
using System.Data.SqlClient;
using System.Data;

namespace EntityModel.Command
{
    class QuestionCommandBuilder : SqlDBAdvanceCommandBuilder<QuestionEntity>
    {
        public override bool InsertSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            bool result = false;
            if (this.Entity.QuestionType != QuestionTypeEnum.Nothing)
                if (this.InsertQuestionItem(command))
                    result = true;
            if (this.Entity.QuestionLinkType != QuestionLinkTypeEnum.Nothing)
                if (this.InsertLinkItem(command))
                    result = true;
            return result;
        }

        private bool InsertQuestionItem(System.Data.SqlClient.SqlCommand command)
        {
            SqlDataReader reader = null;
            try
            {
                command.Parameters.Clear();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                    INSERT INTO es_question(ques_type, ques_link, ques_text, ques_diff, ques_answer, ques_score, 
                            ques_data, ques_short, ques_item) values( @QUES_TYPE, @QUES_LINK, @QUES_CONTENT, @QUES_DIFF, @QUES_ANSWER, @QUES_SCORE,
                               GETDATE(), @QUES_SHORT, @QUES_ITEM );
                    SELECT SCOPE_IDENTITY();
                ";
                SqlParameter param = new SqlParameter("@QUES_TYPE", SqlDbType.SmallInt);
                param.Value = (int)this.Entity.QuestionType;
                command.Parameters.Add(param);

                param = new SqlParameter("@QUES_LINK", SqlDbType.Int);
                param.Value = this.Entity.QuestionLinkId;
                command.Parameters.Add(param);

                param = new SqlParameter("@QUES_CONTENT", SqlDbType.Text);
                param.Value = this.Entity.QuestionContent;
                command.Parameters.Add(param);

                param = new SqlParameter("@QUES_DIFF", SqlDbType.SmallInt);
                param.Value = this.Entity.QuestionDifficulty;
                command.Parameters.Add(param);

                param = new SqlParameter("@QUES_ANSWER", SqlDbType.VarChar, 50);
                param.Value = this.Entity.QuestionAnswer;
                command.Parameters.Add(param);

                param = new SqlParameter("@QUES_SCORE", SqlDbType.SmallInt);
                param.Value = this.Entity.QuestionScore;
                command.Parameters.Add(param);

                param = new SqlParameter("@QUES_SHORT", SqlDbType.VarChar, 200);
                param.Value = this.Entity.QuestionShortDescription;
                command.Parameters.Add(param);

                param = new SqlParameter("@QUES_ITEM", SqlDbType.Text);
                param.Value = this.Entity.QuestionItem;
                command.Parameters.Add(param);

                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.QuestionId = (int)reader.GetDecimal(0);
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

        private bool InsertLinkItem(System.Data.SqlClient.SqlCommand command)
        {
            SqlDataReader reader = null;
            try
            {
                command.Parameters.Clear();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                    INSERT INTO es_link(link_type, link_text, link_diff, link_data, link_short, link_bak) 
                            values( @LINK_TYPE, @LINK_CONTENT, @LINK_DIFF, GETDATE(), @LINK_SHORT, @LINK_ATTACHED);
                    SELECT SCOPE_IDENTITY();
                ";
                SqlParameter param = new SqlParameter("@LINK_TYPE", SqlDbType.SmallInt);
                param.Value = (int)this.Entity.QuestionLinkType;
                command.Parameters.Add(param);

                param = new SqlParameter("@LINK_CONTENT", SqlDbType.Text);
                param.Value = this.Entity.QuestionLinkContent;
                command.Parameters.Add(param);

                param = new SqlParameter("@LINK_DIFF", SqlDbType.SmallInt);
                param.Value = this.Entity.QuestionLinkDifficulty;
                command.Parameters.Add(param);

                param = new SqlParameter("@LINK_SHORT", SqlDbType.VarChar, 200);
                param.Value = this.Entity.QuestionLinkShortDescription;
                command.Parameters.Add(param);

                param = new SqlParameter("@LINK_ATTACHED", SqlDbType.VarChar, 50);
                param.Value = this.Entity.QuestionLinkAttachedInfo;
                command.Parameters.Add(param);

                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.QuestionLinkId = (int)reader.GetDecimal(0);
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



        public override bool UpdateSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            bool result = false;
            if (this.Entity.QuestionId > 0)
                if (this.UpdateQuestionItem(command))
                    result = true;
            if (this.Entity.QuestionLinkId > 0)
                if (this.UpdateLinkItem(command))
                    result = true;
            return result;
        }

        private bool UpdateQuestionItem(System.Data.SqlClient.SqlCommand command)
        {
            command.Parameters.Clear();
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                 UPDATE es_question SET ques_text = @QUES_CONTENT, ques_diff = @QUES_DIFF, ques_answer = @QUES_ANSWER, 
                        ques_score = @QUES_SCORE, ques_short = @QUES_SHORT, ques_item = @QUES_ITEM
                    WHERE ques_id = @QUES_ID
             ";

            SqlParameter param = new SqlParameter("@QUES_CONTENT", SqlDbType.Text);
            param.Value = this.Entity.QuestionContent;
            command.Parameters.Add(param);

            param = new SqlParameter("@QUES_DIFF", SqlDbType.SmallInt);
            param.Value = this.Entity.QuestionDifficulty;
            command.Parameters.Add(param);

            param = new SqlParameter("@QUES_ANSWER", SqlDbType.VarChar, 50);
            param.Value = this.Entity.QuestionAnswer;
            command.Parameters.Add(param);

            param = new SqlParameter("@QUES_SCORE", SqlDbType.SmallInt);
            param.Value = this.Entity.QuestionScore;
            command.Parameters.Add(param);

            param = new SqlParameter("@QUES_SHORT", SqlDbType.VarChar, 200);
            param.Value = this.Entity.QuestionShortDescription;
            command.Parameters.Add(param);

            param = new SqlParameter("@QUES_ITEM", SqlDbType.Text);
            param.Value = this.Entity.QuestionItem;
            command.Parameters.Add(param);

            param = new SqlParameter("@QUES_ID", SqlDbType.Int);
            param.Value = (int)this.Entity.QuestionId;
            command.Parameters.Add(param);

            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }

        private bool UpdateLinkItem(System.Data.SqlClient.SqlCommand command)
        {
            command.Parameters.Clear();
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                 UPDATE es_link SET link_text = @LINK_CONTENT, link_diff = @LINK_DIFF, link_short = @LINK_SHORT, link_bak = @LINK_ATTACHED
                    WHERE link_id = @LINK_ID
             ";

            SqlParameter param = new SqlParameter("@LINK_CONTENT", SqlDbType.Text);
            param.Value = this.Entity.QuestionLinkContent;
            command.Parameters.Add(param);

            param = new SqlParameter("@LINK_DIFF", SqlDbType.SmallInt);
            param.Value = this.Entity.QuestionLinkDifficulty;
            command.Parameters.Add(param);

            param = new SqlParameter("@LINK_SHORT", SqlDbType.VarChar, 200);
            param.Value = this.Entity.QuestionLinkShortDescription;
            command.Parameters.Add(param);

            param = new SqlParameter("@LINK_ATTACHED", SqlDbType.VarChar, 50);
            param.Value = this.Entity.QuestionLinkAttachedInfo;
            command.Parameters.Add(param);

            param = new SqlParameter("@LINK_ID", SqlDbType.Int);
            param.Value = this.Entity.QuestionLinkId;
            command.Parameters.Add(param);

            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }



        public override bool SelectSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType)
        {
            bool result = false;
            if (this.Entity.QuestionId > 0)
                if (this.SelectQuestionItem(command))
                    result = true;
            if (this.Entity.QuestionLinkId > 0)
                if (this.SelectLinkItem(command))
                    result = true;
            return result;
        }

        private bool SelectQuestionItem(System.Data.SqlClient.SqlCommand command)
        {
            SqlDataReader reader = null;
            try
            {
                command.Parameters.Clear();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                        select es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                    es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                    es_question.ques_score, es_question.ques_link
                            from es_question(nolock) 
	                        where es_question.ques_id = @ID
                    ";
                SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
                param.Value = this.Entity.QuestionId;
                command.Parameters.Add(param);
                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.QuestionId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    this.Entity.QuestionType = reader.IsDBNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)reader.GetInt16(1);
                    this.Entity.QuestionShortDescription = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    this.Entity.QuestionCreateDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                    this.Entity.QuestionContent = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    this.Entity.QuestionItem = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    this.Entity.QuestionDifficulty = reader.IsDBNull(6) ? 0 : reader.GetInt16(6);
                    this.Entity.QuestionAnswer = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    this.Entity.QuestionScore = reader.IsDBNull(8) ? 0 : reader.GetInt16(8);
                    this.Entity.QuestionLinkId = reader.IsDBNull(9) ? -1 : reader.GetInt32(9);
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

        private bool SelectLinkItem(System.Data.SqlClient.SqlCommand command)
        {
            SqlDataReader reader = null;
            try
            {
                command.Parameters.Clear();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
                        select es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                    es_link.link_text, es_link.link_diff, es_link.link_bak
                            from es_link(nolock) 
	                        where es_link.link_id = @ID
                    ";
                SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
                param.Value = this.Entity.QuestionLinkId;
                command.Parameters.Add(param);
                reader = command.ExecuteReader();
                bool flag = false;
                if (reader.Read())
                {
                    flag = true;
                    this.Entity.QuestionLinkId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                    this.Entity.QuestionLinkType = reader.IsDBNull(1) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)reader.GetInt16(1);
                    this.Entity.QuestionLinkCreateDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);
                    this.Entity.QuestionLinkShortDescription = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    this.Entity.QuestionLinkContent = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    this.Entity.QuestionLinkDifficulty = reader.IsDBNull(5) ? 0 : reader.GetInt16(5);
                    this.Entity.QuestionLinkAttachedInfo = reader.IsDBNull(6) ? "" : reader.GetString(6);
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
            bool result = false;
            if (this.Entity.QuestionId > 0)
                if (this.DeleteQuestionItem(command))
                    result = true;
            if (this.Entity.QuestionLinkId > 0)
                if (this.DeleteLinkItem(command))
                    result = true;
            return result;
        }

        private bool DeleteQuestionItem(System.Data.SqlClient.SqlCommand command)
        {
            command.Parameters.Clear();
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                 DELETE FROM es_question WHERE ques_id = @ID
             ";
            SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
            param.Value = this.Entity.QuestionId;
            command.Parameters.Add(param);
            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }

        private bool DeleteLinkItem(System.Data.SqlClient.SqlCommand command)
        {
            command.Parameters.Clear();
            command.CommandType = CommandType.Text;
            command.CommandText = @"
                 DELETE FROM es_link WHERE link_id = @ID
             ";
            SqlParameter param = new SqlParameter("@ID", SqlDbType.Int);
            param.Value = this.Entity.QuestionLinkId;
            command.Parameters.Add(param);
            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }

    }
}
