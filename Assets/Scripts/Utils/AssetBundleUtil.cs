using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
public static class AssetBundleUtil
{
    /// <summary>
    /// ab包提取的go缓存对象
    /// </summary>
    /// <typeparam name="int">modelId</typeparam>
    /// <typeparam name="GameObject"></typeparam>
    /// <returns></returns>
    public static Dictionary<int, GameObject> DicPrefab = new Dictionary<int, GameObject>();
    /// <summary>
    /// ab包提取的sprite对象
    /// </summary>
    /// <typeparam name="int">modelId</typeparam>
    /// <typeparam name="Sprite"></typeparam>
    /// <returns></returns>
    public static Dictionary<int, Sprite> DicSprite = new Dictionary<int, Sprite>();
    public static void Clear()
    {
        DicPrefab.Clear();
        DicSprite.Clear();
    }
    public static void Load()
    {
        List<Manifest> localManifests = LocalAssetUtil.Manifests;
        WebUtil.FindModelList(rs =>
        {
            Debug.Log(rs);
            Manifest manifestType0 = localManifests.Find(m => m.ModelType.Id == 0);
            ResultData<object> rsData = Json.Parse<ResultData<object>>(rs);
            if (!rsData.Success)
            {
                Debug.LogError(rsData.Obj.ToString());
                PanelLoading.current.Error(rsData.Obj.ToString());
                return;
            }
            List<Model> needDownload;
            if (null == manifestType0)
            {
                needDownload = (rsData.Obj as JArray).ToObject<List<Model>>();
            }
            else
            {
                needDownload = new List<Model>();
                (rsData.Obj as JArray).ToObject<List<Model>>().ForEach(m =>
                {
                    if (!manifestType0.Models.Exists(i => i.Id == m.Id))
                    {
                        needDownload.Add(m);
                    }
                });
            }
            MyWebRequest.current.LoadAssetBundle(needDownload, localManifests, () =>
            {
                if (needDownload.Count == 0)
                {
                    Debug.Log("无更新项");
                    return;
                }
                LocalAssetUtil.Manifests = localManifests;
            });
        },
        () =>
        {
            MyWebRequest.current.LoadAssetBundle(null, localManifests, null);
        });
    }
}