using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PanelControllerItem
{
    public string Name { get; set; }
    public string Url { get; set; }
    public PanelControllerItem(string name, string url)
    {
        Name = name;
        Url = url;
    }
}
