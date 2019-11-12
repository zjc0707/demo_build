namespace Jc.SqlTool.Core.MyCommand
{
    using Core.Base;
    using Core.Metadata.SqlInfo;
    using System.Collections.Generic;
    using Helper;
    public class MyCommand : AbstractObject
    {
        public string CommandText { get; set; }
        public List<Parameter> Parameters { get; set; }

        public MyCommand()
        {

        }
        public MyCommand(string text, List<WhereInfo> whereInfos)
        {
            this.CommandText = text;
            this.Parameters = ParameterHelper.Parse(whereInfos);
        }
    }
}