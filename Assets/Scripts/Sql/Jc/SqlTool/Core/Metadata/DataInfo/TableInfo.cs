namespace Jc.SqlTool.Core.Metadata.DataInfo
{
    using System.Text;
    using System.Reflection;
    using System;
    using System.Collections.Generic;
    using Attribute;
    public class TableInfo
    {
        public string TableName { get; set; }
        public List<TableFieldInfo> TableFieldInfoList { get; set; }
        public string AllSqlSelect { get; set; }
        public TableInfo(Type type)
        {
            this.TableName = StringUtil.CamelToUnderline(type.Name);
            this.TableFieldInfoList = new List<TableFieldInfo>();
            StringBuilder sb = new StringBuilder(50);
            bool existId = false;
            foreach (PropertyInfo info in type.GetProperties())
            {
                string column = StringUtil.CamelToUnderline(info.Name);
                Id id = info.GetCustomAttribute<Id>(true);
                if (id != null)
                {
                    //找到Id并置顶
                    existId = true;
                    this.TableFieldInfoList.Insert(0, new TableFieldInfo(column, info.Name));
                    sb.Insert(0, string.Format("{0},", string.IsNullOrEmpty(id.Value) ? column : id.Value));
                }
                else
                {
                    this.TableFieldInfoList.Add(new TableFieldInfo(column, info.Name));
                    sb.AppendFormat("{0},", column);
                }
            }
            if (!existId)
            {
                //必须要求有Id项，便于updateById等操作
                throw new Exception("实体类不存在主键映射属性");
            }
            this.AllSqlSelect = sb.Remove(sb.Length - 1, 1).ToString();
        }
        public TableInfo()
        {

        }
    }
}