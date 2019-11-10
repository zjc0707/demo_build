using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System;
using System.IO;
using System.Text;

public class Json : MonoBehaviour
{

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sb"></param>
    /// <returns></returns>
    public static T Parse<T>(String sb)
    {
        JsonSerializer serializer = new JsonSerializer();
        var r = new System.IO.StringReader(sb);
        JsonReader reader = new JsonTextReader(r);
        return serializer.Deserialize<T>(reader);
    }


    ///// <summary>
    ///// 序列化
    ///// </summary>
    ///// <param name="o"></param>
    ///// <returns></returns>
    public static string Serialize(System.Object o)
    {
        //JsonSerializer serializer = new JsonSerializer();
        //StringBuilder sb = new StringBuilder();
        //StringWriter sw = new StringWriter(sb);
        //JsonWriter writer = new JsonTextWriter(sw);
        //serializer.Serialize(writer, o);
        //return sb.ToString();
        return JsonConvert.SerializeObject(o, Formatting.None,new JsonSerializerSettings(){ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
    }



}
