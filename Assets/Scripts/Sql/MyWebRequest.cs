using System.Net;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class MyWebRequest : BaseUniqueObject<MyWebRequest>
{
    private const string PATH_FOLDER_AB_INTERNET = "http://47.102.133.53/AB/";
    public string PATH_FOLDER_AB_LOACL;
    private void Awake()
    {
        this.PATH_FOLDER_AB_LOACL = Application.persistentDataPath + "/assets/";
        if (!Directory.Exists(PATH_FOLDER_AB_LOACL))
        {
            Directory.CreateDirectory(PATH_FOLDER_AB_LOACL);
        }
    }
    /// <summary>
    /// seconds
    /// </summary>
    private const int TIMEOUT = 10;
    public void Post(string url, WWWForm form, Action<string> action, bool closeLoadingBeforeAction = true)
    {
        StartCoroutine(_Post(url, form, action, closeLoadingBeforeAction));
    }
    public void LoadAssetBundle(List<ModelData> internet, List<ModelData> local, Action action)
    {
        StartCoroutine(_LoadAB(internet, local, action));
    }
    IEnumerator _LoadAB(List<ModelData> internet, List<ModelData> local, Action action)
    {
        int amount = 0;
        yield return _DownAssetBundle(internet, i => amount += i);
        yield return _DownAssetBundleLocal(local, i => amount += i);
        if (amount == internet.Count + local.Count)
        {
            List<ModelData> list = new List<ModelData>();
            list.AddRange(internet);
            list.AddRange(local);
            yield return _LoadAssetBundle(list);
            action();
        }
        else
        {
            Debug.Log("资源数目不相等:" + amount + "/" + (internet.Count + local.Count));
        }
    }
    IEnumerator _DownAssetBundle(List<ModelData> datas, Action<int> action)
    {
        Debug.Log("下载资源数目：" + datas.Count);
        float amount = 0f;
        PanelLoading.current.WebLoading();
        for (int i = 0; i < datas.Count; ++i)
        {
            ModelData data = datas[i];
            string url = PATH_FOLDER_AB_INTERNET + data.ABName;
            Debug.Log(url);
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            {
                string str = string.Format("下载网络资源{0}%,正在下载：{1}", (int)((amount + webRequest.downloadProgress) / datas.Count * 100), data.Name);
                PanelLoading.current.Progress(amount + webRequest.downloadProgress, datas.Count, str);
                yield return 1;
            }
            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
                PanelLoading.current.Error(webRequest.error);
                break;
            }
            else
            {
                amount += 1;
                //保存ab包到本地
                string path = PATH_FOLDER_AB_LOACL + data.ABName;
                if (!File.Exists(path))
                {
                    File.Create(PATH_FOLDER_AB_LOACL + data.ABName).Dispose();
                }
                File.WriteAllBytes(path, webRequest.downloadHandler.data);
                data.AssetBundle = AssetBundle.LoadFromMemory(webRequest.downloadHandler.data);
            }
        }
        action((int)amount);
    }
    IEnumerator _DownAssetBundleLocal(List<ModelData> datas, Action<int> action)
    {
        Debug.Log("读取本地资源数目：" + datas.Count);
        float amount = 0f;
        PanelLoading.current.WebLoading();
        for (int i = 0; i < datas.Count; ++i)
        {
            ModelData data = datas[i];
            string url = PATH_FOLDER_AB_LOACL + data.ABName;
            Debug.Log(url);
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(url);
            while (!request.isDone)
            {
                string str = string.Format("读取本地资源{0}%,正在读取：{1}", (int)((amount + request.progress) / datas.Count * 100), data.Name);
                yield return 1;
            }
            amount += 1;
            data.AssetBundle = request.assetBundle;
        }
        action((int)amount);
    }
    IEnumerator _LoadAssetBundle(List<ModelData> datas)
    {
        Debug.Log("加载资源数目：" + datas.Count);
        float amount = 0f;
        foreach (ModelData data in datas)
        {
            AssetBundleRequest request = data.AssetBundle.LoadAllAssetsAsync();
            while (!request.isDone)
            {
                string str = string.Format("已加载{0}%,正在加载：{1}", (int)((amount + request.progress) / datas.Count * 100), data.Name);
                PanelLoading.current.Progress(amount + request.progress, datas.Count, str);
                yield return 1;
            }
            amount += 1;
            foreach (UnityEngine.Object obj in request.allAssets)
            {
                // Debug.Log(obj.name + "   " + obj.GetType());
                if (obj.GetType() == typeof(GameObject))
                {
                    AssetBundleUtil.DicPrefab.Add(data.Id, obj as GameObject);
                }
                if (obj.GetType() == typeof(Sprite))
                {
                    AssetBundleUtil.DicSprite.Add(data.Id, obj as Sprite);
                }
            }
            data.AssetBundle.Unload(false);
            data.AssetBundle = null;
        }
        if (AssetBundleUtil.DicPrefab.Count != datas.Count)
        {
            PanelLoading.current.Error("错误（资源缺失）：" + AssetBundleUtil.DicPrefab.Count + "/" + datas.Count);
        }
        else
        {
            PanelLoading.current.Close();
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