// using System;
// using System.Collections.Generic;
// public static class IOCUtil
// {
//     private static Dictionary<string, object> dic = new Dictionary<string, object>();
//     public static T Find<T>() where T : class
//     {
//         Type type = typeof(T);
//         if (!dic.ContainsKey(type.Name))
//         {
//             dic.Add(type.Name, Activator.CreateInstance<T>());
//         }
//         return dic[type.Name] as T;
//     }
// }