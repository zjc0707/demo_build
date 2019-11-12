namespace Jc.SqlTool.Core.Helper
{
    using Metadata.DataInfo;
    using System.Collections.Generic;
    using System;
    public class TableInfoHelper
    {
        private static Dictionary<string, TableInfo> cache = new Dictionary<string, TableInfo>();
        public static TableInfo GetTableInfo(Type type)
        {
            if (!cache.ContainsKey(type.Name))
            {
                cache.Add(type.Name, new TableInfo(type));
            }
            return cache[type.Name];
        }
        public static TableInfo GetTableInfo(string key)
        {
            return cache[key];
        }
    }
}