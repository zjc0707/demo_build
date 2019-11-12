namespace Jc.SqlTool.Core.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class Id : System.Attribute
    {
        public string Value { get; set; }
        public Id()
        {

        }
        public Id(string value)
        {
            Value = value;
        }
    }
}