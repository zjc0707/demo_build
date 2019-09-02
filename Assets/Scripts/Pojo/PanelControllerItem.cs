using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PanelControllerItem
{
    public string Name { get; private set; }
    public string Url { get; private set; }
    /// <summary>
    /// 是否生效building的MyUpdate  0-不可操作；1-可操作
    /// </summary>
    public int Operate { get; private set; }
    public PanelControllerItem(string name, string url, int operate)
    {
        this.Name = name;
        this.Url = url;
        this.Operate = operate;
    }
    public PanelControllerItem(string name, string url)
    {
        this.Name = name;
        this.Url = url;
        this.Operate = 0;
    }
}
