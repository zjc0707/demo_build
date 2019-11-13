namespace Jc.SqlTool.Core.Helper
{
    using MyCommand;
    using Metadata.DataInfo;
    using UnityEngine;
    using System.Collections.Generic;
    using System;
    using System.Data;
    using Access;
    public static class SqlHelper
    {
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
                    object value = row[fieldInfo.Column];

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