using System.Collections.Generic;
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
    public void DownAssetBundle(List<ModelData> datas)
    {
        StartCoroutine(_DownAssetBundle(datas));
    }
    IEnumerator _DownAssetBundle(List<ModelData> datas)
    {
        List<AssetBundle> rs = new List<AssetBundle>();
        Debug.Log("下载资源数目：" + datas.Count);
        float amount = 0f;
        PanelLoading.current.WebLoading();
        for (int i = 0; i < datas.Count; ++i)
        {
            ModelData data = datas[i];
            Debug.Log(data.ABUrl);
            UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(data.ABUrl);
            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            {
                string str = string.Format("已下载{0}%,正在下载：{1}", (int)((amount + webRequest.downloadProgress) / datas.Count * 100), data.Name);
                PanelLoading.current.Progress(amount + webRequest.downloadProgress, datas.Count, str);
                yield return 1;
            }
            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                PanelLoading.current.Error(webRequest.error);
                break;
            }
            else
            {
                amount += 1;
                AssetBundle ab = DownloadHandlerAssetBundle.GetContent(webRequest);
                ab.name = data.Name;
                rs.Add(ab);
            }
        }
        if (rs.Count == datas.Count)
        {
            yield return _LoadAssetBundle(rs);
            PanelLoading.current.Close();
        }
    }
    IEnumerator _LoadAssetBundle(List<AssetBundle> assetBundles)
    {
        Debug.Log("加载资源数目：" + assetBundles.Count);
        float amount = 0f;
        foreach (AssetBundle ab in assetBundles)
        {
            AssetBundleRequest request = ab.LoadAllAssetsAsync();
            while (!request.isDone)
            {
                string str = string.Format("已加载{0}%,正在加载：{1}", (int)((amount + request.progress) / assetBundles.Count * 100), ab.name);
                PanelLoading.current.Progress(amount + request.progress, assetBundles.Count, str);
                yield return 1;
            }
            amount += 1;
            Debug.Log(request.allAssets.Length);
            foreach (UnityEngine.Object obj in request.allAssets)
            {
                Debug.Log(obj.name + "   " + obj.GetType());
            }
        }
    }
    IEnumerator _Post(string url, WWWForm form, Action<string> action, bool closeLoadingBeforeAction)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
        webRequest.timeout = TIMEOUT;
        webRequest.SendWebRequest();
        PanelLoading.current.WebLoading();
        while (!webRequest.isDone)
        {
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