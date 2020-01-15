using System;
using UnityEngine;
public static class WebUtil
{
    public const string HOST = "http://127.0.0.1:4567/unity/";
    // public const string HOST = "http://47.102.133.53:4567/unity";
    #region model相关操作
    public static void FindModelList(Action<string> success, Action failure, long typeId = 0)
    {
        MyWebRequest.current.Get(HOST + "model/findList?typeId=" + typeId, success, failure);
    }
    public static void FindModelById(Action<string> success, long id)
    {
        MyWebRequest.current.Get(HOST + "model/findById?id=" + id, success);
    }
    public static void FindModelTypeList(Action<string> success)
    {
        MyWebRequest.current.Get(HOST + "modelType/findList", success);
    }
    #endregion

    #region scene相关操作
    public static void Save(Scene scene, Action<string> success)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", scene.Name);
        form.AddBinaryData("content", scene.Content);
        form.AddField("deployTime", Convert.ToString(scene.DeployTime));
        MyWebRequest.current.Post(HOST + "scene/save", form, success);
    }
    public static void DetailSence(int id, Action<string> success)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        MyWebRequest.current.Post(HOST + "scene/detail", form, success, null, false);
    }
    public static void PageSence(int startIndex, Action<string> success)
    {
        PageSence(startIndex, 5, success);
    }
    public static void PageSence(int startIndex, int pageSize, Action<string> success)
    {
        WWWForm form = new WWWForm();
        form.AddField("startIndex", startIndex);
        form.AddField("pageSize", pageSize);
        MyWebRequest.current.Post(HOST + "scene/page", form, success);
    }
    #endregion
}