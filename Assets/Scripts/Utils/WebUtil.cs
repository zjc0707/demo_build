using System;
using UnityEngine;
public static class WebUtil
{
    public const string HOST = "http://127.0.0.1:4567/unity/";
    // public const string HOST = "http://47.102.133.53:4567/unity";
    #region model相关操作
    public static void FindModelList(Action<string> action, long typeId = 0)
    {
        MyWebRequest.current.Get(HOST + "model/findList?typeId=" + typeId, action);
    }
    public static void FindModelById(Action<string> action, long id)
    {
        MyWebRequest.current.Get(HOST + "model/findById?id=" + id, action);
    }
    public static void FindModelTypeList(Action<string> action)
    {
        MyWebRequest.current.Get(HOST + "modelType/findList", action);
    }
    #endregion

    #region scene相关操作
    public static void Save(Scene scene, Action<string> action)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", scene.Name);
        form.AddBinaryData("content", scene.Content);
        form.AddField("deployTime", Convert.ToString(scene.DeployTime));
        MyWebRequest.current.Post(HOST + "scene/save", form, action);
    }
    public static void DetailSence(int id, Action<string> action)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        MyWebRequest.current.Post(HOST + "scene/detail", form, action, false);
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
        MyWebRequest.current.Post(HOST + "scene/page", form, action);
    }
    #endregion
}