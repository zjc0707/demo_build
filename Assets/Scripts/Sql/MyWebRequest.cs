using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class MyWebRequest : BaseUniqueObject<MyWebRequest>
{
    /// <summary>
    /// seconds
    /// </summary>
    private const int TIMEOUT = 10;
    public void Post(string url, WWWForm form, Action<string> action, bool closeLoadingBeforeAction = true)
    {
        StartCoroutine(_Post(url, form, action, closeLoadingBeforeAction));
    }
    IEnumerator _Post(string url, WWWForm form, Action<string> action, bool closeLoadingBeforeAction)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
        webRequest.timeout = TIMEOUT;
        webRequest.SendWebRequest();
        while (!webRequest.isDone)
        {
            PanelLoading.current.WebLoading();
            yield return 1;
        }
        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            PanelLoading.current.Error(webRequest.error);
            Debug.Log(webRequest.error);
        }
        else
        {
            if (closeLoadingBeforeAction)
            {
                PanelLoading.current.Close();
            }
            if (action != null)
            {
                action(webRequest.downloadHandler.text);
            }
        }
    }
    IEnumerator Page()
    {
        WWWForm form = new WWWForm();
        form.AddField("startIndex", 0);
        form.AddField("pageSize", 5);
        UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:4567/unity/scene/page", form);
        webRequest.timeout = 10;
        webRequest.SendWebRequest();
        while (!webRequest.isDone)
        {
            // Debug.Log(webRequest.downloadProgress);
            PanelLoading.current.Open();
            yield return 1;
        }
        PanelLoading.current.Close();
        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            Page<Scene> page = Json.Parse<Page<Scene>>(webRequest.downloadHandler.text);
            Debug.Log(page);
            string str = System.Text.Encoding.UTF8.GetString(page.Records[0].Content);
            Debug.Log(str);
        }
    }
}