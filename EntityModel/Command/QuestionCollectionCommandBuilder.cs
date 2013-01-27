using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core.Command;
using System.Data.SqlClient;
using System.Data;

namespace EntityModel.Command
{
    class QuestionCollectionCommandBuilder : SqlDBAdvanceCommandBuilder<QuestionCollection>
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
            return this.DefaultFill(command);
        }

        public override bool CustomSqlCommand(System.Data.SqlClient.SqlCommand command, string commandType, object commandParam)
        {
            bool result = false;
            switch (commandType)
            {
                case "FILL_BY_CONDITION":
                    result = this.FillByCondition(command, (string)commandParam);
                    break;
                case "FILL_BY_LINK_ID":
                    result = this.FillByLinkId(command, (int)commandParam);
                    break;
                case "FILL_BY_LINK_TYPE":
                    result = this.FillByLinkType(command, (QuestionLinkTypeEnum)commandParam);
                    break;
                case "FILL_BY_QUESTION_TYPE":
                    result = this.FillByQuestionType(command, (QuestionTypeEnum)commandParam);
                    break;
                case "DELETE_BY_QUESTION_IDS":
                    result = this.DeleteByQuestionIds(command, (int[])commandParam);
                    break;
                case "DELETE_BY_LINK_IDS":
                    result = this.DeleteByLinkIds(command, (int[])commandParam);
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
                        select es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                    es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                    es_question.ques_score, es_question.ques_link, es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                    es_link.link_text, es_link.link_diff, es_link.link_bak
	                        from es_question(nolock) full outer join es_link(nolock) 
		                        on es_question.ques_link = es_link.link_id 
	                        where 
	                        (
		                        es_question.ques_link = 0 or 
		                        es_question.ques_id in (
			                        select max(es_question.ques_id) 
				                        from es_question(nolock) 
				                        group by es_question.ques_link
			                        ) or 
		                        es_question.ques_id is null
	                        ) 
	                        order by es_question.ques_id desc
                        ";
                }
                else
                {
                    command.CommandText = @"
                        SELECT T.ques_id, T.ques_type, T.ques_short, T.ques_data, T.ques_text, T.ques_item, T.ques_diff, T.ques_answer,
			                    T.ques_score, T.ques_link, T.link_id, T.link_type, T.link_data, T.link_short, T.link_text, T.link_diff, T.link_bak
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY es_question.ques_id DESC) AS ROW_NUM,
		                                es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                            es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                            es_question.ques_score, es_question.ques_link, es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                            es_link.link_text, es_link.link_diff, es_link.link_bak
	                                FROM es_question(nolock) full outer join es_link(nolock) 
		                                on es_question.ques_link = es_link.link_id 
	                                WHERE 
	                                (
		                                es_question.ques_link = 0 or 
		                                es_question.ques_id in (
			                                select max(es_question.ques_id) 
				                                from es_question(nolock) 
				                                group by es_question.ques_link
			                                ) or 
		                                es_question.ques_id is null
	                                )
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
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionId = row.IsNull(0) ? 0 : Convert.ToInt32(row[0]);
                        entity.QuestionType = row.IsNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)Convert.ToInt16(row[1]);
                        entity.QuestionShortDescription = row.IsNull(2) ? "" : row[2].ToString();
                        entity.QuestionCreateDate = row.IsNull(3) ? DateTime.MinValue : Convert.ToDateTime(row[3]);
                        entity.QuestionContent = row.IsNull(4) ? "" : row[4].ToString();
                        entity.QuestionItem = row.IsNull(5) ? "" : row[5].ToString();
                        entity.QuestionDifficulty = row.IsNull(6) ? 0 : Convert.ToInt16(row[6]);
                        entity.QuestionAnswer = row.IsNull(7) ? "" : row[7].ToString();
                        entity.QuestionScore = row.IsNull(8) ? 0 : Convert.ToInt16(row[8]);
                        entity.QuestionLinkId = row.IsNull(9) ? (row.IsNull(10) ? -1 : Convert.ToInt32(10)) : Convert.ToInt32(row[9]);
                        entity.QuestionLinkType = row.IsNull(11) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)Convert.ToInt16(row[11]);
                        entity.QuestionLinkCreateDate = row.IsNull(12) ? DateTime.MinValue : Convert.ToDateTime(row[12]);
                        entity.QuestionLinkShortDescription = row.IsNull(13) ? "" : row[13].ToString();
                        entity.QuestionLinkContent = row.IsNull(14) ? "" : row[14].ToString();
                        entity.QuestionLinkDifficulty = row.IsNull(15) ? 0 : Convert.ToInt16(row[15]);
                        entity.QuestionLinkAttachedInfo = row.IsNull(16) ? "" : row[16].ToString();
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        entity.QuestionType = reader.IsDBNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)reader.GetInt16(1);
                        entity.QuestionShortDescription = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        entity.QuestionCreateDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                        entity.QuestionContent = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        entity.QuestionItem = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        entity.QuestionDifficulty = reader.IsDBNull(6) ? 0 : reader.GetInt16(6);
                        entity.QuestionAnswer = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        entity.QuestionScore = reader.IsDBNull(8) ? 0 : reader.GetInt16(8);
                        entity.QuestionLinkId = reader.IsDBNull(9) ? (reader.IsDBNull(10) ? -1 : reader.GetInt32(10)) : reader.GetInt32(9);
                        entity.QuestionLinkType = reader.IsDBNull(11) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)reader.GetInt16(11);
                        entity.QuestionLinkCreateDate = reader.IsDBNull(12) ? DateTime.MinValue : reader.GetDateTime(12);
                        entity.QuestionLinkShortDescription = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        entity.QuestionLinkContent = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        entity.QuestionLinkDifficulty = reader.IsDBNull(15) ? 0 : reader.GetInt16(15);
                        entity.QuestionLinkAttachedInfo = reader.IsDBNull(16) ? "" : reader.GetString(16);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = @"
                        select count(isnull(es_question.ques_id, 0)) as total
	                        from es_question(nolock) full outer join es_link(nolock) 
		                        on es_question.ques_link = es_link.link_id 
	                        where 
	                        (
		                        es_question.ques_link = 0 or 
		                        es_question.ques_id in (
			                        select max(es_question.ques_id) 
				                        from es_question(nolock) 
				                        group by es_question.ques_link
			                        ) or 
		                        es_question.ques_id is null
	                        )
                    ";
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
                        select es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                    es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                    es_question.ques_score, es_question.ques_link, es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                    es_link.link_text, es_link.link_diff, es_link.link_bak
	                        from es_question(nolock) full outer join es_link(nolock) 
		                        on es_question.ques_link = es_link.link_id 
	                        where {0}
	                        order by es_question.ques_id desc
                        ", condition);
                }
                else
                {
                    command.CommandText = string.Format(@"
                        SELECT T.ques_id, T.ques_type, T.ques_short, T.ques_data, T.ques_text, T.ques_item, T.ques_diff, T.ques_answer,
			                    T.ques_score, T.ques_link, T.link_id, T.link_type, T.link_data, T.link_short, T.link_text, T.link_diff, T.link_bak
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY es_question.ques_id DESC) AS ROW_NUM,
		                                es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                            es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                            es_question.ques_score, es_question.ques_link, es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                            es_link.link_text, es_link.link_diff, es_link.link_bak
	                                FROM es_question(nolock) full outer join es_link(nolock) 
		                                on es_question.ques_link = es_link.link_id 
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
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionId = row.IsNull(0) ? 0 : Convert.ToInt32(row[0]);
                        entity.QuestionType = row.IsNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)Convert.ToInt16(row[1]);
                        entity.QuestionShortDescription = row.IsNull(2) ? "" : row[2].ToString();
                        entity.QuestionCreateDate = row.IsNull(3) ? DateTime.MinValue : Convert.ToDateTime(row[3]);
                        entity.QuestionContent = row.IsNull(4) ? "" : row[4].ToString();
                        entity.QuestionItem = row.IsNull(5) ? "" : row[5].ToString();
                        entity.QuestionDifficulty = row.IsNull(6) ? 0 : Convert.ToInt16(row[6]);
                        entity.QuestionAnswer = row.IsNull(7) ? "" : row[7].ToString();
                        entity.QuestionScore = row.IsNull(8) ? 0 : Convert.ToInt16(row[8]);
                        entity.QuestionLinkId = row.IsNull(9) ? (row.IsNull(10) ? -1 : Convert.ToInt32(10)) : Convert.ToInt32(row[9]);
                        entity.QuestionLinkType = row.IsNull(11) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)Convert.ToInt16(row[11]);
                        entity.QuestionLinkCreateDate = row.IsNull(12) ? DateTime.MinValue : Convert.ToDateTime(row[12]);
                        entity.QuestionLinkShortDescription = row.IsNull(13) ? "" : row[13].ToString();
                        entity.QuestionLinkContent = row.IsNull(14) ? "" : row[14].ToString();
                        entity.QuestionLinkDifficulty = row.IsNull(15) ? 0 : Convert.ToInt16(row[15]);
                        entity.QuestionLinkAttachedInfo = row.IsNull(16) ? "" : row[16].ToString();
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        entity.QuestionType = reader.IsDBNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)reader.GetInt16(1);
                        entity.QuestionShortDescription = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        entity.QuestionCreateDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                        entity.QuestionContent = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        entity.QuestionItem = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        entity.QuestionDifficulty = reader.IsDBNull(6) ? 0 : reader.GetInt16(6);
                        entity.QuestionAnswer = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        entity.QuestionScore = reader.IsDBNull(8) ? 0 : reader.GetInt16(8);
                        entity.QuestionLinkId = reader.IsDBNull(9) ? (reader.IsDBNull(10) ? -1 : reader.GetInt32(10)) : reader.GetInt32(9);
                        entity.QuestionLinkType = reader.IsDBNull(11) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)reader.GetInt16(11);
                        entity.QuestionLinkCreateDate = reader.IsDBNull(12) ? DateTime.MinValue : reader.GetDateTime(12);
                        entity.QuestionLinkShortDescription = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        entity.QuestionLinkContent = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        entity.QuestionLinkDifficulty = reader.IsDBNull(15) ? 0 : reader.GetInt16(15);
                        entity.QuestionLinkAttachedInfo = reader.IsDBNull(16) ? "" : reader.GetString(16);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = string.Format(@"
                        select count(isnull(es_question.ques_id, 0)) as total
	                        from es_question(nolock) full outer join es_link(nolock) 
		                        on es_question.ques_link = es_link.link_id 
	                        where {0}
                    ", condition);
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

        #region LinkId Fill
        private bool FillByLinkId(System.Data.SqlClient.SqlCommand command, int linkId)
        {
            SqlDataReader reader = null;
            try
            {
                SqlParameter param = null;
                command.CommandType = CommandType.Text;
                if (this.Entity.PageSize < 1)
                {
                    command.CommandText = @"
                        select es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                    es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                    es_question.ques_score, es_question.ques_link, es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                    es_link.link_text, es_link.link_diff, es_link.link_bak
                            from es_question(nolock) left join es_link(nolock) 
                               on es_question.ques_link = es_link.link_id 
                            where es_question.ques_link = @LinkId 
                            order by es_question.ques_id desc
                       ";
                }
                else
                {
                    command.CommandText = @"
                        SELECT T.ques_id, T.ques_type, T.ques_short, T.ques_data, T.ques_text, T.ques_item, T.ques_diff, T.ques_answer,
			                    T.ques_score, T.ques_link, T.link_id, T.link_type, T.link_data, T.link_short, T.link_text, T.link_diff, T.link_bak
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY es_question.ques_id DESC) AS ROW_NUM,
		                                es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                            es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                            es_question.ques_score, es_question.ques_link, es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                            es_link.link_text, es_link.link_diff, es_link.link_bak
	                                FROM es_question(nolock) left join es_link(nolock) 
		                                on es_question.ques_link = es_link.link_id
	                                WHERE es_question.ques_link = @LinkId
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
                param = new SqlParameter("@LinkId", SqlDbType.Int);
                param.Value = linkId;
                command.Parameters.Add(param);
                this.Entity.Clear();
                if (this.Entity.IsReturnDataTable)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable fillDataTable = new DataTable();
                    adapter.Fill(fillDataTable);
                    foreach (DataRow row in fillDataTable.Rows)
                    {
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionId = row.IsNull(0) ? 0 : Convert.ToInt32(row[0]);
                        entity.QuestionType = row.IsNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)Convert.ToInt16(row[1]);
                        entity.QuestionShortDescription = row.IsNull(2) ? "" : row[2].ToString();
                        entity.QuestionCreateDate = row.IsNull(3) ? DateTime.MinValue : Convert.ToDateTime(row[3]);
                        entity.QuestionContent = row.IsNull(4) ? "" : row[4].ToString();
                        entity.QuestionItem = row.IsNull(5) ? "" : row[5].ToString();
                        entity.QuestionDifficulty = row.IsNull(6) ? 0 : Convert.ToInt16(row[6]);
                        entity.QuestionAnswer = row.IsNull(7) ? "" : row[7].ToString();
                        entity.QuestionScore = row.IsNull(8) ? 0 : Convert.ToInt16(row[8]);
                        entity.QuestionLinkId = row.IsNull(9) ? (row.IsNull(10) ? -1 : Convert.ToInt32(10)) : Convert.ToInt32(row[9]);
                        entity.QuestionLinkType = row.IsNull(11) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)Convert.ToInt16(row[11]);
                        entity.QuestionLinkCreateDate = row.IsNull(12) ? DateTime.MinValue : Convert.ToDateTime(row[12]);
                        entity.QuestionLinkShortDescription = row.IsNull(13) ? "" : row[13].ToString();
                        entity.QuestionLinkContent = row.IsNull(14) ? "" : row[14].ToString();
                        entity.QuestionLinkDifficulty = row.IsNull(15) ? 0 : Convert.ToInt16(row[15]);
                        entity.QuestionLinkAttachedInfo = row.IsNull(16) ? "" : row[16].ToString();
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        entity.QuestionType = reader.IsDBNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)reader.GetInt16(1);
                        entity.QuestionShortDescription = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        entity.QuestionCreateDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                        entity.QuestionContent = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        entity.QuestionItem = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        entity.QuestionDifficulty = reader.IsDBNull(6) ? 0 : reader.GetInt16(6);
                        entity.QuestionAnswer = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        entity.QuestionScore = reader.IsDBNull(8) ? 0 : reader.GetInt16(8);
                        entity.QuestionLinkId = reader.IsDBNull(9) ? (reader.IsDBNull(10) ? -1 : reader.GetInt32(10)) : reader.GetInt32(9);
                        entity.QuestionLinkType = reader.IsDBNull(11) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)reader.GetInt16(11);
                        entity.QuestionLinkCreateDate = reader.IsDBNull(12) ? DateTime.MinValue : reader.GetDateTime(12);
                        entity.QuestionLinkShortDescription = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        entity.QuestionLinkContent = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        entity.QuestionLinkDifficulty = reader.IsDBNull(15) ? 0 : reader.GetInt16(15);
                        entity.QuestionLinkAttachedInfo = reader.IsDBNull(16) ? "" : reader.GetString(16);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = @"
                        select count(isnull(es_question.ques_id, 0)) as total
	                        from es_question(nolock) left join es_link(nolock)
		                        on es_question.ques_link = es_link.link_id
	                        where es_question.ques_link = @LinkId
                        ";
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

        #region LinkType Fill
        private bool FillByLinkType(System.Data.SqlClient.SqlCommand command, QuestionLinkTypeEnum linkType)
        {
            SqlDataReader reader = null;
            try
            {
                SqlParameter param = null;
                command.CommandType = CommandType.Text;
                if (this.Entity.PageSize < 1)
                {
                    command.CommandText = @"
                        select es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                    es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                    es_question.ques_score, es_question.ques_link, es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                    es_link.link_text, es_link.link_diff, es_link.link_bak
                            from es_link(nolock) left join es_question(nolock) 
                               on es_question.ques_link = es_link.link_id 
                            where es_link.link_type = @LinkType
                            order by es_question.ques_id desc
                       ";
                }
                else
                {
                    command.CommandText = @"
                        SELECT T.ques_id, T.ques_type, T.ques_short, T.ques_data, T.ques_text, T.ques_item, T.ques_diff, T.ques_answer,
			                    T.ques_score, T.ques_link, T.link_id, T.link_type, T.link_data, T.link_short, T.link_text, T.link_diff, T.link_bak
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY es_question.ques_id DESC) AS ROW_NUM,
		                                es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                            es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                            es_question.ques_score, es_question.ques_link, es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                            es_link.link_text, es_link.link_diff, es_link.link_bak
	                                FROM es_link(nolock) left join es_question(nolock) 
		                                on es_question.ques_link = es_link.link_id
	                                WHERE es_link.link_type = @LinkType
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
                param = new SqlParameter("@LinkType", SqlDbType.SmallInt);
                param.Value = (int)linkType;
                command.Parameters.Add(param);
                this.Entity.Clear();
                if (this.Entity.IsReturnDataTable)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable fillDataTable = new DataTable();
                    adapter.Fill(fillDataTable);
                    foreach (DataRow row in fillDataTable.Rows)
                    {
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionId = row.IsNull(0) ? 0 : Convert.ToInt32(row[0]);
                        entity.QuestionType = row.IsNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)Convert.ToInt16(row[1]);
                        entity.QuestionShortDescription = row.IsNull(2) ? "" : row[2].ToString();
                        entity.QuestionCreateDate = row.IsNull(3) ? DateTime.MinValue : Convert.ToDateTime(row[3]);
                        entity.QuestionContent = row.IsNull(4) ? "" : row[4].ToString();
                        entity.QuestionItem = row.IsNull(5) ? "" : row[5].ToString();
                        entity.QuestionDifficulty = row.IsNull(6) ? 0 : Convert.ToInt16(row[6]);
                        entity.QuestionAnswer = row.IsNull(7) ? "" : row[7].ToString();
                        entity.QuestionScore = row.IsNull(8) ? 0 : Convert.ToInt16(row[8]);
                        entity.QuestionLinkId = row.IsNull(9) ? (row.IsNull(10) ? -1 : Convert.ToInt32(10)) : Convert.ToInt32(row[9]);
                        entity.QuestionLinkType = row.IsNull(11) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)Convert.ToInt16(row[11]);
                        entity.QuestionLinkCreateDate = row.IsNull(12) ? DateTime.MinValue : Convert.ToDateTime(row[12]);
                        entity.QuestionLinkShortDescription = row.IsNull(13) ? "" : row[13].ToString();
                        entity.QuestionLinkContent = row.IsNull(14) ? "" : row[14].ToString();
                        entity.QuestionLinkDifficulty = row.IsNull(15) ? 0 : Convert.ToInt16(row[15]);
                        entity.QuestionLinkAttachedInfo = row.IsNull(16) ? "" : row[16].ToString();
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        entity.QuestionType = reader.IsDBNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)reader.GetInt16(1);
                        entity.QuestionShortDescription = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        entity.QuestionCreateDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                        entity.QuestionContent = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        entity.QuestionItem = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        entity.QuestionDifficulty = reader.IsDBNull(6) ? 0 : reader.GetInt16(6);
                        entity.QuestionAnswer = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        entity.QuestionScore = reader.IsDBNull(8) ? 0 : reader.GetInt16(8);
                        entity.QuestionLinkId = reader.IsDBNull(9) ? (reader.IsDBNull(10) ? -1 : reader.GetInt32(10)) : reader.GetInt32(9);
                        entity.QuestionLinkType = reader.IsDBNull(11) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)reader.GetInt16(11);
                        entity.QuestionLinkCreateDate = reader.IsDBNull(12) ? DateTime.MinValue : reader.GetDateTime(12);
                        entity.QuestionLinkShortDescription = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        entity.QuestionLinkContent = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        entity.QuestionLinkDifficulty = reader.IsDBNull(15) ? 0 : reader.GetInt16(15);
                        entity.QuestionLinkAttachedInfo = reader.IsDBNull(16) ? "" : reader.GetString(16);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = @"
                        select count(isnull(es_question.ques_id, 0)) as total
	                        from es_link(nolock) left join es_question(nolock)
		                        on es_question.ques_link = es_link.link_id
	                        where es_link.link_type = @LinkType
                        ";
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

        #region QuestionType Fill
        private bool FillByQuestionType(System.Data.SqlClient.SqlCommand command, QuestionTypeEnum questionType)
        {
            SqlDataReader reader = null;
            try
            {
                SqlParameter param = null;
                command.CommandType = CommandType.Text;
                if (this.Entity.PageSize < 1)
                {
                    command.CommandText = @"
                        select es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                    es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                    es_question.ques_score, es_question.ques_link, es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                    es_link.link_text, es_link.link_diff, es_link.link_bak
                            from es_question(nolock) left join es_link(nolock)
                               on es_question.ques_link = es_link.link_id 
                            where es_question.ques_link = 0 and es_question.ques_type = @QuestionType
                            order by es_question.ques_id desc
                       ";
                }
                else
                {
                    command.CommandText = @"
                        SELECT T.ques_id, T.ques_type, T.ques_short, T.ques_data, T.ques_text, T.ques_item, T.ques_diff, T.ques_answer,
			                    T.ques_score, T.ques_link, T.link_id, T.link_type, T.link_data, T.link_short, T.link_text, T.link_diff, T.link_bak
                            FROM (
	                            SELECT ROW_NUMBER() OVER(ORDER BY es_question.ques_id DESC) AS ROW_NUM,
		                                es_question.ques_id, es_question.ques_type, es_question.ques_short, es_question.ques_data,
			                            es_question.ques_text, es_question.ques_item, es_question.ques_diff, es_question.ques_answer,
			                            es_question.ques_score, es_question.ques_link, es_link.link_id, es_link.link_type, es_link.link_data, es_link.link_short,
			                            es_link.link_text, es_link.link_diff, es_link.link_bak
	                                FROM es_question(nolock) left join es_link(nolock)
		                                on es_question.ques_link = es_link.link_id
	                                WHERE es_question.ques_link = 0 and es_question.ques_type = @QuestionType
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
                param = new SqlParameter("@QuestionType", SqlDbType.SmallInt);
                param.Value = (int)questionType;
                command.Parameters.Add(param);
                this.Entity.Clear();
                if (this.Entity.IsReturnDataTable)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable fillDataTable = new DataTable();
                    adapter.Fill(fillDataTable);
                    foreach (DataRow row in fillDataTable.Rows)
                    {
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionId = row.IsNull(0) ? 0 : Convert.ToInt32(row[0]);
                        entity.QuestionType = row.IsNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)Convert.ToInt16(row[1]);
                        entity.QuestionShortDescription = row.IsNull(2) ? "" : row[2].ToString();
                        entity.QuestionCreateDate = row.IsNull(3) ? DateTime.MinValue : Convert.ToDateTime(row[3]);
                        entity.QuestionContent = row.IsNull(4) ? "" : row[4].ToString();
                        entity.QuestionItem = row.IsNull(5) ? "" : row[5].ToString();
                        entity.QuestionDifficulty = row.IsNull(6) ? 0 : Convert.ToInt16(row[6]);
                        entity.QuestionAnswer = row.IsNull(7) ? "" : row[7].ToString();
                        entity.QuestionScore = row.IsNull(8) ? 0 : Convert.ToInt16(row[8]);
                        entity.QuestionLinkId = row.IsNull(9) ? (row.IsNull(10) ? -1 : Convert.ToInt32(10)) : Convert.ToInt32(row[9]);
                        entity.QuestionLinkType = row.IsNull(11) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)Convert.ToInt16(row[11]);
                        entity.QuestionLinkCreateDate = row.IsNull(12) ? DateTime.MinValue : Convert.ToDateTime(row[12]);
                        entity.QuestionLinkShortDescription = row.IsNull(13) ? "" : row[13].ToString();
                        entity.QuestionLinkContent = row.IsNull(14) ? "" : row[14].ToString();
                        entity.QuestionLinkDifficulty = row.IsNull(15) ? 0 : Convert.ToInt16(row[15]);
                        entity.QuestionLinkAttachedInfo = row.IsNull(16) ? "" : row[16].ToString();
                        this.Entity.AddItem(entity);
                    }
                    this.Entity.fillDataTable = fillDataTable;
                }
                else
                {
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        QuestionEntity entity = new QuestionEntity();
                        entity.QuestionId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        entity.QuestionType = reader.IsDBNull(1) ? QuestionTypeEnum.Nothing : (QuestionTypeEnum)reader.GetInt16(1);
                        entity.QuestionShortDescription = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        entity.QuestionCreateDate = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                        entity.QuestionContent = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        entity.QuestionItem = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        entity.QuestionDifficulty = reader.IsDBNull(6) ? 0 : reader.GetInt16(6);
                        entity.QuestionAnswer = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        entity.QuestionScore = reader.IsDBNull(8) ? 0 : reader.GetInt16(8);
                        entity.QuestionLinkId = reader.IsDBNull(9) ? (reader.IsDBNull(10) ? -1 : reader.GetInt32(10)) : reader.GetInt32(9);
                        entity.QuestionLinkType = reader.IsDBNull(11) ? QuestionLinkTypeEnum.Nothing : (QuestionLinkTypeEnum)reader.GetInt16(11);
                        entity.QuestionLinkCreateDate = reader.IsDBNull(12) ? DateTime.MinValue : reader.GetDateTime(12);
                        entity.QuestionLinkShortDescription = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        entity.QuestionLinkContent = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        entity.QuestionLinkDifficulty = reader.IsDBNull(15) ? 0 : reader.GetInt16(15);
                        entity.QuestionLinkAttachedInfo = reader.IsDBNull(16) ? "" : reader.GetString(16);
                        this.Entity.AddItem(entity);
                    }
                    if (reader != null)
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                command.CommandText = @"
                        select count(isnull(es_question.ques_id, 0)) as total
	                        from es_question(nolock) left join es_link(nolock)
		                        on es_question.ques_link = es_link.link_id
	                        where es_question.ques_link = 0 and es_question.ques_type = @QuestionType
                        ";
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

        #region QuestionIds Deleted
        private bool DeleteByQuestionIds(System.Data.SqlClient.SqlCommand command, int[] questionIds)
        {
            if (questionIds == null || questionIds.Length == 0)
                return true;
            StringBuilder delIds = null;
            foreach (int questionId in questionIds)
            {
                if (delIds == null)
                    delIds = new StringBuilder(questionId.ToString());
                else
                    delIds.AppendFormat(",{0}", questionId);
            }
            if (delIds == null)
                return true;
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format("DELETE FROM es_question WHERE ques_id In ({0})", delIds.ToString());
            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }
        #endregion

        #region LinkIds Deleted
        private bool DeleteByLinkIds(System.Data.SqlClient.SqlCommand command, int[] linkIds)
        {
            if (linkIds == null || linkIds.Length == 0)
                return true;
            StringBuilder delIds = null;
            foreach (int linkId in linkIds)
            {
                if (delIds == null)
                    delIds = new StringBuilder(linkId.ToString());
                else
                    delIds.AppendFormat(",{0}", linkId);
            }
            if (delIds == null)
                return true;
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format(@"
                        DELETE FROM es_question WHERE ques_link In ({0});
                        DELETE FROM es_link WHERE link_id In ({0})
                    ", delIds.ToString());
            int affected = command.ExecuteNonQuery();
            return (affected > 0);
        }
        #endregion

    }
}
