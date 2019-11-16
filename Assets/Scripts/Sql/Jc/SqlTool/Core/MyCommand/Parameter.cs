namespace Jc.SqlTool.Core.MyCommand
{
    using ToStringTool;
    public class Parameter : AbstractToStringObject
    {
        public string Param { get; set; }
        public object Value { get; set; }
    }
}