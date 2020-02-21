using System.Net;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;
public class MyWebRequest : BaseUniqueObject<MyWebRequest>
{
    public string LOCAL_ASSET_PATH;
    private void Awake()
    {
        this.LOCAL_ASSET_PATH = Application.persistentDataPath + "/assets/";
        if (!Directory.Exists(LOCAL_ASSET_PATH))
        {
            Directory.CreateDirectory(LOCAL_ASSET_PATH);
        }
    }
    /// <summary>
    /// seconds
    /// </summary>
    private const int TIMEOUT = 10;
    public void Post<T>(string url, WWWForm form, Action<T> success, Action<string> failure, bool closeLoadingBeforeAction = true) where T : class
    {
        Debug.Log(string.Format("Post: [url: {0}, data: {1}]", url, System.Text.Encoding.UTF8.GetString(form.data)));
        StartCoroutine(IPost(url, form, success, failure, closeLoadingBeforeAction));
    }
    public void Get<T>(string url, Action<T> success, Action<string> failure, bool closeLoadingBeforeAction = true) where T : class
    {
        Debug.Log(string.Format("Get: [url: {0}]", url));
        StartCoroutine(IGet(url, success, failure, closeLoadingBeforeAction));
    }
    public void LoadAssetBundle(List<Model> internet, Manifest local, Action action)
    {
        StartCoroutine(ILoadAB(internet, local, action));
    }
    IEnumerator ILoadAB(List<Model> internet, Manifest local, Action action)
    {
        Debug.Log(string.Format("internet:{0}\nlocal:{1}", Json.Serialize(internet), Json.Serialize(local)));
        if (internet != null && internet.Count > 0)
        {
            int amount = 0;
            //网络下载缺少的资源
            yield return IDownAssetBundle(internet, i => amount += i);
            if (amount != internet.Count)
            {
                yield break;
            }
            //下载完毕后加入localManifest
            local.Models.AddRange(internet);
        }
        //加载ab包
        yield return ILoadAssetBundle(local);
        if (action != null)
        {
            action();
        }
    }
    /// <summary>
    /// 下载网络资源并缓存至本地
    /// </summary>
    IEnumerator IDownAssetBundle(List<Model> datas, Action<int> action)
    {
        Debug.Log("下载资源数目：" + datas.Count);
        float amount = 0f;
        PanelLoading.current.WebLoading();
        foreach (Model data in datas)
        {

            string url = WebUtil.HOST + "fileServer/download/" + data.FileUrlMac;
#if UNITY_STANDALONE_OSX
            url = WebUtil.HOST + "fileServer/download/" + data.FileUrlMac;
#elif UNITY_STANDLONE_WIN
            url = WebUtil.HOST + "fileServer/download/" + data.FileUrlWindows;
#endif
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
                string directoryPath = LOCAL_ASSET_PATH + data.ModelTypeId;
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string filePath = directoryPath + '/' + data.Id;
                Debug.Log((File.Exists(filePath) ? "覆盖:" : "新建:") + filePath);
                File.Create(filePath).Dispose();
                File.WriteAllBytes(filePath, webRequest.downloadHandler.data);
            }
        }
        action((int)amount);
    }
    /// <summary>
    /// 加载本地缓存资源
    /// </summary>
    IEnumerator IDownAssetBundleLocal(Model data, Action<AssetBundle> action)
    {
        PanelLoading.current.WebLoading();
        string url = LOCAL_ASSET_PATH + data.ModelTypeId + '/' + data.Id;
        Debug.Log(url);
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(url);
        while (!request.isDone)
        {
            yield return 1;
        }
        action(request.assetBundle);
    }
    IEnumerator ILoadAssetBundle(Manifest manifest)
    {
        int all = 0;
        float amount = 0f;
        foreach (Model data in manifest.Models)
        {
            AssetBundle assetBundle = null;
            yield return IDownAssetBundleLocal(data, ab => assetBundle = ab);
            AssetBundleRequest request = assetBundle.LoadAllAssetsAsync();
            while (!request.isDone)
            {
                string str = string.Format("类型{0} 已加载{1}%, 正在加载：{2}",
                                     data.ModelTypeId, (int)((amount + request.progress) / manifest.Models.Count * 100), data.Name);
                PanelLoading.current.Progress(amount + request.progress, manifest.Models.Count, str);
                yield return 1;
            }
            amount++;
            all++;
            foreach (UnityEngine.Object obj in request.allAssets)
            {
                if (obj.GetType() == typeof(GameObject))
                {
                    AssetBundleUtil.DicPrefab.Add(data.Id, obj as GameObject);
                }
                if (obj.GetType() == typeof(Sprite))
                {
                    AssetBundleUtil.DicSprite.Add(data.Id, obj as Sprite);
                }
            }
            assetBundle.Unload(false);
        }
        if (AssetBundleUtil.DicPrefab.Count != all)
        {
            PanelLoading.current.Error("错误（资源缺失）：" + AssetBundleUtil.DicPrefab.Count + "/" + all);
        }
        else
        {
            Debug.Log("总资源数目：" + all);
            PanelLoading.current.Close();
        }
    }
    IEnumerator IPost<T>(string url, WWWForm form, Action<T> success, Action<string> failure, bool closeLoadingBeforeAction) where T : class
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
            Debug.Log(webRequest.error);
            if (failure != null)
            {
                failure(webRequest.error);
            }
        }
        else
        {
            if (closeLoadingBeforeAction)
            {
                PanelLoading.current.Close();
            }
            ResultData<object> rsData = Json.Parse<ResultData<object>>(webRequest.downloadHandler.text);
            Debug.Log(webRequest.downloadHandler.text);
            if (!rsData.Success)
            {
                failure(rsData.Obj.ToString());
            }
            else if (success != null)
            {
                Debug.Log("success:" + typeof(T));
                success(typeof(T) == typeof(string) ? rsData.Obj.ToString() as T : Json.Parse<T>(rsData.Obj.ToString()));
            }
        }
    }
    IEnumerator IGet<T>(string url, Action<T> success, Action<string> failure, bool closeLoadingBeforeAction) where T : class
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.timeout = TIMEOUT;
        webRequest.SendWebRequest();
        PanelLoading.current.WebLoading();
        while (!webRequest.isDone)
        {
            yield return 1;
        }
        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.Log(webRequest.error);
            if (failure != null)
            {
                failure(webRequest.error);
            }
        }
        else
        {
            if (closeLoadingBeforeAction)
            {
                PanelLoading.current.Close();
            }
            ResultData<object> rsData = Json.Parse<ResultData<object>>(webRequest.downloadHandler.text);
            Debug.Log(webRequest.downloadHandler.text);
            if (!rsData.Success)
            {
                failure(rsData.Obj.ToString());
            }
            else if (success != null)
            {
                Debug.Log("success:" + typeof(T));
                success(typeof(T) == typeof(string) ? rsData.Obj.ToString() as T : Json.Parse<T>(rsData.Obj.ToString()));
            }
        }
    }
    // IEnumerable IResult<T>(UnityWebRequest webRequest, Action<T> success, Action<string> failure, bool closeLoadingBeforeAction)
    // {
    //     Debug.Log("result");
    //     while (!webRequest.isDone)
    //     {
    //         yield return 1;
    //     }
    //     if (webRequest.isHttpError || webRequest.isNetworkError)
    //     {
    //         Debug.Log(webRequest.error);
    //         if (failure != null)
    //         {
    //             failure(webRequest.error);
    //         }
    //     }
    //     else
    //     {
    //         if (closeLoadingBeforeAction)
    //         {
    //             PanelLoading.current.Close();
    //         }
    //         ResultData<object> rsData = Json.Parse<ResultData<object>>(webRequest.downloadHandler.text);
    //         Debug.Log(rsData.Obj.ToString());
    //         if (!rsData.Success)
    //         {
    //             failure(rsData.Obj.ToString());
    //         }
    //         if (success != null)
    //         {
    //             success((rsData.Obj as JArray).ToObject<T>());
    //         }
    //     }
    //     // yield return 1;
    // }

}