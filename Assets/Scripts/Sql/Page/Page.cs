using System.Collections.Generic;
public class Page<T> : AbstractToStringObject
{
    public List<T> Records { get; set; }
    public int Total { get; set; }
    public int Size { get; set; }
    public int Current { get; set; }
    public int Pages { get; set; }
}