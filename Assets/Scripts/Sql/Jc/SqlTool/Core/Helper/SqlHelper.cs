namespace Jc.SqlTool.Core.Helper
{
    using MyCommand;
    using Metadata.DataInfo;
    using UnityEngine;
    using System.Collections.Generic;
    using System;
    using System.Data;
    using Page;
    using Condition;
    using Access;
    public static class SqlHelper
    {
        public static Page<T> GetResult<T>(MyCommand myCmd, Page<T> page)
        {
            page.Count = Count<T>();
            if (page.StartIndex * page.PageCount > page.Count)
            {
                throw new Exception("page页数超出");
            }
            page.Data = GetResult<T>(myCmd);
            return page;
        }
        public static int Count<T>()
        {
            TableInfo tableInfo = TableInfoHelper.GetTableInfo(typeof(T));
            string sql = string.Format(SqlCondition.COUNT, tableInfo.Key.Column, tableInfo.TableName);
            MyCommand myCommand = new MyCommand(sql, null);
            DataTable dataTable = GetSqlAccess().ExecuteQuery(myCommand);
            return Convert.ToInt32(dataTable.Rows[0][0].ToString());
        }
        public static List<T> GetResult<T>(MyCommand myCmd)
        {
            List<T> rs = new List<T>();
            DataTable dataTable = GetSqlAccess().ExecuteQuery(myCmd);
            Type type = typeof(T);
            TableInfo tableInfo = TableInfoHelper.GetTableInfo(type);
            foreach (DataRow row in dataTable.Rows)
            {
                T entity = System.Activator.CreateInstance<T>();
                foreach (TableFieldInfo fieldInfo in tableInfo.TableFieldInfoList)
                {
                    object value = DBNull.Value;
                    try
                    {
                        value = row[fieldInfo.Column];
                    }
                    // catch (ArgumentException e)
                    catch
                    {
                        // Debug.Log(e.ToString());
                        continue;
                    }

                    if (value.GetType().Equals(typeof(DBNull)))
                    {
                        continue;
                    }
                    // Debug.Log("注入:" + fieldInfo.Column + ":[" + value + "]");
                    type.GetProperty(fieldInfo.Property).SetValue(entity, value);
                }
                rs.Add(entity);
            }
            return rs;
        }
        public static bool GetNonQueryResult(MyCommand myCmd)
        {
            return GetSqlAccess().ExecuteNonQuery(myCmd);
        }
        public static SqlAccess GetSqlAccess()
        {
            return new SqlAccess();
        }
    }
}