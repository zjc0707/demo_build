namespace Jc.SqlTool.Core.MyCommand
{
    using Core.Base;
    public class Parameter : AbstractObject
    {
        public string Param { get; set; }
        public object Value { get; set; }
    }
}