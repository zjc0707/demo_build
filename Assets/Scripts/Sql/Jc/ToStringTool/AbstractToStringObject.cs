namespace Jc.ToStringTool
{
    using System;
    using System.Text;
    using System.Reflection;
    using UnityEngine;
    /// <summary>
    /// 通过反射自动实现ToString()
    /// </summary>
    public abstract class AbstractToStringObject
    {
        public override string ToString()
        {
            PropertyInfo[] infos = this.GetType().GetProperties();
            StringBuilder sb = new StringBuilder(50);
            Deep(sb, this, null, false);
            return sb.ToString();
        }
        private void Deep(StringBuilder sb, object value, PropertyInfo propertyInfo, bool endWithComma)
        {
            if (value == null || StringUtil.IsNullOrEmpty(value))
            {
                if (!endWithComma && sb.Length >= 2)
                {
                    sb.Remove(sb.Length - 2, 2);
                }
                return;
            }
            Type type = value.GetType();
            Debug.Log(type.Name);
            if (propertyInfo != null)
            {
                sb.AppendFormat("\"{0}({1})\"=", propertyInfo.Name, propertyInfo.PropertyType.Name);
            }
            if (!StringUtil.IsBulitinType(type))
            {
                sb.Append("{");
                PropertyInfo[] propertyInfos = type.GetProperties();
                int count = propertyInfos.Length;
                for (int i = 0; i < count; ++i)
                {
                    PropertyInfo info = propertyInfos[i];
                    object o = info.GetValue(value, null);
                    Deep(sb, o, info, i < count - 1);
                }
                sb.Append("}");
            }
            else if (type.IsArray)
            {
                sb.Append("[");
                Array array = (Array)value;
                for (int i = 0; i < array.Length; ++i)
                {
                    Deep(sb, array.GetValue(i), null, i < array.Length - 1);
                }
                sb.Append("]");
            }
            else if (type.IsGenericType)
            {
                sb.Append("[");
                int count = Convert.ToInt32(type.GetProperty("Count").GetValue(value, null));
                for (int i = 0; i < count; ++i)
                {
                    object item = type.GetProperty("Item").GetValue(value, new object[] { i });
                    Deep(sb, item, null, i < count - 1);
                }
                sb.Append("]");
            }
            else
            {
                sb.Append(value);
            }
            if (endWithComma)
            {
                sb.Append(",");
            }
        }

    }
}