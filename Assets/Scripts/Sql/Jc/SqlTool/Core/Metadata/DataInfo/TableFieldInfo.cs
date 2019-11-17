namespace Jc.SqlTool.Core.Metadata.DataInfo
{
    public class TableFieldInfo
    {
        public string Column { get; set; }
        public string Property { get; set; }
        public bool IsKey { get; set; }
        public TableFieldInfo(string Column, string Property)
        {
            this.Column = Column;
            this.Property = Property;
            this.IsKey = false;
        }
        public TableFieldInfo(string Column, string Property, bool IsKey) : this(Column, Property)
        {
            this.IsKey = IsKey;
        }
    }
}