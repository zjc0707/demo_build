namespace Jc.SqlTool.Core.MyCommand.Helper
{
    using System.Collections.Generic;
    using Metadata.SqlInfo;
    public static class ParameterHelper
    {
        public static Parameter Parse(WhereInfo whereInfo)
        {
            return new Parameter()
            {
                Param = "@" + whereInfo.Column,
                Value = whereInfo.Value
            };
        }
        public static List<Parameter> Parse(List<WhereInfo> whereInfos)
        {
            List<Parameter> rs = new List<Parameter>();
            if (whereInfos != null)
            {
                foreach (WhereInfo wi in whereInfos)
                {
                    rs.Add(Parse(wi));
                }
            }
            return rs;
        }
    }
}