using System;
[AttributeUsage(AttributeTargets.Class)]
public class UIType : Attribute
{
    public int value { get; set; }
    public UIType(int value)
    {
        this.value = value;
    }
}