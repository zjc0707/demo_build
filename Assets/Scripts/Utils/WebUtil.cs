using System.Collections.Generic;
using System;
using UnityEngine;
public static class WebUtil
{
    public const string HOST = "http://127.0.0.1:4567/unity/";
    // public const string HOST = "http://47.102.133.53:4567/unity";
    #region model相关操作
    public static void FindModelList(Action<List<Model>> success, Action<string> failure)
    {
        MyWebRequest.current.Get(HOST + "model/findList", success, failure);
    }
    public static void FindModelById(long id, Action<Model> success, Action<string> failure)
    {
        MyWebRequest.current.Get(HOST + "model/findById?id=" + id, success, failure);
    }
    public static void FindModelTypeList(Action<List<ModelType>> success, Action<string> failure)
    {
        MyWebRequest.current.Get(HOST + "modelType/findList", success, failure);
    }
    #endregion

    #region scene相关操作
    public static void Save(Scene scene, Action<string> success, Action<string> failure)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", scene.Name);
        form.AddBinaryData("content", scene.Content);
        form.AddField("deployTime", Convert.ToString(scene.DeployTime));
        MyWebRequest.current.Post(HOST + "scene/save", form, success, failure);
    }
    public static void DetailSence(int id, Action<Scene> success, Action<string> failure)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        MyWebRequest.current.Post(HOST + "scene/detail", form, success, failure, false);
    }
    public static void PageSence(int startIndex, Action<Page<Scene>> success, Action<string> failure)
    {
        PageSence(startIndex, 5, success, failure);
    }
    public static void PageSence(int startIndex, int pageSize, Action<Page<Scene>> success, Action<string> failure)
    {
        WWWForm form = new WWWForm();
        form.AddField("startIndex", startIndex);
        form.AddField("pageSize", pageSize);
        MyWebRequest.current.Post(HOST + "scene/page", form, success, failure);
    }
    #endregion
}