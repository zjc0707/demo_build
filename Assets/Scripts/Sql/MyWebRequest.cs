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

}