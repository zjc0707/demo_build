using System.Text;
using System;
public static class StringUtil
{
    public const int INT_NULL = Int16.MinValue;
    /// <summary>
    /// 通过将类型转为string判断是否为空
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(object o)
    {
        try
        {
            // if (!IsBulitinType(o.GetType())) return o == null;
            if (o == null)
            {
                return true;
            }
            if (!IsBulitinType(o.GetType()))
            {
                //自己定义的!=null
                return false;
            }
            bool flag = string.IsNullOrEmpty(Convert.ToString(o));
            if (flag)
            {
                //空的字符串
                return true;
            }
            return Convert.ToInt16(o) == INT_NULL;
        }
        catch
        {
            //到这里说明转int失败，则为不空的字符串
            return false;
        }
    }
    public static bool IsBulitinType(Type t)
    {
        return t == typeof(object) || Type.GetTypeCode(t) != TypeCode.Object;
    }
    /// <summary>
    /// 对象名首字母小写
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string FirstToLower(string s)
    {
        return s.Substring(0, 1).ToLower() + s.Substring(1);
    }
    public static string FirstToUpper(string s)
    {
        return s.Substring(0, 1).ToUpper() + s.Substring(1);
    }
    public static string UnderlineToCamel(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return "";
        }
        s = FirstToUpper(s);
        int len = s.Length;
        StringBuilder sb = new StringBuilder(len);
        char[] chars = s.ToCharArray();
        bool isUpper = false;
        for (int i = 0; i < len; ++i)
        {
            char c = chars[i];
            if (c.Equals('_'))
            {
                isUpper = true;
                continue;
            }
            sb.Append(isUpper ? Char.ToUpper(c) : c);
            isUpper = false;
        }
        return sb.ToString();
    }
    public static string CamelToUnderline(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return "";
        }
        s = FirstToLower(s);
        int len = s.Length;
        StringBuilder sb = new StringBuilder(len);
        char[] chars = s.ToCharArray();
        for (int i = 0; i < len; ++i)
        {
            char c = chars[i];
            if (Char.IsUpper(c))
            {
                sb.Append("_");
            }
            sb.Append(Char.ToLower(c));
        }
        return sb.ToString();
    }

}