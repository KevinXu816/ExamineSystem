using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Text;

namespace ExamineSystem.action.utility
{
    public static class ActionTaskUtility
    {

        public static string ReturnClientDataArray(DataTable table)
        {
            return ReturnClientDataArray(table, false);
        }

        public static string ReturnClientDataArray(DataTable table, bool isOther)
        {
            string arrayName = (isOther) ? "SDataArray" : "DataArray";
            StringBuilder clientData = new StringBuilder(arrayName + " = [];");
            if (table != null)
            {
                int count = table.Columns.Count;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow row = table.Rows[i];
                    if (i == 0)
                    {
                        clientData = new StringBuilder();
                    }
                    clientData.Append(arrayName + "[" + i.ToString() + "]={");
                    StringBuilder columnData = new StringBuilder();
                    for (int j = 0; j < count; j++)
                    {
                        string columnName = table.Columns[j].ColumnName;
                        if (j == 0)
                        {
                            columnData.Append(columnName);
                            columnData.Append(":'");
                            columnData.Append(row[columnName].ToString().Replace("''", ""));
                            columnData.Append("'");
                        }
                        else
                        {
                            columnData.Append(",");
                            columnData.Append(columnName);
                            columnData.Append(":'");
                            columnData.Append(row[columnName].ToString().Replace("''", ""));
                            columnData.Append("'");
                        }
                    }
                    clientData.Append(columnData.ToString());
                    clientData.Append("};");
                }
            }
            return clientData.ToString();
        }
    }
}