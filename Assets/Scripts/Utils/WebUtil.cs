using System;
using UnityEngine;
public static class WebUtil
{
    private const string HOST = "http://127.0.0.1:4567/unity";
    public static void DetailSence(int id, Action<string> action)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        MyWebRequest.current.Post(HOST + "/scene/detail", form, action, false);
    }
    public static void PageSence(int startIndex, Action<string> action)
    {
        PageSence(startIndex, 5, action);
    }
    public static void PageSence(int startIndex, int pageSize, Action<string> action)
    {
        WWWForm form = new WWWForm();
        form.AddField("startIndex", startIndex);
        form.AddField("pageSize", pageSize);
        MyWebRequest.current.Post(HOST + "/scene/page", form, action);
    }
}