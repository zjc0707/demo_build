namespace Jc.SqlTool.Core.Query
{
    using System.Text;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Metadata.DataInfo;
    using Metadata.SqlInfo;
    using MyCommand;
    using Condition;
    using Helper;
    public abstract class AbstractWrapper<T>
    {
        protected T entity;
        protected Type entityType;
        protected TableInfo tableInfo;
        protected string selectStr;
        protected List<WhereInfo> whereInfos = new List<WhereInfo>();
        protected LimitInfo limitInfo = new LimitInfo();
        protected OrderByInfo orderByInfo = new OrderByInfo();
        public MyCommand ToSelect()
        {
            string select = string.IsNullOrEmpty(selectStr) ? tableInfo.AllSqlSelect : selectStr;
            foreach (TableFieldInfo tableFieldInfo in this.tableInfo.TableFieldInfoList)
            {
                object o = entityType.GetProperty(tableFieldInfo.Property).GetValue(entity, null);
                if (!StringUtil.IsNullOrEmpty(o))
                {
                    whereInfos.Add(WhereInfoHelper.Eq(tableFieldInfo.Column, o));
                }
            }
            string text = string.Format(SqlCondition.SELECT, select, tableInfo.TableName, this.GetWhereStr())
                + orderByInfo.ToString()
                + limitInfo.ToString();
            return new MyCommand(text, whereInfos);
        }
        public MyCommand ToInsert()
        {
            foreach (TableFieldInfo tableFieldInfo in this.tableInfo.TableFieldInfoList)
            {
                object o = entityType.GetProperty(tableFieldInfo.Property).GetValue(entity, null);
                if (!StringUtil.IsNullOrEmpty(o))
                {
                    this.whereInfos.Add(WhereInfoHelper.Eq(tableFieldInfo.Column, o));
                }
            }
            if (this.whereInfos.Count == 0)
            {
                throw new Exception("新增操作数据不得为空");
            }
            StringBuilder sbColumn = new StringBuilder(50);
            StringBuilder sbParam = new StringBuilder(50);
            foreach (WhereInfo whereInfo in whereInfos)
            {
                sbColumn.AppendFormat("{0},", whereInfo.Column);
                sbParam.AppendFormat("@{0},", whereInfo.Column);
            }
            string text = string.Format(SqlCondition.INSERT,
                tableInfo.TableName,
                sbColumn.Remove(sbColumn.Length - 1, 1).ToString(),
                sbParam.Remove(sbParam.Length - 1, 1).ToString());
            return new MyCommand(text, whereInfos);
        }
        public MyCommand ToUpdate()
        {
            List<WhereInfo> valueList = new List<WhereInfo>();
            foreach (TableFieldInfo tableFieldInfo in this.tableInfo.TableFieldInfoList)
            {
                object o = entityType.GetProperty(tableFieldInfo.Property).GetValue(entity, null);
                if (!StringUtil.IsNullOrEmpty(o))
                {
                    valueList.Add(WhereInfoHelper.Eq(tableFieldInfo.Column, o));
                }
            }
            if (valueList.Count == 0 || whereInfos.Count == 0)
            {
                throw new Exception("更新操作数据不得为空");
            }
            string text = string.Format(SqlCondition.UPDATE, tableInfo.TableName, string.Join(", ", valueList), this.GetWhereStr());
            this.whereInfos.AddRange(valueList);
            return new MyCommand(text, this.whereInfos);
        }
        public MyCommand ToUpdateById()
        {
            TableFieldInfo idInfo = this.tableInfo.TableFieldInfoList[0];
            PropertyInfo propertyInfo = entityType.GetProperty(idInfo.Property);
            object o = propertyInfo.GetValue(entity, null);
            if (!StringUtil.IsNullOrEmpty(o))
            {
                throw new Exception("ToUpdateById操作id需要有id");
            }
            whereInfos.Clear();
            whereInfos.Add(WhereInfoHelper.Eq(idInfo.Column, o));
            propertyInfo.SetValue(entity, StringUtil.INT_NULL);
            return this.ToUpdate();
        }
        public MyCommand ToDelete()
        {
            foreach (TableFieldInfo tableFieldInfo in this.tableInfo.TableFieldInfoList)
            {
                object o = entityType.GetProperty(tableFieldInfo.Property).GetValue(entity, null);
                if (!StringUtil.IsNullOrEmpty(o))
                {
                    whereInfos.Add(WhereInfoHelper.Eq(tableFieldInfo.Column, o));
                }
            }
            string text = string.Format(SqlCondition.DELETE, tableInfo.TableName, this.GetWhereStr());
            return new MyCommand(text, whereInfos);
        }
        private string GetWhereStr()
        {
            if (whereInfos.Count == 0)
            {
                return "1 = 1";
            }
            return string.Join(" AND ", whereInfos);
        }
    }
}