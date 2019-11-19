using System;
using UnityEngine;
public static class WebUtil
{
    // private const string HOST = "http://127.0.0.1:4567/unity";
    private const string HOST = "http://47.102.133.53:4567/unity";
    public static void Save(Scene scene, Action<string> action)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", scene.Name);
        form.AddBinaryData("content", scene.Content);
        form.AddField("deployTime", Convert.ToString(scene.DeployTime));
        MyWebRequest.current.Post(HOST + "/scene/save", form, action);
    }
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